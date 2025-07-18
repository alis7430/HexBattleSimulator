using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Battle,
    }

    public enum Sound
    {
        BGM,
        SFX,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum CameraMode
    {
        QuaterView
    }
}
