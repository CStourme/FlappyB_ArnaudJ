using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    //tableau(Array) de prefabs
    [SerializeField] private GameObject[] _pipePrefabs;
    [SerializeField] private int _poolCapacity = 10;

    private List<GameObject> _list = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_pipePrefabs.Length == 0) return;

        for (int i = 0; i < _poolCapacity; i++)
        {
            // --- LA LOGIQUE CHANGE ICI ---
        
            // Au lieu d'un random total, on tourne en boucle sur les modèles :
            // Si i = 0 -> Prefab 0
            // Si i = 1 -> Prefab 1
            // Si i = 2 -> Prefab 0 (si on n'a que 2 modèles)
            int indexDuModele = i % _pipePrefabs.Length; 
        
            CreateSpecificPipeInPool(indexDuModele, i);
        }
    }

    // fonction de création
    private GameObject CreateSpecificPipeInPool(int prefabIndex, int instanceID)
    {
        GameObject prefabToInstantiate = _pipePrefabs[prefabIndex];

        GameObject pipe = Instantiate(prefabToInstantiate, transform);
        pipe.name = "Pipe_" + prefabToInstantiate.name + "_" + instanceID;
        pipe.SetActive(false);
        _list.Add(pipe);
        return pipe;
    }

    public GameObject GetFirstAvailablePipe()
    {
        foreach (GameObject pipe in _list)
        {
            if (pipe.activeSelf == false) return pipe;
        }

        // Si on arrive ici, c'est que le pool est trop petit.
        // On en crée un nouveau au hasard pour dépanner.
        int randomIndex = Random.Range(0, _pipePrefabs.Length);
        return CreateSpecificPipeInPool(randomIndex, _list.Count);
    }
}