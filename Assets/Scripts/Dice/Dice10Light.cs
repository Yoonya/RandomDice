using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//11��° Light���̽�
public class Dice10Light : Dice
{
    public float attackTimePlus = 0.06f;
    public float attackTimePlusClassUp = 0.03f;
    public float attackTimePlusPowerUp = 0.1f;

    //������ �� 4����, �������� ���� ���� -1
    public int upLocate = -1; 
    public int downLocate = -1;
    public int rightLocate = -1;
    public int leftLocate = -1;
    //Ȥ�� �ֻ����� �ٲ� �� ������ �������� ����
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

    //���� �ִ� 4���� ����
    public void BuffLocation()
    {
        //��
        if (diceLocation - 5 >= 0)
            upLocate = diceLocation - 5;
        //�Ʒ�
        if (diceLocation + 5 <= 14)
            downLocate = diceLocation + 5;
        //������, �� �ٲ� ����
        if (diceLocation + 1 <= 14 && (diceLocation + 1) % 5 != 0) 
            rightLocate = diceLocation + 1;
        //����, �� �ٲ� ����
        if (diceLocation - 1 >= 0 && diceLocation % 5 != 0)
            leftLocate = diceLocation - 1;

    }

    //���̽��� �ٲ���� �� �ٽ� ���� ����
    public void CheckBuff()
    {
        //dice parent ������Ʈ�� �޾ƿͼ� �ڽĵ��� �˻��Ͽ� location�� �޾Ƽ� ���Ѵ�.
        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
            {
                //������ �ִ� ������ �ƴϰ� �ٲ������
                if (upDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
                    break;

                }

            }
        }

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == downLocate)
            {
                //������ �ִ� ������ �ƴϰ� �ٲ������
                if (downDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                         dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                         (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
                    break;
                }

            }
        }

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == rightLocate)
            {
                //������ �ִ� ������ �ƴϰ� �ٲ������
                if (rightDiceLV != dices.transform.GetChild(i).GetComponent<Dice>().diceLV)
                {
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
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
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
                    break;
                }

            }
        }
    }

    //������ �ش�.
    public void GiveBuff()
    {
        //dice parent ������Ʈ�� �޾ƿͼ� �ڽĵ��� �˻��Ͽ� location�� �޾Ƽ� ���Ѵ�.
        if (upLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
                {
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
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
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
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
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
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
                    //������ �ش�.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime -=
                        dices.transform.GetChild(i).GetComponent<Dice>().attackTime * diceLV *
                        (attackTimePlus + (attackTimePlusClassUp * diceClassLV) + (attackTimePlusPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //������ ����������
                    break;
                }
            }
        }
    }

    
    //������ ���ش�.
    public void DeleteBuff()
    {
        //dice parent ������Ʈ�� �޾ƿͼ� �ڽĵ��� �˻��Ͽ� location�� �޾Ƽ� ���Ѵ�.
        if (upLocate != -1)
        {
            for (int i = 0; i < dices.transform.childCount; i++)
            {
                if (dices.transform.GetChild(i).GetComponent<Dice>().diceLocation == upLocate)
                {
                    //������ �����.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //�������� ��������
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
                    //������ �����.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //�������� ��������
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
                    //������ �����.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //�������� ��������
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
                    //������ �����.
                    dices.transform.GetChild(i).GetComponent<Dice>().attackTime = dices.transform.GetChild(i).GetComponent<Dice>().defaultAttackTime;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color; //�̹��� 7���� ����Ʈ
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(7).GetComponent<Image>().color = color; //�������� ��������
                    break;
                }
            }
        }
    }
    

}