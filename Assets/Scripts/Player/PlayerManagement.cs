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
        health_bar.SetMaxHP(max_hp);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(1);
        }

        if (hp_display != null)
        {
            hp_display.SetText(current_hp + " / " + max_hp);
        }
    }

    public void TakeDamage(int damage)
    {

        current_hp -= damage;

        health_bar.SetHP(current_hp);

    }

    public int GetCurrentHP()
    {
        return current_hp;
    }

}
