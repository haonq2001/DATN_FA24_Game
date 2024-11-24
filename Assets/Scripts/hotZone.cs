using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotZone : MonoBehaviour
{
    private Enemy enemyParent;
    private bool inRange;
    private Animator anim;
    private void Awake()
    {
        enemyParent = GetComponentInParent<Enemy>();
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if(inRange && anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack")){
            enemyParent.Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
