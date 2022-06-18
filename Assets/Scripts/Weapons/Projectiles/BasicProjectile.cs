using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float damage;
    public float speed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + transform.forward * speed*Time.deltaTime);
    }


    protected virtual void OnImpact(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        OnImpact(other);
    }
}
