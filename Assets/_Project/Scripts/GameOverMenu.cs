using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _retryButton.onClick.AddListener(OnRetryGameButtonClick);
        _quitButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    private void OnRetryGameButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
