using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Vector3 respawnPoint;
    bool seen = false;
    float speed = 30f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime , 0 ,0);
    }

    private void OnBecameVisible() 
    {
        seen = true;
    }

    private void OnBecameInvisible() 
    {
        if (seen)
            transform.position = respawnPoint;
    }
}
