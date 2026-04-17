using UnityEngine;
using TMPro;
using System.IO; // Nécessaire pour sauvegarder le fichier sur le disque

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoringText;
    
    // --- NOUVELLES VARIABLES POUR LE RECORD ---
    public TMP_Text highscoreText; // À relier dans Unity
    private int highscore = 0;    // Pour stocker le meilleur score en mémoire
    private string savePath;      // L'endroit où le fichier sera rangé

    void Start()
    {
        Debug.Log("Le fichier JSON est ici : " + Application.persistentDataPath);
        
        // On définit où sera rangé le fichier JSON
        savePath = Application.persistentDataPath + "/highscoredat.json";
        
        // On charge le record au lancement
        LoadHighScore();
        
        UpdateScore();
    }

    public void AddSinglePoint()
    {
        score++;
        UpdateScore();
    }
    
    public void AddDoublePoint()
    {
        score += 2;
        UpdateScore();
    }
    
    public void AddTriplePoint()
    {
        score += 3;
        UpdateScore();
    }

    void UpdateScore()
    {
        // Mise à jour du texte du score actuel
        scoringText.text = "Score : " + score;

        // LOGIQUE DU MEILLEUR SCORE
        if (score > highscore)
        {
            highscore = score;
            SaveHighScore(); // On enregistre dès que le record change
        }

        // Mise à jour du texte du meilleur score
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore : " + highscore;
        }
    }

    // --- SYSTÈME DE SAUVEGARDE JSON SIMPLIFIÉ ---

    void SaveHighScore()
    {
        // On crée un petit objet temporaire pour le transformer en JSON
        SaveData data = new SaveData();
        data.highscore = highscore;

        string json = JsonUtility.ToJson(data); // Conversion en texte
        File.WriteAllText(savePath, json);      // Écriture dans le fichier
    }

    void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); // Lecture du fichier
            SaveData data = JsonUtility.FromJson<SaveData>(json); // Conversion vers le script
            highscore = data.highscore;
        }
    }
}

// La "fiche" qui sert à structurer le fichier JSON
[System.Serializable]
public class SaveData
{
    public int highscore;
}