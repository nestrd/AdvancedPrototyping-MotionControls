using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePad : MonoBehaviour 
{
    GameObject characterRef;
    Animator animRef;
    [SerializeField] private DoorController activatingDoor;
    private bool canActivate;

    private void Awake()
    {
        animRef = GetComponent<Animator>();
    }

    private void Start()
    {
        activatingDoor.Activator();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("Grabbable"))
        {
            characterRef = collision.gameObject;
            Debug.Log("Stepped on");
            if (!canActivate)
            {
                activatingDoor.AddToActivated(1);
                canActivate = true;
                animRef.SetBool("Active", true);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == characterRef)
        {
            characterRef = null;
            if (canActivate)
            {
                activatingDoor.AddToActivated(-1);
                canActivate = false;
                animRef.SetBool("Active", false);
            }
        }
    }
}
