using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconChecker : MonoBehaviour
{
    public Image joyconLeft;
    private Sprite joyconLInactive;
    public Sprite joyconLActive;
    public Image joyconRight;
    private Sprite joyconRInactive;
    public Sprite joyconRActive;

    private JoyconInput inputManager;
    private JoyconManager controlManager;

    private void Awake()
    {
        inputManager = GetComponent<JoyconInput>();
        joyconLInactive = joyconLeft.GetComponent<Image>().sprite;
        joyconRInactive = joyconRight.GetComponent<Image>().sprite;
    }

    private void FixedUpdate()
    {
        if (controlManager.j.Count == 0)
        {
            if(inputManager.jc_ind == 1)
            {
                joyconRight.sprite = joyconRActive;
            } else
            {
                joyconRight.sprite = joyconRInactive;
            }
        }
    }
}
