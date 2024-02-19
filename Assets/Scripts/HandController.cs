using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HandController : MonoBehaviour
{

    private PlayerController playerController;

    public GameObject heldObject;
    public GameObject handPivot;
    [SerializeField] private bool isLeft;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (playerController.inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2 && isLeft)
        {
            Debug.Log("Button pressed");
            GrabWithHand();

        }
        else
        {
            DropWithHand();
        }
        if (playerController.inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2 && !isLeft)
        {
            Debug.Log("Button pressed");
            GrabWithHand();
        }
        else
        {
            DropWithHand();
        }
        //if (playerController.inputs.m_pressedButtonL == Joycon.Button.SHOULDER_1 && isLeft)
        //{
        //    GoHere();
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable"))
        {
            heldObject = other.gameObject;
        }
        else
        {
            heldObject = null;
        }
    }

    public void GrabWithHand()
    {
        var newGrab = false;

        if (heldObject != null && !newGrab)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.GetComponent<Rigidbody>().MovePosition(handPivot.transform.position);
            //heldObject.transform.SetParent(handPivot.transform, false);
            Physics.IgnoreCollision(heldObject.GetComponent<BoxCollider>(), playerController.GetComponent<CharacterController>());
            heldObject.GetComponent<IInteractable>().Activate();
            heldObject.SendMessage("Activate");
            newGrab = true;
        }
    }

    public void DropWithHand()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            //heldObject.transform.parent = null;
            //heldObject.GetComponent<IInteractable>().Deactivate();
            heldObject.SendMessage("Deactivate");
        }
    }

    public void GoHere()
    {
        var agent = FindObjectOfType<AiController>();
        RaycastHit ray;
        int instantiateCount = 0;

        if (Physics.Raycast(transform.position, transform.right, out ray, 100.0F, 9))
        {
            Debug.DrawLine(transform.position, -transform.up * 100.0F, Color.green, 5.0F);
            Debug.DrawLine(transform.position, transform.forward * 100.0F, Color.red, 5.0F);
            Debug.DrawLine(transform.position, transform.right * 100.0F, Color.blue, 5.0F);

            agent.AgentGoTo(ray.transform.position);
            if(instantiateCount == 0)
            {
                //Instantiate(pingToken, ray.transform);
                instantiateCount += 1;
            }
        }
    }
}

