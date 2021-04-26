using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    public GameObject debrisPrefab;
    public Camera camera;

    private float spawnDelayLeft;

    private float spawnDelayMin = 1;
    private float spawnDelayMax = 5;

    void Update()
    {
        if (GameplayCore.Instance.paused || GameplayCore.Instance.ended)
            return;

        if (spawnDelayLeft > 0)
        {
            spawnDelayLeft -=Time.deltaTime;
        }
        else
        {
            var horzBound = (camera.orthographicSize * Screen.width / Screen.height) + 2f;
            var vertBound = camera.orthographicSize;
            var spawnPos = new Vector3(horzBound , Random.Range(-vertBound , vertBound) , 0);
            Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
            spawnDelayLeft = Random.Range(spawnDelayMin, spawnDelayMax);
        }
    }
}
