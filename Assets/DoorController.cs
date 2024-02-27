using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private Animator animRef;
    private int activatorCount;
    private int simultaneousActivates;
    [SerializeField] private int requiredActivates = 2;
    private AudioSource audioToPlay;

    private void Awake()
    {
        animRef = GetComponent<Animator>();
        audioToPlay = GetComponent<AudioSource>();
    }

    public void Activator()
    {
        ++activatorCount;
        Debug.Log("Amount of activates:" + activatorCount);
    }

    public void AddToActivated(int toAdd)
    {
        simultaneousActivates += toAdd;
        if(!(simultaneousActivates >= requiredActivates))
        {
            DoorControls(false);
            return;
        }
        DoorControls(true);
        Debug.Log(simultaneousActivates);
        return;
    }

    public void DoorControls(bool opening)
    {
        if (opening)
        {
            animRef.SetBool("openDoor", true);
            audioToPlay.Play();
        }
        else
        {
            animRef.SetBool("openDoor", false);
        }
    }
}
