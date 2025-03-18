using UnityEngine;

public class ColliderTrigger2 : MonoBehaviour
{
    public string targetTag = "Player"; // Tag dell'oggetto che deve attivare l'evento
    public bool destroyOnContact = false; // Se vero, l'oggetto verrà distrutto al contatto
    public timescript playerTimeScript;
    public PlayerHitEffect playerHitEffect;
    public AudioClip attackSound;          // Suono per l'attacco
    private AudioSource audioSource;

    void Start()
    {
        // Verifica se c'è un AudioSource, altrimenti aggiungilo
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verifica e imposta il collider come trigger
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>(); // Puoi cambiarlo in SphereCollider o CapsuleCollider
        }
        collider.isTrigger = true; // Rende il collider un trigger (non blocca il movimento)
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto entrato nel trigger è il player
        if (other.CompareTag(targetTag))
        {
            // Attiva gli effetti per il guadagno di tempo e guarigione
            if (playerTimeScript != null)
            {
                playerTimeScript.GainTime(10f);
            }

            if (playerHitEffect != null)
            {
                playerHitEffect.OnHeal();
            }

            // Riproduci il suono dell'attacco se esiste
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);  // Riproduce il suono dell'attacco
            }

            // Distruggi l'oggetto con un ritardo per dare il tempo al suono di essere riprodotto
            if (destroyOnContact)
            {
                float soundDuration = attackSound.length;  // Ottieni la durata del suono
                Invoke("DestroyObject", soundDuration);   // Distruggi l'oggetto dopo la durata del suono
            }
        }
    }

    // Metodo per distruggere l'oggetto
    private void DestroyObject()
    {
        Destroy(gameObject);  // Distrugge l'oggetto
    }
}
