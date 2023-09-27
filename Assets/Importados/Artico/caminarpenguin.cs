using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caminarpenguin : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Velocidad de movimiento del pingüino
    public float moveDistanceZ = 5.0f; // Distancia de movimiento en el eje Z
    public float rotationSpeed = 30.0f; // Velocidad de rotación del pingüino
    private Vector3 initialPosition; // Posición inicial del pingüino
    private Quaternion initialRotation; // Rotación inicial del pingüino
    private Quaternion targetRotation; // Rotación objetivo
    private bool isRotating = false; // Indica si el pingüino está girando
    private bool isMovingForward = true; // Indica si el pingüino se está moviendo hacia adelante o hacia atrás
    private int rotationDirection = 1; // Dirección de rotación (1: hacia la derecha, -1: hacia la izquierda)

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0); // Rotación objetivo de 180 grados en el eje Y

        // Comienza el ciclo
        StartCycle();
    }

    private void Update()
    {
        if (!isRotating)
        {
            // Calcula la dirección del movimiento (eje Z)
            Vector3 moveDirection = Vector3.forward * (isMovingForward ? 1 : -1);

            // Calcula la nueva posición del pingüino
            Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

            // Mueve al pingüino solo en el eje Z
            transform.position = new Vector3(transform.position.x, transform.position.y, newPosition.z);

            // Si el pingüino ha alcanzado la distancia de movimiento en el eje Z
            if ((isMovingForward && transform.position.z >= initialPosition.z + moveDistanceZ) ||
                (!isMovingForward && transform.position.z <= initialPosition.z))
            {
                // Detiene el movimiento y comienza la rotación
                isMovingForward = !isMovingForward;
                isRotating = true;
                // Cambia la rotación objetivo al comienzo del ciclo o al regresar
                if (isMovingForward)
                {
                    targetRotation = Quaternion.Euler(0, 360, 0); // Rotación objetivo de 360 grados en el eje Y
                }
                else
                {
                    targetRotation = Quaternion.Euler(0, 180, 0); // Rotación objetivo de 180 grados en el eje Y
                }
            }
        }
        else
        {
            // Rota gradualmente hacia la dirección opuesta
            float rotationAngle = rotationDirection * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAngle);

            // Si ha completado la rotación
            if (Mathf.Abs(transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y) < 1.0f)
            {
                // Cambia la dirección de rotación y permite el movimiento nuevamente
                rotationDirection *= -1;
                isRotating = false;
            }
        }
    }

    // Función para iniciar el ciclo
    private void StartCycle()
    {
        // Reinicia la posición y rotación
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        // Inicia el ciclo de movimiento y rotación
        isRotating = false;
        isMovingForward = true;
        targetRotation = Quaternion.Euler(0, 180, 0); // Al comienzo, rotación objetivo de 180 grados en el eje Y
    }
}