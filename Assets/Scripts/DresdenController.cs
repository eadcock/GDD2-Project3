using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private Stamina stamina;
    private Health health;

    /// <summary>
    /// Is Dresden currently dashing?
    /// </summary>
    public bool Dashing { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        lastDash = 0;
        stamina = GetComponent<Stamina>();
        health = GetComponent<Health>();

        health.OnDeath += OnDeath;

        /*debugMode.debugActions += () =>
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                health.TakeDamage(debugMode.gameObject, 10);
            }
        };*/
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime;
        transform.position = transform.position + movement * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();

    }

    private void Dash()
    {
        if(Time.unscaledTime - lastDash > dashCooldown && stamina.RequestStaminaDrain(dashCost))
        {
            lastDash = Time.unscaledTime;
            speed *= 2;
            Dashing = true;
            // Stop dashing after dash duration is complete
            Task.Delay(TimeSpan.FromSeconds(dashDuration)).ContinueWith(t =>
            {
                speed /= 2;
                Dashing = false;
            });
        }
    }

    private void OnDeath(HealthEventData hed)
    {
        Debug.Log("Oh no! Dresden died to " + hed.Source);
    }
}
