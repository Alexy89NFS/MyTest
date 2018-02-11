using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public int damagePerBite = 20;
    public float timeBetweenBullets = 3;
    public float range = 1;
    public EnemyHealth enemyHealth;
    public bool biting;
    Animator anim;

    public float timer;
    ParticleSystem biteParticles;
    float effectsDisplayTime = 0.2f;

	// Use this for initialization
	void Awake () {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        biteParticles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        biting = Input.GetButton("Fire1");
	}

    private void OnTriggerStay(Collider other)
    {
        if (biting && timer >= timeBetweenBullets && other.gameObject.tag == "Cat")
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
            Bite();
        }
        /*else if(biting && timer >= timeBetweenBullets && other.gameObject.tag != "Cat")
        {
            Play grab anim
        }*/
        else
        {
            enemyHealth = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemyHealth = null;
    }

    public void Bite()
    {
        timer = 0f;

        if (enemyHealth != null)
        {
            Hit();
            //Change anim state.
        }
       /* else
        {
            Play grab anim
        }*/
    }

    public void Hit()
    {
        enemyHealth.TakeDamage(damagePerBite);
        //Play bite anim
    }
}
