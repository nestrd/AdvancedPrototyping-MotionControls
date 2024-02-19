using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate();
    void Deactivate();


}

public class MotionInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject UiPrompt;
    private GameObject uiPrompt_temp;
    private int uiCount = 0;

    private void Awake()
    {
        Physics.IgnoreLayerCollision(6, 8, true);
    }

    public void Activate()
    {
        Debug.Log("Activated interactable");
        if(uiCount == 0)
        {
            uiPrompt_temp = Instantiate(UiPrompt);
            uiCount += 1;
        }
    }

    public void Deactivate()
    {
        Debug.Log("Deactivated interactable");
        Destroy(uiPrompt_temp);
        uiCount = 0;
    }
}
