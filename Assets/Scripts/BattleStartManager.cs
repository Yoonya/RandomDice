using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleStartManager : MonoBehaviour
{
    private GameManager gameManager;
    private DiceCreateManager diceCreateManager;
    private AIBehavior aiBehavior;

    public Image[] selectDices = new Image[2]; //������ ���̽� 2�� ������ �޾ƿ´�.
    public Sprite[] dices = new Sprite[16]; //���̽� �̹������� �����Ѵ�.
    public Text selectTxT;
    private int select = 0; //���õ� ����

    public int[] playerNums = new int[5] { -1, -1, -1, -1, -1}; //���õ� ���̽��� ���ڸ� ����
    public int[] AINums = new int[5] { -1, -1, -1, -1, -1 }; //-1�� �ؼ� random�˻翡 �ɸ��� �ʰ�
    public Image[] playerDices = new Image[5]; //���õ� ���̽��� ����
    public Image[] AIDices = new Image[5];

    private int random1; //���̽��� �������� �̾ƿ´�.
    private int random2; //���̽��� �������� �̾ƿ´�.
    private bool isPlayer = false;
    private bool isAI = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        diceCreateManager = FindObjectOfType<DiceCreateManager>();
        aiBehavior = FindObjectOfType<AIBehavior>();
        randomDice();
    }

    public void randomDice() //ó�� ������ �� �ֻ��� �������� �����ֱ�
    {
        while (true)
        {
            while (true)
            {
                random1 = Random.Range(0, 16); //���̽��� �������� �̾ƿ´�.
                random2 = Random.Range(0, 16); //���̽��� �������� �̾ƿ´�.

                if (random1 == random2) //���� ���ٸ� �ٽ�
                {
                    random1 = Random.Range(0, 16);
                    random2 = Random.Range(0, 16);
                }
                else
                    break;
            }

            for (int i = 0; i < playerNums.Length; i++)
            {
                //���� �̹� ���õ� ���ڶ�� �ٽ� ������ ������.
                if (playerNums[i] == random1 || playerNums[i] == random2)
                {
                    isPlayer = false;
                    isAI = false;
                    break;
                }
                else if(i == playerNums.Length - 1) //�˻縦 ������ ��ħ
                    isPlayer = true;
            }
           
            for (int i = 0; i < AINums.Length; i++)
            {
                //���� �̹� ���õ� ���ڶ�� �ٽ� ������ ������.
                if (AINums[i] == random1 || AINums[i] == random2)
                {
                    isPlayer = false;
                    isAI = false;
                    break;
                }
                else if (i == AINums.Length - 1) //�˻縦 ������ ��ħ
                    isAI = true;
            }
           
            //�˻縦 �Ϸ��ϸ� �ݺ��� ����
            if (isPlayer && isAI)
                break;
        }

        //�˻簡 ���� ���� �̹����� ǥ��
        selectDices[0].sprite = dices[random1];
        selectDices[1].sprite = dices[random2];
        selectTxT.text = (select + 1).ToString(); //���ö��� ǥ��
    }

    //����ó�� ��ư
    public void BtnSelectDice()
    {
        //��� Ŭ���� ���� ������Ʈ�� �����ͼ� ����
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        //���̽��� ������ �� ������ ���̽� �̹��� �ٲ��ְ� ���� ����
        if (clickObject.name.Equals("Dice1"))
        {
            playerDices[select].sprite = dices[random1];
            AIDices[select].sprite = dices[random2];
            playerNums[select] = random1;
            AINums[select] = random2;
        }
        else if (clickObject.name.Equals("Dice2"))
        {
            playerDices[select].sprite = dices[random2];
            AIDices[select].sprite = dices[random1];
            playerNums[select] = random2;
            AINums[select] = random1;
        }

        select++;

        //���� 5��° ���ñ����� �ٽ� ȣ���Ѵ�.
        if (select <= 4)
            randomDice();
        else if (select > 4) //��� ���õǾ��ٸ�
        {
            //���� diceCreateManager�� �����ش�.
            for (int i = 0; i < playerNums.Length; i++)
            {
                diceCreateManager.playerNums[i] = playerNums[i];
            }

            gameManager.GameStart(); //���ӽ���
            gameObject.SetActive(false); //�� ������Ʈ�� ��Ȱ��ȭ
        }
    }
}
