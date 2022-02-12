using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1��° Fire���̽�
public class Dice0Fire : Dice
{
    public int fireDamage = 20;
    public int fireDamageClassUp = 20;
    public int fireDamagePowerUp = 20;

    public Dice0Fire()
        : base(0, true, 20, 0.8f, 3, 10, 0.01f, 0f)
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
