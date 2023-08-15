using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleLock : MonoBehaviour
{
    private Vector3 positionAdj;
    private Vector3 targetPosition;
    public GameObject parent;
    private float angle;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = transform.localRotation * Quaternion.Inverse(transform.rotation);
        // inspired by https://www.pixelatedplaygrounds.com/sidequests/gamesmithing-stationary-children
        angle = (parent.GetComponent<PlayerController>().angle-90) * Mathf.Deg2Rad;        
        transform.localPosition = new Vector3(Mathf.Cos(angle)*0.45f,-Mathf.Sin(angle)*0.45f,0f);
    }
}