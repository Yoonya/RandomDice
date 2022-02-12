using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;
    public Transform respawnLocation;

    public int wave; //웨이브
    public int enemyLV; //적 레벨
    private float appearTime = 2.0f; //기본 등장 시간
    private float appearTime2 = 0.5f; //특수 적 등장 시간
    public int enemyNum = 0; //총 등장 적 수
    public int randomBoss = 0;

    public bool isBoss = false; //보스타임인가 아닌가
    public bool isBossArrive = false; //보스가 살아있는가

    public Coroutine enemyCoroutine;

    //현재 필드에 있는 적을 저장, 라운드 끝날 때마다 초기화
    public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //후에 웨이브 바뀔 때 초기화
        enemyCoroutine = StartCoroutine(StartMakeEnemy(0));                
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoss && !isBossArrive) //보스가 생성될 때라면 생성
        {
            StopCoroutine(enemyCoroutine); //적 만들기 코루틴 정지
            StartCoroutine(StartMakeBoss(randomBoss)); //오브젝트 풀링에 저장되어있는 값에 따라 +3
            isBoss = false; //한번만 실행되도록
            isBossArrive = true;
        }
    }

    IEnumerator StartMakeEnemy(int enemyType) // 적 생성 코루틴
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(appearTime);
            CreateEnemy(enemyType);

            //일정 숫자의 적이 나오면 소형 적 출현
            if(enemyNum % 10 == 0 && enemyNum % 20 != 0)
            {
                yield return new WaitForSeconds(appearTime2);
                CreateEnemy(1);
            }
            //일정 숫자의 적이 나오면 중보스 출현
            if (enemyNum % 20 == 0)
            {
                yield return new WaitForSeconds(appearTime2);
                CreateEnemy(1);
                CreateEnemy(2);
            }
        }
    }

    IEnumerator StartMakeBoss(int bossType) // 적 생성 코루틴
    {
        yield return new WaitForSeconds(appearTime);
        CreateBoss(bossType);
    }


    public void CreateEnemy(int enemyType, int hp = 0, bool isChild = false) //적 생성
    {
        GameObject tempEnemy = ObjectPool.instance.queue[enemyType].Dequeue(); //오브젝트 풀링 사용
        tempEnemy.GetComponent<Enemy>().move = 0;
        tempEnemy.GetComponent<Enemy>().isChild = isChild;
        if (hp == 0)//hp를 지정해주지 않았다면
        {
            tempEnemy.GetComponent<Enemy>().wave = wave;
            tempEnemy.GetComponent<Enemy>().enemyLV = enemyLV;
        }
        else
        {
            tempEnemy.GetComponent<Enemy>().wave = 0;
            tempEnemy.GetComponent<Enemy>().hp = hp;
        }
           
        tempEnemy.transform.localPosition = respawnLocation.transform.localPosition;
        tempEnemy.GetComponent<Enemy>().enemyNum = enemyNum;

        tempEnemy.SetActive(true); //실체화

        enemies.Add(tempEnemy); //리스트에 저장

        enemyNum += 1;
    }

    private void CreateBoss(int bossType) //적 생성
    {
        GameObject tempEnemy = ObjectPool.instance.queue[bossType + 3].Dequeue(); //오브젝트 풀링 사용
        tempEnemy.GetComponent<EnemyBoss>().move = 0;
        tempEnemy.GetComponent<EnemyBoss>().wave = wave;
        tempEnemy.GetComponent<EnemyBoss>().isBossArrive = true;

        tempEnemy.transform.localPosition = respawnLocation.transform.localPosition;

        tempEnemy.SetActive(true); //실체화
    }

    public void ReStartMakeEnemy()
    {
        enemyCoroutine = StartCoroutine(StartMakeEnemy(0));
    }
}
