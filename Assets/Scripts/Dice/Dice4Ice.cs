using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//5��° Ice���̽�
public class Dice4Ice : Dice
{
    public float speedDown = 0.05f;
    public float speedDownClassUp = 0.005f;
    public float speedDownPowerUp = 0.02f;

    public Dice4Ice()
        : base(4, true, 30, 1.5f, 3, 30, 0.02f, 0f)
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
