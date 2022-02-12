using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceCreateManager : MonoBehaviour
{
    public Transform[] createLocations = new Transform[15]; //���̽� ���� ���
    public GameObject[] dicePrefabs = new GameObject[16]; //���̽� �������
    public List<GameObject> dices = new List<GameObject>(); //������ ���̽���
    public Text useSPTxT;
    public Text remainSPTxT;
    public Image diceCreateImg;
    public GameObject diceParent;

    public int useSP = 10;
    public int remainSP = 100;

    private int randomDiceNum; //�����ϰ� ���̽� ���� 
    private int randomLocation; //�����ϰ� ������ ��� ��ȣ
    public List<int> randomLocations = new List<int>(); //�ߺ����� �ʰ� ����Ʈ�� ����

    //�÷��̾��� ���̽�
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
        setSPTxT(); //sp����
        ActiveButton(); //��ư Ȱ��ȭ ����
    }

    public void BtnCreateDice()
    {     
        if (IsCreate() && remainSP >= useSP) //�ʵ� �ִ� ���� ���� �ʴ� ���� sp�� �����ִ� ��쿡��
        {
            //sp���
            remainSP -= useSP;
            useSP += 10;
            setDiceCreateColor();

            while (true)
            {
                //��������
                randomLocation = Random.Range(0, 15);
                //����Ʈ �ȿ� �ߺ��� ���ٸ� �ݺ��� ����
                if (!randomLocations.Contains(randomLocation))
                {
                    randomLocations.Add(randomLocation);
                    break;
                }

            }
            //�÷��̾��� ���̽� �� ����
            randomDiceNum = Random.Range(0, 5);

            //�÷��̾��� ���̽� �߿��� �����ϰ� �����ؼ� �´� ���̽��� ����
            //�� ������ ������Ʈ Ǯ���� ȿ���� ���� ���̶� ������ Instantiate ���
            GameObject gameObject = Instantiate(dicePrefabs[playerNums[randomDiceNum]]); //����
            gameObject.transform.localPosition = createLocations[randomLocation].transform.localPosition; //��ġ ����
            gameObject.GetComponent<Dice>().diceLocation = randomLocation; //���̽� ��ġ ����
            gameObject.transform.SetParent(diceParent.transform, false); //���̽� ������Ʈ�� ����          
            dices.Add(gameObject); //���̽� ����Ʈ�� ����
        }
    }

    //���̽� ���� ����
    public void CreateDice(int location, int diceLv)
    {
        if (IsCreate())
        {
            randomDiceNum = Random.Range(0, 5);

            //�÷��̾��� ���̽� �߿��� �����ϰ� �����ؼ� �´� ���̽��� ����
            GameObject gameObject = Instantiate(dicePrefabs[playerNums[randomDiceNum]]); //����
            gameObject.transform.localPosition = createLocations[location].transform.localPosition; //��ġ ����
            gameObject.GetComponent<Dice>().diceLocation = location; //���̽� ��ġ ����
            gameObject.GetComponent<Dice>().diceLV = diceLv;
            //������ �����Ǿ� �ִ� ������ ��Ȱ��ȭ(1����)
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //������ �� �Ŀ� ���� Ȱ��ȭ
            gameObject.transform.GetChild(diceLv - 1).gameObject.SetActive(true);

            gameObject.transform.SetParent(diceParent.transform, false); //���̽� ������Ʈ�� ����     
            dices.Add(gameObject); //���̽� ����Ʈ�� ����
        }
    }

    //sp���� �ؽ�Ʈ ����
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
                playerNumsImg[i].GetComponent<Image>().color = color; //������ ����������
                playerNumsActive[i] = true;
            }
            else
            {
                Color color = playerNumsImg[i].GetComponent<Image>().color;
                color.a = 0.5f;
                playerNumsImg[i].GetComponent<Image>().color = color; //�������� ��������
                playerNumsActive[i] = false;
            }
        }
    }

    //Ȱ��ȭ ��Ȱ��ȭ�� ���������� �����ֱ� ����
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

    //������������ �˻�
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

        //0~14���� ���� ���ԵǾ� �ִٸ� false
        if (count == 15)
            return false;
        else
            return true;
      
    }
}
