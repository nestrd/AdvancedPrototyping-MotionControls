using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private Animator gameplayFade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EnterMenu(2.0F));
        }
    }

    private IEnumerator EnterMenu(float timeToEnd)
    {
        gameplayFade.SetBool("Active", true);

        float timer = 0F;
        while (timer < timeToEnd)
        {
            timer = timer + 0.1F;
            if (timer >= timeToEnd)
            {
                yield return new WaitForSeconds(timeToEnd);
                SceneManager.LoadScene("Scene_TitleScreen");
            }
        }
    }
}
