using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocità di movimento
    public float sprintSpeed = 10f;  // Velocità di corsa
    public float rotationSpeed = 700f;  // Velocità di rotazione
    public Camera playerCamera;  // Riferimento alla telecamera del giocatore

    public AudioClip walkSound;  // Suono di camminata
    public AudioClip runSound;   // Suono di corsa
    private AudioSource audioSource;  // Componente AudioSource

    private Vector3 moveDirection;  // Direzione del movimento
    private float rotationX = 0f;  // Angolo di rotazione orizzontale
    private float rotationY = 0f;  // Angolo di rotazione verticale
    private Rigidbody rb;  // Riferimento al Rigidbody del giocatore

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Ottieni il componente Rigidbody
        audioSource = GetComponent<AudioSource>();  // Ottieni il componente AudioSource
        rb.freezeRotation = true;  // Disabilita la rotazione automatica del Rigidbody
    }

    void Update()
    {
        // Controllo del movimento (WASD)
        float horizontalInput = Input.GetAxis("Horizontal");  // A/D o frecce sinistra/destra
        float verticalInput = Input.GetAxis("Vertical");  // W/S o frecce su/giù

        // Calcola la direzione del movimento (relativa alla telecamera)
        moveDirection = playerCamera.transform.forward * verticalInput + playerCamera.transform.right * horizontalInput;

        // Mantieni la direzione del movimento orizzontale sulla superficie del piano (evita il movimento verso l'alto o il basso)
        moveDirection.y = 0f;

        // Ruotare la telecamera in base al movimento del mouse
        LookAround();

        // Gestisci i suoni di camminata o corsa
        HandleFootstepsSound();
    }

    void FixedUpdate()
    {
        // Muovi il giocatore
        MovePlayer();
    }

    void MovePlayer()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            // Verifica se il giocatore sta correndo o camminando
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;  // Usa la velocità di corsa se Shift è premuto

            // Muovi il giocatore con Rigidbody per gestire la fisica
            Vector3 movement = moveDirection.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);  // Usa Rigidbody per muovere il personaggio in modo fisico
        }
    }

    void HandleFootstepsSound()
    {
        // Se il giocatore si sta muovendo
        if (moveDirection.magnitude >= 0.1f)
        {
            // Se il giocatore sta correndo (Shift premuto)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!audioSource.isPlaying || audioSource.clip != runSound)
                {
                    audioSource.clip = runSound;  // Imposta il suono della corsa
                    audioSource.Play();  // Riproduci il suono
                }
            }
            else  // Se il giocatore sta camminando
            {
                if (!audioSource.isPlaying || audioSource.clip != walkSound)
                {
                    audioSource.clip = walkSound;  // Imposta il suono della camminata
                    audioSource.Play();  // Riproduci il suono
                }
            }
        }
        else
        {
            // Se il giocatore non si sta muovendo, ferma il suono
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void LookAround()
    {
        // Acquisisci il movimento del mouse per la rotazione della telecamera
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotazione orizzontale del giocatore
        rotationX += mouseX * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, rotationX, 0f);  // Rotazione del corpo del giocatore

        // Rotazione verticale della telecamera (su/giù)
        rotationY -= mouseY * rotationSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);  // Limita la rotazione verticale per evitare flip

        playerCamera.transform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);  // Rotazione della telecamera
    }
}
