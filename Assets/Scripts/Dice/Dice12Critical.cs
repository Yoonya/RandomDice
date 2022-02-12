using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//13��° Critical���̽�
public class Dice12Critical : Dice
{
    public float criticalPer = 0.08f;
    public float criticalPerClassUp = 0.002f;
    public float criticalPerPowerUp = 0.05f;

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

    public Dice12Critical()
        : base(12, false, 0, 0f, 0, 0, 0f, 0f)
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    upDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    downDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    rightDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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
                    dices.transform.GetChild(i).GetComponent<Dice>().critical +=
                        dices.transform.GetChild(i).GetComponent<Dice>().critical * diceLV *
                        (criticalPer + (criticalPerClassUp * diceClassLV) + (criticalPerPowerUp * dicePowerLV));

                    leftDiceLV = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 1f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //������ ����������
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

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //�������� ��������
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

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //�������� ��������
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

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //�������� ��������
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

                    Color color = dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color; //�̹��� 6���� ũ��Ƽ��
                    color.a = 0f;
                    dices.transform.GetChild(i).gameObject.transform.GetChild(6).GetComponent<Image>().color = color; //�������� ��������
                    break;
                }
            }
        }
    }
    
}
