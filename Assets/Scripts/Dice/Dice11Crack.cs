using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//12��° Crack���̽�
public class Dice11Crack : Dice
{
    public float plusDamage = 0.02f;
    public float plusDamageClassUp = 0.002f;
    public float plusDamagePowerUp = 0.05f;

    public Dice11Crack()
        : base(10, true, 20, 1.5f, 4, 20, 0f, 0f)
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
