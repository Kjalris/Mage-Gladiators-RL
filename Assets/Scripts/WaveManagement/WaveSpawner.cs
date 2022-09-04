using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public static int enemies_alive = 0;

    public WaveAttributes[] waves;

    public Transform spawn_point_1;
    public Transform spawn_point_2;
    public Transform spawn_point_3;
    public Transform spawn_point_4;

    public TextMeshProUGUI stage_counter_text;

    public float time_between_waves = 5f;
    private float countdown = 2f;

    private int stage_index = 1;

    private void Start()
    {
        enemies_alive = 0;
    }

    private void Update()
    {
        if (enemies_alive > 0)
        {
            return;
        }

        if (countdown <= 0f)
        {
            // TODO: Make the upgrade system and ensure the time before next wave is either high or pause so the player can 
            // pick an upgrade without having to worry about time
            StartCoroutine(SpawnWave());
            countdown = time_between_waves;
            return;
        }

        countdown -= Time.deltaTime;

        stage_counter_text.SetText("STAGE: " + Mathf.Floor(stage_index).ToString());
    }

    IEnumerator SpawnWave ()
    {
        WaveAttributes wave = waves[stage_index];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.spawn_rate);
        }

        stage_index++;

        if (stage_index == waves.Length)
        {
            stage_counter_text.SetText("YOU WON!");
            this.enabled = false;
        }

    }

    void SpawnEnemy(GameObject enemy)
    {
        // for each spawnpoint, spawn an enemy and count the total number of enemies up
        // A bit of a code smell, should probably make a loop here instead when refactoring
        Instantiate(enemy, spawn_point_1.position, spawn_point_1.rotation);
        enemies_alive++;
        
        Instantiate(enemy, spawn_point_2.position, spawn_point_2.rotation);
        enemies_alive++;
        
        Instantiate(enemy, spawn_point_3.position, spawn_point_3.rotation);
        enemies_alive++;

        Instantiate(enemy, spawn_point_4.position, spawn_point_4.rotation);
        enemies_alive++;
        
    }
}
