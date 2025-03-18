using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           // Il giocatore da inseguire
    public float detectionRange = 10f; // Raggio massimo di rilevamento
    public float attackRange = 2f;     // Raggio di attacco
    public float moveSpeed = 3f;       // Velocità del nemico
    public Material normalMaterial;    // Materiale normale del nemico
    public Material alertMaterial;     // Materiale quando il nemico rileva il giocatore

    private bool canSeePlayer = false;
    private Renderer enemyRenderer;

    void Start()
    {
        // Ottieni il Renderer del nemico per poter cambiare il materiale
        enemyRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Verifica la visibilità del giocatore
        CheckLineOfSight();

        // Se il nemico può vedere il giocatore e il giocatore è nel raggio di rilevamento
        if (canSeePlayer && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Cambia il colore del nemico in rosso
            enemyRenderer.material = alertMaterial;

            // Fai ruotare il nemico per guardare il giocatore
            LookAtPlayer();

            // Insegui il giocatore
            MoveTowardsPlayer();

            // Se il nemico è nel raggio di attacco, fai attaccare
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Se non vede il giocatore, rimetti il materiale normale
            enemyRenderer.material = normalMaterial;
        }
    }

    void CheckLineOfSight()
    {
        // Calcola la direzione verso il giocatore
        Vector3 directionToPlayer = player.position - transform.position;

        RaycastHit hit;

        // Lancia un raycast dalla posizione del nemico verso il giocatore
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, detectionRange))
        {
            // Se il raycast colpisce il giocatore senza ostacoli
            if (hit.transform == player)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Muovi il nemico verso il giocatore
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void LookAtPlayer()
    {
        // Ruota il nemico per guardare verso il giocatore
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Mantieni solo la rotazione su Y, altrimenti ruoterebbe anche sul piano verticale
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f); // Slerp per un movimento di rotazione fluido
    }

    void AttackPlayer()
    {
        // Qui va il codice per l'attacco (per esempio, danni al giocatore)
        Debug.Log("Nemico attacca il giocatore!");
    }
}
