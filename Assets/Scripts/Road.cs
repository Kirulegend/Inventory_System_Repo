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
    bool _left = false;
    bool _right = false;
    bool _leftO = false;
    bool _rightO = false;
    GameObject _Oroad;
    GameObject _Oroad1;
    GameObject _Oroad2;
    GameObject _Oroad3;
    GameObject _Oroad4;
    GameObject _OroadC;
    GameObject activeRoad;
    int trueCount;

    void RoadChange()
    {
        trueCount = (_left ? 1 : 0) + (_leftO ? 1 : 0) + (_right ? 1 : 0) + (_rightO ? 1 : 0);

        if (trueCount == 0)
        {
            _Oroad.SetActive(true);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
            activeRoad = _Oroad;
        }
        else if (trueCount == 1)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(true);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
            activeRoad = _Oroad1;
            if (_left) activeRoad.transform.localRotation = Quaternion.Euler(0, 0, 0);
            else if (_right) activeRoad.transform.localRotation = Quaternion.Euler(0, 90, 0);
            else if (_leftO) activeRoad.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else if (_rightO) activeRoad.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (trueCount == 2)
        {
            if ((_left && _right) || (_leftO && _rightO) || (_leftO && _right) || (_left && _rightO))
            {
                _Oroad.SetActive(false);
                _Oroad1.SetActive(false);
                _Oroad2.SetActive(false);
                _Oroad3.SetActive(false);
                _Oroad4.SetActive(false);
                _OroadC.SetActive(true);
                activeRoad = _OroadC;
            }
            else
            {
                _Oroad.SetActive(false);
                _Oroad1.SetActive(false);
                _Oroad2.SetActive(true);
                _Oroad3.SetActive(false);
                _Oroad4.SetActive(false);
                _OroadC.SetActive(false);
                activeRoad = _Oroad2;
            }
            if ((_left && _rightO) || (_left && _leftO)) activeRoad.transform.localRotation = Quaternion.Euler(0, 0, 0);
            else if (_right && _leftO) activeRoad.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else if (_rightO && _leftO) activeRoad.transform.localRotation = Quaternion.Euler(0, -90, 0);
            else if ((_right && _left) || (_right && _rightO)) activeRoad.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (trueCount == 3)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(true);
            _Oroad4.SetActive(false);
            _OroadC.SetActive(false);
            activeRoad = _Oroad3;
            if ((_left && _rightO && _leftO)) activeRoad.transform.localRotation = Quaternion.Euler(0, -90, 0);
            else if (_right && _leftO && _rightO) activeRoad.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else if (_left && _right && _leftO) activeRoad.transform.localRotation = Quaternion.Euler(0, 90, 0);
            else if ((_left && _right && _rightO)) activeRoad.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (trueCount == 4)
        {
            _Oroad.SetActive(false);
            _Oroad1.SetActive(false);
            _Oroad2.SetActive(false);
            _Oroad3.SetActive(false);
            _Oroad4.SetActive(true);
            _OroadC.SetActive(false);
            activeRoad = _Oroad4;
        }
    }
    void RayCastCheck()
    {
        _orgin = new Vector3(transform.position.x, transform.position.y * 1.5f, transform.position.z);
        if (Physics.Raycast(_orgin, transform.forward, out RaycastHit AhitInfo, 2))
        {
            if (AhitInfo.collider.gameObject.CompareTag("Road"))
            {                
                Debug.DrawRay(_orgin, transform.forward * 2.5f, Color.green);
                _left = true;
            }
            else
            {
                _left = false;
                //Debug.DrawRay(_orgin, transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _left = false;
            //Debug.DrawRay(_orgin, transform.forward * 10f, Color.red);
        }
        if (Physics.Raycast(_orgin, -transform.forward, out RaycastHit BhitInfo, 2))
        {
            if (BhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, -transform.forward * 2.5f, Color.green);
                _leftO = true;
            }
            else
            {
                _leftO = false;
                //Debug.DrawRay(_orgin, -transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _leftO = false;
            //Debug.DrawRay(_orgin, -transform.forward * 10f, Color.red);
        }
        if (Physics.Raycast(_orgin, transform.right, out RaycastHit ChitInfo, 2))
        {
            if (ChitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, transform.right * 2.5f, Color.green);
                _right = true;
            }
            else
            {
                _right = false;
                //Debug.DrawRay(_orgin, transform.right * 10f, Color.red);
            }
        }
        else
        {
            _right = false;
            //Debug.DrawRay(_orgin, transform.right * 10f, Color.red);
        }
        if (Physics.Raycast(_orgin, -transform.right, out RaycastHit DhitInfo, 2))
        {
            if (DhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.DrawRay(_orgin, -transform.right * 2.5f, Color.green);
                _rightO = true;
            }
            else
            {
                _rightO = false;
                //Debug.DrawRay(_orgin, -transform.right * 10f, Color.red);
            }
        }
        else
        {
            _rightO = false;
            //Debug.DrawRay(_orgin, -transform.right * 10f, Color.red);
        }
    }
}
