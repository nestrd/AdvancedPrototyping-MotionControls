using UnityEngine;

public class HandController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private Transform upperArm;
    public GameObject heldObject;
    public GameObject handPivot;
    [SerializeField] private bool isLeft;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Interactive"))
        {
            if(isLeft)
            {
                playerController.inputs.m_joyconL.SetRumble(100, 100, 0.6f, 200);
            }
            if (!isLeft)
            {
                playerController.inputs.m_joyconR.SetRumble(100, 100, 0.6f, 200);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grabbable") || other.gameObject.CompareTag("Interactive"))
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
        if (heldObject != null)
        {
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), playerController.GetComponent<Collider>(), true);
            if (heldObject.GetComponent<MonoBehaviour>() is not IInteractable)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                heldObject.GetComponent<Rigidbody>().Move(handPivot.transform.position, handPivot.transform.rotation);
            }
            if (heldObject.GetComponent<MonoBehaviour>() is IInteractable)
            {
                heldObject.SendMessage("Activate", upperArm);
                //playerController.interacting = true;
            }
        }
    }
    public void DropWithHand()
    {
        if (heldObject != null)
        {
            if(heldObject.GetComponent<MonoBehaviour>() is not IInteractable)
            {
                Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), playerController.GetComponent<Collider>(), false);
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            if(heldObject.GetComponent<MonoBehaviour>() is IInteractable)
            {
                heldObject.SendMessage("Deactivate");
                playerController.interacting = false;
            }
            heldObject = null;
        }
    }
}