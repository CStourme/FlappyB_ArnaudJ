using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player References"), Space(10)] 
    [SerializeField, Tooltip("The player object you must instantiate at start.")] private GameObject _playerPrefab;
    [SerializeField, Tooltip("The position of the player at start.")] private Transform _playerSpawnPosition;
    
    [SerializeField, Tooltip("The Background object you must instantiate at start.")] private GameObject _backgroundPrefab;
    private GameObject _bird;
    private GameObject _background;
    
    [Header("Managers References"), Space(10)]
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private SpawnPipeManager _pipeManager;
    private BirdController _birdscale;
    
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
        _background = Instantiate(_backgroundPrefab);
        _bird = Instantiate(_playerPrefab, _playerSpawnPosition.position, Quaternion.identity);
        // On récupère le script de l'oiseau qu'on vient de créer
        _birdscale = _bird.GetComponent<BirdController>();
        _birdscale.m_manager = this;
    }
    
    public void GameOver()
    {
        _uiManager.DisplayGameOverMenu();
        Time.timeScale = 0.25f;
    }

    public void StartDash()
    {
        _birdscale.ApplyDashScale();
        _pipeManager.SetCurrentSpeed(15);
    }
}
