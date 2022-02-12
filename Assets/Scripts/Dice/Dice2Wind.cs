using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3��° Wind���̽�
public class Dice2Wind : Dice
{
    public float windAttackTime = 0.1f;
    public float windAttackTimeClassUp = 0.02f;
    public float windAttackTimePowerUp = 0.1f;

    public Dice2Wind()
        : base(2, true, 20, 0.45f, 3, 15, 0f, 0f)
    {
    }

    //���ݼӵ��� �����Ѵ�.
    public void WindDice()
    {
        attackTime -= attackTime * (windAttackTime + (windAttackTimeClassUp * diceClassLV)
                            + (windAttackTimePowerUp * dicePowerLV));
    }

    private void Start()
    {
        base.Start();
        WindDice();
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
