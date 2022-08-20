using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void Set_hp(int health)
    {
        slider.value = health;
    }

    public void Set_max_hp(int health)
    {
        slider.value = health;
        slider.maxValue = health;
    }

}
