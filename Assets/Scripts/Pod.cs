using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Pod : MonoBehaviour
{
    public static Pod Instance;
    public static bool Click = false;
    public bool Nutri_Algae = false;
    public bool Bio_Luminary = false;
    public string _activeCrop;
    Transform _pod;
    public bool Start = false;
    float Timer = 0;
    public bool _cropReady = false;

    public int _cropTimer = 5;

    void Awake()
    {
        _pod = transform.Find("Pod");
        Instance = this;
    }
    void Check()
    {
        if (Click)
        {
            if (Nutri_Algae)
            {
                _activeCrop = "Nutri-Algae";
                Start = true;
                Click = false;
                Nutri_Algae = false;
            }
            if (Bio_Luminary)
            {
                _activeCrop = "Bio_Luminary";
                _cropTimer *= 2;
                Start = true;
                Click = false;
                Bio_Luminary = false;
            }
        }
    }
    void Rot()
    {
        if (Start && Timer < _cropTimer)
        {
            Timer += Time.deltaTime;
            _pod.Rotate(0, 50 * Time.deltaTime, 0);
            if(Timer >= _cropTimer)
            {
                Start = false;
                Timer = 0;
                _cropReady = true;
                //_activeCrop = string.Empty;
            }
        }
    }
    void Update()
    {
        Check();
        Rot();
    }
}
