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

    public Image[] selectDices = new Image[2]; //선택할 다이스 2개 변수를 받아온다.
    public Sprite[] dices = new Sprite[16]; //다이스 이미지들을 저장한다.
    public Text selectTxT;
    private int select = 0; //선택된 갯수

    public int[] playerNums = new int[5] { -1, -1, -1, -1, -1}; //선택된 다이스의 숫자를 저장
    public int[] AINums = new int[5] { -1, -1, -1, -1, -1 }; //-1로 해서 random검사에 걸리지 않게
    public Image[] playerDices = new Image[5]; //선택된 다이스를 저장
    public Image[] AIDices = new Image[5];

    private int random1; //다이스를 랜덤으로 뽑아온다.
    private int random2; //다이스를 랜덤으로 뽑아온다.
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

    public void randomDice() //처음 시작할 때 주사위 랜덤으로 보여주기
    {
        while (true)
        {
            while (true)
            {
                random1 = Random.Range(0, 16); //다이스를 랜덤으로 뽑아온다.
                random2 = Random.Range(0, 16); //다이스를 랜덤으로 뽑아온다.

                if (random1 == random2) //둘이 같다면 다시
                {
                    random1 = Random.Range(0, 16);
                    random2 = Random.Range(0, 16);
                }
                else
                    break;
            }

            for (int i = 0; i < playerNums.Length; i++)
            {
                //만약 이미 선택된 숫자라면 다시 랜덤을 돌린다.
                if (playerNums[i] == random1 || playerNums[i] == random2)
                {
                    isPlayer = false;
                    isAI = false;
                    break;
                }
                else if(i == playerNums.Length - 1) //검사를 끝까지 마침
                    isPlayer = true;
            }
           
            for (int i = 0; i < AINums.Length; i++)
            {
                //만약 이미 선택된 숫자라면 다시 랜덤을 돌린다.
                if (AINums[i] == random1 || AINums[i] == random2)
                {
                    isPlayer = false;
                    isAI = false;
                    break;
                }
                else if (i == AINums.Length - 1) //검사를 끝까지 마침
                    isAI = true;
            }
           
            //검사를 완료하면 반복문 퇴장
            if (isPlayer && isAI)
                break;
        }

        //검사가 끝난 것을 이미지로 표시
        selectDices[0].sprite = dices[random1];
        selectDices[1].sprite = dices[random2];
        selectTxT.text = (select + 1).ToString(); //선택라운드 표시
    }

    //선택처리 버튼
    public void BtnSelectDice()
    {
        //방금 클릭한 게임 오브젝트를 가져와서 저장
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        //다이스를 눌렀을 때 각각에 다이스 이미지 바꿔주고 숫자 저장
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

        //아직 5번째 선택까지면 다시 호출한다.
        if (select <= 4)
            randomDice();
        else if (select > 4) //모두 선택되었다면
        {
            //값을 diceCreateManager로 보내준다.
            for (int i = 0; i < playerNums.Length; i++)
            {
                diceCreateManager.playerNums[i] = playerNums[i];
            }

            gameManager.GameStart(); //게임시작
            gameObject.SetActive(false); //이 오브젝트는 비활성화
        }
    }
}
