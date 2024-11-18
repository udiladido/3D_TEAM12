using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Player player = (Player)target;
        if (GUILayout.Button("Fix Camera Facing"))
        {
            player.FixCameraFacing();
        }

        if (Application.isPlaying)
        {
            List<ItemEntity> items = Managers.DB.GetAll<ItemEntity>();
            items = items.Where(
                s => s.itemType == Defines.ItemType.Equipment && s.equipableEntity?.equipmentType == Defines.ItemEquipmentType.Weapon
            ).ToList();
            GUILayout.Space(20);
            GUILayout.Label("콤보 무기 장착");
            DrawComboWeaponButtons(items);
            
            GUILayout.Space(20);
            GUILayout.Label("그냥 무기 장착");
            DrawWeaponButtons(items);
        }
    }

    private void DrawComboWeaponButtons(List<ItemEntity> items)
    {
        foreach (var item in items)
        {
            if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.None) continue;
            if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.ComboAttack)
                DrawGenerateButton(item);
        }
    }

    private void DrawWeaponButtons(List<ItemEntity> items)
    {
        foreach (var item in items)
        {
            if (item.equipableEntity.combatStyleType == Defines.CharacterCombatStyleType.None) continue;
            if (item.equipableEntity.combatStyleType != Defines.CharacterCombatStyleType.ComboAttack)
                DrawGenerateButton(item);
        }
    }

    private void DrawGenerateButton(ItemEntity item)
    {
        if (GUILayout.Button($"{item.displayTitle} 장착"))
        {
            Player player = (Player)target;
            player.gameObject.name = nameof(Player);
            player.Equipment.Equip(item);
        }
    }
}