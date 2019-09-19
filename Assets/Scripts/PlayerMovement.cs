using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 15.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float transformLerpSpeed = 10.0f;

    public Transform topLeftTransform;
    public Transform leftTransform;
    public Transform middleTransform;
    public Transform rightTransform;
    public Transform topRightTransform;
    public Transform topTransform;

    public AudioSource movementSound;


    private Vector3 moveDirection = Vector3.zero;
    //private CharacterController controller;
    
    private bool movingRight = false;
    private bool movingLeft = false;
    private bool falling = false;

    private int currentLane = 0; // Middle
    private int targetLane = 0;
    private Vector3 targetVector3 = new Vector3();
    private Quaternion targetQuaternion = new Quaternion();

  

    void Awake()
    {
        //controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Keep Moving Forward
        transform.position += Vector3.forward * Time.deltaTime * speed;

        //Jump
        if (isGrounded())
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // Apply gravity
            moveDirection.y -= gravity * Time.deltaTime;
        }
        transform.Translate(moveDirection * Time.deltaTime);


        // Switch to the lane on the right
        if (Input.GetKeyDown(KeyCode.D) && !falling)
        {
            if (movingRight)
            {
                currentLane = targetLane;
                targetLane++;
                Debug.Log("Moving Right from current lane: " + currentLane + " to target lane: " + targetLane);
            } else if(movingLeft){
                currentLane = targetLane;
                targetLane++;
                Debug.Log("Moving Left from current lane: " + currentLane + " to target lane: " + targetLane);
            } else
            {
                movingRight = true;
                switch (currentLane)
                {
                    case -2:
                        targetLane = -1;
                        break;
                    case -1:
                        targetLane = 0;
                        break;
                    case 0:
                        targetLane = 1;
                        break;
                    case 1:
                        targetLane = 2;
                        break;
                    default:
                        targetLane = 3;
                        break;
                }
                Debug.Log("Moving Right from current lane: " + currentLane + " to target lane: " + targetLane);
            }
        }

        // Switch to the lane on the left
        if (Input.GetKeyDown(KeyCode.A) && !falling)
        {
            if (movingLeft)
            {
                currentLane = targetLane;
                targetLane--;
                Debug.Log("Moving Left from current lane: " + currentLane + " to target lane: " + targetLane);
            }
            else if (movingRight)
            {
                currentLane = targetLane;
                targetLane--;
                Debug.Log("Moving Right from current lane: " + currentLane + " to target lane: " + targetLane);
            } else
            {
                movingLeft = true;
                switch (currentLane)
                {
                    case -1:
                        targetLane = -2;
                        break;
                    case 0:
                        targetLane = -1;
                        break;
                    case 1:
                        targetLane = 0;
                        break;
                    case 2:
                        targetLane = 1;
                        break;
                    default:
                        targetLane = -3;
                        break;
                }
                Debug.Log("Moving Left from current lane: " + currentLane + " to target lane: " + targetLane);
            }
        }
        if(targetLane > 2 || targetLane < -2)
        {
            Fall();
            Invoke("Reset", 5);
        } else
        {
            if (movingRight)
            {
                MoveRight();
            }
            if (movingLeft)
            {
                MoveLeft();
            }
        }
        
    }

    private void Fall()
    {
        falling = true;
        gravity = 50f;
        targetVector3 = new Vector3(topTransform.position.x, topTransform.position.y, transform.position.z);
        targetQuaternion = topTransform.rotation;

        transform.position = Vector3.Lerp(transform.position, targetVector3, Time.deltaTime * transformLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * transformLerpSpeed);

    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool isGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        //Vector3 dir = new Vector3(0, -1);
        Vector3 dir = -transform.up;

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveRight()
    {
        switch (targetLane)
        {
            case -1:
                targetVector3 = new Vector3(leftTransform.position.x, leftTransform.position.y, transform.position.z);
                targetQuaternion = leftTransform.rotation;
                PlayMovementSound();
                break;
            case 0:
                targetVector3 = new Vector3(middleTransform.position.x, middleTransform.position.y, transform.position.z);
                targetQuaternion = middleTransform.rotation;
                PlayMovementSound();
                break;
            case 1:
                targetVector3 = new Vector3(rightTransform.position.x, rightTransform.position.y, transform.position.z);
                targetQuaternion = rightTransform.rotation;
                PlayMovementSound();
                break;
            case 2:
                targetVector3 = new Vector3(topRightTransform.position.x, topRightTransform.position.y, transform.position.z);
                targetQuaternion = topRightTransform.rotation;
                PlayMovementSound();
                break;
            default:
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetVector3, Time.deltaTime * transformLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * transformLerpSpeed);

        if(Vector3.Distance(transform.position, targetVector3) < 0.01)
        {
            movingRight = false;
            currentLane = targetLane;
        }
    }

    private void MoveLeft()
    {
        switch (targetLane)
        {
            case -2:
                targetVector3 = new Vector3(topLeftTransform.position.x, topLeftTransform.position.y, transform.position.z);
                targetQuaternion = topLeftTransform.rotation;
                PlayMovementSound();
                break;
            case -1:
                targetVector3 = new Vector3(leftTransform.position.x, leftTransform.position.y, transform.position.z);
                targetQuaternion = leftTransform.rotation;
                PlayMovementSound();
                break;
            case 0:
                targetVector3 = new Vector3(middleTransform.position.x, middleTransform.position.y, transform.position.z);
                targetQuaternion = middleTransform.rotation;
                PlayMovementSound();
                break;
            case 1:
                targetVector3 = new Vector3(rightTransform.position.x, rightTransform.position.y, transform.position.z);
                targetQuaternion = rightTransform.rotation;
                PlayMovementSound();
                break;
            default:
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetVector3, Time.deltaTime * transformLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * transformLerpSpeed);

        if (Vector3.Distance(transform.position, targetVector3) < 0.01)
        {
            movingLeft = false;
            currentLane = targetLane;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exiting " + other.gameObject.name);
        if (other.gameObject.CompareTag("Pipe"))
        {
            other.gameObject.GetComponent<EndlessPipes>().MoveToFront();
        }

    }


    public void PlayMovementSound()
    {
        movementSound.Play();
    }

}
