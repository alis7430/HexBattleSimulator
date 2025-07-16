using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


/// <summary>
/// author: ur-dev
/// A generic singleton class for unity monobehaviour
/// Inherit from this base class to create a singleton.
/// 
/// - Automatically creates object in scene if not exist.
/// - Find existing instance first, so it can't create object when scene was changed
/// - It has thread lock
/// 
/// e.g. public class MyClassName : MonoBehaviourSingleton<MyClassName>
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region[Fields]
    private static T _instance;
    private static bool _shuttingDown = false;
    private static object _instanceLock = new object();

    [SerializeField]
    private bool _DontDestoryOnLoad = true;
    #endregion

    #region [Properties]
    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning("A singleton instance '" + typeof(T) + "' already destroyed. It returns null.");
                return null;
            }

            lock (_instanceLock)
            {
                // at first, find existing instance
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    var go = new GameObject();
                    _instance = go.AddComponent<T>();
                    go.name = "[Singleton]" + typeof(T).Name;
                }

                return _instance;
            }
        }
    }
    #endregion

    #region [Methods]
    private void Awake()
    {
        // make persistent intance
        if (_DontDestoryOnLoad)
            DontDestroyOnLoad(this.gameObject);

        OnInit();
    }

    private void OnApplicationQuit() => _shuttingDown = true;
    private void OnDestroy() => _shuttingDown = true;
    protected virtual void OnInit() { }
    #endregion
}
