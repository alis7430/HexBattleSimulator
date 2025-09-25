using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected UnitStatsSO _unitStatsSO;
    // for manage buff, debuff
    [SerializeField] protected List<StatModifier> _runtimeMods = new List<StatModifier>();
    [SerializeField, Min(1)] private int _level = 1;

    public int Level => _level;

    public void SetLevel(int level)
    {
        if (_unitStatsSO == null)
        {
            Debug.LogError("UnitStatsSO is null! Cannot set level");
            return;
        }

        _level = Mathf.Clamp(level, 1, _unitStatsSO.maxLevel);
    }

    public float GetStat(StatType statType)
    {
        if (_unitStatsSO == null)
        {
            Debug.LogWarning("UnitStatsSO is null");
            return 0f;
        }

        float baseValue = _unitStatsSO.GetStatAtLevel(statType, _level);
        float flat = 0f;
        float pct = 0f;

        for (int i = 0; i < _runtimeMods.Count; i++)
        {
            var m = _runtimeMods[i];
            if (m.stat != statType) continue;

            switch (m.kind)
            {
                case StatModKind.Flat:
                    flat += m.value;
                    break;
                case StatModKind.PercentAdd:
                    pct += m.value;
                    break;
            }
        }
        return (baseValue + flat) * (1f + pct);
    }

    public Stats GetAllStats()
    {
        return new Stats
        (
            GetStat(StatType.HP),
            GetStat(StatType.MP),
            GetStat(StatType.Attack),
            GetStat(StatType.Defense)
        );
    }

    public void AddModifier(StatModifier stat)
    {
        _runtimeMods?.Add(stat);
    }

    public void ClearModifier()
    {
        _runtimeMods?.Clear();
    }

    [ContextMenu("Log Stats")]
    private void LogStats()
    {
        var s = GetAllStats();
        Debug.Log($"{name} Stats => HP:{s.HP}  MP:{s.MP}  ATK:{s.Attack}  DEF:{s.Defense}");
    }
}
