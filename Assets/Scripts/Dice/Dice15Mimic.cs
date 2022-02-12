using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//16번째 mimic다이스
public class Dice15Mimic : Dice
{
    public Dice15Mimic()
        : base(15, true, 20, 1f, 5, 10, 0f, 0f)
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
