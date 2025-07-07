using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  HexTile 관련 기능은 모두 여기서 참조.
/// </summary>
[RequireComponent(typeof(HexMeshRenderer))]
public class HexTile : MonoBehaviour
{
    public enum TileType
    {
        Default,
        Selectable,
    }

    [Header("Tile Settings")]
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private TileType _tileType;

    [Header("Outline Settings")]
    [SerializeField] private bool _showOutline = true;
    [SerializeField] private float _outlineWidth = 0.05f;
    [SerializeField] private Color _outlineColor = Color.black;


    HexMeshRenderer _hexMesh;
    HexOutlineRenderer _hexOutline;

    bool _needUpdate = false;

    void Awake()
    {
        _hexMesh = gameObject.GetComponent<HexMeshRenderer>();
        _hexOutline = transform.GetComponentInChildren<HexOutlineRenderer>();
    }

    public void SetTile(float radius, TileType tileType = TileType.Default)
    {
        this._radius = radius;
        SetTileType(tileType);
        _needUpdate = true;
    }

    private void LateUpdate()
    {
        if (_needUpdate)
        {
            GenerateHexMesh();
            UpdateOutline();
            _needUpdate = false;
        }
    }

    private void SetTileType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Default:
                _showOutline = false;
                break;
            case TileType.Selectable:
                _showOutline = true;
                _outlineColor = Color.black;
                _outlineColor.a = 0.5f;
                break;
            default:
                return;
        }
    }

    public void GenerateHexMesh()
    {
        if (_hexMesh == null)
            _hexMesh = gameObject.AddMissingComponent<HexMeshRenderer>();
        _hexMesh.GenerateHexMesh(_radius);
    }

    public void UpdateOutline()
    {
        if (_hexOutline == null)
            _hexOutline = transform.GetComponentInChildren<HexOutlineRenderer>();
        _hexOutline?.SetOutline(_radius, _outlineWidth, _outlineColor, _showOutline);
    }

#if UNITY_EDITOR
    // private void OnValidate()
    // {
    //     GenerateHexMesh();
        
    //     if (_hexOutline == null)
    //         _hexOutline = transform.GetComponentInChildren<HexOutlineRenderer>();

    //     UpdateOutline();
    // }
#endif
}
