using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconInputs : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    public List<Joycon> m_joycons;
    [HideInInspector] public Joycon          m_joyconL;
    [HideInInspector] public Joycon          m_joyconR;
    [HideInInspector] public Joycon.Button?  m_pressedButtonL;
    [HideInInspector] public Joycon.Button?  m_pressedButtonR;

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );
    }

    private void Update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        foreach ( var button in m_buttons )
        {
            if ( m_joyconL.GetButton( button ) )
            {
                m_pressedButtonL = button;
            }
            if ( m_joyconR.GetButton( button ) )
            {
                m_pressedButtonR = button;
            }
        }

        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            m_joyconL.SetRumble( 160, 320, 0.6f, 200 );
        }
        if ( Input.GetKeyDown( KeyCode.X ) )
        {
            m_joyconR.SetRumble( 160, 320, 0.6f, 200 );
        }
    }
}