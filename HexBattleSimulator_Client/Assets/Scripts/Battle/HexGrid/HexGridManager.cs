using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 맵의 기하 구조(생성, 좌표 변환, 이웃 타일, 범위 계산)
/// flat-top (평평한 변이 위/아래)
/// pointy-top (뾰족한 꼭짓점이 위/아래)타일 생성/배치
/// </summary>
public class HexGridManager : MonoBehaviour
{
    public enum HexOrientation
    {
        PointyTop,
        FlatTop,
    }
    public Transform Center { get; private set; }
    public Transform Root { get; private set; }

    public int width = 6;
    public int height = 6;
    public float tileSize = 1f;

    private HexOrientation currOrientation = HexOrientation.PointyTop;
    private Dictionary<HexCoord, HexTile> _tileMap = new();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // set root
        if (Root == null)
        {
            Root = Utils.FindOrCreate("Grid_Root").transform;
            Root.position = Vector3.zero;
        }
    }

    private void SetCenterAnchor()
    {
        // set center Anchor
        if (Center == null)
        {
            Center = Utils.FindOrCreate("Grid_Center").transform;
        }

        float centerQ = (width - 1) / 2f;
        float centerR = (height - 1) / 2f;

        Vector3 centerPos = (currOrientation == HexOrientation.FlatTop)
                    ? HexGridCalculator.HexToWorld_FlatTop(tileSize, centerQ, centerR)
                    : HexGridCalculator.HexToWorld_PointyTop(tileSize, centerQ, centerR);
        Center.transform.position = centerPos;
    }

    public void SetData(int width, int height)
    {
        if (width < 0 || height < 0)
        {
            Debug.Log("Invaild Data!");
            return;
        }

        this.width = width;
        this.height = height;
    }

    public void GenerateHexGrid(HexOrientation orientation)
    {
        currOrientation = orientation;
        SetCenterAnchor();

        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Vector3 pos = (orientation == HexOrientation.FlatTop)
                    ? HexGridCalculator.HexToWorld_FlatTop(tileSize, q, r)
                    : HexGridCalculator.HexToWorld_PointyTop(tileSize, q, r);

                var tile = Managers.Resource.Instantiate("Battle/HexTile", Root)?.GetComponent<HexTile>();

                if (tile == null)
                {
                    Debug.Log("HexTile is null!");
                    continue;
                }

                var coord = new HexCoord(q, r);
                _tileMap[coord] = tile;
                tile.Coord = coord;

                tile.SetTileSize(tileSize);
                tile.transform.position = pos;

                //Pointy-Top은 30도 회전
                tile.transform.rotation = Quaternion.Euler(0f, orientation == HexOrientation.PointyTop ? 30f : 0f, 0f);

                string namePrefix = (orientation == HexOrientation.FlatTop) ? "FlatTile" : "Tile";
                tile.name = $"{namePrefix}_{q}_{r}";
            }
        }

        Debug.Log($"{orientation} Hex Grid ({width},{height}) is Generated");
    }

    public void GenerateFlatTopGrid()
    {
        GenerateHexGrid(HexOrientation.FlatTop);
    }

    public void GeneratePointyTopGrid()
    {
        GenerateHexGrid(HexOrientation.PointyTop);
    }

    public HexTile GetTile(HexCoord coord)
    {
        if (_tileMap.TryGetValue(coord, out HexTile tile))
            return tile;

        return null;
    }

    public HexTile GetTile(int q, int r)
    {
        HexCoord coord = new HexCoord(q, r);
        return GetTile(coord);
    }

    public List<HexTile> GetAllTiles()
    {
        return _tileMap?.Values.ToList() ?? null;
    }

    public List<HexTile> GetNeighbors(HexTile tile)
    {
        var coord = tile.Coord;
        var neighbors = new List<HexTile>();
        var directions = new List<HexCoord>()
        {
            new HexCoord(1, 0), new HexCoord(1, -1), new HexCoord(0, -1),
            new HexCoord(-1, 0), new HexCoord(-1, 1), new HexCoord(0, 1)
        };

        foreach (var dir in directions)
        {
            var findCoord = coord + dir;
            if (_tileMap.TryGetValue(findCoord, out var ret))
            {
                neighbors.Add(ret);
            }
        }
        return neighbors;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_tileMap == null || _tileMap.Count == 0)
            return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 16;
        style.alignment = TextAnchor.MiddleCenter;

        foreach (var pair in _tileMap)
        {
            if (pair.Value.gameObject.activeSelf == false)
                continue;

            Vector3 worldPos = pair.Value.transform.position + Vector3.up * 0.1f;
            string label = $"{pair.Key.q},{pair.Key.r}";
            Handles.Label(worldPos, label, style);
        }
    }
#endif
}
