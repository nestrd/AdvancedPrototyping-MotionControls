using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate(GameObject interactor);
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
    [SerializeField] Transform directionRef;
    float successAngle = 4.0F;
    bool puzzleSuccess = false;
    private enum RotationType
    {
        HORIZONTALROT, VERTICALROT, TESTROT
    }
    [SerializeField] private RotationType rotationType;
    [SerializeField] private float minAngle = -5;
    [SerializeField] private float maxAngle = -9;

    void Awake()
    {
        pivotRb = pivotPoint.GetComponent<Rigidbody>();
    }

    public void Activate(GameObject interactor)
    {
        if (!activated)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            playerHand = interactor.transform;
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
        if (activated && !puzzleSuccess)
        {
            switch (rotationType)
            {
                case RotationType.HORIZONTALROT:
                    var horzInput = Quaternion.Euler(-playerHand.localRotation.x, 0, 0);
                    float angleTemp = Quaternion.Angle(pivotPoint.transform.rotation, horzInput);
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
                case RotationType.TESTROT:
                    Vector3 targetDir = Vector3.ProjectOnPlane(playerHand.position - pivotRb.transform.position, directionRef.forward);
                    pivotRb.transform.up = targetDir;

                    Vector3 clampedDir = new Vector3
                        (Mathf.Clamp(targetDir.x, minAngle, maxAngle),
                        targetDir.y, targetDir.z);
                    pivotRb.rotation = Quaternion.Euler(clampedDir);

                    bool approxValue = Math.Abs((pivotRb.rotation.z * 360 ) - successAngle) < 0.1F;
                    if (approxValue)
                    {
                        Debug.Log("Success!");
                        puzzleSuccess = true;
                    }

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