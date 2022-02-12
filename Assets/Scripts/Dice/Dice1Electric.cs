using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2��° Electric���̽�
public class Dice1Electric : Dice
{
    public int electricDamage = 30;
    public int electricDamageClassUp = 3;
    public int electricDamagePowerUp = 20;

    public int electricDamage2;
    public int electricDamage3;

    public Dice1Electric()
        : base(1, true, 30, 0.7f, 3, 10, 0.02f, 0f)
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
