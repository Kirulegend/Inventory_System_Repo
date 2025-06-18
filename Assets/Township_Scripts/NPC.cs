using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //public Vector3 _pos;
    //public Vector3 _taragt;
    public GameObject _npc;
    public GameObject[] _points; 
    public int _randomNum;
    public int _randomPlace;
    private void Awake()
    {
        _npc = GetComponentInParent<Road>()._npc;
    }
    void Update()
    {
        //_pos = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
        //Debug.DrawRay(_pos, transform.forward * 2, Color.red);
        //if (Physics.Raycast(_pos, transform.forward, out RaycastHit _hit))
        //{
        //    if (_hit.collider.gameObject.CompareTag("NPCSpawner"))
        //    {
        //        _taragt = _hit.collider.transform.position;
        //    }
        //}
        //if(transform.position != _taragt) transform.position = Vector3.MoveTowards(transform.position, _taragt, Time.deltaTime * 2);
    }
    void OnEnable()
    {
        _randomNum = Random.Range(0, 11);
        _randomPlace = Random.Range(0, _points.Length);
        if (_npc)
        {
            if (_randomNum == 6 || _randomNum == 9)
            {
                StartCoroutine(EnableWithDelay());
            }
            else
            {
                _npc.SetActive(false);
            }
        }
    }
    void OnDisable()
    {
        if (_npc) _npc.SetActive(false);
    }
    IEnumerator EnableWithDelay()
    {
        yield return new WaitForSeconds(.1f);

        _npc.SetActive(true);
        _npc.transform.position = _points[_randomPlace].transform.position;
        _npc.transform.Rotate(0, 90 * _randomPlace, 0);
        
    }
}
