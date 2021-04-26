using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public GameObject enemyPrefab;
    public Camera camera;
    public int maxEnemy = 3;

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
                newEnemy.GetComponent<EnemyAI>().targetEnemy = PlayerController.Instance.spaceship;
                enemyList.Add(newEnemy.GetComponent<EnemyAI>().spaceship);
            }
        }
    }
}
