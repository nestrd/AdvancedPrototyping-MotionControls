using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public JoyconInputs inputs;
    [SerializeField] private Rigidbody playerRB;
    [Range(0.0f, 100.0f)] public float cameraSensitivity = 2.0f;
    [Range(0.0f, 100.0f)] public float movementSpeed = 2.0f;
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

        //inputs.gyro = new Vector3(0, 0, 0);
        //accel = new Vector3(0, 0, 0);
    }

    void FixedUpdate()
    {
        if (inputs.m_joycons != null)
        {
            if (inputs.m_pressedButtonR == Joycon.Button.DPAD_RIGHT)
            {
                Debug.Log("Interact button pressed");
            }


            UpdateLook();
            UpdateMovement();
            UpdateArms();

        }

    }

    private void UpdateLook()
    {
        //playerCamera.transform.position = playerCameraPos.position;

        inputs.m_joyconR.Recenter();

        var inputX = inputs.m_joyconR.GetStick()[1] * Time.deltaTime * cameraSensitivity;
        var inputY = inputs.m_joyconR.GetStick()[0] * Time.deltaTime * cameraSensitivity;
        xRot += inputX;
        yRot -= inputY;

        playerCamera.transform.rotation = Quaternion.Euler(-xRot, -yRot, 0); //cam
        transform.rotation = Quaternion.Euler(0, -yRot, 0); //body

        Mathf.Clamp(yRot, -90f, 90f);
    }

    private void UpdateMovement()
    {
        var direction = (playerRB.transform.forward * inputs.m_joyconL.GetStick()[1]) + (playerRB.transform.right * inputs.m_joyconL.GetStick()[0]);

        playerRB.AddForce(direction * movementSpeed * Time.deltaTime * 2000, ForceMode.Force);
        if (direction.magnitude > 1f)
        {
            direction = direction.normalized * movementSpeed;
        }
    }

    private void UpdateArms()
    {
        Quaternion rotL = Quaternion.Euler(inputs.m_joyconL.GetGyro().z, inputs.m_joyconL.GetGyro().x, inputs.m_joyconL.GetGyro().y);
        armLeft.transform.rotation = armLeft.transform.rotation * rotL;

        Quaternion rotR = Quaternion.Euler(inputs.m_joyconR.GetGyro().z, -inputs.m_joyconR.GetGyro().x, -inputs.m_joyconR.GetGyro().y);
        armRight.transform.rotation = armRight.transform.rotation * rotR;
    }
}

