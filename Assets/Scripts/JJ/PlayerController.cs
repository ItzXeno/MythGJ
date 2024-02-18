using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private int health = 1000;
    public float moveSpeed = 5f; // Adjust this to change movement speed
    public int points;

    bool canMove = true;
    bool isAlive = true;

    [SerializeField] float whenDamageIsTaken = .1f;
    [SerializeField] float tiltTimeDamageIntervals = .5f;
    [SerializeField] bool pressPunish = false;

    float damageCD = 0;

    [SerializeField] bool isTilting;
    [SerializeField] float tiltSpeed = 3f;
    float momentumIncreaseSpeed = 1f;
    [SerializeField] float tiltCorrectionSpeed = 3f;

    [SerializeField] float tiltDirection = 0f;
    [SerializeField]float wingTransitionSpeed = 1f;
    [SerializeField] float pressTiltPunish = .1f;

    [SerializeField] SpriteRenderer leftWing;
    [SerializeField] SpriteRenderer rightWing;

    float leftMomentum; // the more u hold the direction the more tilt
    float rightMomentum;

    float TiltSpeed { get { return tiltSpeed; } set { tiltSpeed = value; }}
    float TiltCorrectionSpeed { get { return tiltCorrectionSpeed / 10; } set { tiltCorrectionSpeed = value; } }
    float MomentumIncreaseSpeed { get { return momentumIncreaseSpeed / 10; } set { momentumIncreaseSpeed = value; } }
    float TiltDirection { get { return tiltDirection; } set { tiltDirection = Mathf.Clamp(value, -1, 1); } }
   
    public int Health
    {
        get { return health; }
        set
        {
            if(health > value){
                print("Lost HP: " + value);
            }
            else
            {
                print("Gained HP: " + value);
            }

            if (value <= 0) {
                Death(); } 
            else { 
                health = value; 
            }
        }
    }

    Rigidbody2D rb; // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageCD = tiltTimeDamageIntervals;
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

    public float Normalize(float value, float sourceMin, float sourceMax, float targetMin, float targetMax)
    {
        return ((value - sourceMin) / (sourceMax - sourceMin)) * (targetMax - targetMin) + targetMin;
    }

    void LeftWingColorLerp()
    {
        // Calculate the target color based on tiltDirection
        Color targetColor = Color.Lerp(Color.white, Color.black, Normalize(TiltDirection, -1, 1, 1, -1) );//(TiltDirection + 1f) / 2f

        // Smoothly transition the color of the left wing
        leftWing.color = Color.Lerp(leftWing.color, targetColor, 1);//wingTransitionSpeed * Time.deltaTime

    }

    void RightWingColorLerp()
    {
        // Calculate the target color based on tiltDirection
        Color targetColor = Color.Lerp(Color.white, Color.black, Normalize(TiltDirection, -1, 1, -1, 1));//(TiltDirection + 1f) / 2f

        // Smoothly transition the color of the left wing
        rightWing.color = Color.Lerp(leftWing.color, targetColor, 1);//wingTransitionSpeed * Time.deltaTime

    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        //isTilting = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ? true : false;

        if (pressPunish && isAlive)
        {
            if (Input.GetKeyDown(KeyCode.A)) { TiltDirection -= pressTiltPunish; }
            if (Input.GetKeyDown(KeyCode.D)) { TiltDirection += pressTiltPunish; }
        }

        if (canMove)
        {
            if (!Input.GetKey(KeyCode.A)) { leftMomentum = 0f; }
            if (!Input.GetKey(KeyCode.D)) { rightMomentum = 0f; }

            TiltCorrection(dt);
            LeftWingColorLerp();
            RightWingColorLerp();

        }
        if (isAlive)
        {
            if (TiltDirection >= 1 - whenDamageIsTaken || TiltDirection <= -1 + whenDamageIsTaken)
            {
                damageCD -= 1 * Time.deltaTime;
                if (damageCD <= 0)
                {
                    damageCD = tiltTimeDamageIntervals;
                    DealDamage(1);
                }
            }
            else
            {
                damageCD = tiltTimeDamageIntervals;
            }

        }

        //print(TiltDirection);
    }

    public void DealDamage(int damage)
    {
        Health -= damage;
    }
    void Death()
    {
        isAlive = false;
        canMove = false;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {


            //float moveHorizontal = Input.GetAxis("Horizontal");
            //float moveVertical = Input.GetAxis("Vertical");

            // Can clamp these to reduce the drift
            //float moveHorizontal = 0 ;

            //if (Input.GetKey(KeyCode.A)){
            //    moveHorizontal -= 1f;
            // }

            //if (Input.GetKey(KeyCode.D))
            //{
            //    moveHorizontal += 1f;
            //}

            //moveHorizontal = Mathf.Clamp(moveHorizontal, -1, 1);

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Check if both A and D keys are pressed
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                moveHorizontal = 0f; // Don't move horizontally
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                moveVertical = 0f; // Don't move horizontally
            }


            // Calculate movement direction based on input
            Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

            isTilting = movement.x > 0 || movement.x < 0 ? true : false;
            if (isTilting) { print(isTilting); }

            float dt = Time.deltaTime;

            if (movement.x != 0)
            {
                if (movement.x < 0)
                {
                    TiltLeft(dt);
                    if (!Input.GetKey(KeyCode.D))
                    {
                        //leftMomentum += momentumIncreaseSpeed * dt;
                    }
                    else
                    {
                        leftMomentum = 0f;
                    }
                }


                if (movement.x > 0)
                {
                    TiltRight(dt);
                    if (!Input.GetKey(KeyCode.A))
                    {
                        //rightMomentum += momentumIncreaseSpeed * dt;
                    }
                    else
                    {
                        rightMomentum = 0f;
                    }
                }
            }

            // Move the character
            transform.position += new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.fixedDeltaTime;
            // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            //rb.velocity = movement * moveSpeed;
        }
    }
}
