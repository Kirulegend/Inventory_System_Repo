using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject _hoverData;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverData.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
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
