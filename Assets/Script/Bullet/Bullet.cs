using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected Collider2D collider;

    [SerializeField] protected Transform target;

    [Header("移动速度")]
    [SerializeField] protected float speed;

    [Header("伤害数值")]
    [SerializeField] protected float damage;

    [Header("伤害次数")]
    [SerializeField] protected int damageTimes;

    [Header("伤害CD")]
    [SerializeField] protected float damageCD;
    protected float lastDamageInterval;

    [Header("射击间隔加成")]
    public float shootIntervalChange;

    [Header("装弹时间加成")]
    public float reloadTimeChange;

    [Header("存在时间")]
    public float existTime;
    private float existedTime;



    protected bool hasCollideWithTarget;



    protected Vector3 flyDirection;

    protected Dictionary<Enemy, float> lastHitEnemyInterval = new Dictionary<Enemy, float>();

    private BulletSlot slot;

    public Vector3 lastFrameVelocity;



    private void Start()
    {
        CalculateDamage();
        
    }

    protected void Update()
    {
        CalculateExistingTime();
    }

    protected void CalculateExistingTime()
    {
        existedTime += Time.deltaTime;

        if (existedTime >= existTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform transform)
    {
        this.target = transform;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public virtual void FlyTowardsTarget()
    {
        return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public void SetFlyDirection(Vector3 flyDirection)
    {
        this.flyDirection = flyDirection;
    }

    protected void DetectDamageTimes()
    {
        if(damageTimes <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void DamageCDTurns()
    {

        foreach(var enemy in lastHitEnemyInterval.Keys)
        {
            if (lastHitEnemyInterval[enemy] < damageCD)
            {
                damageCD += Time.deltaTime;
            }
        }
    }

    public void SetSlot(BulletSlot slot)
    {
        this.slot = slot;
    }

    public BulletSlot GetBulletSlot()
    {
        return this.slot;   
    }

    private void CalculateDamage()
    {
        damage = damage * slot.GetTowerTurret().GetCurrentPower();
    }


}
