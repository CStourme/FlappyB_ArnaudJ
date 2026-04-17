using System.Collections;
using UnityEngine;

public class SpawnPipeManager : MonoBehaviour
{
    [Header("Pipe Spawn Settings"), Space(10)] 
    [SerializeField] private float _repeatRate = 1.5f; // Temps moyen
    [SerializeField, Range(0f, 4f)] private float _variation = 2f; // Hasard (+ ou -)
    
    [Header("Pipe Spawn Y Position")] 
    [SerializeField, Range(-4f, 4f)] private float _Up = 3f;
    [SerializeField, Range(-4f, 4f)] private float _Down = -3f;
    
    [Header("Speed Settings")]
    [SerializeField] private float _normalSpeed = 5f; // La vitesse de base
    [SerializeField] private float _currentSpeed = 5f;
    
    [Header("Dash Transition Settings")]
    [Tooltip("Temps pendant lequel le décor défile à vitesse max avant de freiner")]
    [SerializeField] private float _dashDuration = 0.1f;
    [Tooltip("Temps que met la vitesse pour redescendre à la normale")]
    [SerializeField] private float _lerpReturnDuration = 0.5f;
    
    private PipePool _pipePool;
    // Pour stocker et stopper la transition si on re-dash
    private Coroutine _speedRoutine;

    void Start()
    {
        // On établit une connexion avec le script "PipePool" qui est attaché au même objet.
        // Récupère la référence du composant de type PipePool sur cet objet et la stocke dans la variable _pipePool.
        _pipePool = GetComponent<PipePool>();
        // On s'assure d'être à la bonne vitesse au départ
        _currentSpeed = _normalSpeed;
        // On lance le tout premier appel immédiatement
        SpawnPipe();
    }

    private void SpawnPipe()
    {
        // 1. On fait apparaître le tuyau
        Vector2 spawnPosition = new Vector2(15, Random.Range(_Up, _Down));
        GameObject availablePipe = _pipePool.GetFirstAvailablePipe();
        
        if (availablePipe != null)
        {
            availablePipe.transform.position = spawnPosition;
            availablePipe.SetActive(true);
            availablePipe.GetComponent<SimpleMove>().SetManager(this);
        }

        // 2. On calcule un délai aléatoire pour préparer le coup d'après (Le relais)
        float delay = _repeatRate + Random.Range(-_variation, _variation);
        // Sécurité : on s'assure que le délai n'est pas négatif ou trop court
        if (delay < 0.5f) delay = 0.5f;

        // 3. On programme l'exécution suivante
        Invoke("SpawnPipe", delay);
    }

    public void StopSpawning()
    {
        // Si on veut arrêter, on annule tous les Invokes en cours
        CancelInvoke("SpawnPipe");
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public void SetCurrentSpeed(float newSpeed)
    {
        // Si on était déjà en train de ralentir, on coupe l'ancien ralentissement
        if (_speedRoutine != null)
        {
            StopCoroutine(_speedRoutine);
        }
        
        // On lance la nouvelle séquence d'accélération puis de ralentissement
        _speedRoutine = StartCoroutine(SmoothSpeedReturn(newSpeed));
    }
    
    private IEnumerator SmoothSpeedReturn(float targetDashSpeed)
    {
        // 1. Le "Coup de boost" immédiat (l'équivalent de ton ancienne impulsion)
        _currentSpeed = targetDashSpeed;

        // 2. On maintient la vitesse max pendant la durée du dash
        yield return new WaitForSeconds(_dashDuration);

        // 3. Le fameux LERP pour un freinage progressif
        float elapsed = 0f;

        while (elapsed < _lerpReturnDuration)
        {
            elapsed += Time.deltaTime;
            
            // t = pourcentage d'avancement du temps entre 0 et 1
            float t = elapsed / _lerpReturnDuration;
            
            // On utilise une courbe "SmoothStep" pour que le ralentissement 
            // soit très doux, sans choc visuel
            float smoothT = t * t * (3f - 2f * t);

            // On fait glisser la valeur de la vitesse du dash vers la vitesse normale
            _currentSpeed = Mathf.Lerp(targetDashSpeed, _normalSpeed, smoothT);
            
            yield return null; // On attend l'image suivante
        }

        // 4. Sécurité finale : on force la vitesse exacte de base
        _currentSpeed = _normalSpeed;
        _speedRoutine = null;
    }

    private void ResetSpeed()
    {
        _currentSpeed = 5;
    }
}
