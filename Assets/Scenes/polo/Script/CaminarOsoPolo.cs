using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaminarOsoPolo : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f; // Velocidad de movimiento del oso
    [SerializeField] private float semiMajorAxis = 40.0f; // Semieje mayor de la elipse en el eje X
    [SerializeField] private float semiMinorAxis = 25.0f; // Semieje menor de la elipse en el eje Z
    [SerializeField] private float rotationSpeed = 30.0f; // Velocidad de rotación del oso
    private float angle = 0.0f; // Ángulo actual en radianes
    private Vector3 initialPosition; // Posición inicial del oso

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calcula la nueva posición del oso en la elipse
        float x = initialPosition.x + semiMajorAxis * Mathf.Cos(angle);
        float z = initialPosition.z + semiMinorAxis * Mathf.Sin(angle);

        // Calcula la dirección hacia el semieje mayor
        Vector3 targetDirection = new Vector3(semiMajorAxis * -Mathf.Sin(angle), 0.0f, semiMajorAxis * Mathf.Cos(angle));

        // Rota gradualmente hacia la dirección del semieje mayor con una velocidad más lenta
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Mueve al oso a la nueva posición
        transform.position = new Vector3(x, transform.position.y, z);

        // Incrementa el ángulo para avanzar en la elipse
        angle += moveSpeed * Time.deltaTime;

        // Ajusta el ángulo para asegurarte de que esté dentro de 0 a 2*pi para evitar un aumento infinito
        if (angle > Mathf.PI * 2)
        {
            angle -= Mathf.PI * 2;
        }
    }
}