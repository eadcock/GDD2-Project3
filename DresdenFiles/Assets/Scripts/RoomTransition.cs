using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomTransition : MonoBehaviour
{
    public DungeonManager dm;
    public RoomManager destination;
    public Vector3 startPos;

    public bool moving;

    public float moveDuration;
    private float startMove;
    private float endMove;

    // REPLACE THIS WITH A PROPER PAUSE SYSTEM
    public DresdenController dc;

    public void Start()
    {
        moving = false;
        dm = FindObjectOfType<DungeonManager>();
        dc = FindObjectOfType<DresdenController>();
    }

    public void Update()
    {
        if(moving)
        {
            Camera.main.transform.position = new Vector3(Mathf.Lerp(startPos.x, destination.Center.x, quiet.Math.Map(Time.time, startMove, endMove, 0, 1)),
            Mathf.Lerp(startPos.y, destination.Center.y, quiet.Math.Map(Time.time, startMove, endMove, 0, 1)), 
            -10);

            if (Time.time >= endMove)
            {
                moving = false;
                // REPLACE THIS WITH A POPER PAUSE SYSTEM            
                dc.SetPause(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(!moving)
        {
            destination = dm.Move(gameObject);
            moving = true;
            startMove = Time.time;
            endMove = Time.time + moveDuration;
            startPos = Camera.main.transform.position;
            dc.SetPause(true);
        }
    }
}
