using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public JoyconManager managerMain;
    public HandController leftHand;
    public HandController rightHand;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    public List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }


    private void FixedUpdate()
    {
        var tempPos_L = leftHand.transform.position + new Vector3(managerMain.j[0].GetAccel().x, managerMain.j[0].GetAccel().z, 0) / 2;
        var tempPos_R = rightHand.transform.position + new Vector3(managerMain.j[1].GetGyro().x, managerMain.j[1].GetGyro().z, 0) / 2;
        
        leftHand.transform.position = tempPos_L;
        rightHand.transform.position = tempPos_R;
    }
}
