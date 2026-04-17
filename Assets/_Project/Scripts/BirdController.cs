using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BirdController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Camera _camera;
    private Transform _birdscale;
    
    [Header("Bird Settings"), Space(10)]
    public float jumpForce = 8;
    public float accelForce = 15;
    
    // Référence au script "GameManager" qui gère les règles globales du jeu.
    // [HideInInspector] signifie qu'on ne le voit pas dans l'Unity Inspector, car il sera modifié par un autre script.
    [HideInInspector] public GameManager m_manager;
    
    [SerializeField] private bool isDead;
    
    // Création d'une variable pour stocker l'état actuel
    public enum GameState { Playing, GameOver }
    // On commence en "Playing"
    private GameState currentState = GameState.Playing;
    [SerializeField] private ParticleSystem _birdDashParticles;


    void Start()
    {
        _camera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _birdscale = GetComponent<Transform>();
        _birdDashParticles = GetComponentInChildren<ParticleSystem>();
    }


    void Update()
    {
        // On ne fait rien si on n'est pas en train de jouer
        if (currentState == GameState.Playing)
        {
            
            // Si j'appuie sur barre d'espace, le joueur saute.
            HandleInput();

            // On vérifie si la hauteur de l'oiseau dépasse la limite haute ou basse de la caméra.
            if (Mathf.Abs(transform.position.y) > _camera.orthographicSize)
            {
                // Si l'oiseau sort de l'écran (trop haut ou trop bas), on déclenche la fin de partie.
                TriggerGameOver();
                // On arrête immédiatement la fonction Update pour ne pas exécuter le reste du code.
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
        // Pour ne pas mourir deux fois
        if (currentState == GameState.GameOver) return;
        // Change l'état de current state à "GameOver"
        currentState = GameState.GameOver;
        
        // On prévient le manager que la partie est finie. 
        // C'est lui qui décidera d'afficher l'écran de Game Over ou de couper le son.
        m_manager.GameOver();
    }

    private void HandleInput()
    {
        // Saut avec la touche "spaceKey" :
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // On applique la force de saut
            _rigidbody2D.AddForceY(jumpForce, ForceMode2D.Impulse);
            // On déclenche l'animation via le trigger : "FlyTrigger"
            _animator.SetTrigger("FlyTrigger");
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            m_manager.StartDash();
        }
    }
    
    public void ApplyDashScale()
    {
        // On lance la Coroutine pour gérer l'animation de déformation
        StartCoroutine(DashScaleRoutine());
    }
    
    public void ActiveDashParticles()
    {
        //_birdDashParticles.SetActive(true);
    }

    private IEnumerator DashScaleRoutine()
    {
        // On déforme l'oiseau (plus long en X, plus fin en Y)
        transform.localScale = new Vector3(1.3f, 0.85f, 1f);

        // On garde cette déformation pendant une courte durée (le temps du boost)
        yield return new WaitForSeconds(0.2f);

        // RETOUR À LA NORMALE PROGRESSIF (Lerp)
        float elapsed = 0;
        float duration = 0.5f; // Le retour à la forme initiale dure 0.5 secondes
        Vector3 stretchedScale = transform.localScale;
        Vector3 normalScale = Vector3.one; // C'est-à-dire (1, 1, 1)

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            
            // On calcule le pourcentage d'avancement du retour
            float t = elapsed / duration;
            
            // Lissage de la transition pour un effet plus organique
            float smoothT = t * t * (3f - 2f * t);

            // On fait glisser doucement la taille de "déformée" vers "normale"
            transform.localScale = Vector3.Lerp(stretchedScale, normalScale, smoothT);
            
            yield return null; // Attendre l'image suivante
        }
        // On s'assure que l'oiseau termine exactement à sa taille d'origine
        transform.localScale = Vector3.one;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Dès qu'on touche un obstacle, on délègue la gestion de la fin de partie au manager.
        m_manager.GameOver();
    }
}
