using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Description : MonoBehaviour
{
    [SerializeField] TextMeshPro _dec;

    void Start()
    {
        _dec = GetComponent<TextMeshPro>();
        _dec.enabled = false;
    }
    public void OnMouseEnter()
    {
        _dec.enabled = true;
    }
    public void OnMouseExit()
    {
        _dec.enabled = false;
    }
}
