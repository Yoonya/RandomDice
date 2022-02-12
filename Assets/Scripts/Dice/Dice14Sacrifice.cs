using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//15번째 Sacrifice다이스
public class Dice14Sacrifice : Dice
{
    public int spPlus = 80;

    public Dice14Sacrifice()
        : base(14, true, 80, 1f, 10, 10, 0f, 0f)
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

    //파괴될 때 sp를 올려준다.
    private void OnDestroy()
    {
        diceCreateManager.remainSP += spPlus * diceLV;
    }
}