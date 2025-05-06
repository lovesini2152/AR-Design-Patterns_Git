using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ResetFunctionManager : MonoBehaviour
{
    [System.Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    [SerializeField]
    private List<GameObject> objectsToReset = new List<GameObject>();

    private Dictionary<GameObject, TransformData> initialTransforms = new Dictionary<GameObject, TransformData>();

    public UnityEvent OnReset;

    public void SavePosition()
    {
        foreach (var obj in objectsToReset)
        {
            if (obj != null)
            {
                string key = obj.name;

                initialTransforms[obj] = new TransformData(obj.transform.position, obj.transform.rotation, obj.transform.localScale);

                // Save position
                PlayerPrefs.SetFloat(key + "_pos_x", obj.transform.position.x);
                PlayerPrefs.SetFloat(key + "_pos_y", obj.transform.position.y);
                PlayerPrefs.SetFloat(key + "_pos_z", obj.transform.position.z);

                // Save rotation
                PlayerPrefs.SetFloat(key + "_rot_x", obj.transform.rotation.x);
                PlayerPrefs.SetFloat(key + "_rot_y", obj.transform.rotation.y);
                PlayerPrefs.SetFloat(key + "_rot_z", obj.transform.rotation.z);
                PlayerPrefs.SetFloat(key + "_rot_w", obj.transform.rotation.w);

                // Save scale
                PlayerPrefs.SetFloat(key + "_scale_x", obj.transform.localScale.x);
                PlayerPrefs.SetFloat(key + "_scale_y", obj.transform.localScale.y);
                PlayerPrefs.SetFloat(key + "_scale_z", obj.transform.localScale.z);
            }
        }

        PlayerPrefs.Save(); // Ensure data is written to disk
    }

    public void LoadPosition()
    {
        foreach (var obj in objectsToReset)
        {
            if (obj != null)
            {
                string key = obj.name;

                // Check if data exists in PlayerPrefs
                if (PlayerPrefs.HasKey(key + "_pos_x"))
                {
                    // Load position
                    Vector3 position = new Vector3(
                        PlayerPrefs.GetFloat(key + "_pos_x"),
                        PlayerPrefs.GetFloat(key + "_pos_y"),
                        PlayerPrefs.GetFloat(key + "_pos_z")
                    );

                    // Load rotation
                    Quaternion rotation = new Quaternion(
                        PlayerPrefs.GetFloat(key + "_rot_x"),
                        PlayerPrefs.GetFloat(key + "_rot_y"),
                        PlayerPrefs.GetFloat(key + "_rot_z"),
                        PlayerPrefs.GetFloat(key + "_rot_w")
                    );

                    // Load scale
                    Vector3 scale = new Vector3(
                        PlayerPrefs.GetFloat(key + "_scale_x"),
                        PlayerPrefs.GetFloat(key + "_scale_y"),
                        PlayerPrefs.GetFloat(key + "_scale_z")
                    );

                    // Apply the loaded transform data
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.transform.localScale = scale;

                    // Update initialTransforms dictionary
                    initialTransforms[obj] = new TransformData(position, rotation, scale);
                }
            }
        }
    }

    public void Reset()
    {
        foreach (var obj in objectsToReset)
        {
            if (obj != null && initialTransforms.ContainsKey(obj))
            {
                var data = initialTransforms[obj];
                obj.transform.position = data.position;
                obj.transform.rotation = data.rotation;
                obj.transform.localScale = data.scale;
            }
        }

        OnReset?.Invoke();
    }
}
