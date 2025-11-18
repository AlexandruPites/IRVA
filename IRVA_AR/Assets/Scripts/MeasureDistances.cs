using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="pointPrefab"/> is instantiated
/// and moved to the hit position. Then, the <see cref="linePrefab"/> is instantiated,
/// scaled and placed between the last two points added on screen. The <see cref="textPrefab"/>
/// is also instantiated above the <see cref="linePrefab"/> to show the distance between the
/// last two points. The total distance is displayed on a canvas.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class MeasureDistances : MonoBehaviour
{
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary>
    public Camera FirstPersonCamera;
    public Canvas parentCanvas;

    /// <summary>
    /// A prefab to place when a raycast from a user touch hits a plane.
    /// </summary>
    public GameObject pointPrefab;

    /// <summary>
    /// A prefab to place to unite two adiacent points.
    /// </summary>
    public GameObject linePrefab;

    /// <summary>
    /// A prefab to place to display the distance between two adiacent points.
    /// </summary>
    public TMP_Text textPrefab;

    /// <summary>
    /// The canvas needed to display text on screen.
    /// </summary>
    public Canvas parent;

    /// <summary>
    /// A list of all added points on screen.
    /// </summary>
    public List<GameObject> points = new List<GameObject>();

    /// <summary>
    /// A list of distances of adiacent points on screen.
    /// </summary>
    public List<TMP_Text> distances = new List<TMP_Text>();

    /// <summary>
    /// The total distance of all points on screen.
    /// </summary>
    public TMP_Text totalDistance;
    float distanceSum = 0;
    
    [Header("Selection Settings")]
    public Material defaultMaterial;
    public Material selectedMaterial;
    public string selectableTag = "Player";
    private List<GameObject> lines = new List<GameObject>();
    private GameObject selectedObject = null;
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            return true;
        }
#endif
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            touchPosition = default;
            return false;
        }

        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }
    
    void SelectObject(GameObject obj)
    {
        if (selectedObject != null && defaultMaterial != null)
        {
            selectedObject.transform.GetChild(0).GetComponent<Renderer>().material = defaultMaterial;
        }
        if (selectedObject == obj)
        {
            selectedObject = null;
            return;
        }
        selectedObject = obj;
        if (selectedObject != null && selectedMaterial != null)
        {
            selectedObject.transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
        }
    }
    
    void MoveSelectedObject(Vector3 newPosition)
    {
        ARAnchor anchor = selectedObject.GetComponent<ARAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }
        
        selectedObject.transform.position = newPosition;
        selectedObject.AddComponent<ARAnchor>();
        
    }
    
    void CreateNewPoint(Pose hitPose)
    {
        GameObject newPoint = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
        newPoint.AddComponent<ARAnchor>();
        
        newPoint.tag = selectableTag; 
        points.Add(newPoint);

        if (points.Count > 1)
        {
            GameObject newLine = Instantiate(linePrefab);
            lines.Add(newLine);

            TMP_Text newText = Instantiate(textPrefab, parentCanvas.transform);
            distances.Add(newText);
        }
    }
    
    void UpdateGeometryAndUI()
    {
        float totalDist = 0;
        
        for (int i = 0; i < points.Count - 1; i++)
        {
            GameObject p1 = points[i];
            GameObject p2 = points[i + 1];

            GameObject line = lines[i];
            TMP_Text distText = distances[i];

            float dist = Vector3.Distance(p1.transform.position, p2.transform.position);
            totalDist += dist;

            line.transform.position = (p1.transform.position + p2.transform.position) / 2.0f;
            line.transform.LookAt(p2.transform.position);
            line.transform.localScale = new Vector3(0.5f, 0.5f, dist * 20); 
            distText.text = dist.ToString("F2") + "m";
            distText.transform.position = FirstPersonCamera.WorldToScreenPoint(line.transform.position);
            
        }

        if (totalDistance != null)
        {
            totalDistance.text = "Total: " + totalDist.ToString("F2") + "m";
        }
    }

    void Update()
    {
        UpdateGeometryAndUI();
        
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        Ray ray = FirstPersonCamera.ScreenPointToRay(touchPosition);
        RaycastHit physicsHit;

        if (Physics.Raycast(ray, out physicsHit))
        {
            GameObject hitObj = physicsHit.collider.gameObject;

            if (hitObj.CompareTag(selectableTag) || points.Contains(hitObj))
            {
                SelectObject(hitObj.transform.parent.gameObject);
                return;
            }
        }

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            if (selectedObject != null)
            {
                MoveSelectedObject(hitPose.position);
                SelectObject(null); 
            }
            else
            {
                CreateNewPoint(hitPose);
            }
        }
        
        
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}