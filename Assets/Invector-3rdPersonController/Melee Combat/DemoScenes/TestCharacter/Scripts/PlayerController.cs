using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;              //аниматор персонажа
    public float rotationSpeed = 10;        //скорость поворота персонажа при использовании клавиш A/D
    public float speed = 4f;                //скорость бега персонажа на W/S
    public float smoothTime;                //вспомогательная переменная для поворота персонажа за камерой
    float smoothVelocity;                   //то же самое
    public Transform characterCamera;       //трансформ камеры для получения её поворота
    public CharacterController controller;  //контроллер персонажа

    public LayerMask groundLayer;           //на случай, если понадобятся рэйкасты
    public float JumpForce=2f;              //сила прыжка
    public float gravity = 0.83f;            //сила тяжести, которая будет опускать героя на землю
    void Start()
    {
        animator = GetComponent<Animator>();    //при старте скрипта получаем аниматор персонажа
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");       //получили движение по горизонтали
        float v = Input.GetAxisRaw("Vertical");         //получили по вертикали             [0f, 1f]

        Vector3 directionVector = new Vector3(h, 0f, v).normalized;     //вектор направления персонажа
        //directionVector.y += gravity * Time.deltaTime;
        Vector3 jumpVector = directionVector * speed;

        if (directionVector.magnitude > Mathf.Abs(0.05f))   //если длина вектора больше -0.05||0.05, то
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 10f;
                animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1f).magnitude);
            }        //здесь надо сделать норм спринт
            else
            {
                speed = 5f;
                animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 0.7f).magnitude);
            }



            float rotationAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + characterCamera.eulerAngles.y;            //формируем угол поворота, подмешивая сюда поворот камеры по горизонтали
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);                        //сглаживаем, чтоб не дергался

            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);               //поворачиваем модель
            Vector3 move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;     //формируем вектор движения модели

            if (!controller.isGrounded)
            {
                Debug.Log("Ur flying, dude");
                move.y -= gravity;
            }
            else
            {
                Debug.Log("We're on the ground");
            }
            controller.Move(move.normalized * speed * Time.deltaTime);                  //двигаем модель
            directionVector = move;

        }
        else
            animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 0f).magnitude);//в аниматор передаем текущую длину вектора в качестве скорости движения персонажа, чтобы играть соответствующую анимацию
    }
}
