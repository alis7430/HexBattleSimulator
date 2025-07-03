using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; }
    
    private Define.Scene _sceneType  = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    // call by Start
    protected virtual void Init()
    {
        // check EventSystem
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
