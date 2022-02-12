using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //private Dice10Crack dice;
    private EnemyManager enemyManager;
    private BattleStartManager battleStartManager;


    public int wave = 1; //웨이브
    public int enemyLV = 1; //적 레벨
    public float time = 90f; // 시간
    public float upTime = 10f; //적 레벨이 증가하는 시간
    public float bossTime = 10f; // 보스가 스킬 쓰는 시간
    public float clearTime = 1f; // 청소시간

    public bool isBoss = false; //보스전중인지 아닌지
    public bool isBossArrive = false; //보스가 살아있는가
    public int randomBoss = 0;

    public Text waveTxT;
    public Text timeTxT;

    public Sprite[] bossSprites = new Sprite[3];
    public Image bossImage; //center에 보스 이미지

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);

        //가장 처음에는 BattleStart부터 실행되어야 한다.
        battleStartManager = FindObjectOfType<BattleStartManager>();
        GamePause(); //BattleStart 될 때까지 정지
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();

        randomBoss = Random.Range(0, 3);
        enemyManager.randomBoss = randomBoss;

        //초기설정
        setWave(1); //wave설정
        enemyManager.enemyLV = 1;//적레벨설정
        enemyManager.randomBoss = randomBoss;//보스설정
        bossImage.sprite = bossSprites[randomBoss];//보스이미지삽입
    }

    // Update is called once per frame
    void Update()
    {
        if (clearTime > 0f) //청소시간 타이머
            clearTime -= Time.deltaTime;
        else
        {
            BulletEffectPlusClear();
            clearTime = 1f;
        }

        if (!isBoss && !isBossArrive) //일반 웨이브 중일때
        {
            if (upTime > 0f) //적 레벨 타이머
                upTime -= Time.deltaTime;
            else
            {
                upTime = 10f;
                enemyLV += 1;
                enemyManager.enemyLV += 1; //enemyManager에 입력
            }

            if (time > 0f) //타이머
                time -= Time.deltaTime;
            else //타이머가 끝나면 초기화
            {
                time = 90f;
                isBoss = true;
            }

            setTime(time);

        }
        else if (isBoss && !isBossArrive)
        {
            enemyManager.isBoss = true; //보스 설정
            isBossArrive = true;
            timeTxT.text = "보스 출현!";
        }
    }

    public void setWave(int wave)
    {
        this.wave = wave;
        enemyManager.wave = wave; //enemyManager에 입력
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
        bossImage.sprite = bossSprites[randomBoss];//보스이미지삽입
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

    // BulletEffect들을 setActive(false)가 실행되었음에도 남아있는 버그로 인해 시간이 없어서 고치지 못하여 임시방편
    public void BulletEffectPlusClear()
    {
        GameObject bulletEffect = GameObject.Find("BulletEffect");
        for (int i = 0; i < 400; i++)
        {
            bulletEffect.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
