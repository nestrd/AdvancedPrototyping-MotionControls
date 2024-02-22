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

    [SerializeField] private GameObject UiPrompt;
    private GameObject uiPrompt_temp;
    private bool uiEnabled = false;

    private void Awake()
    {
        //Physics.IgnoreLayerCollision(6, 8, true);
    }

    public void Activate()
    {
        Debug.Log("Activated interactable");
        if(!uiEnabled)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            uiEnabled = true;
        }
    }

    public void Deactivate()
    {
        Debug.Log("Deactivated interactable");
        Destroy(uiPrompt_temp);
        uiEnabled = false;
    }
}
