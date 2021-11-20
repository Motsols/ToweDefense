using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestWaves : MonoBehaviour
{
    private WaveSpawner _waveSpawner;

    private void Awake()
    {
        _waveSpawner = GameObject.Find("Wave Spawner").GetComponent<WaveSpawner>();
    }

    private void OnDestroy()
    {
        int enemiesLeft = 0;
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesLeft == 0)
            _waveSpawner.RestartCounter();
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 previousPosition = transform.position;
        Debug.Log(deltaTime);
        float newX = transform.position.x - 1 * deltaTime * 15;
        
        if (newX < 0) {

            Destroy(gameObject, 0);
            var newScore = int.Parse(GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text) +1;
            GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = newScore.ToString();
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
