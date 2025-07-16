using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// author: ur-dev
/// Custom debbuger for rendering
/// </summary>
public class RenderDebbuger : MonoBehaviour
{
    public enum DebugMode
    {
        None,
        Awake,
        Start,
        Update,
        Once,
    }
    
    [SerializeField] private DebugMode _debugMode = DebugMode.Start;
    [SerializeField] private bool _uniqueMaterial = true;

    void Start()
    {
        StartDebug(DebugMode.Start);
    }

    void Update()
    {
        StartDebug(DebugMode.Update);
    }

    void LateUpdate()
    {
        if (_debugMode == DebugMode.Once)
        {
            StartDebug(DebugMode.Once);
            _debugMode = DebugMode.None;
        }
    }

    private void StartDebug(DebugMode debugMode)
    {
        if (_debugMode != debugMode) return;
        if (_uniqueMaterial)
            CheckUniqueMaterialCount();
    }

    /// <summary>
    /// Check how many material instances are generated, excluding shared materials.
    /// </summary>
    private void CheckUniqueMaterialCount()
    {
        var renderers = FindObjectsOfType<MeshRenderer>();
        int instanceCount = 0;

        foreach (var r in renderers)
        {
            // material에 접근하지 않고, sharedMaterial의 name을 기반으로 체크
            // if (r.material != r.sharedMaterial) <- 이 경우 material이 프로퍼티여서 강제로 인스턴싱 함
            var mat = r.sharedMaterial;
            if (mat != null && mat.name.EndsWith("(Instance)"))
            {
                instanceCount++;
                Debug.Log($"{r.gameObject.name} has a unique material instance: {mat.name}");
            }
        }

        Debug.Log($"Total unique material instances: {instanceCount}");
    }
}
