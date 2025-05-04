using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    bool _check = false;
    bool _default = true;
    Animator _ani;
    
    void Start()
    {
        _ani = GetComponent<Animator>();
    }
    void Update()
    {
        _ani.SetBool("Check", _check);
        _ani.SetBool("Default", _default);
    }
    public void Check()
    {
        _default = false;
        _check = _check ? false : true;
    }
    public void Default()
    {
        _default = true;
    }
}
