using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player References"), Space(10)] 
    [SerializeField, Tooltip("The player object you must instantiate at start.")] private GameObject _playerPrefab;
    [SerializeField,Tooltip("The position of the player at start.")] private Transform _playerSpawnPosition;
    private GameObject _bird;
    
    [Header("Managers References"), Space(10)]
    [SerializeField] private UiManager _uiManager;
    
    [Header("Gameplay Data")]
    [SerializeField ]private int _score;
    
    private enum STATE
    {
        Playing,
        Paused,
        GameOver
    }
    
    private STATE _state = STATE.Playing;
    
    void Start()
    {
        _bird = Instantiate(_playerPrefab, _playerSpawnPosition.position, Quaternion.identity);
        _bird.GetComponent<BirdController>().m_manager =this;
    }
    
    public void GameOver()
    {
        _uiManager.DisplayGameOverMenu();
    }

    public void AddPoint()
    {
        _score++;
        _uiManager.DisplayScore(_score);
    }
}
