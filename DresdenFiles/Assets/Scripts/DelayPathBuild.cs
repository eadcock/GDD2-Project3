using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPathBuild : MonoBehaviour
{
    bool hasBuilt;
    // Start is called before the first frame update
    void Start()
    {
        hasBuilt = false;    
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!hasBuilt)
        {
            hasBuilt = true;
            FindObjectOfType<AstarPath>().Scan();
        }
    }
}
