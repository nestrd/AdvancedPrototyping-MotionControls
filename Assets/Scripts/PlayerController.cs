using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [HideInInspector] public JoyconInputs inputs;
    
    // Player camera
    private GameObject playerCamera;
    [SerializeField] private Transform playerCameraPos;
    [Range(0.0f, 100.0f)] public float cameraSensitivity = 2.0f;
    private float xLookRot;
    private float yLookRot;

    // Player physics
    private CharacterController charController;
    [Range(0.0f, 100.0f)] public float movementSpeed = 50.0f;

    // Player arms
    [SerializeField] private GameObject armLeft;
    [SerializeField] private GameObject fingersLeft;
    [SerializeField] private GameObject armRight;
    [SerializeField] private GameObject pingToken;
    int pingCount = 0;
    [HideInInspector] public bool interacting;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
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

            if (!interacting) // When interacting with 'MotionInteractable' objects, look and movement are disabled to prevent frustrating gameplay experiences
            {
                UpdateLook();
                UpdateMovement();
                GoHerePing();
            }

            UpdateArmRotations();
            ResetHandPositions();

        }

    }

    private void UpdateLook()
    {
        float inputX = inputs.m_joyconR.GetStick()[1] * Time.deltaTime * cameraSensitivity * 5;
        float inputY = inputs.m_joyconR.GetStick()[0] * Time.deltaTime * cameraSensitivity * 5;
        xLookRot += inputX;
        yLookRot -= inputY;

        playerCamera.transform.rotation = Quaternion.Euler(Mathf.Clamp(-xLookRot, -90f, 90f), -yLookRot, 0); //cam
        transform.rotation = Quaternion.Euler(0, -yLookRot, 0); //body
    }

    private void UpdateMovement()
    {
        Vector3 direction = (charController.transform.forward * inputs.m_joyconL.GetStick()[1]) + (charController.transform.right * inputs.m_joyconL.GetStick()[0]);
         
        //playerRB.AddForce(10000 * movementSpeed * Time.deltaTime * direction.normalized, ForceMode.Force);
        charController.SimpleMove(movementSpeed * direction.normalized);
    }

    private void UpdateArmRotations()
    {
        armLeft.transform.localRotation = new Quaternion(inputs.m_joyconL.GetVector().x, 0, -inputs.m_joyconL.GetVector().z, inputs.m_joyconL.GetVector().w);
        armRight.transform.localRotation = new Quaternion(inputs.m_joyconR.GetVector().x, 0, -inputs.m_joyconR.GetVector().z, inputs.m_joyconR.GetVector().w);
    }

    private void ResetHandPositions() // Reorientates the 'forward pose' of the joycons during play, helpful for realigning drifted/offset motion controls
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

    private void GrabDropObject()
    {
        if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2)
        {
            // Grab with left hand

        }
        else
        {
            // Drop with left hand
        }

        if (inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2)
        {
            // Grab with right hand
        }
        else
        {
            // Drop with right hand
        }
    }

    private void GoHerePing() // Fix this!! Currently the transform is totally broken for hand pointing and raycasts do not always fire for some reason.
    {
        if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_1 || inputs.m_pressedButtonR == Joycon.Button.SHOULDER_1)
        {
            var agent = FindObjectOfType<AiController>(); // Replace with solid reference

            if (Physics.Raycast(fingersLeft.transform.position, fingersLeft.transform.right, out RaycastHit ray, 50.0F, 1)) // Check ground layer object to ping NPC to move to
            {
                Debug.DrawLine(fingersLeft.transform.position, fingersLeft.transform.right * 50.0F, Color.green, 5.0F);
                agent.AgentGoTo(ray.transform.position);
                if (pingCount == 0)
                {
                    Instantiate(pingToken, ray.transform.position, Quaternion.identity);
                    pingCount += 1;
                }
            }
        }
        else 
        { 
            pingCount = 0; 
        }
    }
}

