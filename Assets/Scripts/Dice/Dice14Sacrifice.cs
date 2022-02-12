using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//15��° Sacrifice���̽�
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

    //�ı��� �� sp�� �÷��ش�.
    private void OnDestroy()
    {
        diceCreateManager.remainSP += spPlus * diceLV;
    }
}