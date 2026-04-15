using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField, Range(2,4)] private float _unitsPerSecond;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * (_unitsPerSecond * Time.deltaTime));
        if(transform.position.x < -15) gameObject.SetActive(false);
    }
}
