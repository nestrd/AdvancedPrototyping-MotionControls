using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public JoyconInputs inputs;
    private AiController agent;
    private bool controllerCheck = false;
    [SerializeField] private UiManager uiRef;
    private AudioSource footstepAudio;

    [HeaderAttribute("PLAYER CAMERA", order = 0)]
    [SerializeField] private Transform playerCameraPos;
    [SerializeField][Range(0.0f, 100.0f)] public float cameraSensitivity = 2.0f;
    private GameObject playerCamera;
    private float xLookRot;
    private float yLookRot;

    [HeaderAttribute("PHYSICS SETTINGS", order = 1)]
    [SerializeField][Range(0.0f, 100.0f)] public float movementSpeed = 50.0f;
    private CharacterController charController;

    [HeaderAttribute("ARMS/HANDS SETUP", order = 2)]
    [SerializeField] private HandController leftHand;
    [SerializeField] private GameObject ShoulderLeft;
    [SerializeField] private HandController rightHand;
    [SerializeField] private GameObject ShoulderRight;
    [SerializeField] private GameObject pingToken;
    private bool canPing = true;

    [HeaderAttribute("INTERACTION SYSTEM", order = 3)]
    [SerializeField] public GameObject interactingObject = null;
    [HideInInspector] public bool interacting;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
        playerCamera = GetComponentInChildren<Camera>().gameObject;
        charController = GetComponent<CharacterController>();
        agent = FindObjectOfType<AiController>();
        footstepAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (inputs.m_joycons != null && inputs.m_joyconL != null) // remove second half if broken!
        {
            if (!interacting) // When interacting with 'MotionInteractable' objects, look and movement are disabled to prevent frustrating gameplay experiences
            {
                UpdateMovement();
                GoHerePing();
            }

            UpdateLook();
            UpdateArmRotations();
            ResetHandPositions();
            GrabReleaseObject();

        }
        ControllerCheck();
        if(!controllerCheck)
        {
            Time.timeScale = 0F;
            
            uiRef.SetAnimationState(1);
        }
        else
        {
            Time.timeScale = 1F;
            uiRef.SetAnimationState(0);
        }
    }

    private void ControllerCheck()
    {
        IntPtr ptr = HIDapi.hid_enumerate(0x57e, 0x0);

        if (ptr == IntPtr.Zero)
        {
            ptr = HIDapi.hid_enumerate(0x057e, 0x0);
            if (ptr == IntPtr.Zero)
            {
                controllerCheck = false;
            }
            else
            {
                controllerCheck = true;
                Time.timeScale = 1F;
            }
        }
        else
        {
            controllerCheck = true;
            Time.timeScale = 1F;
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
        charController.SimpleMove(movementSpeed * direction.normalized);
        if(direction != Vector3.zero)
        {
            float pitchVariance = UnityEngine.Random.Range(0.3F, 0.55F);
            footstepAudio.pitch = pitchVariance;
            footstepAudio.enabled = true;
        }
        else
        {
            footstepAudio.enabled = false;
        }
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
        if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2) // Grab and drop with left hand
        {
            leftHand.GrabWithHand();
        }
        else
        {
            leftHand.DropWithHand();
        }
        if (inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2) // Grab and drop with right hand
        {
            rightHand.GrabWithHand();
        }
        else
        {
            rightHand.DropWithHand();
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