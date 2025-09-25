using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "HexBattle/UnitStats")]
public class UnitStatsSO : ScriptableObject
{
    [Header("기본 스탯")]
    public Stats baseStats = new Stats(100f, 50f, 10f, 5f);

    [Header("레벨당 증가량")]
    public Stats perLevelStats = new Stats(10f, 5f, 1f, 1f);

    [Header("최대 레벨"), Min(1)]
    public int maxLevel = 99;

    public float GetStatAtLevel(StatType statType, int level)
    {
        level = Mathf.Clamp(level, 1, maxLevel);
        int per = level - 1;

        switch (statType)
        {
            case StatType.HP: return baseStats.HP + perLevelStats.HP * per;
            case StatType.MP: return baseStats.MP + perLevelStats.MP * per;
            case StatType.Attack: return baseStats.Attack + perLevelStats.Attack * per;
            case StatType.Defense: return baseStats.Defense + perLevelStats.Defense * per;
        }
        return 0f;
    }

    public Stats GetAllStatsAtLevel(int level)
    {
        level = Mathf.Clamp(level, 1, maxLevel);
        int per = level - 1;

        return new Stats(
            baseStats.HP + perLevelStats.HP * per,
            baseStats.MP + perLevelStats.MP * per,
            baseStats.Attack + perLevelStats.Attack * per,
            baseStats.Defense + perLevelStats.Defense * per
        );
    }

    private void OnValidate()
    {
        baseStats.HP = Mathf.Max(0, baseStats.HP);
        baseStats.MP = Mathf.Max(0, baseStats.MP);
        baseStats.Attack = Mathf.Max(0, baseStats.Attack);
        baseStats.Defense = Mathf.Max(0, baseStats.Defense);
    }
}
