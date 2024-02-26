using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate(Transform interactPoint);
    void Deactivate();
}

public class MotionInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject UiPrompt;
    private GameObject uiPrompt_temp;
    private bool activated = false;
    [SerializeField] private GameObject pivotPoint;
    private Rigidbody pivotRb;
    private Transform playerHand;
    private enum RotationType
    {
        HORIZONTALROT, VERTICALROT
    }
    [SerializeField] private RotationType rotationType;
    [SerializeField] private float minAngle = 45;
    [SerializeField] private float maxAngle = 115;

    void Awake()
    {
        pivotRb = pivotPoint.GetComponent<Rigidbody>();
    }

    public void Activate(Transform interactPoint)
    {
        if (!activated)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            playerHand = interactPoint;
            activated = true;
        }
    }

    public void Deactivate()
    {
        if (activated)
        {
            Destroy(uiPrompt_temp);
            activated = false;
            //playerHand = null;
        }
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            switch (rotationType)
            { 
                case RotationType.HORIZONTALROT:
                    var horzInput = Quaternion.Euler(-playerHand.localRotation.x, 0, 0);
                    float angleTemp = Quaternion.Angle(pivotPoint.transform.localRotation, horzInput);
                    if (angleTemp > minAngle)
                    {
                        RotateObject(true, horzInput);
                        Debug.Log("can rot");
                    }
                    if (angleTemp < maxAngle)
                    {
                        RotateObject(false, horzInput);
                    }
                    break;
                case RotationType.VERTICALROT:
                    var roundedVert = Mathf.Round(-playerHand.localRotation.z);
                    var vertInput = Quaternion.Euler(0, roundedVert, 0);
                    pivotRb.MoveRotation(transform.rotation * vertInput);
                    break;
                default: return;
            }
        }
    }

    private void RotateObject(bool directionA, Quaternion rot)
    {
        if (directionA)
        {
            pivotRb.MoveRotation(pivotPoint.transform.localRotation * rot);
        }
        else
        {
            var inverseRot = Quaternion.Inverse(rot);
            pivotRb.MoveRotation(pivotPoint.transform.localRotation * inverseRot);
        }
    }
}