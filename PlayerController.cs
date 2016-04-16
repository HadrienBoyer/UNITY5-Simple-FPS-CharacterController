using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MinYaw = -360;
    public float MaxYaw = 360;
    public float MinPitch = -60;
    public float MaxPitch = 60;
    public float LookSensitivity = 1;

    public float MoveSpeed = 10;
    public float SprintSpeed = 30;
    private float currMoveSpeed = 0;

    public float TurnSpeed = 90;

    protected CharacterController movementController;
    protected Camera playerCamera;

    protected bool isControlling;
    protected float yaw;
    protected float pitch;
	
    protected Vector3 velocity;


    protected virtual void Start() {

        movementController = GetComponent<CharacterController>();   //  Character Controller
        playerCamera = GetComponentInChildren<Camera>();            //  Player Camera

        isControlling = true;
        ToggleControl();    //  Toggle Player control
    }

	protected virtual void Update() {

        if (Input.GetKeyDown(KeyCode.R))
                    transform.position = new Vector3(3, 0, 0); // Hit "R" to spawn in this position

        Vector3 direction = Vector3.zero;
        direction += transform.forward * Input.GetAxisRaw("Vertical");
        direction += transform.right * Input.GetAxisRaw("Horizontal");

        direction.Normalize();

		if(movementController.isGrounded)
			velocity = Vector3.zero;
		else
			velocity += -transform.up * (9.81f * 10) * Time.deltaTime; // Gravity


        if (Input.GetKey(KeyCode.LeftShift))  // Player can sprint by holding "Left Shit" keyboard button
        {
                currMoveSpeed = SprintSpeed;
        }else{
                currMoveSpeed = MoveSpeed;
        }

		direction += velocity * Time.deltaTime;
        movementController.Move(direction * Time.deltaTime * currMoveSpeed);

        // Camera Look
        yaw += Input.GetAxisRaw("Mouse X") * LookSensitivity;
        pitch += Input.GetAxisRaw("Mouse Y") * LookSensitivity;

        yaw = ClampAngle(yaw, MinYaw, MaxYaw);
        pitch = ClampAngle(pitch, MinPitch, MaxPitch);

        transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);
        playerCamera.transform.localRotation = Quaternion.AngleAxis(pitch, -Vector3.right);
	}

    protected float ClampAngle(float angle) {
        return ClampAngle(angle, 0, 360);
    }

    protected float ClampAngle(float angle, float min, float max) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    protected void ToggleControl() {

		playerCamera.gameObject.SetActive(isControlling);

        #if UNITY_5
            Cursor.lockState = (isControlling) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isControlling;
        #else
            Screen.lockCursor = isControlling;
        #endif
            
    }


}