using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;

    bool isDead;
    bool damaged;

	// Use this for initialization
	void Awake () {
        currentHealth = startingHealth;
	}

    private void Update()
    {
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
        Debug.Log(transform.name + " took " + amount + " damage.");
    }

    void Death()
    {
        isDead = true;

        Destroy(gameObject);
    }
}
