using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private ScoreManager _scoreManager;

    void Start()
    {
        // On récupère le manager une seule fois pour la performance
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // On vérifie toujours si c'est le joueur
        if (!other.CompareTag("Player")) return;

        // On "switch" sur le tag de l'objet qui porte ce script
        switch (gameObject.tag)
        {
            // --- CAS POUR 1 POINT ---
            case "PassTroughUp":
                _scoreManager.AddSinglePoint();
                Debug.Log("Point Simple (+1)");
                break;
            case "PassTroughDown":
                _scoreManager.AddSinglePoint();
                Debug.Log("Point Simple (+1)");
                break;

            // --- CAS POUR 2 POINTS (Tes nouveaux tags) ---
            case "PassTroughDoubleL":
                _scoreManager.AddDoublePoint();
                Debug.Log("Point Double (+2) via : " + gameObject.tag);
                break;
            case "PassTroughDoubleN":
                _scoreManager.AddTriplePoint();
                Debug.Log("Point Triple (+3) via : " + gameObject.tag);
                break;
        }
    }
}