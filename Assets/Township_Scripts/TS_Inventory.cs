using UnityEngine;
using UnityEngine.EventSystems;

public class TS_Inventory : MonoBehaviour
{
    public Canvas _canvas;
    void OnMouseDown()
    {
        Debug.Log("Hello");
        _canvas.enabled = true;
    }
    void Close()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && _canvas.enabled)
        {
            Debug.Log("Hello");
            _canvas.enabled = false;
        }
    }
    private void Update()
    {
        Close();
    }
}
