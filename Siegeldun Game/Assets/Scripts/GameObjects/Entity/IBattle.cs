using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle 
{
    // Sending
    bool doAttack { get; }

    bool hasWeapon { get; }
    GameObject weaponGameobject { get; }
    WeaponProperties weaponProp { get; }
    WeaponProperties defaultPower { get; }

    float wpnDamage { get; }
    float wpnStamCost { get; }
    float wpnKbForce { get; }
    float wpnAtkRange { get; }
    float wpnAtkSpeed { get; }
    float wpnAtkDelay { get; }
    float wpnAtkCrit { get; }

    bool doAtkCombo { get; }
    int dcurAtkCombo { get; }
    float comboTime { get; }

    int kbFacing { get; }
    float kbForce { get; }

    int atkFacing { get; }

    // Receiving
    float KbDisplacement { get; }

    LayerMask enemyLayers { get; }
    Transform attackPoint { get; }

    void Attack();
    void TakeDamage(float damageTaken, int attackID, int kbDir, float knockbackedForce, bool isCrit = false);
    void Knockback(int kbDir, float kbHorDisplacement);
    void Die();
}
