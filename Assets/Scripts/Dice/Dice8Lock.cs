using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//9번째 Lock다이스
public class Dice8Lock : Dice
{
    public float lockPer = 0.04f;
    public float lockPerClassUp = 0.01f;
    public float lockPerPowerUp = 0.02f;
    public float lockTime = 3f;
    public float lockTimeClassUp = 0.2f;
    public float lockTimePowerUp = 0.5f;

    public Dice8Lock()
        : base(8, true, 30, 0.8f, 5, 25, 0.01f, 0f)
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