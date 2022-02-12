using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//10번째 Mine다이스
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

            //sp흭득 주기
            if (time > spTime - (spClassUp * diceClassLV))
            {
                time = 0f;

                //sp를 더해준다. 눈금 수만큼 곱해준다->bullet으로 처리 안하기 때문
                diceCreateManager.remainSP += diceLV * (spPlus + (spPlusPowerUp * dicePowerLV));
            }
        }
    }
}
