using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxStam(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;

    }
    public void SetStam(float stamina)
    {
        slider.value = stamina;
    }
}
