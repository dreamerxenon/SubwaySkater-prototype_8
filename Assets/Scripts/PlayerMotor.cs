using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3f;
    private CharacterController controller;
    private bool isRunning = false;
    private float jumpForce = 4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private float desiredLane = 1;
    private Animator anim;

     public float speed = 7.0f;
     private float originalSpeed = 7.0f;
     private float speedIncreaseLastTick;
     private float speedIncreaseTime = 2.5f;
     private float speedIncreaseAmount = 0.1f;
     public TMP_InputField speedField;



//     private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {


        controller = GetComponent<CharacterController>();
       // audioSource =  GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        speedField.text = PlayerPrefs.GetInt("Speed").ToString();
                    speed =  PlayerPrefs.GetInt("Speed");

    }

    // Update is called once per frame
    void Update()
    {

        if(!isRunning)
        {
        return;
        }

        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(speed - originalSpeed);

        }

        if (MobileInput.Instance.SwipeLeft)
        {
            Debug.Log("swiped left");
            MoveLane(false);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        } else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);
        if (isGrounded)
        {
            verticalVelocity = -0.1f;
            if (MobileInput.Instance.SwipeUp)
            {
                anim.SetTrigger("Jump");
               // audioSource.Play();
                verticalVelocity = jumpForce;
            } else if(MobileInput.Instance.SwipeDown)
            {
                StartSliding();
                Invoke("StopSliding", .6f);
            }
        }else{
            verticalVelocity -= (gravity * Time.deltaTime);
             if (MobileInput.Instance.SwipeDown)
             {
                verticalVelocity = -jumpForce;
             }
        }
        moveVector.y = verticalVelocity;
        moveVector.z = speed;
        controller.Move(moveVector * Time.deltaTime);

        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.05f);
        }
    }

    private void MoveLane(bool goingRight)
    {
       desiredLane += (goingRight) ? 1: -1;
       desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay =  new Ray( new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    public void StartedRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center =  new Vector3(controller.center.x, controller.center.y  / 2, controller.center.z);
    }
    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center =  new Vector3(controller.center.x, controller.center.y  * 2, controller.center.z);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        switch (hit.gameObject.tag)
        {           
            case "Obstacle":
            Debug.Log("obstacle hit");
            Crash();
            break;
        }
    }
    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDead();
    }
    public void SetSpeed()
    {
    
    int speedtext;
     if (int.TryParse(speedField.text, out speedtext)) {
    PlayerPrefs.SetInt("Speed", speedtext); // Set the value in PlayerPrefs
    PlayerPrefs.Save(); // Optionally, save to make sure the data is persisted immediately
     } else {
    Debug.LogError("Invalid input! Please enter a valid number.");
     }
            speed =  PlayerPrefs.GetInt("Speed");


    }
}
