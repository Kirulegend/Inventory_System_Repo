using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : Singleton_Script<Test>
{
    public int _id;
    public string _name;
    public string _description;

    protected override void Awake()
    {
        base.Awake();
        _id = 0;
        _name = string.Empty;
        _description = string.Empty;
    }

    public void TempMethod()
    {
        Debug.Log($"Method Called from {this.name}");
    }
}
