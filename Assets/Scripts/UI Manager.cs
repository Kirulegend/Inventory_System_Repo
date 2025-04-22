using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _canvas;
    public GameObject _keyBinds;
    public GameObject _buttons;
    public GameObject _back;
    public void GameStart()
    {
        GameManager._isGame = true;
        _canvas.SetActive(false);
    }
    public void Keybinds()
    {
        _back.SetActive(true);
        _buttons.SetActive(false);
        _keyBinds.SetActive(true);
    }
    public void Back()
    {
        if (_buttons.activeInHierarchy)
        {
            //EditorApplication.isPlaying = false;
            Application.Quit();
        }
        else
        {
            _buttons.SetActive(true);
            _keyBinds.SetActive(false);
        }
    }
}
