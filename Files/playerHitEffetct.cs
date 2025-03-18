using UnityEngine;
using UnityEngine.UI;  // Necessario per lavorare con le UI Image

public class PlayerHitEffect : MonoBehaviour
{
    public Image hitImage;          // Riferimento all'immagine UI rossa
    public Image healImage;         // Riferimento all'immagine UI verde
    public float effectDuration = 1.5f; // Durata dell'effetto (quanto tempo l'immagine rimane visibile)
    private float hitTimer = 0f;    // Timer per la durata dell'effetto
    private float healTimer = 0f;   // Timer per l'effetto di guarigione
    private bool isHit = false;     // Flag per determinare se il giocatore è stato colpito
    private bool isHealed = false;  // Flag per determinare se il giocatore è stato curato

    void Start()
    {
        // Assicurati che le immagini partano trasparenti
        hitImage.color = new Color(1f, 0f, 0f, 0f);  // Rosso trasparente
        healImage.color = new Color(0f, 1f, 0f, 0f); // Verde trasparente
    }

    void Update()
    {
        // Gestione dell'effetto colpo (rosso)
        if (isHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer > effectDuration)
            {
                hitImage.color = new Color(1f, 0f, 0f, 0f); // Nasconde l'effetto
                isHit = false;
                hitTimer = 0f;
            }
        }

        // Gestione dell'effetto cura (verde)
        if (isHealed)
        {
            healTimer += Time.deltaTime;
            if (healTimer > effectDuration)
            {
                healImage.color = new Color(0f, 1f, 0f, 0f); // Nasconde l'effetto
                isHealed = false;
                healTimer = 0f;
            }
        }
    }

    // Metodo per attivare l'effetto di colpo (rosso)
    public void OnHit()
    {
        hitImage.color = new Color(1f, 0f, 0f, 0.5f);  // Colore rosso con trasparenza
        isHit = true;
    }

    // Metodo per attivare l'effetto di guarigione (verde)
    public void OnHeal()
    {
        healImage.color = new Color(0f, 1f, 0f, 0.5f);  // Colore verde con trasparenza
        isHealed = true;
    }
}
