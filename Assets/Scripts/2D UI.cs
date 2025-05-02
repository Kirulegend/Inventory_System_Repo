using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    bool _check = false;
    bool _default = true;
    Animator _ani;
    public TextMeshProUGUI _road;
    public TextMeshProUGUI _build;
    
    void Start()
    {
        _ani = GetComponent<Animator>();
    }
    void Update()
    {
        _ani.SetBool("Check", _check);
        _ani.SetBool("Default", _default);
        _road.text = "ROADS : " + Placement._roadCount.ToString();
        _build.text = "BUILD : " + Placement._buildCount.ToString();
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
