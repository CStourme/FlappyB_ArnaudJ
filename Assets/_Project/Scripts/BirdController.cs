using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float jumpForce = 0;
    private Camera _camera;
    [HideInInspector] public GameManager m_manager;

    [SerializeField]private bool isDead;
    
    // Création d'une variable pour stocker l'état actuel
    public enum GameState { Playing, GameOver }
    // On commence en "Playing"
    private GameState currentState = GameState.Playing;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null) Debug.LogError("BirdController: No Rigidbody2D found!");
    }

    // Update is called once per frame
    void Update()
    {
        // On ne fait rien si on n'est pas en train de jouer
        if (currentState != GameState.Playing) return;
        {
            // Si j'appuie sur barre d'espace, le joueur saute.
            HandleInput();

            if (Mathf.Abs(transform.position.y) > _camera.orthographicSize)
            {
                TriggerGameOver();
                return;
            }

            // 1. Je limite la vitesse verticale
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x,
                Mathf.Clamp(_rigidbody2D.linearVelocity.y, -10f, 10f));

            // 2. LOGIQUE DE ROTATION
            // Je calcule un angle basé sur la vitesse (y)
            // Si on monte (positif), l'angle sera positif (haut). 
            // Si on descend (négatif), l'angle sera négatif (bas).
            float targetAngle = _rigidbody2D.linearVelocity.y * 8f; // Multiplier par 8 pour accentuer l'effet

            // Je limite l'angle entre -65° (piqué) et 40° (montée)
            targetAngle = Mathf.Clamp(targetAngle, -65f, 40f);

            // J'applique la rotation sur l'axe Z (l'axe de rotation en 2D)
            // On crée la rotation cible
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            // On tourne vers cette cible progressivement (vitesse de 5f ici)
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    
    // Pour éviter de répéter le code de mort, on crée une fonction
    private void TriggerGameOver()
    {
        if (currentState == GameState.GameOver) return; // Évite de mourir deux fois
    
        currentState = GameState.GameOver;
        m_manager.GameOver();
        // On ne met pas "enabled = false" ici, sinon l'oiseau s'arrête de tomber net
    }

    private void HandleInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _rigidbody2D.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        m_manager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_manager.AddPoint();
    }
}
