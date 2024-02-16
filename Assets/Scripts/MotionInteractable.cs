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
    private GameObject m_UiPrompt;

    private void Awake()
    {
        Physics.IgnoreLayerCollision(6, 8, true);
    }

    public void Activate()
    {
        Debug.Log("Activated interactable");
        m_UiPrompt = Instantiate(UiPrompt);
    }

    public void Deactivate()
    {
        Debug.Log("Deactivated interactable");
        Destroy(m_UiPrompt);
    }
}
