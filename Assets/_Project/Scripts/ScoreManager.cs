using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoringText;

    void Start()
    {
        UpdateScore();
    }

    public void AddSinglePoint()
    {
        // J'ajoute 1 point
        score++;
        UpdateScore();
    }
    
    public void AddDoublePoint()
    {
        // J'ajoute 2 points
        score+=2;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoringText.text = "Score : " + score;
    }
}