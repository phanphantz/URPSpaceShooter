using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Spaceship spaceship;
    public Spaceship targetEnemy;
    public Seeker seeker;

    public Vector3 targetPosition;

    private float fireDelayMax = 10f;

    private float fireDelayLeft;
    private List<Vector3> pathList = new List<Vector3>();
    private bool seeking;

    private void Awake() 
    {
        targetPosition = spaceship.transform.position;
    }

    private void Update() 
    {
        if (spaceship.currentHealth <= 0)
        {
           EnemySpawner.Instance.enemyList.Remove(spaceship);
           enabled = false;
        }

        if (targetPosition == spaceship.transform.position)
        {
            if (pathList.Count == 0)
            {
                if (!seeking)
                    SeekForPath(PlayerController.Instance.GetRandomPositionInCameraBound());
            }
            else
            {
                SetTargetPosition(pathList[0]);
                pathList.RemoveAt(0);
            }
        }
        else
        {
            spaceship.SetTargetPosition(targetPosition);
        }

        if (fireDelayLeft <= 0)
        {
            if (!targetEnemy.isInvisible)
            {
                spaceship.Shoot((targetEnemy.transform.position - spaceship.transform.position).normalized);
                fireDelayLeft = fireDelayMax;
            }
        }
        else
        {
            fireDelayLeft -= Time.deltaTime;
        }

    }

    public void SeekForPath(Vector3 targetPos)
    {
        seeking =true;
        seeker.StartPath(spaceship.transform.position , targetPos , OnPathComplete);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public void OnPathComplete(Path p)
    {
        seeking =false;
        pathList.Clear();
        if (!p.error)
            pathList = p.vectorPath;
    }
}
