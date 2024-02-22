using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingInstance : MonoBehaviour
{
    // Refs
    private PlayerController playerRef;

    private void Awake()
    {
        playerRef = FindObjectOfType<PlayerController>();
        StartCoroutine(PingTimer());
    }

    private void FixedUpdate()
    {
        transform.LookAt(playerRef.transform.position);
    }
    IEnumerator PingTimer()
    {
        yield return new WaitForSeconds(1.0F);

        Destroy(this.gameObject);
        
    }
}
