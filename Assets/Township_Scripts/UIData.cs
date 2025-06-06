using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public Slider _xpSlider;
    public TextMeshProUGUI _qcText;
    public TextMeshProUGUI _levelText;
    public TextMeshProUGUI _road;
    public TextMeshProUGUI _build;
    public TextMeshProUGUI _podCrop;
    public TextMeshProUGUI _energyBar;

    void Update()
    {
        UI();
    }
    void UI()
    {
        if (GameData._instance == null) return;
        _road.text = "ROADS : " + GameData._instance._roadCount.ToString();
        _build.text = "BUILD : " + GameData._instance._buildCount.ToString();
        _podCrop.text = "A : " + GameData._instance._nutriAlgaeCrop.ToString() + " L : " + GameData._instance._bioLuminaryCount.ToString();
        _energyBar.text = "B : " + GameData._instance._energyBarCount + " M : " + GameData._instance._energyMealCount;

        _qcText.text = GameData._instance._qc.ToString();

        _levelText.text = "LEVEL : " + GameData._instance._level.ToString();

        _xpSlider.maxValue = GameData._instance._level * 100;
        _xpSlider.value = GameData._instance._xp;
    }
}
