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
        DevScene
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

    public enum CharacterCombatStyleType
    {
        None, // Base Layer Index
        ComboAttack,
        MeleeSkill,
        RangedSkill,
        MagicSkill,
    }

    
    public const string ITEM_RARIY_COMMON_COLOR = "808080";
    public const string ITEM_RARIY_RARE_COLOR = "0070dd";
    public const string ITEM_RARIY_EPIC_COLOR = "a335ee";
    public const string ITEM_RARIY_UNIQUE_COLOR = "ff8000";

    public const float MOVE_ANIMATION_SPEED_OFFSET = 3.5f;
    public const float ATTACK_ANIMATION_SPEED_OFFSET = 1f;
}