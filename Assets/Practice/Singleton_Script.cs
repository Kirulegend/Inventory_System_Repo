using UnityEngine;

public class Singleton_Script<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if(_instance == null)
                {
                    GameObject GO = new GameObject(typeof(T).Name);
                    _instance = GO.AddComponent<T>();
                }
            }
            Debug.Log($"Obj Created with Name : {_instance.name}");
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
