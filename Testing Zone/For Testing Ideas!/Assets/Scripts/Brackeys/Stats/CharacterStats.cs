﻿using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way. This method is meant to be over ridden.
        Debug.Log(transform.name + " died.");
    }

}
