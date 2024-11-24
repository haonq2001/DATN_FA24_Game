using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followDeathBarrie : MonoBehaviour
{
    public Transform giotDoc;
    public Vector3 vecto3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = giotDoc.position + vecto3;
    }
}
