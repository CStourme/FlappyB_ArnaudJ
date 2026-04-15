using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    [SerializeField] private GameObject _pipePrefab;
    [SerializeField] private int _poolCapacity = 10;

    private List<GameObject> _list = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        for (int i = 0; i < _poolCapacity; i++)
        {
            GameObject pipe = Instantiate(_pipePrefab, transform);
            pipe.name = "Pipe" + i;
            pipe.SetActive(false);
            _list.Add(pipe);
        }
    }

    public GameObject GetFirstAvailablePipe()
    {
        foreach (GameObject pipe in _list)
        {
            if (pipe.activeSelf == false) return pipe;
        }

        GameObject newPipe = Instantiate(_pipePrefab, transform);
        newPipe.SetActive(false);
        _list.Add(newPipe);
        return newPipe;
    } 
}
