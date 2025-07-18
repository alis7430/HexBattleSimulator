using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleUnit을 드래그 가능하게 함
/// 다른 타일로 유닛을 옮길 수 있음
/// </summary>
public class UnitDragHandler : MonoBehaviour
{
    private Camera _mainCamera;  // for raycasting
    private BoardStateManager _board;

    private BattleUnit _selectedUnit;
    private BattleHexTile _originTile;
    private BattleHexTile _candidateTile;
    private Vector3 _originalPosition;
    private Vector3 _dragOffset;

    private void Start()
    {
        _board = FindObjectOfType<BoardStateManager>();

        if (_board == null)
        {
            var scene = GameObject.Find("@BattleScene");
            if (scene != null)
            {
                _board = scene.AddMissingComponent<BoardStateManager>();
            }
        }

        Managers.Input.OnMouseEvent -= OnMouseEvent;
        Managers.Input.OnMouseEvent += OnMouseEvent;

        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        Managers.Input.OnMouseEvent -= OnMouseEvent;
    }

    private void OnMouseEvent(MouseEventArgs args)
    {
        switch (args.EventType)
        {
            case MouseEventType.Press:
                TrySelectUnit(args.ScreenPosition);
                break;

            case MouseEventType.DragStart:
                if (_selectedUnit != null)
                {
                    _dragOffset = _selectedUnit.transform.position - VectorUtils.ScreenToWorld(_mainCamera, args.ScreenPosition);
                }
                break;

            case MouseEventType.Drag:
                if (_selectedUnit != null)
                {
                    Vector3 world = VectorUtils.ScreenToWorld(_mainCamera, args.ScreenPosition) + _dragOffset;
                    _selectedUnit.transform.position = world;
                    FindPlaceableTile(args.ScreenPosition);
                }
                break;

            case MouseEventType.DragEnd:
                if (_selectedUnit != null)
                {
                    TryPlaceUnit(args.ScreenPosition);
                    _selectedUnit = null;
                }
                break;
        }
    }

    private void TrySelectUnit(Vector3 screenPos)
    {
        // 유닛 클릭 감지
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BattleUnit unit = hit.collider.GetComponent<BattleUnit>();
            if (unit != null)
            {
                _selectedUnit = unit;
                _originTile = _board.GetTileOfUnit(unit);
                _originalPosition = _selectedUnit.transform.position;
            }
        }
    }

    private void FindPlaceableTile(Vector3 screenPos)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        int tileLayerMask = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayerMask))
        {
            BattleHexTile tile = hit.collider.GetComponent<BattleHexTile>();
            if (tile != null)
            {
                if (_candidateTile != null)
                    _candidateTile.SetHighlight(BattleHexTile.HighlightState.None);

                // 상태를 아웃라인으로 보여줌
                if (tile == _originTile)
                    tile.SetHighlight(BattleHexTile.HighlightState.Current);
                else if (tile.BattleTileType == BattleHexTile.TileType.Selectable)
                    tile.SetHighlight(BattleHexTile.HighlightState.Placeable);
                else
                    tile.SetHighlight(BattleHexTile.HighlightState.Blocked);

                _candidateTile = tile;
            }
        }
    }

    private void TryPlaceUnit(Vector3 screenPos)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        int tileLayerMask = LayerMask.GetMask("Tile");
        bool isMoved = false;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayerMask))
        {
            BattleHexTile tile = hit.collider.GetComponent<BattleHexTile>();
            if (tile != null)
            {
                bool canPlace = (tile != _originTile) &&
                                (tile.BattleTileType == BattleHexTile.TileType.Selectable);
                if (canPlace)
                {
                    _board.PlaceUnit(_selectedUnit, tile);
                    isMoved = true;
                }
            }
        }

        // 실패하면 원래 타일로 돌아감
        if (isMoved == false)
        {
            _board.PlaceUnit(_selectedUnit, _originTile);
            if (_originTile == null)
                _selectedUnit.transform.position = _originalPosition;
        }

        if (_candidateTile != null)
        {
            _candidateTile.SetHighlight(BattleHexTile.HighlightState.None);
            _candidateTile = null;
        }
    }
}
