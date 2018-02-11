using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlDoggy : MonoBehaviour {


    public Transform m_Cam;                
    bool isGrounded;
    bool isMoving;

    [Header("Stats")]
    public float walkSpeed = 10.0f;
    public float runSpeed = 60.0f;
    public float speedIncrease = 8.5f;
    public bool runTog;
    public string AnimState;

    Animator anim;
    public CharacterController characterController;

    void Start () 
    {
        anim = GetComponent<Animator>();
        m_Cam = Camera.main.transform;
        characterController = GetComponentInChildren<CharacterController>();
	}
	
	void Update () 
    {
        Animate();

        UpdateMotor();
        GetLocomotionInput();
        isGrounded = characterController.isGrounded;
    }
        
    float moveVecFloat;
    public bool moveBackwards;

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
    }


    void GetLocomotionInput()
    {
        var deadZone = 0.0f;

        verticalVelocity = MoveVector.y;
        MoveVector = Vector3.zero;

        if (Input.GetAxis("Vertical") > deadZone)
        {
            MoveVector += new Vector3(0, 0, Input.GetAxis("Vertical"));
            isMoving = true;
        }

        if (Input.GetAxis("Vertical") < -deadZone)
        {
            MoveVector += new Vector3(0, 0, -Input.GetAxis("Vertical"));
            isMoving = true;
        }

        if (Input.GetAxis("Horizontal") > deadZone)
        {
            MoveVector += new Vector3(0, 0, Input.GetAxis("Horizontal"));
            isMoving = true;
        }

        if (Input.GetAxis("Horizontal") < -deadZone)
        {
            MoveVector += new Vector3(0, 0, -Input.GetAxis("Horizontal"));
            isMoving = true;
        }
    }


  
    public float moveSpeed = 10.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = 40f;
    public float terminalVelocity = 90f;
    public bool canSlide = true;
    public float slideThreshhold = 0.6f;
    public float maxControllableSlideMagnitude = 0.4f;

    Vector3 slideDirection;

    Vector3 MoveVector { get; set; }

    public float rotateSpeed = 100f;
    public Vector3 targetDirection;

    float verticalVelocity { get; set; }

    public float targetSpeed = 10.0f;
    float v;
    float h;

    void UpdateMotor()
    {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
    }


    void ProcessMotion()
    {
            //Transform MoveVector to World Space
            MoveVector = transform.TransformDirection(MoveVector);

            // Normalize our moveVector is Magnitude > 1
            if (MoveVector.magnitude > 1)
                MoveVector = Vector3.Normalize(MoveVector);

            //apply slidipudding if apliccklelabele im so bORED omg
            ApplySlide();


            //Multiply MoveVector by MoveSpeed
            MoveVector *= moveSpeed;

            //Reapply VerticalVelocity movevector.y
            MoveVector = new Vector3(MoveVector.x, verticalVelocity, MoveVector.z);

            // Move the Character in World Space
            characterController.Move(MoveVector * Time.deltaTime);


            ///if (Input.GetKeyDown (KeyCode.LeftShift))
            //  moveSpeed = 30.0f;
    }
        

    void ApplySlide()
    {
            if (canSlide == true)
            {
                if (!characterController.isGrounded)
                    return;

                slideDirection = Vector3.zero;

                RaycastHit hitInfo;

                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo))
                {
                    if (hitInfo.normal.y < slideThreshhold)
                        slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
                }

                if (slideDirection.magnitude < maxControllableSlideMagnitude)
                    MoveVector += slideDirection;
                else
                {
                    MoveVector = slideDirection;
                }
            }

    }

    public void Jump()
    {
        if (isGrounded)
        {
            verticalVelocity = jumpSpeed;
        }
    }

   
    void SnapAlignCharacterWithCamera()
    {
            //Vector3 forward = Camera.main.transform.forward; //forward axis is determined by the camera
            Vector3 forward = transform.forward; //forward axis is determined by the character's orientation
            forward.y = 0;
            forward = forward.normalized;


            // Right vector relative to the camera
       
            // Always orthogonal to the forward vector
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");


                targetDirection = h * right + v * forward;
            // Smooth the speed based on the current target direction

            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
            //moveDirection = Vector3.zero;
            Vector3 moveDirection = transform.TransformDirection(Vector3.forward);

            if (targetDirection != Vector3.zero)
            {
                moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                moveDirection = moveDirection.normalized;
            }
            float verticalSpeed = 0.0f;

            Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);
            movement *= Time.deltaTime;


            transform.rotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);
        }

    void Animate()
    {

        if (Input.GetKey(KeyCode.LeftShift)) //hold shift to run
            runTog = true;
        else
            runTog = false;

        if (!isMoving) // if we arent moving at all
        {
                if (moveSpeed > 0)
                    moveSpeed -= speedIncrease * Time.deltaTime;
                else
                    moveSpeed = 0;
        }


        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if (runTog)
            {
                    if (moveSpeed > runSpeed)
                        moveSpeed = runSpeed;
                    else
                        moveSpeed = moveSpeed + speedIncrease * Time.deltaTime;

                if (AnimState != "Run") //this is to prevent it from calling twice
                    AnimState = "Run";
            }
        else
            { 
                    if (moveSpeed < walkSpeed)
                    {
                        moveSpeed = moveSpeed + 9f * Time.deltaTime;
                    }
                    else if (moveSpeed > walkSpeed)
                        moveSpeed = walkSpeed;

                if (AnimState != "Walk") //this is to prevent it from calling twice
                    AnimState = "Walk";
            }

        }
        else
        {
            isMoving = false;
            if (AnimState != "Idle")  //this is to prevent it from calling twice
                AnimState = "Idle";
        }

        //NOTE THIS IS EXTREMELY RUDIMENTARY,  YOU CAN ADJUST THIS TO LOOK BETTER

        anim.SetInteger("AnimState", AnimState == "Idle" ? 0 : AnimState == "Walk" ? 1 : 2);

    }


}
