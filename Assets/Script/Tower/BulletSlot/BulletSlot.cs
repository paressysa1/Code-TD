using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BulletSlot : MonoBehaviour
{

    public Bullet bullet;

    private TowerTurret turret;

    protected Transform target;

    public bool isCorutine;

    private void Start()
    {
        bullet.SetSlot(this);
    }


    public void SetTurret(TowerTurret turret)
    {
        this.turret = turret;
    }

    public TowerTurret GetTowerTurret()
    {
        return turret;
    }

    public Enemy GetTarget()
    {
        return turret.GetTarget();
    }

    public virtual void Shoot(Vector3 targetPosition)
    {
        Bullet newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position;
        newBullet.SetTarget(GetTarget().GetComponent<Transform>());
        newBullet.SetSlot(this);
    }

    public virtual IEnumerator ShootCoroutine(Vector3 targetPosition)
    {
        yield return null;
    }

}
