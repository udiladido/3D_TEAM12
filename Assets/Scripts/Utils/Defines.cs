public static class Defines
{
    /// <summary>
    /// Scene Type
    /// </summary>
    public enum SceneType
    {
        None,
        TitleScene,
        GameScene,
        DevScene,
        IntroScene,
    }

    /// <summary>
    /// UI EventType
    /// </summary>
    public enum UIEvent
    {
        None,
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        Drag,
        BeginDrag,
        EndDrag,
    }

    public enum UIAnimationType
    {
        None,
        Bounce,
    }

    public enum CalcType
    {
        Add,
        Multiply,
        Override,
    }

    /// <summary>
    /// 아이템 타입
    /// </summary>
    public enum ItemType
    {
        None,
        Equipment,
        Consumable,
        Resource,
        Gold,
    }

    /// <summary>
    /// 아이템 소모품 타입
    /// </summary>
    public enum ItemConsumableType
    {
        None,
        HpRecovery,
        MpRecovery,
        MoveSpeed,
        AttackSpeed,
        AttackDamage,
        MaxHp,
    }

    /// <summary>
    /// 아이템 장비 타입
    /// </summary>
    public enum ItemEquipmentType
    {
        None,
        Weapon,
        Armor,
        Accessory,
    }

    /// <summary>
    /// 아이템 희귀도 타입
    /// </summary>
    public enum ItemRarityType
    {
        None,
        Common,
        Rare,
        Epic,
        Unique,
    }

    /// <summary>
    /// 캐릭터 스탯 타입
    /// </summary>
    public enum CharacterStatType
    {
        None,
        Hp,
        Mp,
        AttackDamage,
        AttackSpeed,
        MoveSpeed,
        Armor,
        HpRegen,
        MpRegen,
        CooltimeReduction,
    }

    public enum CharacterMovementType
    {
        None,
        Forward,
        Backward,
        LeftStep,
        RightStep,
    }

    public enum CharacterAttackInputType
    {
        None,
        ComboAttack,
        Skill,
    }

    public enum ItemQuickSlotInputType
    {
        None,
        QuickSlot1,
        QuickSlot2,
    }

    public enum CharacterCombatStyleType
    {
        None, // Base Layer Index
        ComboAttack,
        MeleeSkill,
        RangedSkill,
        MagicSkill,
    }

    public enum UIStatusType
    {
        None,
        Hp,
        Mp,
    }
    
    public enum UIEquipmentType
    {
        None,
        ComboWeapon,
        SkillWeapon,
        Armor,
        Accessory,
    }

    public enum UIRewardItemType
    {
        None,
        ComboWeapon,
        SkillWeapon,
        Armor,
        Accessory,
        HpPotion,
        MpPotion,
        
        ItemCount,
    }

    
    public const string ITEM_RARIY_COMMON_COLOR = "808080";
    public const string ITEM_RARIY_RARE_COLOR = "0070dd";
    public const string ITEM_RARIY_EPIC_COLOR = "a335ee";
    public const string ITEM_RARIY_UNIQUE_COLOR = "ff8000";

    public const float MOVE_ANIMATION_SPEED_OFFSET = 3.5f;
    public const float ATTACK_ANIMATION_SPEED_OFFSET = 1f;
    
    public const float STAT_MAX_ATTACK_SPEED = 2.5f;
    public const float STAT_MAX_MOVE_SPEED = 10f;
    public const float STAT_MAX_ARMOR = 80f;
    public const float STAT_MAX_COOLTIME_REDUCE_RATE = 1.8f; // 최대 80% 까지 감소
    
    public const int REWARD_HP_POTION_ID = 1001;
    public const int REWARD_MP_POTION_ID = 1002;
    public const int DEFAULT_COMBO_WEAPON_ID = 1501;
}