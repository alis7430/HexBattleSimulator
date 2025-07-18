using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleScene : BaseScene
{
    private HexGridManager _hexGridManager;
    private BoardStateManager _boardStateManager;
    private PathFinderModule _module;
    private CinemachineVirtualCamera _camera;
    private List<BattleHexTile> _tileList = new();

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Battle;

        _hexGridManager = gameObject.AddMissingComponent<HexGridManager>();
        _boardStateManager = gameObject.AddMissingComponent<BoardStateManager>();
        _module = new PathFinderModule(_hexGridManager, _boardStateManager);
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

#if UNITY_EDITOR
    private IEnumerator ShowTileNeighborTest()
    {
        foreach (var tile in _tileList)
        {
            var neighbors = _hexGridManager.GetNeighbors(tile);
            tile.SetOutline(Color.yellow);
            foreach (var y in neighbors)
            {
                y.SetTileColor(Color.red);
                yield return new WaitForSeconds(0.05f);
            }
            tile.SetOutline(Color.green);
            yield return new WaitForSeconds(0.2f);
            foreach (var x in neighbors)
            {
                x.SetTileColor(Color.white);
            }
        }
        yield break;
    }

    private IEnumerator PathFindingBFSTest(HexTile start, HexTile goal)
    {
        foreach (var tile in _tileList)
        {
            tile.SetTileColor(Color.gray);
            tile.SetOutline(Color.black);
        }

        Queue<HexTile> queue = new Queue<HexTile>();
        Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
        HashSet<HexTile> visited = new HashSet<HexTile>();

        queue.Enqueue(start);
        visited.Add(start);
        List<HexTile> path = null;
        while (queue.Count > 0)
        {
            HexTile current = queue.Dequeue();
            if (current == goal)
            {
                path = _module.ReconstructPath(cameFrom, current);
                break;
            }

            foreach (var neighbor in _hexGridManager.GetNeighbors(current))
            {
                if (!_boardStateManager.IsWalkable(neighbor) && neighbor != goal)
                    continue;

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    neighbor.SetTileColor(Color.yellow);
                    queue.Enqueue(neighbor);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        if (path != null)
        {
            foreach (var tile in path)
            {
                tile.SetTileColor(Color.green);
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
#endif
}
