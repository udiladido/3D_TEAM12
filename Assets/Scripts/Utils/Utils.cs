using System;
using UnityEngine;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if (go.TryGetComponent(out T component) == false)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
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
                    return component;
            }
        }

        return null;
    }

    public static Color HexToColor(string hex)
    {
        // hex = "#FF00FFFF"
        if (!hex.StartsWith("#"))
            hex = $"#{hex}";

        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;

        return Color.clear;
    }

    public static Color GetItemRarityColor(Defines.ItemRarityType type)
    {
        return type switch
        {
            Defines.ItemRarityType.Common => HexToColor(Defines.ITEM_RARIY_COMMON_COLOR),
            Defines.ItemRarityType.Rare => HexToColor(Defines.ITEM_RARIY_RARE_COLOR),
            Defines.ItemRarityType.Epic => HexToColor(Defines.ITEM_RARIY_EPIC_COLOR),
            Defines.ItemRarityType.Unique => HexToColor(Defines.ITEM_RARIY_UNIQUE_COLOR),
            _ => Color.black,
        };
    }
    
    public static bool LayerMaskContains(LayerMask layerMask, int targetLayer)
    {
        return (layerMask & (1 << targetLayer)) != 0;
    }
    
    public static Func<float, float, float> Operation (Defines.CalcType calcType) {
        return calcType switch
        {
            Defines.CalcType.Add => (current, value) => current + value,
            Defines.CalcType.Multiply => (current, value) => current * value,
            _ => (current, value) => value
        };
    }
    
    public static string GetTimeString(float time)
    {
        int minute = (int)time / 60;
        int second = (int)time % 60;
        return $"{minute:D2}:{second:D2}";
    }
    
    public static string GetStatName(Defines.CharacterStatType statType)
    {
        return statType switch
        {
            Defines.CharacterStatType.Hp => "HP",
            Defines.CharacterStatType.Mp => "MP",
            Defines.CharacterStatType.AttackDamage => "공격력",
            Defines.CharacterStatType.AttackSpeed => "공격속도",
            Defines.CharacterStatType.MoveSpeed => "이동속도",
            Defines.CharacterStatType.Armor => "방어도",
            Defines.CharacterStatType.HpRegen => "HP회복",
            Defines.CharacterStatType.MpRegen => "MP회복",
            Defines.CharacterStatType.CooltimeReduction => "쿨타임감소",
            _ => "None"
        };
    }
}