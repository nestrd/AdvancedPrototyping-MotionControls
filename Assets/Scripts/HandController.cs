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
            heldObject = null;
        }
    }
}

