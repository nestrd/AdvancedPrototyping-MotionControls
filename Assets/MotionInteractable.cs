using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate();
    void Deactivate();
}

public class MotionInteractable : MonoBehaviour, IInteractable
{

    private void Awake()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    public void Activate()
    {
        Debug.Log("Activated interactable");
    }

    public void Deactivate()
    {
        Debug.Log("Deactivated interactable");
    }
}
