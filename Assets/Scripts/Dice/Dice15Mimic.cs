using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//16��° mimic���̽�
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
            //���� �ֱ� ���� �߻�
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
