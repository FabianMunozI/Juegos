using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volarcirculo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad del movimiento
    public float radio = 5f; // Radio del círculo
    private float angulo = 0f;
    private Vector3 posicionInicial; // Variable para almacenar la posición inicial

    // Start is called before the first frame update
    void Start()
    {
        // Guarda la posición inicial del objeto
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la posición en el círculo
        float posX = Mathf.Sin(angulo) * radio;
        float posY = transform.position.y;
        float posZ = Mathf.Cos(angulo) * radio;

        // Establece la posición del objeto en el círculo
        transform.position = new Vector3(posX, posY, posZ);

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