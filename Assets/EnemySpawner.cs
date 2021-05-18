using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public GameObject enemyPrefab;
    public Camera camera;
    public int maxEnemy = 3;

    public float powerDelay = 5;
    public float powerDuration = 6;
    public float increaseTheshold = 60;
    public float currentDelay;
    public List<Spaceship> enemyList = new List<Spaceship>();

    private float spawnDelayLeft;

    private float spawnDelayMin = 1;
    private float spawnDelayMax = 5;
    

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameplayCore.Instance.paused || GameplayCore.Instance.ended)
            return;

        if (currentDelay < increaseTheshold)
        {
            currentDelay += Time.deltaTime;
        }
        else
        {
            powerDelay -= 0.5f;
            powerDuration += 0.5f;

            if (powerDelay < 5)
            {
                powerDelay = 5f;
            }

            currentDelay = 0;
        }

        if (spawnDelayLeft > 0)
        {
            spawnDelayLeft -= Time.deltaTime;
        }
        else
        {
            if (enemyList.Count < maxEnemy)
            {
                var horzBound = (camera.orthographicSize * Screen.width / Screen.height) + 2f;
                var vertBound = camera.orthographicSize;
                var spawnPos = new Vector3(horzBound, Random.Range(-vertBound, vertBound), 0);
                var newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnDelayLeft = Random.Range(spawnDelayMin, spawnDelayMax);
                newEnemy.GetComponent<EnemyAI>().SetPowerDelay(powerDelay);
                newEnemy.GetComponent<EnemyAI>().powerCode = Random.Range(1,3);
                newEnemy.GetComponent<EnemyAI>().powerDurationMax = powerDuration;


                if (PlayerController.Instance)
                    newEnemy.GetComponent<EnemyAI>().targetEnemy = PlayerController.Instance.spaceship;
                    
                enemyList.Add(newEnemy.GetComponent<EnemyAI>().spaceship);
            }
        }
    }
}
