using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public float sensiMouse = 3.0f;

    //Rotaciones en X y Y
    private float rotY, rotX;

    //Nuestro objeto a seguir

    public Transform target;

    //Disggatncia de la camara a nuestro objeto a seguir
    public float distanceTarget = 3.0f;

    //Variables de rotacion
    Vector3 curRotation;
    Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 0.2f;
   
    //variables x y Y para restringir la rotacion total en Y
    [SerializeField]
    private Vector2 MaxMinRota = new Vector2(-20, 40);

    void Start()
    {
        //Bloquamos la posicion del moouse y lo desaparecemos de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Capturamos los valores de movimiento del Mouse
        float mouseX = Input.GetAxis("Mouse X") * sensiMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensiMouse;

        //Asignamos los datos de movimiento y evitamos el "Inverted Axis"
        rotX -= mouseY;
        rotY += mouseX;


        //Clamping para restringir la rotacion en total en Y
        rotX = Mathf.Clamp(rotX, MaxMinRota.x, MaxMinRota.y);

        Vector3 nextRotation = new Vector3(rotX, rotY);

        //Aplicamos entre los cambios de rotacion un efecto de suavizado
        curRotation = Vector3.SmoothDamp(curRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = curRotation;
        //Hallamos la posicion relativa entre el vector del objeteo y el vector del target
        transform.position = target.position - transform.forward * distanceTarget;
    }
}
