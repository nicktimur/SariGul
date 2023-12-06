using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    public void setMaxStamina(float maxStamina)
    {
        staminaSlider.maxValue = (int)maxStamina;
        staminaSlider.value = (int)maxStamina;
    }
    public void setStamina(float stamina)
    {
        staminaSlider.value = (int)stamina; 
    }
}
