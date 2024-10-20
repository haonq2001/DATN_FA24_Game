﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:Assets/Scripts/SoiBoss.cs
public class SoiBoss : MonoBehaviour
{
=======
public class PlayerMove : MonoBehaviour { 
     private int m_ColCount = 0;

private float m_DisableTimer;

>>>>>>> db0ca9a1d76c00dab8eae1aaf7158a8c43a51299:Assets/PlayerMove.cs
    // Start is called before the first frame update
    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

  void OnTriggerEnter2D(Collider2D other)
{
    // Kiểm tra nếu đối tượng va chạm có phải là lớp mặt đất hay không
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
        m_ColCount++;
    }
}

void OnTriggerExit2D(Collider2D other)
{
    // Kiểm tra nếu đối tượng rời đi có phải là lớp mặt đất hay không
    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
        m_ColCount--;
    }
}


    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
