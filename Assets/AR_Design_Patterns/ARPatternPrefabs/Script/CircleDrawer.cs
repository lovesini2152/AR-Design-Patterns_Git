using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircleDrawer : MonoBehaviour
{
    #region variables

    private List<GameObject> wayNodes;
    [SerializeField] private bool deleteCircles;
    [Space(10)]
    [Header("Customizing the spawning behaviour of circles")]
    [SerializeField] private float segmentSize;
    [SerializeField] private int numberOfCirclesPerSegment;
    [SerializeField] private float circleSpawnrate;
    [SerializeField] private float pathWidth;
    [SerializeField] private int seed;

    [Space(10)]

    [Header("Customizing the appearance of one circle")]
    [SerializeField] private float colorThreshold;
    [SerializeField] private float linethicknessMin, linethicknessMax;
    //Determines how many points are creating a circle. The lower the number, the better the performance, but the cirlce looks more edgy
    [SerializeField] private int lineCount;
    [SerializeField] private float radiusUpperBound;
    [SerializeField] private float radiusLowerBound;
    [SerializeField] private float heightUpperBound;
    [SerializeField] private float heightLowerBound;

    public UnityEvent eventsAfterLastNodeReached;
    public UnityEvent eventsWhenPatternIsEnabled;

    private bool firstStart = true;
    static Material lineMaterial;
    private List<Circle> circles;
    private int nodePassedCounter = 0;
    private System.Random rnd;
    private bool coroutineStarted = false;
    Vector3 endPoint = Vector3.one;
    private int numberOfCirclesToDrawPerFrame = 0;

    #endregion


    public void SaveWaypoints()
    {
        for (int i = 0; i < wayNodes.Count; i++)
        {
            Transform waypoint = wayNodes[i].transform;
            string waypointName = waypoint.name;

            // Speichern der x-, y- und z-Koordinate in PlayerPrefs unter dem Namen des Objekts
            PlayerPrefs.SetFloat(waypointName + "_x", waypoint.position.x);
            PlayerPrefs.SetFloat(waypointName + "_y", waypoint.position.y);
            PlayerPrefs.SetFloat(waypointName + "_z", waypoint.position.z);
        }
        Debug.Log("Count" + wayNodes.Count);
        // Speichern der Anzahl der Wegpunkte
        PlayerPrefs.SetInt("Waypoint_Count", wayNodes.Count);
        PlayerPrefs.Save();

        Debug.Log("Wegpunkte mit Objektnamen gespeichert!");
        var tmp = FindObjectOfType<ResetFunctionManager>();
        if (tmp != null)
        {
            tmp.SavePosition();
        }
    }

    // Laden der Wegpunkte aus PlayerPrefs und Setzen der Positionen der vorhandenen Objekte
    public void LoadWaypoints()
    {
        // Prüfen, ob die Anzahl der Wegpunkte in den PlayerPrefs gespeichert ist
        if (PlayerPrefs.HasKey("Waypoint_Count"))
        {
            int waypointCount = PlayerPrefs.GetInt("Waypoint_Count");

            for (int i = 0; i < waypointCount; i++)
            {
                // Namen des Wegpunkts-Objekts verwenden
                Transform waypoint = wayNodes[i].transform;
                string waypointName = waypoint.name;

                // Prüfen, ob die Position für den jeweiligen Wegpunkt in den PlayerPrefs existiert
                if (PlayerPrefs.HasKey(waypointName + "_x") && PlayerPrefs.HasKey(waypointName + "_y") && PlayerPrefs.HasKey(waypointName + "_z"))
                {
                    float x = PlayerPrefs.GetFloat(waypointName + "_x");
                    float y = PlayerPrefs.GetFloat(waypointName + "_y");
                    float z = PlayerPrefs.GetFloat(waypointName + "_z");

                    // Setzen der gespeicherten Position auf das vorhandene Objekt
                    waypoint.position = new Vector3(x, y, z);

                    Debug.Log(waypointName + " Position geladen: " + waypoint.position);
                }
                else
                {
                    Debug.LogWarning("Keine gespeicherte Position für " + waypointName + " gefunden.");
                }
            }

            Debug.Log("Alle Wegpunkte-Positionen wurden geladen!");
        }
        else
        {
            Debug.Log("Keine gespeicherten Wegpunkte gefunden.");
        }
        var tmp = FindObjectOfType<ResetFunctionManager>();
        if (tmp != null)
        {
            tmp.LoadPosition();
        }
    }

    private void Awake()
    {

        PatternManager.resetAllPatterns += resetPattern;
        wayNodes = new List<GameObject>();
        refreshNodesList();
    }

    private void disableAllMeshRenderers()
    {
        foreach (var node in wayNodes)
        {
            node.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void refreshNodesList()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<BoxCollider>())
                wayNodes.Add(this.transform.GetChild(i).gameObject);
            if (firstStart)
            {
                if (i > 1) this.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
            }
        }
        firstStart = false;
    }
    public void FreezeNodes()
    {
        foreach (var node in wayNodes)
        {
            var rb = node.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void unFreezeNodes()
    {
        foreach (var node in wayNodes)
        {
            var rb = node.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void resetPattern()
    {
        this.gameObject.SetActive(false);
        Reset();

    }
    private void OnEnable()
    {
        eventsWhenPatternIsEnabled.Invoke();
    }

    private void Start()
    {
        rnd = new System.Random(seed);
        circles = new List<Circle>();

        disableAllMeshRenderers();
        Debug.Log(wayNodes.Count);
        //Get the first path
        if (wayNodes.Count >= 2)
        {
            CreateListOfCirclePositions(nodePassedCounter);
        }
        else
        {
            Debug.Log("Not enough nodes set");
        }

        CreateLineMaterial();
    }

    private void OnDisable()
    {
        Reset();
    }

    //return a random float depending on the seed
    public float rndFloat(float min, float max)
    {
        return (float)rnd.NextDouble() * (max - min) + min;

    }
    //Create a random seed
    public void rndSeed()
    {
        if (!Application.isPlaying)
        {
            rnd = new System.Random();
        }
        seed = rnd.Next();
    }
    private void CreateListOfCirclePositions(int startIndex)
    {
        if (startIndex >= wayNodes.Count - 1) return;
        //calculate distance between current waypoint and the next one
        float dist = Vector3.Distance(wayNodes[startIndex].transform.localPosition, wayNodes[startIndex + 1].transform.localPosition);
        //divide the distance in segments
        var numberOfSegments = Mathf.Ceil(dist / segmentSize);
        //calculate the vector between the current position and the next position
        var vec = wayNodes[startIndex + 1].transform.localPosition - wayNodes[startIndex].transform.localPosition;
        var segmentLength = dist / numberOfSegments;

        if (startIndex == 0)
        {
            wayNodes[startIndex + 1].GetComponent<BoxCollider>().enabled = true;
            wayNodes[startIndex].GetComponent<BoxCollider>().enabled = false;
        }
        else if (startIndex <= wayNodes.Count - 1)
        {
            wayNodes[startIndex + 1].GetComponent<BoxCollider>().enabled = true;
            wayNodes[startIndex - 1].GetComponent<BoxCollider>().enabled = false;
            wayNodes[startIndex].GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            wayNodes[startIndex].GetComponent<BoxCollider>().enabled = false;
        }



        for (int j = 0; j < numberOfSegments; j++)
        {
            Vector3 startPoint = Vector3.one;

            //calculate the current segments coordinates
            if (j == 0)
            {
                startPoint = wayNodes[startIndex].transform.localPosition;
            }
            else
            {
                startPoint = endPoint;
            }
            endPoint = vec.normalized * segmentLength + startPoint;

            //for each segment create random circle positions
            for (int i = 0; i < numberOfCirclesPerSegment; i++)
            {
                if (seed == 0)
                {
                    circles.Add(new Circle(calculateCirclePos(startPoint, endPoint, vec, segmentLength, i), Random.Range(0.01f, 0.2f), new Color(Random.Range(colorThreshold, 1f), Random.Range(colorThreshold, 1f), Random.Range(colorThreshold, 1f), 1f),
                        Random.Range(linethicknessMin, linethicknessMax)));
                }
                else
                {
                    circles.Add(new Circle(calculateCirclePos(startPoint, endPoint, vec, segmentLength, i), rndFloat(0.01f, 0.2f), new Color(rndFloat(colorThreshold, 1f), rndFloat(colorThreshold, 1f), rndFloat(colorThreshold, 1f), 1f), rndFloat(linethicknessMin, linethicknessMax)));
                }
            }
        }
    }



    private Vector3 calculateCirclePos(Vector3 start, Vector3 end, Vector3 vec, float segmentLength, int i)
    {
        Vector3 addedDistance;
        if (seed == 0)
        {
            addedDistance = vec.normalized * Random.Range(segmentLength / numberOfCirclesPerSegment * i / 2, segmentLength);
        }
        else
        {
            addedDistance = vec.normalized * rndFloat(segmentLength / numberOfCirclesPerSegment * i / 2, segmentLength);
        }

        var randomPoint = start + addedDistance;
        float heightVariation = Random.Range(heightLowerBound, heightUpperBound);
        randomPoint.y = Mathf.Lerp(start.y, end.y, (randomPoint - start).magnitude / segmentLength) + heightVariation; // Höhe mit Pfadverlauf berücksichtigen

        if (seed == 0)
        {
            return Vector3.Cross(randomPoint, Vector3.up).normalized * Random.Range(-pathWidth / 2, pathWidth / 2) + randomPoint;
        }
        else
        {
            return Vector3.Cross(randomPoint, Vector3.up).normalized * rndFloat(-pathWidth / 2, pathWidth / 2) + randomPoint;
        }
    }

    public void setNextNodes()
    {
        if (deleteCircles)
        {
            circles.Clear();
            numberOfCirclesToDrawPerFrame = 0;
        }
        nodePassedCounter++;
        CreateListOfCirclePositions(nodePassedCounter);

    }
    public void Reset()
    {
        rnd = new System.Random(seed);
        if (circles != null) circles.Clear();
        nodePassedCounter = 0;
        numberOfCirclesToDrawPerFrame = 0;
        firstStart = true;
        StopAllCoroutines();
        coroutineStarted = false;
        if (circles != null) CreateListOfCirclePositions(nodePassedCounter);
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    private void OnRenderObject()
    {

        if (!coroutineStarted)
        {
            coroutineStarted = true;
            StartCoroutine(increaseNumberOfCirclesDrawn(1 / circleSpawnrate));
        }
        for (int i = 0; i < numberOfCirclesToDrawPerFrame; i++)
        {
            DrawCircle(circles[i].center, circles[i].radius, circles[i].color, circles[i].lineThickness);
        }

    }

    private IEnumerator increaseNumberOfCirclesDrawn(float delay)
    {
        if (numberOfCirclesToDrawPerFrame < circles.Count) numberOfCirclesToDrawPerFrame++;
        yield return new WaitForSeconds(delay);
        StartCoroutine(increaseNumberOfCirclesDrawn(delay));
    }

    public void DrawCircle(Vector3 center, float radius, Color color, float linethickness)
    {
        Vector3 ver;
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.QUADS);
        GL.Color(color);
        for (int i = 0; i < lineCount; i++)
        {
            if (i > 0 || i < lineCount - 1)
            {
                ver = getVector(center, radius - (linethickness / 100) / 2, i - 1);
                GL.Vertex3(ver.x, center.y, ver.z);
                ver = getVector(center, radius + (linethickness / 100) / 2, i - 1);
                GL.Vertex3(ver.x, center.y, ver.z);
            }
            ver = getVector(center, radius + (linethickness / 100) / 2, i);
            GL.Vertex3(ver.x, center.y, ver.z);
            ver = getVector(center, radius - (linethickness / 100) / 2, i);
            GL.Vertex3(ver.x, center.y, ver.z);
        }
        GL.End();

        GL.PopMatrix();
    }

    //returns a vector on the circle
    private Vector3 getVector(Vector3 center, float radius, int i)
    {

        float a = (float)i / (float)lineCount;

        float angle = a * Mathf.PI * 2;

        float x = Mathf.Cos(angle) * radius + center.x;
        float z = Mathf.Sin(angle) * radius + center.z;
        float y = center.y;
        return new Vector3(x, y, z);
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleDrawer : MonoBehaviour
{
    #region variables

    [Header("Changable while playmode is running")]
    //Determines how many points are creating a circle. The lower the number, the better the performance, but the cirlce looks more edgy
    [SerializeField] private int lineCount;
    [SerializeField] private float linethickness;
    public Color color;


    [Space(10)]

    [Header("Changable only outside of playmode")]
    [SerializeField] private List<Transform> wayNodes;
    [SerializeField] private int numberOfCirclesPerSegment;

    [Space(10)]

    [SerializeField] private float pathWidth;
    [SerializeField] private float radiusUpperBound;
    [SerializeField] private float radiusLowerBound;
    [SerializeField] private float circleSpawnrate;
    

    
    [Space(10)]
    [Tooltip("if checked the circles will be created randomly within the path width, otherwise just in a straight line ")]
    [SerializeField] private bool randomize;
    [SerializeField] private int seed;
    [SerializeField] private float segmentSize;
    [SerializeField] private int numberOfCirclesToDrawPerFrame;

    static Material lineMaterial;
    private List<Circle> circles;
    private int nodePassedCounter = 0;
    private System.Random rnd;
    private bool coroutineStarted = false;

    Vector3 endPoint = Vector3.one;
    #endregion

    private void Start()
    {
        rnd = new System.Random(seed);
        circles = new List<Circle>();

        //Get the first path
        if (wayNodes.Count >= 2)
        {
            CreateListOfCirclePositions(nodePassedCounter);
        }
        else {
            Debug.Log("No nodes set");
        }

        CreateLineMaterial();
    }

    //return a random float depending on the seed
    public float rndFloat(float min, float max) {
        return (float)rnd.NextDouble() * (max - min) + min;

    }
    //Create a random seed
    public void rndSeed() {
        if (!Application.isPlaying) {
            rnd = new System.Random();
        }
        seed = rnd.Next();
    }
    private void CreateListOfCirclePositions(int startIndex)
    {
        if (startIndex >= wayNodes.Count - 1) return;
        //calculate distance between current waypoint and the next one
        float dist = Vector3.Distance(wayNodes[startIndex].localPosition, wayNodes[startIndex + 1].localPosition);
        //divide the distance in segments
        var numberOfSegments = Mathf.Ceil(dist/segmentSize);
        Debug.Log("numberOfSegments " + numberOfSegments);
        //calculate the vector between the current position and the next position
        var vec = wayNodes[startIndex+1].localPosition - wayNodes[startIndex].localPosition;
        var segmentLength = dist / numberOfSegments;
        for (int j = 0; j < numberOfSegments; j++)
        {
            Vector3 startPoint = Vector3.one;
            
            //calculate the current segments coordinates
            if (j == 0) { 
                 startPoint = wayNodes[startIndex].localPosition; 
            } else {
                 startPoint = endPoint;
            }
            endPoint = vec.normalized * segmentLength + startPoint;
            /*#region debugging
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.gameObject.transform;
            cube.transform.localScale = Vector3.one / 100;
            cube.name = j + " start " + startPoint;
            cube.transform.localPosition = startPoint;
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.gameObject.transform;
            cube.transform.localScale = Vector3.one / 100;
            cube.name = j + " end " + endPoint;
            cube.transform.localPosition = endPoint;
            #endregion
//for each segment create random circle positions
for (int i = 0; i < numberOfCirclesPerSegment; i++)
            {
                if (seed == 0)
                {
                    circles.Add(new Circle(calculateCirclePos(startPoint, endPoint, vec, segmentLength,i), Random.Range(0.01f, 0.2f), j));
                }
                else
                {
                    var point = calculateCirclePos(startPoint, endPoint, vec, segmentLength,i);
                    //Debug.Log(point + " " + j + " " + startPoint + " " + endPoint);
                    circles.Add(new Circle(point, rndFloat(0.01f, 0.2f), j));
                }
                //Debug.Log(j);
            }

        }

    }

    private Vector3 calculateCirclePos(Vector3 start, Vector3 end, Vector3 vec, float segmentLength, int i)
    {
        Vector3 addedDistance;
        if (seed == 0) {
             addedDistance = vec.normalized * Random.Range(segmentLength/numberOfCirclesPerSegment*i/2, segmentLength);
        } else {
            addedDistance = vec.normalized * rndFloat(segmentLength / numberOfCirclesPerSegment * i / 2, segmentLength);
        }
        
        var randomPoint =start + addedDistance;

        if (randomize)
        {
            if (seed == 0) {
                return Vector3.Cross(randomPoint, Vector3.up).normalized * Random.Range(-pathWidth / 2, pathWidth / 2) + randomPoint;
            } else {
                return Vector3.Cross(randomPoint, Vector3.up).normalized * rndFloat(-pathWidth / 2, pathWidth / 2) + randomPoint;
            }          
        }
        else {
            return randomPoint;
        }
    }

    public void setNextNodes() {
        circles.Clear();
        nodePassedCounter++;
        CreateListOfCirclePositions(nodePassedCounter);

    }
    public void Reset()
    {
        rnd = new System.Random(seed);
        circles.Clear();
        nodePassedCounter = 0;
        CreateListOfCirclePositions(nodePassedCounter);
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void resetCounter() {
        numberOfCirclesToDrawPerFrame = 0;
    }

    private void OnRenderObject()
    {   
        if (!coroutineStarted){
            coroutineStarted = true;
            StartCoroutine(increaseCirclesDrawn(1/ circleSpawnrate));
        }
        for (int i = 0; i <numberOfCirclesToDrawPerFrame; i++)
        {
            DrawCircle(circles[i].center, circles[i].radius);
           //Debug.Log(circles[i].segment + " " + numberOfCirclesToDrawPerFrame);
            
        }        
    }

    IEnumerator increaseCirclesDrawn(float delay) {
        if (numberOfCirclesToDrawPerFrame < circles.Count) numberOfCirclesToDrawPerFrame++;
        yield return new WaitForSeconds(delay);
        StartCoroutine(increaseCirclesDrawn(delay));
    }

    public void DrawCircle(Vector3 center, float radius)
    {
        Vector3 ver;
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        
        
        if (linethickness == 1) {
            GL.Begin(GL.LINES);
            GL.Color(color);
            for (int i = 0; i < lineCount; i++)
            {
                if (i > 0 || i < lineCount - 1)
                {
                    ver = getVector(center, radius, i - 1);
                    GL.Vertex3(ver.x, ver.y, ver.z);
                }

                ver = getVector(center, radius, i);
                GL.Vertex3(ver.x, ver.y, ver.z);

            }
            GL.End();
        }
        else {
            //Since the Gl library does not support line thickness, it is neccessary to draw a filled quad for each line.
            GL.Begin(GL.QUADS);
            GL.Color(color);
            for (int i = 0; i < lineCount; i++)
            {
                if (i > 0 || i < lineCount - 1)
                {
                    ver = getVector(center, radius - (linethickness / 100) / 2, i-1);
                    GL.Vertex3(ver.x, ver.y, ver.z);
                    ver = getVector(center, radius + (linethickness / 100) / 2, i-1);
                    GL.Vertex3(ver.x, ver.y, ver.z);
                }
                ver = getVector(center, radius + (linethickness / 100) / 2, i); 
                GL.Vertex3(ver.x, ver.y, ver.z);
                ver = getVector(center, radius - (linethickness /100) / 2, i);
                GL.Vertex3(ver.x, ver.y, ver.z);
            }
            GL.End();
        }

        GL.PopMatrix();
    }

    //returns a vector on the circle
    private Vector3 getVector(Vector3 center, float radius, int i)
    {

        float a = (float)i / (float)lineCount;

        float angle = a * Mathf.PI * 2;

        float x = Mathf.Cos(angle) * radius + center.x;
        float z = Mathf.Sin(angle) * radius + center.z;
        float y = center.y;
        return new Vector3(x, y, z);
    }
}
*/