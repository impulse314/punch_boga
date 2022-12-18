using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;              //�������� ���������
    public float rotationSpeed = 10;        //�������� �������� ��������� ��� ������������� ������ A/D
    public float speed = 4f;                //�������� ���� ��������� �� W/S
    public float smoothTime;                //��������������� ���������� ��� �������� ��������� �� �������
    float smoothVelocity;                   //�� �� �����
    public Transform characterCamera;       //��������� ������ ��� ��������� � ��������
    public CharacterController controller;  //���������� ���������

    public LayerMask groundLayer;           //�� ������, ���� ����������� ��������
    public float JumpForce=2f;              //���� ������
    public float gravity = 0.83f;            //���� �������, ������� ����� �������� ����� �� �����
    void Start()
    {
        animator = GetComponent<Animator>();    //��� ������ ������� �������� �������� ���������
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");       //�������� �������� �� �����������
        float v = Input.GetAxisRaw("Vertical");         //�������� �� ���������             [0f, 1f]

        Vector3 directionVector = new Vector3(h, 0f, v).normalized;     //������ ����������� ���������
        //directionVector.y += gravity * Time.deltaTime;
        Vector3 jumpVector = directionVector * speed;

        if (directionVector.magnitude > Mathf.Abs(0.05f))   //���� ����� ������� ������ -0.05||0.05, ��
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 10f;
                animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1f).magnitude);
            }        //����� ���� ������� ���� ������
            else
            {
                speed = 5f;
                animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 0.7f).magnitude);
            }



            float rotationAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + characterCamera.eulerAngles.y;            //��������� ���� ��������, ���������� ���� ������� ������ �� �����������
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);                        //����������, ���� �� ��������

            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);               //������������ ������
            Vector3 move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;     //��������� ������ �������� ������

            if (!controller.isGrounded)
            {
                Debug.Log("Ur flying, dude");
                move.y -= gravity;
            }
            else
            {
                Debug.Log("We're on the ground");
            }
            controller.Move(move.normalized * speed * Time.deltaTime);                  //������� ������
            directionVector = move;

        }
        else
            animator.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 0f).magnitude);//� �������� �������� ������� ����� ������� � �������� �������� �������� ���������, ����� ������ ��������������� ��������
    }
}
