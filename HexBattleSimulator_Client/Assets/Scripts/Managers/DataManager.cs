using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDictable<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    public void Init()
    {
        // Test
        // StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : IDictable<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        if (textAsset == null)
        {
            Debug.LogError("Test Asset is not exist.");
            return default(Loader);
        }
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
