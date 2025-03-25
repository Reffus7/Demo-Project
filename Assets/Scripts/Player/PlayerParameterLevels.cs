using System;
using System.Collections.Generic;
using UnityEngine.Localization;

[Serializable]
public class PlayerParameterLevels {
    public Dictionary<PlayerParameterType, int> Levels = new Dictionary<PlayerParameterType, int>();

    public int GetLevel(PlayerParameterType type) {
        return Levels.TryGetValue(type, out int level) ? level : 0;
    }
}

public enum PlayerParameterType {
    moveSpeed,
    rotationSpeed,
    dodgeDistance,
    dodgeDuration,
    dodgeInvincibility,
    dodgeCooldown,
    attackRange,
    attackSpeed,
    attackDamage,
    maxHealth
}

public class PlayerParameterNames {
    public readonly static LocalizedString moveSpeedLocalizedString = new LocalizedString("General", "MoveSpeed");
    public readonly static LocalizedString rotationSpeedLocalizedString = new LocalizedString("General", "RotationSpeed");

    public readonly static LocalizedString dodgeDistanceLocalizedString = new LocalizedString("General", "DodgeDistance");
    public readonly static LocalizedString dodgeDurationLocalizedString = new LocalizedString("General", "DodgeDuration");
    public readonly static LocalizedString dodgeInvincibilityLocalizedString = new LocalizedString("General", "DodgeInvincibility");
    public readonly static LocalizedString dodgeCooldownLocalizedString = new LocalizedString("General", "DodgeCooldown");

    public readonly static LocalizedString attackRangeLocalizedString = new LocalizedString("General", "AttackRange");
    public readonly static LocalizedString attackSpeedLocalizedString = new LocalizedString("General", "AttackSpeed");
    public readonly static LocalizedString attackDamageLocalizedString = new LocalizedString("General", "AttackDamage");

    public readonly static LocalizedString maxHealthLocalizedString = new LocalizedString("General", "MaxHealth");

}