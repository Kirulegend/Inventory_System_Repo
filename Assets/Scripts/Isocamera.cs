using UnityEngine;

public class Isocamera : MonoBehaviour
{
    Vector3 startPos;
    Vector3 dragPos;
    [SerializeField] float scrollSpeed = 10f;
    [SerializeField] float dragSpeed = 10f;
    Transform xMinTrans;
    Transform xMaxTrans;
    [SerializeField] float zoomMin = 5f;
    [SerializeField] float zoomMax = 11f;


    void Update()
    {
        var camRef = Camera.main;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            camRef.orthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime;
            camRef.orthographicSize = Mathf.Clamp(camRef.orthographicSize, zoomMin, zoomMax);
        }
        if (Input.GetMouseButtonDown(2))
        {
            startPos = camRef.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(2))
        {
            dragPos = camRef.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = startPos - dragPos;
            transform.position += new Vector3(diff.x, 0, diff.z) * dragSpeed * Time.deltaTime;
        }
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            pos.z += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            pos.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            pos.x += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            pos.x -= panSpeed * Time.deltaTime;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        pos.x = Mathf.Clamp(pos.x, minX + camWidth, maxX - camWidth);
        pos.z = Mathf.Clamp(pos.z, minY + camHeight, maxY - camHeight);

        transform.position = pos;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            panSpeed *= 3;
            dragSpeed *= 3;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            panSpeed /= 3;
            dragSpeed /= 3;
        }
    }

    public float panSpeed = 10f;
    public float minX, maxX, minY, maxY;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
}
