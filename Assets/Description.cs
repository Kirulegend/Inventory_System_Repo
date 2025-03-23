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
    private void OnMouseEnter()
    {
        Dec.enabled = true;
    }
    private void OnMouseExit()
    {
        Dec.enabled = false;
    }
}
