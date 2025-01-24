using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBullet : Bullet
{
    protected void Update()
    {
        base.Update();
        FlyTowardsTarget();
        DamageCDTurns();
    }

    public override void FlyTowardsTarget()
    {
        transform.up = flyDirection;

        if (!hasCollideWithTarget)
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                flyDirection = direction;
                Vector3 movement = direction * speed * Time.deltaTime;
                transform.position += movement;
            }
            else
            {
                Vector3 movement = flyDirection * speed * Time.deltaTime;
                transform.position += movement;
            }
        }
        else
        {
            Vector3 movement = flyDirection * speed * Time.deltaTime;
            transform.position += movement;
        }


    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (lastHitEnemyInterval.ContainsKey(enemy))
            {
                if (lastHitEnemyInterval[enemy] < damageCD)
                {
                    return;
                }
                else
                {
                    collision.GetComponent<Enemy>().TakeDamage(damage);
                    damageTimes--;
                    DetectDamageTimes();
                    if (enemy.transform == target)
                    {
                        hasCollideWithTarget = true;
                    }
                }
            }
            else
            {
                lastHitEnemyInterval.Add(enemy, 0f);
                collision.GetComponent<Enemy>().TakeDamage(damage);
                damageTimes--;
                DetectDamageTimes();
                if (enemy.transform == target)
                {
                    hasCollideWithTarget = true;
                }
            }

        }
    }
}
