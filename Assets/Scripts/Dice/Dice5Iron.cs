using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//6��° Iron���̽�
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