using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        // check pooling
        if (typeof(T) == typeof(GameObject))
        {
            GameObject go = FindPoolObject(path);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        var prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab: {path}");
            return null;
        }

        // check pooling
        if (prefab.GetComponent<PoolableObject>() != null)
            return Managers.Pool.Pop(prefab, parent).gameObject;


        var go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;  //remove "(Clone)" string

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        // check pooling
        PoolableObject poolable = go.GetComponent<PoolableObject>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    private GameObject FindPoolObject(string path)
    {
        string name = path;

        int index = name.LastIndexOf('/');
        if (index >= 0)
            name = name.Substring(index + 1);

        GameObject go = Managers.Pool.GetOriginal(name);
        return go;
    }
}
