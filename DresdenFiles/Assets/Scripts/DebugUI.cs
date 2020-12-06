using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    Health health;
    Stamina stamina;
    Spells spells;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
        spells = GetComponent<Spells>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 50), $"Health: {health.CurrentHealth}/{health.MaxHealth}");
        GUI.Label(new Rect(10, 25, 200, 50), $"Stamina: {stamina.CurrentStamina}/{stamina.MaxStamina}");
        GUI.Label(new Rect(10, 40, 500, 50), $"Current Spell: {spells.CurrentSpell}");
    }
}
