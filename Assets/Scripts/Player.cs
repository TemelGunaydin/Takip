using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Player Özellikleri")]
    [Range(0,20)]  public float speed = 5f;
    [Range(0, 20)] public float turnSpeed = 20f;
    public float smoothTime = 0.2f;
    public bool dead;
    public AudioClip duranMotor;
    public AudioClip hareketliMotor;


    private Rigidbody rigid;
    private float smoothAngle;
    private float smoothMove;
    private float vel;
    private Vector3 moveAmount;
    private AudioSource motorSesi;
    private float input;





    private void Start()
    {
        motorSesi = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 playerInput = new Vector3(0, 0, 0);

        if(!dead)
        {
           playerInput = new Vector3(h, 0f, v);
        }

        Vector3 direction = playerInput.normalized;
        input = direction.magnitude;
        Debug.Log(input);

        smoothMove = Mathf.SmoothDamp(smoothMove, input, ref vel, smoothTime);
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        smoothAngle = Mathf.LerpAngle(smoothAngle, angle, turnSpeed * Time.deltaTime * input);

        Vector3 velocity = direction * speed;
        moveAmount = velocity * Time.deltaTime * smoothMove;

        Motor(input);
     
       
    }

    private void Motor(float input)
    {
        if(input <1)
        {
            if(motorSesi.clip == hareketliMotor)
            {
                motorSesi.clip = duranMotor;
                motorSesi.Play();
            }
        }

        else
        {
            if(motorSesi.clip == duranMotor)
            {
                motorSesi.clip = hareketliMotor;
                motorSesi.Play();
            }
        }

    }


    
    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveAmount);
        rigid.MoveRotation(Quaternion.Euler(Vector3.up * smoothAngle));
    }

   public void Dead()
    {
        dead = true;
    }
   

}
