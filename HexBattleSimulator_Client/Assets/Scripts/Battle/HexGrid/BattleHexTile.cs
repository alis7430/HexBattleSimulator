using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHexTile : HexTile
{
    public const float TILE_ALPHA = 0.3f;

    public enum TileType
    {
        Default,        // 기본 타일
        Selectable,     // 선택 가능한 상태
        Blocked,        // 이동 불가한 타일
    }

    public enum HighlightState
    {
        None,
        Current,
        Placeable,
        Blocked
    }

    [SerializeField]
    public TileType BattleTileType { get; private set; }

    public void SetTileType(TileType tileType = TileType.Default)
    {
        BattleTileType = tileType;
        SetTileByType(tileType);
    }

    private void SetTileByType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Default:
                _showOutline = false;
                SetTileColor(Color.gray, TILE_ALPHA);
                break;
            case TileType.Selectable:
                _showOutline = false;
                SetTileColor(Color.yellow, TILE_ALPHA);
                break;
            default:
                return;
        }

        _needUpdate = true;
    }
    
    public void SetHighlight(HighlightState state)
    {
        switch (state)
        {
            case HighlightState.None:
                SetOutline(false);
                break;
            case HighlightState.Current:
                SetOutline(Color.blue);
                break;
            case HighlightState.Placeable:
                SetOutline(Color.green);
                break;
            case HighlightState.Blocked:
                SetOutline(Color.red);
                break;
        }
    }
}
