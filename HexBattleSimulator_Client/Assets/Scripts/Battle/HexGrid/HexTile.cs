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
    public HexCoord Coord = HexCoord.zero;

    [Header("Tile Settings")]
    [SerializeField] protected float _radius = 0.5f;
    [SerializeField] protected Color _color = Color.black;

    [Range(0f, 1f)]
    [SerializeField] private float _alpha = 1f;

    [Header("Outline Settings")]
    [SerializeField] protected bool _showOutline = true;
    [SerializeField] protected float _outlineWidth = 0.05f;
    [SerializeField] protected Color _outlineColor = Color.black;

    protected HexMeshRenderer _hexMesh;
    protected HexOutlineRenderer _hexOutline;

    protected bool _needUpdate = false;

    protected virtual void Awake()
    {
        _hexMesh = gameObject.GetComponent<HexMeshRenderer>();
        _hexOutline = transform.GetComponentInChildren<HexOutlineRenderer>();
    }

    protected virtual void LateUpdate()
    {
        if (_needUpdate)
        {
            GenerateHexMesh();
            UpdateColor();
            UpdateOutline();
            _needUpdate = false;
        }
    }
    public void SetTileSize(float radius)
    {
        this._radius = radius;
        _needUpdate = true;
    }

    public void SetTileColor(Color color)
    {
        _color = color;
        _needUpdate = true;
    }

    public void SetTileColor(Color color, float alpha)
    {
        this._alpha = alpha;
        SetTileColor(color);
    }

    public void SetOutline(Color color, float width, bool bShow = true)
    {
        _outlineColor = color;
        _outlineWidth = width;
        _showOutline = bShow;
        _needUpdate = true;
    }

    public void SetOutline(Color color, bool bShow = true)
    {
        SetOutline(color, _outlineWidth, bShow);
    }
    public void SetOutline(bool bShow = true)
    {
        SetOutline(_outlineColor, _outlineWidth, bShow);
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

    public void UpdateColor()
    {
        if (_color == Color.black) return;
        
        _color.a = Mathf.Clamp(this._alpha, 0, 1);
        _hexMesh?.SetColor(_color);
    }
}
