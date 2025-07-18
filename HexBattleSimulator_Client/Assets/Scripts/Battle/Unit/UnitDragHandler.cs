using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleUnit을 드래그 가능하게 함
/// 다른 타일로 유닛을 옮길 수 있음
/// </summary>
public class UnitDragHandler : MonoBehaviour
{
    private Camera mainCamera;  // for raycasting

    private BattleUnit _selectedUnit;
    private HexTile _originTile;
    private Vector3 _originalPosition;
    private Vector3 _dragOffset;

    private void Start()
    {
        Managers.Input.OnMouseEvent -= OnMouseEvent;
        Managers.Input.OnMouseEvent += OnMouseEvent;

        if (mainCamera == null)
            mainCamera = Camera.main;
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
                    _dragOffset = _selectedUnit.transform.position - VectorUtils.ScreenToWorld(mainCamera, args.ScreenPosition);
                }
                break;

            case MouseEventType.Drag:
                if (_selectedUnit != null)
                {
                    Vector3 world = VectorUtils.ScreenToWorld(mainCamera, args.ScreenPosition) + _dragOffset;
                    _selectedUnit.transform.position = world;
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
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BattleUnit unit = hit.collider.GetComponent<BattleUnit>();
            if (unit != null)
            {
                _selectedUnit = unit;
                _originTile = unit.CurrentTile;
                _originalPosition = _selectedUnit.transform.position;

                // 기존 타일 점유 해제
                // TODO : 기존 유닛이 위치해 있으면 유닛 스왑 기능도 만들어야 함
                if (_originTile != null)
                    _originTile.CurrentUnit = null;
            }
        }
    }

    private void TryPlaceUnit(Vector3 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        int tileLayerMask = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayerMask))
        {
            HexTile tile = hit.collider.GetComponent<HexTile>();
            if (tile != null && !tile.HasUnit)
            {
                _selectedUnit.transform.position = tile.transform.position;
                tile.CurrentUnit = _selectedUnit.gameObject;
                _selectedUnit.CurrentTile = tile;
                return;
            }
        }

        // 실패하면 원래 타일로 돌아감
        _selectedUnit.transform.position = _originalPosition;
        if (_originTile != null)
        {
            _originTile.CurrentUnit = _selectedUnit.gameObject;
            _selectedUnit.CurrentTile = _originTile;
        }
    }
}
