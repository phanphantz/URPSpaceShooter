using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update
    bool loaded;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount != 0 || Input.GetMouseButtonDown(0)) && !loaded)
        {
            SceneManager.LoadScene("MainGame" , LoadSceneMode.Single);
            loaded = true;
        }
    }
}
