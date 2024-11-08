using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovAnim : MonoBehaviour
{
    public float veloMovi = 2f;
    public float gravedad = -9.8f;
    public float salto = 4f;

    public CharacterController controlador;
    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;

    bool enPiso;
    Vector3 velocidad;

    public Animator anima;  // Añadimos el Animator

    void Update()
    {
        // Verificar si está en el suelo
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);

        if (enPiso && velocidad.y < 0)
        {
            velocidad.y = -2f;
            anima.SetBool("IsJumping", false);  // Desactiva la animación de salto cuando toca el suelo
        }

        // Leer los ejes de entrada para moverse hacia adelante/atrás y lateralmente
        float x = Input.GetAxisRaw("Horizontal");  // Movimiento lateral (A/D o Flechas Izq/Der)
        float z = Input.GetAxisRaw("Vertical");    // Movimiento hacia adelante/atrás (W/S o Flechas Arr/Abj)

        Vector3 movimiento = transform.right * x + transform.forward * z;

        // Mover al personaje
        controlador.Move(movimiento * veloMovi * Time.deltaTime);

        // Hacer que el personaje mire hacia la dirección del movimiento
        if (movimiento != Vector3.zero)
        {
            Quaternion rotacion = Quaternion.LookRotation(movimiento); // Orientar el personaje hacia la dirección del movimiento
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, veloMovi * Time.deltaTime);
        }

        // Controlar el salto
        if (Input.GetButtonDown("Jump") && enPiso)
        {
            Debug.Log("Saltando");
            velocidad.y = Mathf.Sqrt(salto * -2f * gravedad);
            anima.SetBool("IsJumping", true);  // Activa la animación de salto cuando el jugador salta
        }

        // Aplicar la gravedad
        velocidad.y += gravedad * Time.deltaTime;
        controlador.Move(velocidad * Time.deltaTime);

        // Actualizar las animaciones de caminar
        ActualizarAnimaciones(x, z);
    }

    // Método para actualizar las animaciones del movimiento
    void ActualizarAnimaciones(float x, float z)
    {
        float velocidadMovimiento = Mathf.Sqrt(x * x + z * z); // La magnitud de movimiento en el plano XZ
        anima.SetFloat("Walk", velocidadMovimiento);  // Controla la animación de caminar

        anima.SetFloat("VelocidadX", x);  // Actualiza el movimiento lateral
        anima.SetFloat("VelocidadZ", z);  // Actualiza el movimiento hacia adelante/atrás
    }
}

