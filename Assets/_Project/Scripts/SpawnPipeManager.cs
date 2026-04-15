using System.Collections;
using UnityEngine;

public class SpawnPipeManager : MonoBehaviour
{
    [SerializeField] private GameObject _pipePrefab;
    [SerializeField] private float _repeatRate = 5f;
    private PipePool _pipePool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pipePool = GetComponent<PipePool>();
        
        InvokeRepeating("SpawnPipe", 0, _repeatRate);
    }

    private void SpawnPipe()
    {
       Vector2 spawnPosition = new Vector2(15,Random.Range(-3,3));
       GameObject availablePipe = _pipePool.GetFirstAvailablePipe();
       availablePipe.transform.position = spawnPosition;
       availablePipe.SetActive(true);
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnPipe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
