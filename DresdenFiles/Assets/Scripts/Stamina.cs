using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Configuration")]
    [SerializeField]
    private int maxStamina;
    [SerializeField]
    private int currentStamina;
    /// <summary>
    /// How much stamina should be restored per second while regening
    /// </summary>
    [Header("Regeneration Configuration")]
    [Tooltip("How much stamina should be restored per second while regening")]
    [SerializeField]
    private float restoreRate;
    /// <summary>
    /// How long after consuming stamina can regen start
    /// Set to 0 to remove regen cooldown
    /// </summary>
    [Tooltip("How long after consuming stamina can regen start")]
    [SerializeField]
    private float regenCooldown;

    private bool lockRegen;
    private Task regenTask;

    /// <summary>
    /// used to keep track of regening stamina 0 > x < 1
    /// </summary>
    private float intermediaryRestore;

    public int MaxStamina => maxStamina;
    public int CurrentStamina
    {
        get => currentStamina;
        // Never let stamina drop below 0 or go above MaxStamina
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
            // if intermediaryRestore is greater than 1, floor it and add it to CurrentStamina
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
            // Don't go on cooldown if there is no cooldown
            if(regenCooldown > 0)
            {
                lockRegen = true;
                regenTask = Task.Delay(TimeSpan.FromSeconds(regenCooldown)).ContinueWith(t => lockRegen = false);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Allow public interfacing to restore stamina points
    /// </summary>
    /// <param name="amount"></param>
    public void AddStamina(int amount) => CurrentStamina += amount;

    public void OnApplicationQuit()
    {
        regenTask.Wait();
    }
}
