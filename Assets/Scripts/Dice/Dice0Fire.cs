using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1번째 Fire다이스
public class Dice0Fire : Dice
{
    public int fireDamage = 20;
    public int fireDamageClassUp = 20;
    public int fireDamagePowerUp = 20;

    public Dice0Fire()
        : base(0, true, 20, 0.8f, 3, 10, 0.01f, 0f)
    {
    }

    private void Update()
    {
        if (!isLock)
        {
            time += Time.deltaTime;

            //공격 주기 별로 발사
            if (time > attackTime - (attackTimeClassUp * diceClassLV) - (attackTimePowerUp * dicePowerLV))
            {
                time = 0f;
                if (isAttack)
                {
                    CreateBullet();
                }
            }
        }
    }
}
