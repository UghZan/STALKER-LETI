using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingSounds : MonoBehaviour
{
    PlayerController pc;
    public AudioClip[] effects;
    public AudioSource leg;
    public float playDelay;
    public float walkDelayMultiplier = 1.25f;
    public float sprintDelayMultiplier = 0.8f;
    public float pitchDiff = 0.25f;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.isMoving && pc.isOnGround)
        {
            if (timer <= 0)
            {
                if (pc.isSlowWalking)
                    leg.volume = 0.25f;
                else
                    leg.volume = 0.5f;
                leg.pitch = Random.Range(1 - pitchDiff, 1 + pitchDiff);
                leg.PlayOneShot(effects[Random.Range(0, effects.Length)]);
                timer = playDelay * (pc.isSprinting ? sprintDelayMultiplier : 1f) *
                        (pc.isSlowWalking ? walkDelayMultiplier : 1f);
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            leg.Stop();
        }
    }
}
