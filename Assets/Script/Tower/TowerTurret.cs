using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.Collections.LowLevel.Unsafe;
using System;
using System.Collections.Concurrent;

public class TowerTurret : MonoBehaviour
{
    [Header("索敌范围")]
    [SerializeField] private float detectRadius;
    [SerializeField] private CircleCollider2D detectCollider;
    [SerializeField] private  LayerMask layerMask = 3;

    [Header("索敌方式")]
    [SerializeField] private TargetPreference targetPreference;


    [Header("射击间隔")]
    [SerializeField] private float shootInterval;
    private float lastShootInterval;
    private bool isInShootCD;

    [Header("装弹时间")]
    [SerializeField] private float reloadTime;
    private float reloadInterval;
    private bool isReloading;


    [Header("最大功率")]
    [SerializeField] private float maxPower;
    private float currentPower;

    [Header("功率系数")]
    [SerializeField] private float powerCoefficient;

    [Header("子弹槽位")]
    [SerializeField] private List<BulletSlot> bulletSlotsList;
    private int currentBulletSlotIndex;
    private Bullet currentBullet;
    
    private List<BulletSlot> actualBulletSlotsList = new List<BulletSlot>();



    private TowerStatus status;

    private Enemy currentTarget;
    
    private List<Enemy> targetsInRange = new List<Enemy>();



    private void Awake()
    {
        currentPower = maxPower;
        status = TowerStatus.Targeting;
        detectCollider.radius = detectRadius;
        currentTarget = null;

        foreach(var currentSlot in bulletSlotsList)
        {
            BulletSlot slot = Instantiate(currentSlot);      
            slot.SetTurret(this);
            slot.transform.position = this.transform.position;
            slot.transform.parent = this.transform;
            actualBulletSlotsList.Add(slot);
        }    
    }




    private void Update()
    {

        if (isInShootCD)
        {            
            ShootCoolDown(currentBullet.shootIntervalChange);
        }

        switch(status)
        {
            case TowerStatus.Idle:
                break;
            case TowerStatus.Targeting:
                GetAllEnemiesInRange();  //获得范围内所有敌人，存入列表
                if (targetsInRange.Count > 0)
                {
                    Debug.Log(this + "搜索到了目标");
                    currentTarget = GetCurrentEnemy(); //按照塔的索敌方式获得对应敌人
                    targetsInRange.Clear();//清空范围内敌人列表
                    status = TowerStatus.Shooting;//转入旋转炮台阶段
                    Debug.Log(this + "当前目标为" + currentTarget);
                    
                }
                break;
            case TowerStatus.Rotating:
                break;
            case TowerStatus.WarmingUp:
                break;
            case TowerStatus.Shooting:

                if (currentTarget != null && !isInShootCD)
                {
                    Debug.Log(this + "射击中,发射子弹");
                    if (actualBulletSlotsList[currentBulletSlotIndex].isCorutine)
                    {
                        StartCoroutine(actualBulletSlotsList[currentBulletSlotIndex].ShootCoroutine(currentTarget.transform.position));
                    }
                    else
                    {
                        actualBulletSlotsList[currentBulletSlotIndex].Shoot(currentTarget.transform.position);
                    }
                    isInShootCD = true;
                    lastShootInterval = 0;
                    currentBulletSlotIndex++;
                    if(currentBulletSlotIndex >= actualBulletSlotsList.Count)
                    {
                        currentBullet = actualBulletSlotsList[0].bullet;
                    }
                    else
                    {
                        currentBullet = actualBulletSlotsList[currentBulletSlotIndex].bullet;
                    }

                    Debug.Log(this + "当前剩余子弹数为" + (actualBulletSlotsList.Count - currentBulletSlotIndex));
                    if (currentBulletSlotIndex >= actualBulletSlotsList.Count)
                    {
                        Debug.Log(this + "需要装填弹药");
                        isReloading = true;
                        status = TowerStatus.Reloading;
                    }
                }
                else if(currentTarget == null)
                {
                    status = TowerStatus.Targeting;
                }

                break;
            case TowerStatus.Reloading:
                if (isReloading)
                {
                    Reload();
                }
                else
                {
                    Debug.Log(this + "装填弹药完毕" );

                    if(currentTarget == null)
                    {
                        status = TowerStatus.Targeting;
                    }
                    else
                    {
                        status = TowerStatus.Shooting;
                    }
                }

                break;
        }
    }

    public float GetCurrentPower()
    {
        return currentPower;
    }

    private void ShootCoolDown(float percentage)
    {
        lastShootInterval += Time.deltaTime;
        float shootInterval = this.shootInterval * (1 + percentage);

        if(lastShootInterval >= shootInterval)
        {
            lastShootInterval = 0;
            isInShootCD = false;
        }
    }

    private void Reload()
    {
        reloadInterval += Time.deltaTime;

        float reloadTimePercentage = 0;

        foreach(var bulletSlot in bulletSlotsList)
        {
            reloadTimePercentage += bulletSlot.bullet.reloadTimeChange;
            
        }


        float reloadTime = this.reloadTime * (1 + reloadTimePercentage);

        
        if(reloadInterval >= reloadTime)
        {
            Debug.Log(this + "装填弹药中");
            reloadInterval = 0;
            currentBulletSlotIndex = 0;
            isReloading = false;
        }
    }

    //获得射击范围内的所有敌人
    public void GetAllEnemiesInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius, layerMask);

        foreach (var collider in colliders)
        {
            if(collider.GetComponent<Enemy>() != null)
            {
                targetsInRange.Add(collider.GetComponent<Enemy>());
            }
        }
    }

    //按照索敌方式获得当前的敌人
    public Enemy GetCurrentEnemy()
    {
        Enemy currentTarget = null;

        switch (targetPreference)
        {
            case TargetPreference.Nearest:
                float distanceN = 9999f;
                foreach(var target in targetsInRange)
                {
                    if(distanceN > Vector3.Distance(transform.position, target.transform.position))
                    {
                        distanceN = Vector3.Distance(transform.position, target.transform.position);
                        currentTarget = target;
                    }
                }
                break;

            case TargetPreference.Farrest:
                float distanceF = 0f;
                foreach (var target in targetsInRange)
                {
                    if (distanceF < Vector3.Distance(transform.position, target.transform.position))
                    {
                        distanceF = Vector3.Distance(transform.position, target.transform.position);
                        currentTarget = target;
                    }
                }
                break;

            case TargetPreference.Strongest:
                float healthS = 9999f;
                foreach (var target in targetsInRange)
                {
                    if (healthS > target.GetCurrentHealth())
                    {
                        healthS = target.GetCurrentHealth();
                        currentTarget = target;
                    }
                }

                break;
            case TargetPreference.Weakest:
                float healthW = 0;
                foreach (var target in targetsInRange)
                {
                    if (healthW < target.GetCurrentHealth())
                    {
                        healthW = target.GetCurrentHealth();
                        currentTarget = target;
                    }
                }
                break;
            case TargetPreference.Random:
                int randomNumber = UnityEngine.Random.Range(0, targetsInRange.Count);
                currentTarget = targetsInRange[randomNumber];
                break;

                
        }

        return currentTarget;
    }


    public Enemy GetTarget()
    {
        return currentTarget;
    }


}





public enum TowerStatus
{
    Idle,
    Targeting,
    Rotating,
    WarmingUp,
    Shooting,
    Reloading,
    
}


public enum TargetPreference
{
    Random,
    Nearest,
    Farrest,
    Strongest,
    Weakest,
    
      
}