using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [HideInInspector] public JoyconInputs inputs;
    private AiController agent;
    
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
    [SerializeField] private HandController leftHand;
    [SerializeField] private GameObject ShoulderLeft;
    [SerializeField] private HandController rightHand;
    [SerializeField] private GameObject ShoulderRight;
    [SerializeField] private GameObject pingToken;
    private bool canPing = true;
    [HideInInspector] public bool interacting;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
        playerCamera = GetComponentInChildren<Camera>().gameObject;
        charController = GetComponent<CharacterController>();
        agent = FindObjectOfType<AiController>();
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
            GrabReleaseObject();

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
        ShoulderLeft.transform.localRotation = new Quaternion(inputs.m_joyconL.GetVector().x, 0, -inputs.m_joyconL.GetVector().z, inputs.m_joyconL.GetVector().w);
        ShoulderRight.transform.localRotation = new Quaternion(inputs.m_joyconR.GetVector().x, 0, -inputs.m_joyconR.GetVector().z, inputs.m_joyconR.GetVector().w);
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

    private void GrabReleaseObject()
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

    private void GoHerePing() // Raycast outward to hitpoint from each hand. Hitpoint determines AI destination. 
    {
        if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_1 && canPing)
        {
            if (Physics.Raycast(leftHand.transform.position, -leftHand.transform.up, out RaycastHit ray, 100.0F)) // Check ground layer object to ping NPC to move to
            {
                //Debug.DrawLine(leftHand.transform.position, ray.point, Color.green, 2.0F);
                agent.AgentGoTo(ray.point);
                StartCoroutine(PingTimer());
            }
        }
        if(inputs.m_pressedButtonR == Joycon.Button.SHOULDER_1 && canPing)
        {
            if (Physics.Raycast(rightHand.transform.position, -rightHand.transform.up, out RaycastHit ray, 100.0F)) // Check ground layer object to ping NPC to move to
            {
                //Debug.DrawLine(rightHand.transform.position, ray.point, Color.green, 2.0F);
                agent.AgentGoTo(ray.point);
                StartCoroutine(PingTimer());
            }
        }
    }

    IEnumerator PingTimer()
    {
        canPing = false;
        var _pos = agent.GetComponent<NavMeshAgent>().destination + new Vector3 (0,5,0);
        Instantiate(pingToken, _pos, Quaternion.identity);
        yield return new WaitForSeconds(1.0F);
        canPing = true;
    } // Timer for when next ping token can be placed, reevaluates AI path destination and route.
}

