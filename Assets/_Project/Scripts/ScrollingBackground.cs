using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Background Settings")]
    // Slider exposé avec 5 comme la valeur par défaut
    [SerializeField, Range(0,5)] private float _backgroundSpeed = 0.1f;
    
    private Material _material;
    private Vector2 _offset;
    

    void Start()
    {
        // On récupère le matériau attaché au Renderer de l'objet
        _material = GetComponent<Renderer>().material;
    }


    void Update()
    {
        // 1. On calcule le nouvel offset X en fonction du temps et de la vitesse
        // On utilise _offset.x + ... pour que ça s'accumule à chaque image
        _offset.x += _backgroundSpeed * Time.deltaTime;

        // 2. On applique cet offset au matériau
        // "_MainTex" est le nom standard de la texture principale dans la plupart des shaders
        _material.SetTextureOffset("_MainTex", _offset);
    }
}