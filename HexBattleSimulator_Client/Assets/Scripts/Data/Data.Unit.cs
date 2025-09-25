using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region stat
public enum StatType { HP, MP, Attack, Defense }

[System.Serializable]
public struct Stats
{
    public float HP;
    public float MP;
    public float Attack;
    public float Defense;

    public Stats(float hp, float mp, float atk, float def)
    {
        HP = hp;
        MP = mp;
        Attack = atk;
        Defense = def;
    }
}
#endregion

#region stat mod
// 버프 등으로 인한 스탯 수정치 관리용
public enum StatModKind
{
    Flat,        // +A
    PercentAdd,  // +B%
    PercentMult  // xC
}

[System.Serializable]
public struct StatModifier
{
    public StatType stat;
    public StatModKind kind;
    public float value; // Flat: +10, PercentAdd: 0.2f(=20%), PercentMult

    public StatModifier(StatType stat, StatModKind kind, float value)
    {
        this.stat = stat;
        this.kind = kind;
        this.value = value;
    }
}
#endregion