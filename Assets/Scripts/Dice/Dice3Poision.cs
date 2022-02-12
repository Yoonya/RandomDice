using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4번째 Poision다이스
public class Dice3Poision : Dice
{
    public int poisionDamage = 50;
    public int poisionDamageClassUp = 5;
    public int poisionDamagePowerUp = 25;

    public Dice3Poision()
        : base(3, true, 20, 1.3f, 2, 10, 0f, 0f)
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
