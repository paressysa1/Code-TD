using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    [Header("�������ֵ")]
    [SerializeField] private float maxHealth;
    private float currentHealth;



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

}
