using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject _road;
    public GameObject _road1;
    public GameObject _road2;
    public GameObject _road3;
    public GameObject _road4;
    public GameObject _roadC;

    void Start()
    {
        _Oroad = Instantiate(_road, transform);
        _Oroad1 = Instantiate(_road1, transform);
        _Oroad2 = Instantiate(_road2, transform);
        _Oroad3 = Instantiate(_road3, transform);
        _Oroad4 = Instantiate(_road4, transform);
        _OroadC = Instantiate(_roadC, transform);
        _Oroad1.SetActive(false);
        _Oroad2.SetActive(false);
        _Oroad3.SetActive(false);
        _Oroad4.SetActive(false);
        _OroadC.SetActive(false);
    }
    void Update()
    {
        RayCastCheck();
        RoadChange();
    }

    Vector3 _orgin;
    public bool _left = false;
    public bool _right = false;
    public bool _leftO = false;
    public bool _rightO = false;
    public GameObject _Oroad;
    public GameObject _Oroad1;
    public GameObject _Oroad2;
    public GameObject _Oroad3;
    public GameObject _Oroad4;
    public GameObject _OroadC;

    void RoadChange()
    {
        int trueCount = (_left ? 1 : 0) + (_leftO ? 1 : 0) + (_right ? 1 : 0) + (_rightO ? 1 : 0);

        if(trueCount == 0)
        {
            _Oroad.SetActive(true);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
            if (_left)
            {
                transform.Rotate(0, 90, 0);         
            }
        }
        else if (trueCount == 1)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(true);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
        }
        else if (trueCount == 2)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(true);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
        }
        else if (trueCount == 3)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(true);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
        }
        else if (trueCount == 4)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(true);
            _OroadC.SetActive(false);
        }
    }
    void RayCastCheck()
    {
        Debug.Log("Hello");
        _orgin = new Vector3(transform.position.x, transform.position.y * 1.5f, transform.position.z);
        if (Physics.Raycast(_orgin, transform.forward, out RaycastHit AhitInfo, 2))
        {
            Debug.Log("Hello");
            if (AhitInfo.collider.gameObject.CompareTag("Road"))
            {                
                Debug.DrawRay(_orgin, transform.forward * 2.5f, Color.green);
                _left = true;
            }
            else
            {
                _left = false;
                Debug.DrawRay(_orgin, transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _left = false;
            Debug.DrawRay(_orgin, transform.forward * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, -transform.forward, out RaycastHit BhitInfo, 2))
        {
            Debug.Log("Hello");
            if (BhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, -transform.forward * 2.5f, Color.green);
                _leftO = true;
            }
            else
            {
                _leftO = false;
                Debug.DrawRay(_orgin, -transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _leftO = false;
            Debug.DrawRay(_orgin, -transform.forward * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, transform.right, out RaycastHit ChitInfo, 2))
        {
            Debug.Log("Hello");
            if (ChitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, transform.right * 2.5f, Color.green);
                _right = true;
            }
            else
            {
                _right = false;
                Debug.DrawRay(_orgin, transform.right * 10f, Color.red);
            }
        }
        else
        {
            _right = false;
            Debug.DrawRay(_orgin, transform.right * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, -transform.right, out RaycastHit DhitInfo, 2))
        {
            Debug.Log("Hello");
            if (DhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, -transform.right * 2.5f, Color.green);
                _rightO = true;
            }
            else
            {
                _rightO = false;
                Debug.DrawRay(_orgin, -transform.right * 10f, Color.red);
            }
        }
        else
        {
            _rightO = false;
            Debug.DrawRay(_orgin, -transform.right * 10f, Color.red);
        }
    }
}
