using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugMode : MonoBehaviour
{
    public bool Active { get; set; }

    public event Action debugActions;
    // Start is called before the first frame update
    void Start()
    {
        Active = false;   
    }

    // Update is called once per frame
    void Update()
    {
        // If ` and Q pressed simultaneously, toggle debug mode
        if ((Input.GetKey(KeyCode.BackQuote) && Input.GetKeyDown(KeyCode.Q)) || (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.BackQuote)))
            Active = !Active;

        if(Active)
        {
            Debug.LogWarning("Debug Mode is Active");

            if (debugActions != null)
            {
                debugActions.Invoke();
            }
        }
    }
}
