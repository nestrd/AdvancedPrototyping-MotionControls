using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private JoyconInputs inputs;

    public Image leftIcon;
    private Sprite leftInactive;
    [SerializeField] private Sprite leftActive;
    public Image rightIcon;
    private Sprite rightInactive;
    [SerializeField] private Sprite rightActive;

    private void Awake()
    {
        leftInactive = leftIcon.sprite;
        rightInactive = rightIcon.sprite;
    }

    private void FixedUpdate()
    {
        if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2)
        {
            leftIcon.sprite = leftActive;
            leftIcon.GetComponent<Animator>().SetBool("Play", true);
            inputs.m_joyconL.SetRumble(100, 100, 1, 0);
        }
        else
        {
            leftIcon.sprite = leftInactive;
            leftIcon.GetComponent<Animator>().SetBool("Play", false);
            inputs.m_joyconL.SetRumble(0, 0, 0, 0);
        }

        if (inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2)
        {
            rightIcon.sprite = rightActive;
            rightIcon.GetComponent<Animator>().SetBool("Play", true);
            inputs.m_joyconR.SetRumble(100, 100, 10, 0);
        }
        else
        {
            rightIcon.sprite = rightInactive;
            rightIcon.GetComponent<Animator>().SetBool("Play", false);
            inputs.m_joyconR.SetRumble(0, 0, 0, 0);
        }
    }
}