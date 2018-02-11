using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;

    bool isDead;
    CapsuleCollider capsuleCollider;

	// Use this for initialization
	void Awake () {;
        capsuleCollider = GetComponent<CapsuleCollider>();

        currentHealth = startingHealth;
	}

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            Death();
        }

        Debug.Log(transform.name + " took " + amount + " damage.");
    }

    void Death()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;
    }
}
