using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconLogger : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];

    public List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;

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

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle( "label" );
        style.fontSize = 24;

        if ( m_joycons == null || m_joycons.Count <= 0 )
        {
            GUILayout.Label("Joy-Con is not connected");
            return;
        }

        if ( !m_joycons.Any( c => c.isLeft ) )
        {
            GUILayout.Label("Joy-Con (L) is not connected");
            return;
        }

        if ( !m_joycons.Any( c => !c.isLeft ) )
        {
            GUILayout.Label("Joy-Con (R) is not connected");
            return;
        }

        GUILayout.BeginHorizontal( GUILayout.Width( 960 ) );

        foreach ( var joycon in m_joycons )
        {
            var isLeft      = joycon.isLeft;
            var name        = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            var key         = isLeft ? "Z Key" : "X Key";
            var button      = isLeft ? m_pressedButtonL : m_pressedButtonR;
            var stick       = joycon.GetStick();
            var gyro        = joycon.GetGyro();
            var accel       = joycon.GetAccel();
            var orientation = joycon.GetVector();

            GUILayout.BeginVertical( GUILayout.Width( 480 ) );
            GUILayout.Label( name );
            GUILayout.Label( key + "：vibration" );
            GUILayout.Label("Button being pressed：" + button );
            GUILayout.Label( string.Format( "Stick：({0}, {1})", stick[ 0 ], stick[ 1 ] ) );
            GUILayout.Label( "Gyro：" + gyro );
            GUILayout.Label( "Acceleration：" + accel );
            GUILayout.Label( "Inclination：" + orientation );
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
}