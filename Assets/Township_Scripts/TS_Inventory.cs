using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class TS_Inventory : MonoBehaviour
{
    void Start()
    {
        _canvas.enabled = false;
    }
    void Update()
    {
        AutoClose();
    }
    void OnMouseDown()
    {
        if (!_canvas.enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            _canvas.enabled = true;
            wasOpenedThisFrame = true;
        }
    }
    public Canvas _canvas;
    bool wasOpenedThisFrame;
    void AutoClose()
    {
        if (_canvas.enabled)
        {
            if (wasOpenedThisFrame)
            {
                wasOpenedThisFrame = false;
                return;
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _canvas.enabled = false;
            }
        }
    }
    public void ManualClose()
    {
        _canvas.enabled = false;
    }
    void Plus()
    {

    }
    void Minus()
    {

    }
}
