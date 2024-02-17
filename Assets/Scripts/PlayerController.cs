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
    private GameObject playerCamera;
    [SerializeField] private Transform playerCameraPos;
    private float xRot;
    private float yRot;

    [SerializeField] private GameObject armLeft;
    [SerializeField] private GameObject armRight;
    private CharacterController charController;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
        playerRB = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>().gameObject;
        charController = GetComponent<CharacterController>();
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
        float inputX = inputs.m_joyconR.GetStick()[1] * Time.deltaTime * cameraSensitivity * 5;
        float inputY = inputs.m_joyconR.GetStick()[0] * Time.deltaTime * cameraSensitivity * 5;
        xRot += inputX;
        yRot -= inputY;

        playerCamera.transform.rotation = Quaternion.Euler(Mathf.Clamp(-xRot, -90f, 90f), -yRot, 0); //cam
        transform.rotation = Quaternion.Euler(0, -yRot, 0); //body
    }

    private void UpdateMovement()
    {
        Vector3 direction = (charController.transform.forward * inputs.m_joyconL.GetStick()[1]) + (charController.transform.right * inputs.m_joyconL.GetStick()[0]);
         
        //playerRB.AddForce(10000 * movementSpeed * Time.deltaTime * direction.normalized, ForceMode.Force);
        charController.SimpleMove(movementSpeed * direction.normalized);
    }

    private void UpdateHands()
    {
        armLeft.transform.localRotation = new Quaternion(inputs.m_joyconL.GetVector().x, 0, -inputs.m_joyconL.GetVector().z, inputs.m_joyconL.GetVector().w);
        armRight.transform.localRotation = new Quaternion(inputs.m_joyconR.GetVector().x, 0, -inputs.m_joyconR.GetVector().z, inputs.m_joyconR.GetVector().w);
    }

    private void ResetHandPositions() // Unsure if it works
    {
        if (inputs.m_pressedButtonL == Joycon.Button.DPAD_DOWN)
        {
            inputs.m_joyconL.Recenter();
        }
        if (inputs.m_pressedButtonR == Joycon.Button.DPAD_DOWN)
        {
            inputs.m_joyconR.Recenter();
        }
    }

}

