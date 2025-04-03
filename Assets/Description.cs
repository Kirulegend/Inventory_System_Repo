using TMPro;
using UnityEngine;

public class Description : MonoBehaviour
{
    public TextMeshProUGUI Dec;
    void Start()
    {
        Dec = GetComponent<TextMeshProUGUI>();
        Dec.enabled = false;
    }
    void OnMouseEnter()
    {
        Dec.enabled = true;
    }
    void OnMouseExit()
    {
        Dec.enabled = false;
    }
}
