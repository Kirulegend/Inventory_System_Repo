using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAni : MonoBehaviour
{
    bool _check = false;
    bool _default = true;
    Animator _ani;
    public Button _back;
    
    void Start()
    {
        _ani = GetComponent<Animator>();
    }
    void Update()
    {
        _ani.SetBool("Check", _check);
        _ani.SetBool("Default", _default);
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && _check)
        {
            Check();
        }
    }
    public void Check()
    {
        _default = false;
        _check = _check ? false : true;
    }
    public void Default()
    {
        _default = true;
        _back.onClick.Invoke();
    }
}
