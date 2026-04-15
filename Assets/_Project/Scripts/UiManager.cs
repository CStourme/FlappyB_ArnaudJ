using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenu;

    public void DisplayGameOverMenu()
    {
        _gameOverMenu.SetActive(true);
    }

    public void DisplayScore(int score)
    {
        Debug.Log("Score: " + score);
    }
}