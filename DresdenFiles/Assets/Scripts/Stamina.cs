using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private int maxStamina;
    [SerializeField]
    private int currentStamina;
    [SerializeField]
    private float restoreRate;
    [SerializeField]
    private float regenCooldown;

    private bool lockRegen;

    // used to keep track of regening stamina 0 > x < 1
    private float intermediaryRestore;

    public int MaxStamina => maxStamina;
    public int CurrentStamina
    {
        get => currentStamina;
        private set => currentStamina = Mathf.Clamp(value, 0, MaxStamina);
    }

    // Start is called before the first frame update
    void Start()
    {
        lockRegen = false;
        CurrentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(!lockRegen)
        {
            intermediaryRestore += restoreRate * Time.deltaTime;
            int flooredIntRestore = Mathf.FloorToInt(intermediaryRestore);
            CurrentStamina += flooredIntRestore;
            intermediaryRestore -= flooredIntRestore;
        }
    }

    /// <summary>
    /// If CurrentStamina is greater than or equal to amount, drain the stamina and return true, otherwise return false
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>true if CurrentStamina is greater than or equal to amount, false otherwise</returns>
    public bool RequestStaminaDrain(int amount)
    {
        if(CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            lockRegen = true;
            Task.Delay(TimeSpan.FromSeconds(regenCooldown)).ContinueWith(t => lockRegen = false);
            return true;
        }
        return false;
    }

    public void AddStamina(int amount) => CurrentStamina += amount;
}
