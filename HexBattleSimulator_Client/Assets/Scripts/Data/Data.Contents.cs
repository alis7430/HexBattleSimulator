using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region stat
[Serializable]
public class Stat
{
    public int Level;
    public int Hp;
    public int Attack;
}

[Serializable]
public class StatData : IDictable<int, Stat>
{
    public List<Stat> Stats = new List<Stat>();

    public Dictionary<int, Stat> MakeDict()
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
        foreach (Stat stat in Stats)
        {
            dict.Add(stat.Level, stat);
        }
        return dict;
    }
}
#endregion
