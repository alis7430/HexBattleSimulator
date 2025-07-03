using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviourSingleton<Managers>
{
    public static DataManager Data { get { return Instance._data;  }}
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEX Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }

    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEX _scene = new SceneManagerEX();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    protected override void OnInit()
    {
        base.OnInit();
        Sound.Init();
        Pool.Init();
        Data.Init();
    }

    void Update()
    {
        Input?.OnUpdate();
    }

    public static void Clear()
    {
        Input.Clear();
        Scene.Clear();
        Sound.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
