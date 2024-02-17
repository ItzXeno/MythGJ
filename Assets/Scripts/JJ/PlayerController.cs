using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to change movement speed


    [SerializeField] bool isTilting;
    [SerializeField] float tiltSpeed = 3f;
    float momentumIncreaseSpeed = 1f;
    [SerializeField] float tiltCorrectionSpeed = 3f;

    [SerializeField] float tiltDirection = 0f;

    float leftMomentum; // the more u hold the direction the more tilt
    float rightMomentum;

    float TiltSpeed { get { return tiltSpeed; } set { tiltSpeed = value; }}
    float TiltCorrectionSpeed { get { return tiltCorrectionSpeed / 10; } set { tiltCorrectionSpeed = value; } }
    float MomentumIncreaseSpeed { get { return momentumIncreaseSpeed / 10; } set { momentumIncreaseSpeed = value; } }
    float TiltDirection { get { return tiltDirection; } set { tiltDirection = Mathf.Clamp(value, -1, 1); } }


    Rigidbody2D rb; // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void TiltLeft(float dt)
    {
        TiltDirection -= (TiltSpeed + leftMomentum) * dt;
    }

    void TiltRight(float dt)
    {
        TiltDirection += (TiltSpeed + rightMomentum) * dt;
    }

    void TiltCorrection(float dt)
    {
        if (!isTilting)
        {
            if(TiltDirection < 0)
            {
                tiltDirection = Mathf.Clamp((tiltDirection + TiltCorrectionSpeed * dt) , -1, 0);
            }
            else
            {
                tiltDirection = Mathf.Clamp((tiltDirection - TiltCorrectionSpeed * dt) , 0, 1);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        isTilting = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ? true : false;
        
        if (isTilting)
        {
            if (Input.GetKey(KeyCode.A))
            {
                TiltLeft(dt);
                if (!Input.GetKey(KeyCode.D))
                {
                    leftMomentum += momentumIncreaseSpeed * dt;
                }
                else
                {
                    leftMomentum = 0f;
                }
            }


            if (Input.GetKey(KeyCode.D))
            {
                TiltRight(dt);
                if (!Input.GetKey(KeyCode.A))
                {
                    rightMomentum += momentumIncreaseSpeed * dt;
                }
                else
                {
                    rightMomentum = 0f;
                }
            }
        }

        if (!Input.GetKey(KeyCode.A)) { leftMomentum = 0f; } 
        if (!Input.GetKey(KeyCode.D)) { rightMomentum = 0f; }

        TiltCorrection(dt);

        print(TiltDirection);
    }

    void FixedUpdate()
    {

        // Can clamp these to reduce the drift
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // Move the character
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
