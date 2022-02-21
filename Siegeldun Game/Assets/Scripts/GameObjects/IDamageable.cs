using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float attackID, int rcvKbDir, WeaponProperties rcvStatsProp);
}
