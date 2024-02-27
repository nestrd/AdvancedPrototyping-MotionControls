using UnityEngine;

public class InteractiveGrabCube : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();   
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 1)
        {
            float pitchVariance = Random.Range(0.8F, 1.2F);
            float normalizedMag = Mathf.Clamp01(collision.relativeVelocity.magnitude);
            sound.volume = normalizedMag;
            sound.pitch = pitchVariance;
            sound.Play();
        }
    }
}
