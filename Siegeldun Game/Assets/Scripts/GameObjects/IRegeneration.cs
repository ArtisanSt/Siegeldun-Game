using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegeneration
{
    void Regenerate(string hp_stam, float healAmount, string healSpeed, float timeHeal = 0f, float timeSkip = 0.1f);
}
