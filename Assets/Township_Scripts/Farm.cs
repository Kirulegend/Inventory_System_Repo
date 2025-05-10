using UnityEngine;

public class Farm : MonoBehaviour
{
    GameObject _buttonParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _buttonParent = GameObject.Find("Buttons");
        //Instantiate(_buttonData._data._button, _buttonParent.transform).name = "Kiran";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
