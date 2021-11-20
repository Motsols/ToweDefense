using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] _waves;

    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    const int countdown = 3;
    private float timeRemaining = countdown;
    private bool timerIsRunning = true;

    private void Start()
    {
        _enemiesLeftToSpawn = _waves[0].WaveSettings.Length;
        // StartCoroutine(SpawnEnemyInWave());
    }

    void Update() {
        
        var countDownComponent = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        var currentTimer = int.Parse(countDownComponent.text);

        if(timerIsRunning){
            if(timeRemaining > 0){
                timeRemaining -= Time.deltaTime;
            } else {
                timerIsRunning = false;
                LaunchWave();
            }
        }

        countDownComponent.text = ((int)timeRemaining).ToString();
    }

    private IEnumerator SpawnEnemyInWave()
    {        
        if(_enemiesLeftToSpawn > 0)
        {
            yield return new WaitForSeconds(_waves[_currentWaveIndex]
                .WaveSettings[_currentEnemyIndex]
                .SpawnDelay);
                Debug.Log($"current index: {_currentWaveIndex}");
            Instantiate(_waves[_currentWaveIndex]
                .WaveSettings[_currentEnemyIndex].Enemy, 
                _waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex]
                .NeededSpawner.transform.position, Quaternion.identity);
            _enemiesLeftToSpawn--;
            _currentEnemyIndex++;
            StartCoroutine(SpawnEnemyInWave());
        }
        else
        {
            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _enemiesLeftToSpawn = _waves[_currentWaveIndex].WaveSettings.Length;
                _currentEnemyIndex = 0;
            }
        }
    }

    public void RestartCounter(){
        timeRemaining = countdown;
        timerIsRunning = true;
    }

    public void LaunchWave()
    {
        StartCoroutine(SpawnEnemyInWave());
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] private WaveSettings[] _waveSettings;
    public WaveSettings[] WaveSettings { get => _waveSettings; }
}

[System.Serializable]
public class WaveSettings
{
    [SerializeField] private GameObject _enemy;
    public GameObject Enemy { get => _enemy; }
    [SerializeField] private GameObject _neededSpawner;
    public GameObject NeededSpawner { get => _neededSpawner; }
    [SerializeField] private float _spawnDelay;
    public float SpawnDelay { get => _spawnDelay; }
}
