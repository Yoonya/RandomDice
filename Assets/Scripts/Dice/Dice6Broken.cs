using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//7��° Broken���̽�
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
