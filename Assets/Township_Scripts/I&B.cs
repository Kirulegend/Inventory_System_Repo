using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class IB : MonoBehaviour
{
    public static bool[] _buildB = new bool[10];
    public Button[] _build; 

    private void Awake()
    {
        //GameManagerTS.OnDirectiveChanged += CheckDirective;
        //for(int i = 0; i < _buildB.Length; i++)
        //{
        //    _buildB[i] = false;
        //}
        //for (int i = 0; i < _build.Length; i++)
        //{
        //    _build[i].interactable = false ;
        //}
    }
    void CheckDirective(Directive Dir)
    {
        switch (GameManagerTS._currentDirective)
        {
            case Directive._directive1:
                _build[0].interactable = true;
                break;
            case Directive._directive2:
                _build[0].interactable = true;
                break;
        }
    }
}
