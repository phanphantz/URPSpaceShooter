using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public float speed = 3;
    public bool appeared;

    void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0 , 0);
    }

    private void OnBecameVisible() 
    {
        appeared = true;
    }

    private void OnBecameInvisible() 
    {
        if (appeared)
            Destroy(gameObject);
    }
}
