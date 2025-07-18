using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타일과 유닛의 1:1 관계를 전담 관리
/// 타일 위 상태(어느 타일에 어느 유닛이 있는지)
/// </summary>
public class BoardStateManager : MonoBehaviour
{
    private Dictionary<BattleHexTile, BattleUnit> _tileToUnit = new Dictionary<BattleHexTile, BattleUnit>();

    public BattleUnit GetUnitOnTile(BattleHexTile tile)
    {
        if (tile == null)
            return null;
        _tileToUnit.TryGetValue(tile, out var unit);
        return unit;
    }

    public BattleHexTile GetTileOfUnit(BattleUnit unit)
    {
        if (unit == null) return null;
        foreach (var kvp in _tileToUnit)
        {
            if (kvp.Value == unit)
                return kvp.Key;
        }
        return null;
    }

    /// <summary>
    /// 유닛을 타일로 바로이동 (스왑 포함)
    /// </summary>
    public void PlaceUnit(BattleUnit unit, BattleHexTile newTile)
    {
        if (unit == null || newTile == null)
            return;

        BattleHexTile oldTile = GetTileOfUnit(unit);

        // 1. 기존 타일에 있는 다른 유닛 스왑
        if (_tileToUnit.TryGetValue(newTile, out var otherUnit))
        {
            if (oldTile != null)
            {
                _tileToUnit[oldTile] = otherUnit;
                otherUnit.transform.position = oldTile.transform.position;
            }
            else
            {
                // oldTile이 null이면 그냥 제거
                _tileToUnit.Remove(newTile);
            }
        }
        else
        {
            // 새 타일에 유닛 없으면
            if (oldTile != null)
                _tileToUnit.Remove(oldTile);
        }

        // 2. 새 타일에 배치
        _tileToUnit[newTile] = unit;
        unit.transform.position = newTile.transform.position;
    }

    /// <summary>
    /// 유닛을 타일에서 제거
    /// </summary>
    public void RemoveUnit(BattleUnit unit)
    {
        var tile = GetTileOfUnit(unit);
        if (tile != null)
            _tileToUnit.Remove(tile);
    }

    public bool IsWalkable(HexTile tile)
    {
        var t = tile as BattleHexTile;
        if (t == null) return false;

        return IsWalkable(t);
    }

    public bool IsWalkable(BattleHexTile tile)
    {
        if (tile.BattleTileType == BattleHexTile.TileType.Blocked)
            return false;

        if (_tileToUnit.TryGetValue(tile, out var ret))
        {
            return ret != null;
        }
        return true;
    }

    /// <summary>
    /// 현재 상태 디버그용 출력
    /// </summary>
    public void PrintState()
    {
        foreach (var kvp in _tileToUnit)
        {
            Debug.Log($"{kvp.Value.gameObject.name} on {kvp.Key.name}");
        }
    }
}
