using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RapidFireBulletSlot : BulletSlot
{

    public int bulletAmount;
    public float shootInterval;



    public override IEnumerator ShootCoroutine(Vector3 targetPosition)
    {
        target = GetTarget().GetComponent<Transform>();

        for (int i = 0; i < bulletAmount; i++)
        {
            if(target!=null)
            {
                Bullet newBullet = Instantiate(bullet);
                newBullet.transform.position = transform.position;
                newBullet.SetTarget(target);
                newBullet.SetSlot(this);
                yield return new WaitForSeconds(shootInterval);
            }
            else
            {
                Bullet newBullet = Instantiate(bullet);
                newBullet.transform.position = transform.position;
                newBullet.SetSlot(this);
                Vector3 direction = (targetPosition - transform.position).normalized;
                newBullet.SetFlyDirection(direction);
                yield return new WaitForSeconds(shootInterval);
            }
        } 
    }


}
