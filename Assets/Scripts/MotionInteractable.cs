using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate(Transform input);
    void Deactivate();
}

public class MotionInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject UiPrompt;
    private GameObject uiPrompt_temp;
    private bool activated = false;
    [SerializeField] private GameObject pivotPoint;
    private Transform playerHand;
    private enum RotationType{
        HORIZONTALROT, VERTICALROT
    }
    [SerializeField] private RotationType rotationType;

    private void FixedUpdate()
    {
        if(activated)
        {
            switch (rotationType)
            {
                case RotationType.HORIZONTALROT:
                    pivotPoint.transform.Rotate(transform.right, playerHand.rotation.x);
                    break;
                case RotationType.VERTICALROT:
                    pivotPoint.transform.Rotate(transform.up, playerHand.rotation.z);
                    break;
                default: return;
            }
        }
    }

    public void Activate(Transform input)
    {
        if(!activated)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            playerHand = input;
            activated = true;
        }
    }

    public void Deactivate()
    {
        if (activated)
        {
            Destroy(uiPrompt_temp);
            activated = false;
            playerHand = null;
        }
    }
}