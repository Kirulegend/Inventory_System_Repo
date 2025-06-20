using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public Slider _xpSlider;
    public TextMeshProUGUI _qcText;
    public TextMeshProUGUI _levelText;
    public TextMeshProUGUI _populationText;
    public TextMeshProUGUI _road;
    public TextMeshProUGUI _build;
    public TextMeshProUGUI _podCrop;
    public TextMeshProUGUI _energyBar;
    public Toggle _mode;
    public Material _materialB;
    public Material _materialG;
    public Material _materialR;
    public Texture2D BlackB;
    public Texture2D WhiteB;
    public Texture2D BlackG;
    public Texture2D WhiteG;
    public Image _stack;
    void Awake()
    {
        _materialR = Resources.Load<Material>("Road");
        _materialB = Resources.Load<Material>("Base");
        _materialG = Resources.Load<Material>("Ground");
        Off();
    }
    void Update()
    {
        UI();
    }
    void UI()
    {
        if (GameData._instance == null) return;
        _road.text = "ROADS : " + GameData._instance._roadCount.ToString();
        _build.text = "BUILD : " + GameData._instance._buildCount.ToString();
        _podCrop.text = "A : " + GameData._instance._invI.Find(item => item.name == "NutriAlgae").quantity + " L : " + GameData._instance._invI.Find(item => item.name == "BioLuminary").quantity;
        _energyBar.text = "B : " + GameData._instance._invI.Find(item => item.name == "EnergyBar").quantity + " M : " + GameData._instance._invI.Find(item => item.name == "EnergyMeal").quantity;

        _qcText.text = GameData._instance._qc.ToString();

        _levelText.text = "LEVEL : " + GameData._instance._level.ToString();

        _xpSlider.maxValue = GameData._instance._level * 100;
        _xpSlider.value = GameData._instance._xp;
        _populationText.text = GameData._instance._population.ToString();
    }
    public void Mode()
    {
        if (_mode.isOn)
        {
            On();
        }
        else
        {
            Off();
        }
    }
    void OnApplicationQuit()
    {
        Off();
    }
    void On()
    {
        _stack.color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color COL) ? COL : Color.white;
        _populationText.color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color CO) ? CO : Color.white;
        _levelText.color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color C) ? C : Color.white;
        _qcText.color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color c) ? c : Color.white;
        _populationText.transform.parent.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color col) ? col : Color.white;
        _qcText.transform.parent.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color co) ? co : Color.white;
        _xpSlider.transform.Find("XP").GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#B9B9B9", out Color color) ? color : Color.white;
        _materialB.SetTexture("_BaseMap", BlackB);
        _materialG.SetTexture("_BaseMap", BlackG);
        _materialR.SetColor("_BaseColor", ColorUtility.TryParseHtmlString("#CACBCB", out Color colo) ? colo : Color.white);
    }
    void Off()
    {
        _stack.color = ColorUtility.TryParseHtmlString("#525252", out Color COL) ? COL : Color.white;
        _populationText.color = ColorUtility.TryParseHtmlString("#525252", out Color CO) ? CO : Color.white;
        _levelText.color = ColorUtility.TryParseHtmlString("#525252", out Color C) ? C : Color.white;
        _qcText.color = ColorUtility.TryParseHtmlString("#525252", out Color c) ? c : Color.white;
        _populationText.transform.parent.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#525252", out Color co) ? co : Color.white;
        _qcText.transform.parent.GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#525252", out Color col) ? col : Color.white;
        _xpSlider.transform.Find("XP").GetComponent<Image>().color = ColorUtility.TryParseHtmlString("#525252", out Color color) ? color : Color.white;
        _materialB.SetTexture("_BaseMap", WhiteB);
        _materialG.SetTexture("_BaseMap", WhiteG);
        _materialR.SetColor("_BaseColor", ColorUtility.TryParseHtmlString("#757A82", out Color colo) ? colo : Color.white);
    }
}
