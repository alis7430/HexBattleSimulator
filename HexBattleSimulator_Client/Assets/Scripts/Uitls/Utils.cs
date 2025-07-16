using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform t = FindChild<Transform>(go, name, recursive);

        return t == null ? null : t.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform t = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || t.name == name)
                {
                    T component = t.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindOrCreate(string name)
    {
        GameObject go = GameObject.Find(name);
        if (go == null)
            go = new GameObject { name = name };

        return go;
    }
}
