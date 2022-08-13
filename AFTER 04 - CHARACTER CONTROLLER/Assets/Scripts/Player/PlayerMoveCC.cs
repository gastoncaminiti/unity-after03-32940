using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCC : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 10f)]
    private float speed = 3f;

    // Propiedad empleada para almacenar la rotacion de la camara en Y.
    private float cameraAxisX = 0f;


    [SerializeField] Animator playerAnimator;

    private Vector3 playerDirection;
    /*
    private bool canJump = true;
    public bool CanJump { get => canJump; set => canJump = value; }
    */

    private CharacterController playerCC;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        //PRIMERA FORMA DE ANIMAR CON MOVIMIENTO: ANIMAR ANTES SE MOVER
        //Elegimos una animacion en función de la tecla que se empieza a presionar.
        bool forward = Input.GetKeyDown(KeyCode.W);
        bool back = Input.GetKeyDown(KeyCode.S);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);
        //Es posible simplificar la notación del if si el bloque contiene una única línea.
        if (forward) playerAnimator.SetTrigger("FORWARD");
        if (back) playerAnimator.SetTrigger("BACK");
        if (left) playerAnimator.SetTrigger("LEFT");
        if (right) playerAnimator.SetTrigger("RIGHT");
        // Estamos en reposo si se deja de presionar alguna de las teclas de movimiento.
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (!IsAnimation("IDLE")) playerAnimator.SetTrigger("IDLE");
        }
        //Limpiamos la dirección de movimiento en cada frame.
        //playerDirection = Vector3.zero;
        //Elegimos una dirección en función de la tecla que se mantiene presionada.
        if (Input.GetKey(KeyCode.W)) MovePlayer(Vector3.forward);
        if (Input.GetKey(KeyCode.S)) MovePlayer(Vector3.back);
        if (Input.GetKey(KeyCode.D)) MovePlayer(Vector3.right);
        if (Input.GetKey(KeyCode.A)) MovePlayer(Vector3.left);
        //Nos movemos solo si hay una dirección diferente que vector zero.
        if (playerCC.isGrounded)
        {
            playerDirection.y = 0f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //   canJump = false;
            Debug.Log("JUMP");
            playerCC.Move(Vector3.up * 10f * Time.deltaTime);
        }

        //if(!canJump){ playerDirection += Vector3.down; } 


        //APLICAR GRAVEDAD
        playerDirection.y += -9.81f * Time.deltaTime;
        playerCC.Move(playerDirection * Time.deltaTime);

        if (playerDirection != Vector3.zero) MovePlayer(playerDirection);

    }

    private bool IsAnimation(string animName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName);
        
    }


    private void MovePlayer(Vector3 direction)
    {
        playerCC.Move(transform.TransformDirection(direction) * speed * Time.deltaTime);
    }

    public void RotatePlayer()
    {
        /*
        Obtengo el valor del input del cursor (Que en Mouse X va de -1(izquierda) a 1(derecha))
        en función de que tan a la izquierda o derecha se mueve el mouse.
        */
        cameraAxisX += Input.GetAxis("Mouse X");
        // Forma para rotar "inmediatamente" hacia una nueva rotación creada con el método Euler (a partir de los ejes x,y,z)
        //transform.rotation = Quaternion.Euler(0,cameraAxisX * 0.1f, 0);
        // Forma para rotar "gradualmente" hacia una nueva rotación con Lerp.
        Quaternion newRotation = Quaternion.Euler(0, cameraAxisX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 2.5f * Time.deltaTime);
    }
}
