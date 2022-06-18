using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : MonoBehaviour, IInteractable
{
    public bool state;
    public Activatable receiver;
    public bool toggled;
    public int activationLimit; //how much times can it be activated
    public float activeTime; //if not toggled, then for how long should it stay active (in seconds)

    private float timer;
    private int activationTimes;

    public bool OnInteract()
    {
        if(activationLimit != -1)
            if (activationTimes >= activationLimit)
                return false;

        if (!toggled) timer = 0;
        state = !state;
        if (activationLimit != -1) activationTimes++;

        return receiver.OnActivate();
    }

    private void Update()
    {
        if (!toggled && state)
        {
            timer += Time.deltaTime;

            if (timer >= activeTime)
            {
                state = !state;
                receiver.OnActivate();
            }
        }
    }
}
