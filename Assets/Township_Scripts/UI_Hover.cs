using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Panel")]
    [Tooltip("Select the panel for hover display")]
    public GameObject _hoverData;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverData.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _hoverData.SetActive(false);
    }
    void OnMouseEnter()
    {
        _hoverData.SetActive(true);
    }
    void OnMouseExit()
    {
        _hoverData.SetActive(false);
    }
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _hoverData.SetActive(false);
        }
    }
}
