using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate HexTile Mesh
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMeshRenderer : MonoBehaviour
{
    private float radius = 1f;
    MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    public void GenerateHexMesh(float radius)
    {
        this.radius = radius;
        GenerateHexMesh();
    }

    private void GenerateHexMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[7];
        int[] triangles = new int[6 * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        }

        for (int i = 0; i < 6; i++)
        {
            triangles[i * 3 + 0] = 0;                   // center
            triangles[i * 3 + 2] = i + 1;               // current
            triangles[i * 3 + 1] = i == 5 ? 1 : i + 2;  // next

            // 기존 (노멀 아래쪽)
            // triangles[i * 3 + 0] = 0;
            // triangles[i * 3 + 1] = i + 1;
            // triangles[i * 3 + 2] = i == 5 ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

#if UNITY_EDITOR
        mesh.name = "HexMesh (Generated)";
        GetComponent<MeshFilter>().sharedMesh = mesh;
#else
        GetComponent<MeshFilter>().mesh = mesh;
#endif
    }

    public void SetColor(Color color)
    {
        if (_meshRenderer == null) return;
        if (_meshRenderer.sharedMaterial.HasProperty("_BaseColor"))
        {
            var block = new MaterialPropertyBlock();
            _meshRenderer.GetPropertyBlock(block);
            block.SetColor("_BaseColor", color);
            _meshRenderer.SetPropertyBlock(block);
        }
    }
}
