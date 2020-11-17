using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Anim : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }
}
