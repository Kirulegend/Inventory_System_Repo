using UnityEngine;

public class Invenory : MonoBehaviour
{
    public Canvas _canvas;
    void OnMouseDown()
    {
        _canvas.enabled = true;
    }
}
