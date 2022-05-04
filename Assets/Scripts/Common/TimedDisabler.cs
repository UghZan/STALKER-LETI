using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisabler : MonoBehaviour
{
    public float timeToDisable = 3f;

    private float timer = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeToDisable) gameObject.SetActive(false);
    }
}
