using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexGridCalculator
{
    public static Vector3 HexToWorld_FlatTop(float tileSize, int q, int r)
    {
        float x = tileSize * 1.5f * q;
        float z = tileSize * Mathf.Sqrt(3f) * (r + q / 2f);
        return new Vector3(x, 0, z);
    }

    public static Vector3 HexToWorld_FlatTop(float tileSize, float q, float r)
    {
        float x = tileSize * 1.5f * q;
        float z = tileSize * Mathf.Sqrt(3f) * (r + q / 2f);
        return new Vector3(x, 0, z);
    }

    public static Vector3 HexToWorld_PointyTop(float tileSize,  int q, int r)
    {
        float x = tileSize * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = tileSize * 1.5f * r;
        return new Vector3(x, 0, z);
    }

    public static Vector3 HexToWorld_PointyTop(float tileSize,  float q, float r)
    {
        float x = tileSize * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = tileSize * 1.5f * r;
        return new Vector3(x, 0, z);
    }
}
