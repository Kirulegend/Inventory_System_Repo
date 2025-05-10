using UnityEngine;

public class Store : MonoBehaviour
{
    public GameObject _build;
    public GameObject _unit;
    public GameObject _farm;
    public GameObject _decor;

    public void Build()
    {
        _build.SetActive(true);
    }
    public void Unit()
    {
        _unit.SetActive(true);
    }

    public void Farm()
    {
        _farm.SetActive(true);
    }
    public void Decor()
    {
        _decor.SetActive(true);
    }
}
