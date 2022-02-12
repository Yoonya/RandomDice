using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceCreateManager : MonoBehaviour
{
    public Transform[] createLocations = new Transform[15]; //다이스 생성 장소
    public GameObject[] dicePrefabs = new GameObject[16]; //다이스 프리펩들
    public List<GameObject> dices = new List<GameObject>(); //생성된 다이스들
    public Text useSPTxT;
    public Text remainSPTxT;
    public Image diceCreateImg;
    public GameObject diceParent;

    public int useSP = 10;
    public int remainSP = 100;

    private int randomDiceNum; //랜덤하게 다이스 생성 
    private int randomLocation; //랜덤하게 생성될 장소 번호
    public List<int> randomLocations = new List<int>(); //중복되지 않게 리스트로 관리

    //플레이어의 다이스
    public int[] playerNums = new int[5];
    public Image[] playerNumsImg = new Image[5];
    public int[] playerNumsUp = new int[5] {100, 100, 100, 100, 100};
    public int[] playerNumsLV = new int[5] { 1, 1, 1, 1, 1 };
    public Text[] playerNumsLVTxT = new Text[5];
    public Text[] playerNumsUPTxT = new Text[5];
    public bool[] playerNumsActive = new bool[5] { true, true, true, true, true };

    // Start is called before the first frame update
    void Start()
    {
        setSPTxT();
        setDiceCreateColor();
    }

    private void Update()
    {
        setSPTxT(); //sp적용
        ActiveButton(); //버튼 활성화 적용
    }

    public void BtnCreateDice()
    {     
        if (IsCreate() && remainSP >= useSP) //필드 최대 수를 넘지 않는 경우와 sp가 남아있는 경우에만
        {
            //sp계산
            remainSP -= useSP;
            useSP += 10;
            setDiceCreateColor();

            while (true)
            {
                //랜덤설정
                randomLocation = Random.Range(0, 15);
                //리스트 안에 중복이 없다면 반복문 종료
                if (!randomLocations.Contains(randomLocation))
                {
                    randomLocations.Add(randomLocation);
                    break;
                }

            }
            //플레이어의 다이스 중 랜덤
            randomDiceNum = Random.Range(0, 5);

            //플레이어의 다이스 중에서 랜덤하게 선택해서 맞는 다이스를 생성
            //이 정도는 오브젝트 풀링의 효과가 적을 것이라 생각해 Instantiate 사용
            GameObject gameObject = Instantiate(dicePrefabs[playerNums[randomDiceNum]]); //생성
            gameObject.transform.localPosition = createLocations[randomLocation].transform.localPosition; //위치 설정
            gameObject.GetComponent<Dice>().diceLocation = randomLocation; //다이스 위치 저장
            gameObject.transform.SetParent(diceParent.transform, false); //다이스 오브젝트에 보관          
            dices.Add(gameObject); //다이스 리스트에 저장
        }
    }

    //다이스 새로 생성
    public void CreateDice(int location, int diceLv)
    {
        if (IsCreate())
        {
            randomDiceNum = Random.Range(0, 5);

            //플레이어의 다이스 중에서 랜덤하게 선택해서 맞는 다이스를 생성
            GameObject gameObject = Instantiate(dicePrefabs[playerNums[randomDiceNum]]); //생성
            gameObject.transform.localPosition = createLocations[location].transform.localPosition; //위치 설정
            gameObject.GetComponent<Dice>().diceLocation = location; //다이스 위치 저장
            gameObject.GetComponent<Dice>().diceLV = diceLv;
            //이전에 설정되어 있던 눈금은 비활성화(1눈금)
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //레벨업 한 후에 눈금 활성화
            gameObject.transform.GetChild(diceLv - 1).gameObject.SetActive(true);

            gameObject.transform.SetParent(diceParent.transform, false); //다이스 오브젝트에 보관     
            dices.Add(gameObject); //다이스 리스트에 저장
        }
    }

    //sp관련 텍스트 설정
    public void setSPTxT()
    {
        useSPTxT.text = useSP.ToString();
        remainSPTxT.text = remainSP.ToString();
    }

    public void ActiveButton()
    {
        for (int i = 0; i < 5; i++)
        {
            if (remainSP >= playerNumsUp[i])
            {
                Color color = playerNumsImg[i].GetComponent<Image>().color;
                color.a = 1f;
                playerNumsImg[i].GetComponent<Image>().color = color; //투명을 불투명으로
                playerNumsActive[i] = true;
            }
            else
            {
                Color color = playerNumsImg[i].GetComponent<Image>().color;
                color.a = 0.5f;
                playerNumsImg[i].GetComponent<Image>().color = color; //불투명을 투명으로
                playerNumsActive[i] = false;
            }
        }
    }

    //활성화 비활성화를 가시적으로 보여주기 위함
    public void setDiceCreateColor()
    {
        if (remainSP >= useSP)
            diceCreateImg.GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255);
        else
        {
            Color color = diceCreateImg.GetComponent<Image>().color;
            color.a = 0.3f;
            diceCreateImg.GetComponent<Image>().color = color;
        }             
    }

    public void BtnPowerUp(int playerNum)
    {
        if (playerNumsActive[playerNum])
        {
            remainSP -= playerNumsUp[playerNum];
            playerNumsLV[playerNum] += 1;
            playerNumsUp[playerNum] += 100;
            playerNumsLVTxT[playerNum].text = playerNumsLV[playerNum].ToString();
            playerNumsUPTxT[playerNum].text = playerNumsUp[playerNum].ToString();
        }
    }

    //생성가능한지 검사
    public bool IsCreate()
    {
        int count = 0;
        for (int i = 0; i < 15; i++)
        {
            if (randomLocations.Contains(i))
            {
                count++;
            }
        }

        //0~14까지 전부 포함되어 있다면 false
        if (count == 15)
            return false;
        else
            return true;
      
    }
}
