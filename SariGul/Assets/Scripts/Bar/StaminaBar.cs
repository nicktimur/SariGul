using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public TextMeshProUGUI staminaText;

    public void setMaxStamina(float maxStamina)
    {
        staminaSlider.maxValue = (int)maxStamina;
        staminaSlider.value = (int)maxStamina;
        staminaText.SetText(maxStamina + "/" + maxStamina);
    }
    public void setStamina(float stamina, float maxStamina)
    {
        staminaSlider.value = (int)stamina;
        staminaText.SetText((int)stamina + "/" + (int)maxStamina);
    }
}
