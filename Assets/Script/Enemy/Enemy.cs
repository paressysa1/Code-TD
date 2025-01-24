using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("伤害值")]
    [SerializeField] private float damage;


    [Header("移动速度")]
    [SerializeField] private float speed;

    private Transform target;




    private void Awake()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Base")?.GetComponent<Transform>();
    }

    private void Update()
    {
        FlyTowardsTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MainBase>() != null)
        {
            collision.GetComponent<MainBase>().TakeDamage(damage);
            Debug.Log(this + "对基地造成了" + damage + "点伤害");
            Destroy(gameObject);
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        DetectDeath();
    }

    public void DetectDeath()
    {
        if(currentHealth<=0)
        {
            
            Destroy(gameObject);
        }
    }

    public float GetDamage()
    {
        return damage;
    }


    public void FlyTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 movement = direction * speed * Time.deltaTime;
            transform.position += movement;
        }

    }


}
