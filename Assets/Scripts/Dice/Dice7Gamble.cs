using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//8번째 Gamble다이스
public class Dice7Gamble : Dice
{
    public Dice7Gamble()
        : base(7, true, 7, 1f, 10, 77, 0.01f, 0f)
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

