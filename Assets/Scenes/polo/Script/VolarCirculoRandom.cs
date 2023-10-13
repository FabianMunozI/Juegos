using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolarCirculoRandom : MonoBehaviour
{
    public float velocidad = 0.7f; // Velocidad del movimiento
    private float radio; // Radio del círculo
    private float angulo = 0f;
    private Vector3 posicionInicial; // Variable para almacenar la posición inicial

    void Start()
    {
        // Guarda la posición inicial del objeto
        posicionInicial = transform.position;
        angulo = Random.Range(0f,360f);
        radio = Random.Range(80,150);
        velocidad = velocidad * 100/radio;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la posición en el círculo
        float posX = Mathf.Sin(angulo) * radio;
        float posZ = Mathf.Cos(angulo) * radio;

        // Establece la posición del objeto en el círculo
        transform.position = new Vector3(posX, 0, posZ) + posicionInicial;

        // Calcula la dirección tangencial en el eje X
        Vector3 tangentDirection = new Vector3(Mathf.Cos(angulo), 0, -Mathf.Sin(angulo));

        // Rota el objeto para que mire en la dirección tangencial
        transform.rotation = Quaternion.LookRotation(tangentDirection);

        // Incrementa el ángulo para hacer que el pájaro se desplace alrededor del círculo
        angulo += velocidad * Time.deltaTime;

        // Asegúrate de reiniciar el ángulo cuando haya dado una vuelta completa al círculo
        if (angulo >= 360f)
        {
            angulo = 0f;
        }
    }
}