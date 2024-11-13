using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript1 : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    // Start is called before the first frame update

    bool isPressed = false;
    public GameObject Player;
    public float Force;
    void Start()
    {
        if (isPressed)
        {
            Player.transform.Translate(Force * Time.deltaTime,0,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void OnPointerDown(PointerEventData eventData){
            isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        isPressed = false;
    }
}
