using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on axial coordinates
public struct HexCoord
{
    public int q; // column
    public int r; // row

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public override string ToString()
    {
        return $"({q},{r})";
    }
}

public class HexGridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public float tileSize = 1f;

    public Transform Center { get; private set; }
    public Transform Root { get; private set; }

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

        // set center Anchor
        if (Center == null)
        {
            Center = Utils.FindOrCreate("Grid_Center").transform;

            float centerQ = (width - 1) / 2f; // 2.5
            float centerR = (height - 1) / 2f; // 2.5

            Vector3 centerPos = HexToWorld(centerQ, centerR);
            Center.transform.position = centerPos;
        }
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

    public void GenerateGrid()
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Vector3 pos = HexToWorld(q, r);
                GameObject tile = Managers.Resource.Instantiate("Battle/HexTile", Root);
                tile.transform.position = pos;
                tile.name = $"Tile_{q}_{r}";
            }
        }

        Debug.Log($"Hex Grid ({width},{height}) is Generated");
    }

    Vector3 HexToWorld(int q, int r)
    {
        float x = tileSize * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = tileSize * 1.5f * r;
        return new Vector3(x, 0, z);
    }

    Vector3 HexToWorld(float q, float r)
    {
        float x = tileSize * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = tileSize * 1.5f * r;
        return new Vector3(x, 0, z);
    }
}
