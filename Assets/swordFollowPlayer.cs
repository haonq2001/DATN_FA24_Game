using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public  Transform chempos;    // Tham chiếu đến Player
   
    void Start()
    {
  
}

    // Update is called once per frame
    void Update()
    {
        transform.position = chempos.position;
}
}
