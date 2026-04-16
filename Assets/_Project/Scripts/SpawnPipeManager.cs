using System.Collections;
using UnityEngine;

public class SpawnPipeManager : MonoBehaviour
{
    [Header("Pipe Spawn Settings"), Space(10)] 
    [SerializeField] private float _repeatRate = 5f; // Temps moyen
    [SerializeField, Range(0f, 4f)] private float _variation = 2f; // Hasard (+ ou -)
    [Header("Pipe Spawn Y Position")] 
    [SerializeField, Range(-4f, 4f)] private float _Up = 3f;
    [SerializeField, Range(-4f, 4f)] private float _Down = -3f;
    private PipePool _pipePool;

    void Start()
    {
        // On établit une connexion avec le script "PipePool" qui est attaché au même objet.
        // Récupère la référence du composant de type PipePool sur cet objet et la stocke dans la variable _pipePool.
        _pipePool = GetComponent<PipePool>();
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
}
