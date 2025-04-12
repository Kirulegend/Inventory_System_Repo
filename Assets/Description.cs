using TMPro;
using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _dec;

    void Start()
    {
        _dec = GetComponent<TextMeshProUGUI>();
        _dec.enabled = false;
    }
    void OnMouseEnter()
    {
        _dec.enabled = true;
    }
    void OnMouseExit()
    {
        _dec.enabled = false;
    }
}
