using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IStatusBar
{
    bool isAlive { get; }
    float curHp { get; }
    float maxHp { get; }
    float[] maxHpScaling { get; }

    GameObject EntityStatusBar { get; }

    bool hasHp { get; }
    float[] hpRegenScaling { get; }
    Image hpbarF { get; }
    Image hpBarB { get; }
    Text hpText { get; }
    bool isInvulnerable { get; }
    bool doHpRegen { get; }
    float hpRegen { get; }
    float hpRegenDelay { get; }
    float _hpTick { get; }
    float _hpHideTime { get; }

    bool hasStam { get; }
    float[] stamRegenScaling { get; }
    Image stamBar { get; }
    Text stamText { get; }
    bool isInfStamina { get; }
    bool doStamRegen { get; }
    float stamRegen { get; }
    float stamRegenDelay { get; }
    float _stamTick { get; }
    float _stamHideTime { get; }

    void HpBarUIUpdate();
    void StamBarUIUpdate();

    void PassiveSkills();
    void RegenerateHp();
    void RegenerateStam();

    IEnumerator HealOvertime();
    IEnumerator HealInstant();
}
