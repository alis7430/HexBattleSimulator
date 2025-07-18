using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleScene : BaseScene
{
    private HexGridManager _hexGridManager;
    private CinemachineVirtualCamera _camera;
    private List<BattleHexTile> _tileList = new();

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Battle;

        _hexGridManager = gameObject.AddMissingComponent<HexGridManager>();
        _camera = GameObject.FindWithTag("VirtualCamera")?.GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        // generate grid
        _hexGridManager.GeneratePointyTopGrid();
        var alltiles = _hexGridManager.GetAllTiles();
        foreach (var tile in alltiles)
        {
            var battleTile = tile as BattleHexTile;
            if (battleTile != null)
            {
                battleTile.SetTileType(BattleHexTile.TileType.Default);
                _tileList.Add(battleTile);
            }
        }

        // set Camera
        _camera.Follow = _hexGridManager.Center;
        _camera.LookAt = _hexGridManager.Center;

        // set selectable grid
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var tile = _hexGridManager.GetTile(i, j) as BattleHexTile;
                if (tile == null) continue;
                tile.SetTileType(BattleHexTile.TileType.Selectable);
            }
        }
    }

    public override void Clear()
    {

    }

    private void SetUnitDragable()
    {

    }
}
