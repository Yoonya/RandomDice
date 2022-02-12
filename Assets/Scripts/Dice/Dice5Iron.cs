using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//6번째 Iron다이스
public class Dice5Iron : Dice
{
    public Dice5Iron()
        : base(5, true, 100, 1f, 10, 100, 0f, 0f)
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