using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate(GameObject[] inputs);
    void Deactivate();
}

public class MotionInteractable : MonoBehaviour, IInteractable
{
    [HeaderAttribute("GENERAL SETUP", order = 0)]
    [SerializeField] private GameObject UiPrompt;
    private GameObject uiPrompt_temp;
    [SerializeField] private DoorController activatingDoor;
    private bool activated = false;
    bool puzzleSuccess = false;

    [HeaderAttribute("INTERFACE DATA", order = 1)]
    [SerializeField] private PlayerController playerRef;
    [SerializeField] private GameObject handRef;
    [SerializeField] private Transform playerHand;

    [HeaderAttribute("ROTATION DATA", order = 2)]
    [SerializeField] private GameObject pivotPoint;
    private Rigidbody pivotRb;
    [SerializeField] private Transform directionRef;
    [SerializeField][Range(0.0f, 360.0f)] private float successAngle = 35.0F; // WARNING: Rotations from forward position wrap from 0 to 360!
    [SerializeField][Range(0.0f, 360.0f)] private float minAngle;
    [SerializeField][Range(0.0f, 360.0f)] private float maxAngle;
    private enum RotationType
    {
        HORIZONTALROT, VERTICALROT, TESTROT
    }
    [SerializeField] private RotationType rotationType;

    void Awake()
    {
        pivotRb = pivotPoint.GetComponent<Rigidbody>();
    }

    public void Activate(GameObject[] inputs)
    {
        if (!activated && !puzzleSuccess)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            handRef = inputs[0];
            playerHand = inputs[0].transform;
            playerRef = inputs[1].gameObject.GetComponent<PlayerController>();
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
                case RotationType.TESTROT: // Up-down test rotation
                    Vector3 targetDir = Vector3.ProjectOnPlane(playerHand.position - pivotRb.transform.position, directionRef.forward);
                    pivotRb.transform.up = targetDir; // Project on a plane and set the up transform to the result

                    Vector3 clampedRot = new Vector3 // Clamp the appropriate axis between a min and max angle
                        (pivotRb.transform.localRotation.eulerAngles.x, 
                        pivotRb.transform.localRotation.eulerAngles.y, 
                        Mathf.Clamp(pivotRb.transform.localRotation.eulerAngles.z, minAngle, maxAngle));
                    pivotRb.MoveRotation(Quaternion.Euler(clampedRot));

                    bool approxValue = Math.Abs(pivotRb.transform.localRotation.eulerAngles.z - successAngle) < 2.0F; 
                    if (approxValue && !puzzleSuccess)
                    {
                        activatingDoor.AddToActivated(1);
                        playerRef.inputs.m_joyconL.SetRumble(160, 320, 0.6f, 200);
                        playerRef.inputs.m_joyconR.SetRumble(160, 320, 0.6f, 200);
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