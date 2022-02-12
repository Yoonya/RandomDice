using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//14번째 Energy다이스
public class Dice13Energy : Dice
{
    public float spDamage = 0.04f;
    public float spDamageClassUp = 0.003f;
    public float spDamagePowerUp = 0.006f;

    public Dice13Energy()
        : base(13, true, 20, 1.3f, 10, 10, 0f, 0f)
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
