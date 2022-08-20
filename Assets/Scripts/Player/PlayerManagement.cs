using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public int max_hp = 10;
    public int current_hp;

    public HealthBar health_bar;
    public TextMeshProUGUI hp_display;

    private void Start()
    {
        current_hp = max_hp;
        health_bar.Set_max_hp(max_hp);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Take_damage(1);
        }

        if (hp_display != null)
        {
            hp_display.SetText(current_hp + " / " + max_hp);
        }
    }

    private void Take_damage(int damage)
    {
        current_hp -= damage;

        health_bar.Set_hp(current_hp);
    }

    public int Get_current_hp()
    {
        return current_hp;
    }

}
