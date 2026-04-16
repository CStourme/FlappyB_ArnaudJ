using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [Header("Pipe Settings")]
    // Slider exposé avec 5 comme la valeur par défaut
    [SerializeField, Range(2,10)] private float _speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Déplacement vers la gauche
        transform.Translate(Vector2.left * (_speed * Time.deltaTime));
        // Désactivation si l'objet sort de l'écran (Object Pooling)
        if(transform.position.x < -15) gameObject.SetActive(false);
    }
}
