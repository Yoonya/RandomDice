using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//7번째 Broken다이스
public class Dice6Broken : Dice
{
    public Dice6Broken()
        : base(6, true, 50, 0.9f, 10, 50, 0f, 0f)
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
