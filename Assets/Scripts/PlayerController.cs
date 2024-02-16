using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public JoyconInputs inputs;
    [SerializeField] private Rigidbody playerRB;
    [Range(0.0f, 100.0f)] public float cameraSensitivity = 2.0f;
    [Range(0.0f, 100.0f)] public float movementSpeed = 50.0f;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform playerCameraPos;
    private float xRot;
    private float yRot;

    [SerializeField] private GameObject armLeft;
    [SerializeField] private GameObject armRight;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
        playerRB = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        if (inputs.m_joycons != null && inputs.m_joyconL != null) // remove second half if broken!
        {
            if (inputs.m_pressedButtonR == Joycon.Button.DPAD_RIGHT)
            {
                Debug.Log("Interact button pressed");
            }

            UpdateLook();
            UpdateMovement();
            UpdateHands();
            ResetHandPositions();

        }

    }

    private void UpdateLook()
    {
        //playerCamera.transform.position = playerCameraPos.position;

        //inputs.m_joyconR.Recenter();

        float inputX = inputs.m_joyconR.GetStick()[1] * Time.deltaTime * cameraSensitivity * 5;
        float inputY = inputs.m_joyconR.GetStick()[0] * Time.deltaTime * cameraSensitivity * 5;
        xRot += inputX;
        yRot -= inputY;

        playerCamera.transform.rotation = Quaternion.Euler(Mathf.Clamp(-xRot, -90f, 90f), -yRot, 0); //cam
        transform.rotation = Quaternion.Euler(0, -yRot, 0); //body

    }

    private void UpdateMovement()
    {
        Vector3 direction = (playerRB.transform.forward * inputs.m_joyconL.GetStick()[1]) + (playerRB.transform.right * inputs.m_joyconL.GetStick()[0]);

        playerRB.AddForce((direction.normalized) * movementSpeed * Time.deltaTime * 10000, ForceMode.Force);
    }

    private void UpdateHands()
    {
        float zRotL = inputs.m_joyconL.GetGyro().z;
        float yRotL = inputs.m_joyconL.GetGyro().y;
        Quaternion rotL = Quaternion.Euler(zRotL, 0, yRotL);
        armLeft.transform.rotation = armLeft.transform.rotation * rotL;

        float zRotR = -inputs.m_joyconR.GetGyro().z;
        float yRotR = -inputs.m_joyconR.GetGyro().y;
        Quaternion rotR = Quaternion.Euler(zRotR, 0, yRotR);
        armRight.transform.rotation = armRight.transform.rotation * rotR;
        
    }

    private void ResetHandPositions() // Does not work.
    {
        if (inputs.m_pressedButtonL == Joycon.Button.CAPTURE || inputs.m_pressedButtonL == Joycon.Button.HOME)
        {
            armLeft.transform.rotation = new Quaternion(0, -90, 90, 0);
            armRight.transform.rotation = new Quaternion(0, 90, -90, 0);
        }

    }

}

