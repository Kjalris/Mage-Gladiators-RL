using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemy_prefab;

    public Transform spawn_point_1;
    public Transform spawn_point_2;
    public Transform spawn_point_3;
    public Transform spawn_point_4;

    public TextMeshProUGUI stage_counter_text;

    public float time_between_waves = 15f;
    private float countdown = 2f;

    private int stage_index = 0;

    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = time_between_waves;
        }

        countdown -= Time.deltaTime;

        stage_counter_text.SetText("STAGE: " + Mathf.Floor(stage_index).ToString());
    }

    IEnumerator SpawnWave ()
    {
        stage_index++;

        for (int i = 0; i < stage_index; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.2f);
        }


    }

    void SpawnEnemy()
    {
        Instantiate(enemy_prefab, spawn_point_1.position, spawn_point_1.rotation);
        Instantiate(enemy_prefab, spawn_point_2.position, spawn_point_2.rotation);
        Instantiate(enemy_prefab, spawn_point_3.position, spawn_point_3.rotation);
        Instantiate(enemy_prefab, spawn_point_4.position, spawn_point_4.rotation);
    }


}
