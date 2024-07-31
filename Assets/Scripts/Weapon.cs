using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private bool ownerIsPlayer;

    static public event Action PlayerDamaged;
    static public event Action EnemyDamaged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ownerIsPlayer)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyDamaged?.Invoke();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                PlayerDamaged?.Invoke();
            }
        }
    }
}
