using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private PlayerController playerController;

    public GameObject heldObject;
    public GameObject handPivot;
    bool leftGrabbing;
    bool rightGrabbing;
    [SerializeField] private bool isLeft;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if(isLeft)
        {
            if (playerController.inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2)
            {
                leftGrabbing = true;
                Debug.Log("Button pressed");
                GrabWithHand();
            }
            else
            {
                leftGrabbing = false;
                DropWithHand();
            }
        }

        if (!isLeft)
        {
            if (playerController.inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2)
            {
                rightGrabbing = true;
                GrabWithHand();
            }
            else
            {
                rightGrabbing = false;
                DropWithHand();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") && leftGrabbing || rightGrabbing)
        {
            heldObject = other.gameObject;
        } else
        {
            heldObject = null;
        }
    }

    public void GrabWithHand()
    {
        if (heldObject != null)
        {
            heldObject.gameObject.transform.position = handPivot.transform.position;
        }
    }

    public void DropWithHand()
    {

    }
}
