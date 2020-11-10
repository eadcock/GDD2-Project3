using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using quiet;

public class DresdenController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dashCooldown;
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private int dashCost;

    private float lastDash;
    private Stamina stamina;
    // Start is called before the first frame update
    void Start()
    {
        lastDash = 0;
        stamina = GetComponent<Stamina>();
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
            Task.Delay(TimeSpan.FromSeconds(dashDuration)).ContinueWith(ResetSpeed);
        }
    }

    private void ResetSpeed(Task t) => speed /= 2;
}
