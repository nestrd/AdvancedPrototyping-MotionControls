using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public Animator motionInputsAnimator;

    public void SetAnimationState(int inputStates)
    {
        motionInputsAnimator.SetInteger("InputState", inputStates);
    }
}
