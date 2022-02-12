using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//11번째 Light다이스
public class Dice10Light : Dice
{
    public float attackTimePlus = 0.06f;
    public float attackTimePlusClassUp = 0.03f;
    public float attackTimePlusPowerUp = 0.1f;

    //버프를 줄 4방향, 설정되지 않은 값은 -1
    public int upLocate = -1; 
    public int downLocate = -1;
    public int rightLocate = -1;
    public int leftLocate = -1;
    //혹시 주사위가 바뀔 수 있으니 이전값을 저장
    public int upDiceLV = 0;
    public int downDiceLV = 0;
    public int rightDiceLV = 0;
    public int leftDiceLV = 0;

    private GameObject dices;

    public Dice10Light()
        : base(10, false, 0, 0f, 0, 0, 0f, 0f)
    {
    }

    private void Start()
    {
        base.Start();
        dices = GameObject.Find("Dice");
        BuffLocation();
        GiveBuff();
    }

    private void Update()
    {
        if (!isLock)
            CheckBuff();
    }

    private void OnDestroy()
    {
        DeleteBuff();
    }

    //버프 주는 4방향 설정
    public void BuffLocation()
    {
        //위
        if (diceLocation - 5 >= 0)
            upLocate = diceLocation - 5;
        //아래
        if (diceLocation + 5 <= 14)
            downLocate = diceLocation + 5;
        //오른쪽, 줄 바뀜 방지
        if (diceLocation + 1 <= 14 && (diceLocation + 1) % 5 != 0) 
            rightLocate = diceLocation + 1;
        //왼쪽, 줄 바뀜 방지
        if (diceLocation - 1 >= 0 && diceLocation % 5 != 0)
            leftLocate = diceLocation - 1;

    }

    //다이스가 바뀌었을 때 다시 버프 설정
    public void CheckBuff()
    {
        //dice parent 오브젝트를 받아와서 자식들을 검색하여 location을 받아서 비교한다.
        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
            {
                //기존에 주던 버프가 아니고 바뀌었을때
                if (upDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;

                }

            }
        }

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == downLocate)
            {
                //기존에 주던 버프가 아니고 바뀌었을때
                if (downDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                         dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                         (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }

            }
        }

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == rightLocate)
            {
                //기존에 주던 버프가 아니고 바뀌었을때
                if (rightDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }

            }
        }

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == leftLocate)
            {
                if (leftDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }

            }
        }
    }

    //버프를 준다.
    public void GiveBuff()
    {
        //dice parent 오브젝트를 받아와서 자식들을 검색하여 location을 받아서 비교한다.
        if (upLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }
            }
        }
        if (downLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == downLocate)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }
            }
        }
        if (rightLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == rightLocate)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }
            }
        }
        if (leftLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == leftLocate)
                {
                    //버프를 준다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //투명을 불투명으로
                    break;
                }
            }
        }
    }

    
    //버프를 없앤다.
    public void DeleteBuff()
    {
        //dice parent 오브젝트를 받아와서 자식들을 검색하여 location을 받아서 비교한다.
        if (upLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
                {
                    //버프를 지운다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //불투명을 투명으로
                    break;
                }
            }
        }
        if (downLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == downLocate)
                {
                    //버프를 지운다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //불투명을 투명으로
                    break;
                }
            }
        }
        if (rightLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == rightLocate)
                {
                    //버프를 지운다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //불투명을 투명으로
                    break;
                }
            }
        }
        if (leftLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == leftLocate)
                {
                    //버프를 지운다.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //이미지 7번이 라이트
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //불투명을 투명으로
                    break;
                }
            }
        }
    }
    

}