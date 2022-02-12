using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//10��° Mine���̽�
public class Dice9Mine : Dice
{
    public float spTime = 10f;
    public float spClassUp = 0.5f;
    public int spPlus = 3;
    public int spPlusPowerUp = 5;

    public Dice9Mine()
        : base(9, false, 0, 0f, 0, 0, 0f, 0f)
    {
    }

    private void Update()
    {
        if (!isLock)
        {
            time += Time.deltaTime;

            //spŉ�� �ֱ�
            if (time > spTime - (spClassUp * diceClassLV))
            {
                time = 0f;

                //sp�� �����ش�. ���� ����ŭ �����ش�->bullet���� ó�� ���ϱ� ����
                diceCreateManager.remainSP += diceLV * (spPlus + (spPlusPowerUp * dicePowerLV));
            }
        }
    }
}
