using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject player;
    public float radius;
    public Vector3 offset;
    public int damage;

    float distance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= radius)
        {
            Attack();
        }
    }

    public void Attack()
    {
        player.GetComponent<CharacterStats>().TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((transform.position + offset), radius);
    }
}