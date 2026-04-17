using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [Header("Pipe Settings")]
    // Slider exposé avec 5 comme la valeur par défaut
    //[SerializeField, Range(2,10)] private float _speed = 5f;

    private SpawnPipeManager _manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    void Update()
    {
        // Déplacement vers la gauche
        transform.Translate(Vector2.left * (_manager.GetCurrentSpeed() * Time.deltaTime));
        // Désactivation si l'objet sort de l'écran (Object Pooling)
        if(transform.position.x < -15) gameObject.SetActive(false);
    }

    public void SetManager(SpawnPipeManager spawnPipeManager)
    {
        _manager = spawnPipeManager;
    }
}
