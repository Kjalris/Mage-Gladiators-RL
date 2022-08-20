using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void SetHP(int health)
    {
        slider.value = health;
    }

    public void SetMaxHP(int health)
    {
        slider.value = health;
        slider.maxValue = health;
    }

}
