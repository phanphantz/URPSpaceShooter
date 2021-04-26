using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rigidbody;
    public SphereCollider collider;
    public int damage;
    public float speed;
    public Transform targetTransform;
    public bool fromPlayer;

    public bool isTracked => targetTransform != null;
    private Vector3 moveVector;
    private bool isBlocked;
    private ObstacleArea currentObstacle;
    private float circleDir = 0;

    private void Update()
    {
        moveVector.z = 0;

        if (targetTransform)
        {
            var targetVector = (targetTransform.position - transform.position).normalized;
            targetVector.z = 0;
            if (!isBlocked || currentObstacle == null)
            {
                moveVector = targetVector * speed;
                isBlocked = false;
            }
            else
            {
                var targetPos = (currentObstacle.transform.position + new Vector3(0, currentObstacle.collider.radius, 0));
                moveVector = (targetPos - transform.position).normalized * speed;
                // print(moveVector.sqrMagnitude);
                if ((targetPos - transform.position).sqrMagnitude < 1f)
                    isBlocked = false;

                // RaycastHit[] hit = Physics.SphereCastAll(transform.position ,collider.radius , targetVector);

                // if (hit.Length == 1 && hit[0].collider.gameObject == targetTransform.gameObject)
                // {
                //     isBlocked = false;
                // }

            }
        }

        if (GameplayCore.Instance.paused || GameplayCore.Instance.ended)
        {
            rigidbody.velocity = Vector3.zero;
        }
        else
        {
            rigidbody.velocity = moveVector;
        }

        // if (isBlocked)
        // {

        // }
    }

    public void Fire(Vector3 dir)
    {
        moveVector = dir * speed;
    }

    public void Track(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var obstacle = other.gameObject.GetComponent<ObstacleArea>();
        if (obstacle != null && isTracked)
        {
            currentObstacle = obstacle;

            if (targetTransform.position.y >= transform.position.y)
            {
                circleDir = 1f;
            }
            else
            {
                circleDir = -1f;
            }

            isBlocked = true;
            return;
        }

        var shipBody = other.gameObject.GetComponent<Spaceship>();
        if (shipBody)
        {
            if ((fromPlayer && shipBody.isPlayer) || (!fromPlayer && !shipBody.isPlayer))
                return;

            shipBody.TakeDamage(damage);
            Explode();
        }

        if (isTracked && shipBody == null)
            return;

        var debris = other.gameObject.GetComponent<Debris>();
        if (debris)
            Explode();

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
