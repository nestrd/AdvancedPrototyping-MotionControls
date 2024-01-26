using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private JoyconInputs inputs;
    [SerializeField] private Rigidbody playerRB;
    [Range(0.0f, 5.0f)] public float cameraSensitivity = 2.0f;
    [Range(0.0f, 5.0f)] public float movementSpeed = 2.0f;

    [SerializeField] private GameObject armLeft;
    [SerializeField] private GameObject armRight;

    void Awake()
    {
        inputs = GetComponent<JoyconInputs>();
        playerRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (inputs.m_joycons != null)
        {
            if (inputs.m_pressedButtonR == Joycon.Button.DPAD_RIGHT)
            {
                Debug.Log("Interact button pressed");
            }
            if (inputs.m_pressedButtonL == Joycon.Button.SL)
            {
                Debug.Log("Left hand grab.");
            }
            if (inputs.m_pressedButtonR == Joycon.Button.SR)
            {
                Debug.Log("Right hand grab.");
            }

            #region Movement and camera look
            var direction = playerRB.transform.forward * inputs.m_joyconL.GetStick()[1];
            if (direction.magnitude > 1f) { direction = direction.normalized;}

            playerRB.position = transform.position + direction * movementSpeed;
            var camRotation = Quaternion.Euler(playerRB.transform.eulerAngles.x, playerRB.transform.eulerAngles.y, 0);
            playerRB.transform.eulerAngles = camRotation.eulerAngles + new Vector3(-inputs.m_joyconR.GetStick()[1], inputs.m_joyconR.GetStick()[0], 0) * cameraSensitivity;
            #endregion

            #region Hand/arm motion


            Quaternion rotR = Quaternion.Euler(inputs.m_joyconR.GetGyro().x, inputs.m_joyconR.GetGyro().y, inputs.m_joyconR.GetGyro().z);
            armRight.transform.rotation = armRight.transform.rotation * rotR;

            
            Quaternion rotL = Quaternion.Euler(inputs.m_joyconL.GetGyro().x, inputs.m_joyconL.GetGyro().y, inputs.m_joyconL.GetGyro().z);
            armLeft.transform.rotation = armLeft.transform.rotation * rotL;
            #endregion
            
        }

    } 
}

