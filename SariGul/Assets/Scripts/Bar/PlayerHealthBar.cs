using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;


    public void setMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        healthText.SetText(maxHealth + "/" + maxHealth);
    }
    public void setHealth(int health, int maxHealth)
    {
        healthSlider.value = health;
        healthText.SetText(health + "/" + maxHealth);
    }
}
