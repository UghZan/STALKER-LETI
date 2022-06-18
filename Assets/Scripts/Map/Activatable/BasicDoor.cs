using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : Activatable
{
    // true closed, false opened
    public bool isClosed;
    [Header("Door Parameters")]
    //these are local, so you need to specify offsets, not the world coordinates
    //e.g. if door is open by default, openState would be 0 0 0 and closedState would be something like -1 0 0
    public Vector3 closedStateOffset;
    public Vector3 openedStateOffset;
    public float timeToChange; //how much time in seconds should door open/close

    //if true, you cannot activate door
    private bool lockAction;

    private void Start()
    {
        transform.localPosition = isClosed ? closedStateOffset : openedStateOffset;
    }
    public override bool OnActivate()
    {
        return OnActivate(!isClosed);
    }
    public override bool OnActivate(bool state)
    {
        if (lockAction) return false;
        isClosed = state;
        StartCoroutine(ChangePosition(isClosed));
        return true;
    }

    IEnumerator ChangePosition(bool nextState)
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 nextPosition = nextState ? closedStateOffset : openedStateOffset;

        float progress = 0;

        while (progress < timeToChange)
        {
            transform.localPosition = Vector3.Lerp(startPosition, nextPosition, progress/timeToChange);
            progress += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = nextPosition;
        yield return null;
    }
}
