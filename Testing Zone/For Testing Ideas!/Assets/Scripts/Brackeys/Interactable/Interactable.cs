using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 3f;
    public Transform interactionTransform;

    Transform player;

    bool hasInteracted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(player);
    }

    public virtual void Interact()
    {
        // This method is meant to be Overriden
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        if (!hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
     
    }

    public void OnFocused(Transform playerTransform)
    {
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}
