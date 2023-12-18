using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider haelthBarSlider;


    public void GiveFullHealth(float health)
    {
        haelthBarSlider.maxValue = health;
        haelthBarSlider.value = health;
    }

    public void SetHealth(float health)
    {
        haelthBarSlider.value = health;
    }
    
    
}
