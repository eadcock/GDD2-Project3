using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using quiet;

public class DresdenController : MonoBehaviour
{
    [SerializeField]
    private DebugMode debugMode;

    [Tooltip("Base movement speed")]
    [SerializeField]
    private float speed;

    [Header("Dash Configuration")]
    [Tooltip("Time between dashes, in seconds")]
    [SerializeField]
    private float dashCooldown;
    [Tooltip("How long the dash lasts, in seconds")]
    [SerializeField]
    private float dashDuration;
    [Tooltip("How much stamina is required in order to dash")]
    [SerializeField]
    private int dashCost;


    private float lastDash;
    private Task dashTask;
    private CancellationTokenSource dashCancel;
    private float remainingDashDur;
    private Stamina stamina;
    private Health health;
    private Anim animator;

    private Rigidbody2D body;

    /// <summary>
    /// Is Dresden currently dashing?
    /// </summary>
    public bool Dashing { get; private set; }

    public bool Pause { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        lastDash = 0;
        stamina = GetComponent<Stamina>();
        health = GetComponent<Health>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Anim>();

        health.OnDeath += OnDeath;
        health.OnDeath += animator.OnDeath;

        health.OnDamage += hed => Debug.Log($"Dresden took {hed.Damage} damage from {hed.Source}");

        /*debugMode.debugActions += () =>
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                health.TakeDamage(debugMode.gameObject, 10);
            }
        };*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Pause)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            body.MovePosition(transform.position + movement * speed * Time.deltaTime);

            animator.SetBool("DownPressed", movement.y < 0);
            animator.SetBool("UpPressed", movement.y > 0);
            animator.SetBool("LeftPressed", movement.x < 0);
            animator.SetBool("RightPressed", movement.x > 0);



            /*if (Input.GetKeyDown(KeyCode.LeftShift))
                Dash();*/
        }
    }

    private void Dash()
    {
        if(Time.unscaledTime - lastDash > dashCooldown && stamina.RequestStaminaDrain(dashCost))
        {
            lastDash = Time.unscaledTime;
            speed *= 2;
            Dashing = true;
            // Stop dashing after dash duration is complete
            dashCancel = new CancellationTokenSource();
            /*dashTask = Task.Delay(TimeSpan.FromSeconds(dashDuration)).ContinueWith(t =>
            {
                speed /= 2;
                Dashing = false;
            }, dashCancel.Token);*/
            
        }
    }

    private void OnDeath(HealthEventData hed)
    {
        Debug.Log("Oh no! Dresden died to " + hed.Source);
    }

    public void SetPause(bool paused)
    {
        Pause = paused;
        if (paused)
        { 
            /*if(dashTask.Status == TaskStatus.Running)
            {
                dashCancel.Cancel();
                remainingDashDur = Time.time - lastDash;
            }*/
        } 
        else
        {
            /*if(dashTask.Status == TaskStatus.Canceled)
            {
                dashCancel = new CancellationTokenSource();
                dashTask = Task.Delay(TimeSpan.FromSeconds(remainingDashDur)).ContinueWith(t =>
                {
                    speed /= 2;
                    Dashing = false;
                }, dashCancel.Token);
            }*/
        }
    }
}
