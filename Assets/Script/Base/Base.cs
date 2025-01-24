using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    [Header("最大生命值")]
    [SerializeField] private float maxHealth;
    private float currentHealth;



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

}
