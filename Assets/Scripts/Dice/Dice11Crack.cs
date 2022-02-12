using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//12번째 Crack다이스
public class Dice11Crack : Dice
{
    public float plusDamage = 0.02f;
    public float plusDamageClassUp = 0.002f;
    public float plusDamagePowerUp = 0.05f;

    public Dice11Crack()
        : base(10, true, 20, 1.5f, 4, 20, 0f, 0f)
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
