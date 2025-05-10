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
        _road.text = "ROADS : " + GameData._roadCount.ToString();
        _build.text = "BUILD : " + GameData._buildCount.ToString();
        _podCrop.text = "A : " + GameData._nutriAlgaeCrop.ToString() + " L : " + GameData._bioLuminaryCount.ToString();
        _energyBar.text = "BARS : " + GameData._energyBarCount.ToString();

        _qcText.text = GameData._qc.ToString();

        _levelText.text = "LEVEL : " + GameData._level.ToString();

        _xpSlider.maxValue = GameData._level;
        _xpSlider.value = GameData._xp / 10;
    }
}
