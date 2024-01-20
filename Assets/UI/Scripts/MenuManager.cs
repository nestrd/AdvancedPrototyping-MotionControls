using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private JoyconInputs inputs;
    [SerializeField] private AudioSource audioL;
    [SerializeField] private AudioSource audioR;

    public Image leftIcon;
    private Sprite leftInactive;
    [SerializeField] private Sprite leftActive;
    public Image rightIcon;
    private Sprite rightInactive;
    [SerializeField] private Sprite rightActive;

    private bool waitingToStart = false;

    private void Awake()
    {
        leftInactive = leftIcon.sprite;
        rightInactive = rightIcon.sprite;
    }

    private void FixedUpdate()
    {
        if (inputs.m_joycons != null && !waitingToStart)
        {
            if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2)
            {
                leftIcon.sprite = leftActive;
                leftIcon.GetComponent<Animator>().SetBool("Play", true);
                inputs.m_joyconL.SetRumble(0, 99, 0.501F, 0);
                audioL.enabled = true;
            }
            else
            {
                leftIcon.sprite = leftInactive;
                leftIcon.GetComponent<Animator>().SetBool("Play", false);
                inputs.m_joyconL.SetRumble(0, 0, 0, 0);
                audioL.enabled = false;
            }
            if (inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2)
            {
                rightIcon.sprite = rightActive;
                rightIcon.GetComponent<Animator>().SetBool("Play", true);
                inputs.m_joyconR.SetRumble(0, 99, 0.501F, 0);
                audioR.enabled = true;
            }
            else
            {
                rightIcon.sprite = rightInactive;
                rightIcon.GetComponent<Animator>().SetBool("Play", false);
                inputs.m_joyconR.SetRumble(0, 0, 0, 0);
                audioR.enabled = false;
            }
            if (inputs.m_pressedButtonL == Joycon.Button.SHOULDER_2 && inputs.m_pressedButtonR == Joycon.Button.SHOULDER_2)
            {
                inputs.m_joyconL.SetRumble(0, 0, 0, 0);
                inputs.m_joyconR.SetRumble(0, 0, 0, 0);
                waitingToStart = true;
                StartCoroutine(EnterGame(3.0F));
            }
        }
        
    }

    private IEnumerator EnterGame(float timeToStart)
    {
        float timer = 0F;
        while(timer < timeToStart)
        {
            timer = timer + 0.1F;
            if(timer >= timeToStart)
            {
                yield return new WaitForSeconds(timeToStart);
                SceneManager.LoadScene("Scene_MainGame");
            }
        }
    }
}