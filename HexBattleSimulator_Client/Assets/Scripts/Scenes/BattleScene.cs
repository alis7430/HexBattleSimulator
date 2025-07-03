using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleScene : BaseScene
{
    private HexGridManager _hexGridManager;
    private CinemachineVirtualCamera _camera;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Battle;

        _hexGridManager = gameObject.AddMissingComponent<HexGridManager>();
        _camera = GameObject.FindWithTag("VirtualCamera")?.GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _hexGridManager.GenerateGrid();
        _camera.Follow = _hexGridManager.Center;
    }

    public override void Clear()
    {

    }
}
