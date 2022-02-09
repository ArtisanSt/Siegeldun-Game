using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int attackID, int rcvKbDir, WeaponProperties rcvStatsProp);
}
