using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEventData
{
    /// <summary>
    /// The cause of the Health Event
    /// </summary>
    public GameObject Source { get; private set; }
    /// <summary>
    /// The GameObject calling the Health Event
    /// </summary>
    public GameObject Target { get; private set; }
    /// <summary>
    /// How much damage was caused
    /// </summary>
    public float Damage { get; private set; }
    public HealthEventData(GameObject target, GameObject source, float damage)
    {
        Target = target;
        Source = source;
        Damage = damage;
    }
}
