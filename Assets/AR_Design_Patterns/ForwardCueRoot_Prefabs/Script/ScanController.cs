using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

using UnityEngine;

public class ScanController : MonoBehaviour
{
    public GameObject Scan;
    public bool disableMeshrenderer = true;

    public void ToggleScan()
    {
        if (disableMeshrenderer) {
            Scan.GetComponent<MeshRenderer>().enabled = !Scan.GetComponent<MeshRenderer>().enabled;
            Scan.GetComponent<MeshCollider>().enabled = !Scan.GetComponent<MeshCollider>().enabled;

        }
    }
    public void ScanOn()
    {
        // Enable MeshRenderer and MeshCollider when Scan is on
        Scan.GetComponent<MeshRenderer>().enabled = true;
        Scan.GetComponent<MeshCollider>().enabled = true;
    }

    public void ScanOff()
    {
        // Disable MeshRenderer and MeshCollider when Scan is off
        Scan.GetComponent<MeshRenderer>().enabled = false;
        Scan.GetComponent<MeshCollider>().enabled = false;
    }
}
