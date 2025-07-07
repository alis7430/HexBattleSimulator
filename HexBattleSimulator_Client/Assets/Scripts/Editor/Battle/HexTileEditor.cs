using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexTile))]
public class HexTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexTile hexTile = (HexTile)target;

        if (GUILayout.Button("Generate Hex Mesh"))
        {
            hexTile?.GenerateHexMesh();
            hexTile?.UpdateOutline();
        }
    }
}