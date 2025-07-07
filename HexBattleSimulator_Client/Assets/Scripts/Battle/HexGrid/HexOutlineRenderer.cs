using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class HexOutlineRenderer : MonoBehaviour
{
    private bool showOutline = true;
    private float radius = 1f;
    private float lineWidth = 0.05f;
    private Color lineColor = Color.black;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        SetupLineRenderer();
        UpdateOutline();
    }

    public void SetOutline(float radius, float lineWidth = 0.05f, bool bShow = true)
    {
        SetOutline(radius, lineWidth, Color.black, bShow);
    }

    public void SetOutline(float radius, float lineWidth, Color color, bool bShow = true)
    {
        this.showOutline = bShow;
        this.radius = radius;
        this.lineWidth = lineWidth;
        this.lineColor = color;

        SetupLineRenderer();
        UpdateOutline();
    }

    private void SetupLineRenderer()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.loop = true;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void UpdateOutline()
    {
        if (!showOutline)
        {
            _lineRenderer.enabled = false;
            return;
        }

        Vector3[] points = new Vector3[7];
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            points[i] = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        }
        points[6] = points[0];

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
        _lineRenderer.startWidth = _lineRenderer.endWidth = lineWidth;
        _lineRenderer.startColor = _lineRenderer.endColor = lineColor;
    }
}