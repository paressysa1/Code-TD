using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AirMineBullet : Bullet
{

    [Header("减速度")]
    [SerializeField] protected float deceleratePercentage;
    private float currentSpeed;
    private bool shouldStop;

    [Header("是否跟随目标")]
    private bool targetMechanic;
    

    private void Start()
    {
        currentSpeed = speed;
        if (!targetMechanic)
        {
            flyDirection = RandomDirection();
        }
    }

    protected void Update()
    {
        base.Update();
        FlyTowardsTarget();
        DamageCDTurns();
    }

    public override void FlyTowardsTarget()
    {
        if(!shouldStop)
        {
            transform.up = flyDirection;

            if (targetMechanic)
            {
                if (target != null)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    flyDirection = direction;
                    Vector3 movement = flyDirection * currentSpeed * Time.deltaTime;
                    transform.position += movement;
                }
                else
                {
                    Vector3 movement = flyDirection * currentSpeed * Time.deltaTime;
                    transform.position += movement;
                }
            }
            else
            {
                Vector3 movement = flyDirection * currentSpeed * Time.deltaTime;
                transform.position += movement;
            }

          
            currentSpeed -= speed * deceleratePercentage * Time.deltaTime;
            Mathf.Clamp(currentSpeed, 0, speed);

            if(currentSpeed.CompareTo(0)<=0.1f)
            {
                shouldStop = true;
            }
        }
       
    }

    private Vector3 RandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        float radians = randomAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        return direction;
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
                }
            }
            else
            {
                lastHitEnemyInterval.Add(enemy, 0f);
                collision.GetComponent<Enemy>().TakeDamage(damage);
                damageTimes--;
                DetectDamageTimes();
            }

        }
    }
}
