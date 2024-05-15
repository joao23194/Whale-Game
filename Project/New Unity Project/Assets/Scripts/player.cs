using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player : MonoBehaviour
{

    public static player Instance;
    CharacterController characterController; //Componente que controla o jogador

    public float speed = 6.0f; //Velocidade de movimento, definível no Inspector
    public float jumpSpeed = 8.0f; //Velocidade de salto, definível no Inspector
    public float gravity = 20.0f; //Gravidade, definível no Inspector
    public float RotSpeed = 5.0f;

    public int Health;
    public int Exp;
    public TMP_Text HealthText;
    public TMP_Text ExpText;
    private Vector3 moveDirection = Vector3.zero; //Vector que controla a direcção do movimento

    void Start()
    {
        characterController = GetComponent<CharacterController>(); //Ir buscar o componente ao gameObject
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {

        if (characterController.isGrounded) //Se a personagem estiver no chão
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f,
   Input.GetAxis("Vertical")); //Ir buscar os valores do eixo horizontal (A/D ou seta esquerda/ direita) e do eixo vertical(W/ S ou seta frente e trás)
            moveDirection *= speed; //Multiplicar este vector pela variável de velocidade definida no início do script
            if (Input.GetButton("Jump")) //Se a pessoa carregar na tecla de saltar(barra de espaço)
            {
                moveDirection.y = jumpSpeed; //Adicionar velocidade no eixo Y, que até ao momento está definido com 0(ver moveDirection)
            }

            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotSpeed * Time.deltaTime);
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void MouseControl()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void IncreaseHP(int HealValue)
    {
        Health += HealValue;
    }
}
