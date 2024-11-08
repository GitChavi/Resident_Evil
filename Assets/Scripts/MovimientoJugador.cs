using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
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

    void Update()
    {
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);

        

        if (enPiso && velocidad.y < 0)
        {
            velocidad.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movimiento = transform.right * x + transform.forward * z;

        controlador.Move(movimiento * veloMovi * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && enPiso)
        {
            Debug.Log("Saltando");
            velocidad.y = Mathf.Sqrt(salto * -0.5f * gravedad);
        }

        //Implementacion de gravedad en el objeto
        velocidad.y += gravedad * Time.deltaTime;
        controlador.Move(velocidad * Time.deltaTime);
    }
}
