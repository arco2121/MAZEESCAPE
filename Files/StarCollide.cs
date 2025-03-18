using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderTrigger : MonoBehaviour
{
    public string targetTag = "Player"; // Tag dell'oggetto che deve attivare l'evento
    public bool destroyOnContact = false; // Se vero, l'oggetto verrà distrutto al contatto

    void Start()
    {
        // Controlla se esiste un Collider, altrimenti lo aggiunge
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>(); // Puoi cambiarlo in SphereCollider o CapsuleCollider
        }
        collider.isTrigger = true; // Rende il collider un trigger (non blocca il movimento)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag)) // Controlla se l'oggetto ha il tag desiderato
        {
            SceneManager.LoadScene("Win");

            if (destroyOnContact)
            {
                Destroy(other.gameObject); // Distrugge l'oggetto che entra nel trigger
            }
        }
    }
}
