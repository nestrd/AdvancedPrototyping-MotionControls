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
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.GetComponent<Rigidbody>().MovePosition(handPivot.transform.position);
            Physics.IgnoreLayerCollision(6, 7, true);
            Physics.IgnoreLayerCollision(6, 8, true);
        }
    }

    public void DropWithHand()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            Physics.IgnoreLayerCollision(6, 7, false);
            Physics.IgnoreLayerCollision(6, 8, false);
        }
    }

}
