using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //private Dice10Crack dice;
    private EnemyManager enemyManager;
    private BattleStartManager battleStartManager;


    public int wave = 1; //���̺�
    public int enemyLV = 1; //�� ����
    public float time = 90f; // �ð�
    public float upTime = 10f; //�� ������ �����ϴ� �ð�
    public float bossTime = 10f; // ������ ��ų ���� �ð�
    public float clearTime = 1f; // û�ҽð�

    public bool isBoss = false; //������������ �ƴ���
    public bool isBossArrive = false; //������ ����ִ°�
    public int randomBoss = 0;

    public Text waveTxT;
    public Text timeTxT;

    public Sprite[] bossSprites = new Sprite[3];
    public Image bossImage; //center�� ���� �̹���

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);

        //���� ó������ BattleStart���� ����Ǿ�� �Ѵ�.
        battleStartManager = FindObjectOfType<BattleStartManager>();
        GamePause(); //BattleStart �� ������ ����
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();

        randomBoss = Random.Range(0, 3);
        enemyManager.randomBoss = randomBoss;

        //�ʱ⼳��
        setWave(1); //wave����
        enemyManager.enemyLV = 1;//����������
        enemyManager.randomBoss = randomBoss;//��������
        bossImage.sprite = bossSprites[randomBoss];//�����̹�������
    }

    // Update is called once per frame
    void Update()
    {
        if (clearTime > 0f) //û�ҽð� Ÿ�̸�
            clearTime -= Time.deltaTime;
        else
        {
            BulletEffectPlusClear();
            clearTime = 1f;
        }

        if (!isBoss && !isBossArrive) //�Ϲ� ���̺� ���϶�
        {
            if (upTime > 0f) //�� ���� Ÿ�̸�
                upTime -= Time.deltaTime;
            else
            {
                upTime = 10f;
                enemyLV += 1;
                enemyManager.enemyLV += 1; //enemyManager�� �Է�
            }

            if (time > 0f) //Ÿ�̸�
                time -= Time.deltaTime;
            else //Ÿ�̸Ӱ� ������ �ʱ�ȭ
            {
                time = 90f;
                isBoss = true;
            }

            setTime(time);

        }
        else if (isBoss && !isBossArrive)
        {
            enemyManager.isBoss = true; //���� ����
            isBossArrive = true;
            timeTxT.text = "���� ����!";
        }
    }

    public void setWave(int wave)
    {
        this.wave = wave;
        enemyManager.wave = wave; //enemyManager�� �Է�
        waveTxT.text = wave.ToString();
    }

    private void setTime(float time)
    {
        if (time >= 60f)
        {
            if (time - 60f >= 10f) 
                timeTxT.text = "01:" + ((int)(time - 60f)).ToString();
            else
                timeTxT.text = "01:0" + ((int)(time - 60f)).ToString();
        }
        else
        {
            if (time >= 10f)
                timeTxT.text = "00:" + ((int)time).ToString();
            else
                timeTxT.text = "00:0" + ((int)time).ToString();
            
        }
       
    }

    public void setBoss(int randomBoss)
    {
        bossImage.sprite = bossSprites[randomBoss];//�����̹�������
        enemyManager.randomBoss = randomBoss;
    }

    public void GamePause()
    {
        Time.timeScale = 0f; 
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
    }

    // BulletEffect���� setActive(false)�� ����Ǿ������� �����ִ� ���׷� ���� �ð��� ��� ��ġ�� ���Ͽ� �ӽù���
    public void BulletEffectPlusClear()
    {
        GameObject bulletEffect = GameObject.Find("BulletEffect");
        for (int i = 0; i < 400; i++)
        {
            bulletEffect.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
