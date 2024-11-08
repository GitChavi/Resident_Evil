using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento3D : MonoBehaviour
{
    // Controlador de movimiento
    private CharacterController controller;
    public float veloMovi = 2f;
    public float veloRota = 10f;

    public static float X, Z;

    // C�mara para seguir al personaje
    [SerializeField]
    private Camera followCamera;

    // Variables para el movimiento y validaci�n de salto
    private Vector3 veloJugador;
    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;
    public float gravedad = -9.81f;
    public float salto = 1f;

    public CharacterController controlador;

    bool enPiso;

    public Animator anima;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        AnimarSalto();  // A�adimos una funci�n para controlar la animaci�n de salto
    }

    public void Movimiento()
    {
        // Verificar si est� en el suelo
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);

        // Si est� en el suelo, reiniciar la velocidad en Y
        if (enPiso && veloJugador.y < 0)
        {
            veloJugador.y = -2f;
            anima.SetBool("IsJumping", false);  // Desactiva la animaci�n de salto cuando toca el suelo
        }

        // Leer los ejes de entrada
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");

        // Usamos los �ngulos de euler para optimizar el giro en el eje Y
        Vector3 moveInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(X, 0, Z);

        // Normalizar el vector direcci�n (vector unidad)
        Vector3 moveDirection = moveInput.normalized;

        // Mover al personaje
        controller.Move(moveInput * veloMovi * Time.deltaTime);

        // Rotar el personaje para que mire en la direcci�n del movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotacion = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, veloRota * Time.deltaTime);
        }

        // Controlar el salto
        if (Input.GetButtonDown("Jump") && enPiso)
        {
            veloJugador.y = Mathf.Sqrt(salto * -2.0f * gravedad);
            anima.SetBool("IsJumping", true);  // Activa la animaci�n de salto cuando el jugador salta
        }

        // Aplicar la gravedad
        veloJugador.y += gravedad * Time.deltaTime;
        controller.Move(veloJugador * Time.deltaTime);
    }

    void AnimarSalto()
    {
        // Activar la animaci�n de salto si el personaje est� en el aire
        if (!enPiso)
        {
            anima.SetBool("IsJumping", true);
        }
    }

    private void FixedUpdate()
    {
        // Calcular la magnitud combinada del movimiento en X y Z
        float velocidadMovimiento = Mathf.Sqrt(X * X + Z * Z); // La magnitud de movimiento en el plano XZ

        // Actualizar el par�metro "Walk" en funci�n de la velocidad total
        anima.SetFloat("Walk", velocidadMovimiento);

        // Si deseas controlar las animaciones de avance/retroceso y laterales de forma independiente:
        anima.SetFloat("VelocidadX", X);  // Actualiza el movimiento lateral
        anima.SetFloat("VelocidadZ", Z);  // Actualiza el movimiento hacia adelante/atr�s
    }
}

