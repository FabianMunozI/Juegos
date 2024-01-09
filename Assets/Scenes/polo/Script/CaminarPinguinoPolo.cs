using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaminarPinguinoPolo : MonoBehaviour
{
    private float moveSpeed = 5.0f; // Velocidad de movimiento del pingüino
    private float moveDistanceZ = 50.0f; // Distancia de movimiento en el eje Z
    private float rotationSpeed = 50.0f; // Velocidad de rotación del pingüino
    private Vector3 initialPosition; // Posición inicial del pingüino
    private Quaternion initialRotation; // Rotación inicial del pingüino
    private Quaternion targetRotation; // Rotación objetivo
    private bool isRotating = false; // Indica si el pingüino está girando
    private bool isMovingForward = true; // Indica si el pingüino se está moviendo hacia adelante o hacia atrás
    private int rotationDirection = 1; // Dirección de rotación (1: hacia la derecha, -1: hacia la izquierda)

    //LayerMask floorMask = 1 << LayerMask.NameToLayer("Floor");
    LayerMask floorMask;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

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

        //busqueda del piso

        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + Vector3.up * 10f,Vector3.down); // rayo de luz que parte 10m arriba tuyo y apunta hacia abajo

        if (Physics.Raycast(ray, out hitInfo, 20f, floorMask))
        {
            //rayo que busca colision entre rayo y objeto que tenga layer que buscamos (suelo), fake si no encuentra el suelo
            Vector3 pos = transform.position;
            pos.y = hitInfo.point.y;
            transform.position = pos;
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
    //probando
}