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
        _hexGridManager.GeneratePointyTopGrid();

        // set Camera
        _camera.Follow = _hexGridManager.Center;
        _camera.LookAt = _hexGridManager.Center;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var tile = _hexGridManager.GetTile(i, j);
                if (tile == null) continue;
                tile.SetTileColor(Color.yellow, 0.5f);
            }
        }
    }

    public override void Clear()
    {

    }
}
