using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
    /*
     * 모든 보스의 처음 스킬 시전 시간은 2초이며, 그 다음부터는 1.5초간 정지한 뒤 10초마다 스킬을 쓴다. 100점
     * 0 = Snake, 일정 시간마다 정지한 뒤 사각형 몬스터 2기와 원 몬스터 1기를 소환한다.
     * 이 몬스터의 HP는 보스의 남은 HP에 비례하며(10%, 원 몬스터는 5%)
     * 1 = Silence, 일정 시간마다 1.5초간 정지한 뒤 임의의 아군 주사위 두개를 봉인시킨다.
     * 봉인된 주사위는 공격을 하지 못하며, 버프형 주사위는 버프가 사라진다. 해당 보스전이 끝나면 해제된다.
     * 2 = Knight, 일정 시간마다 정지한 뒤 모든 아군 주사위의 종류를 랜덤으로 바꾼다. 
     */
    public int bossType = 0; //보스 타입
    public int wave = 1; //웨이브 레벨
    public int maxHP = 25000;
    public int hp = 25000;
    public int getSP = 100;
    public float speed = 1f;

    public bool isBossArrive = false;

    //스킬 시간
    public float time = 0f;
    public float firstSkillTime = 2f;
    public float skillTime = 10f;
    public float waitTime = 1.5f;
    //스킬 쿨타임상 최대 3번까지 쓸 것이다.
    public bool useSkill1 = false;
    public bool useSkill2 = false;
    public bool useSkill3 = false;

    public int move = 0; //0은 위로, 1은 오른쪽으로, 2는 아래로
    private Vector2 originPos;

    //디버프 타입을 적어둔다.
    //0 = poision, 1 = ice, 2 = lock, 3 = crack
    public bool isPoision = false;
    public bool isIce = false;
    public bool isLock = false;
    public bool isCrack = false;
    public int poisionDamage = 0;
    public float poisionTime = 0f;
    public int iceDup = 0; //얼음 중복
    public float iceSlow = 0f; //얼음 이속 줄어드는 양
    public float lockTime = 0f;
    public float lockPer = 0f;
    public int lockCount = 0; //락걸린 횟수
    public int crackDup = 0; //크랙 중복
    public Image[] debuffImage = new Image[4];

    public Text HPTxT;

    private GameManager gameManager;
    private EnemyManager enemyManager;
    private DiceCreateManager diceCreateManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        diceCreateManager = FindObjectOfType<DiceCreateManager>();

        originPos = transform.localPosition;
    }
    void OnEnable()
    {
        //실행 순서상 해보니 여기 둬야한다.
        enemyManager = FindObjectOfType<EnemyManager>();
        //보스전일때만
        if (isBossArrive)
        {
            maxHP = 25000 * wave + BonusHP();

            hp = maxHP;
        }

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //스킬처리
        if (time >= firstSkillTime)
        {
            if (!useSkill1)
            {
                //스킬 시전중 1.5초대기
                if (waitTime > 0f)
                {
                    speed = 0f;
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    switch (bossType)
                    {
                        case 0:
                            SkillSnake();
                            break;
                        case 1:
                            SkillSilence();
                            break;
                        case 2:
                            SkillKnight();
                            break;
                    }

                    if (waitTime <= 0)
                    {
                        waitTime = 1.5f;
                        speed = 1f;
                    }
                    useSkill1 = true;
                }
  
            }
        }

        if (time >= 2f + skillTime)
        {
            if (!useSkill2)
            {
                //스킬 시전중 1.5초대기
                if (waitTime > 0f)
                {
                    speed = 0f;
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    switch (bossType)
                    {
                        case 0:
                            SkillSnake();
                            break;
                        case 1:
                            SkillSilence();
                            break;
                        case 2:
                            SkillKnight();
                            break;
                    }

                    if (waitTime <= 0)
                    {
                        waitTime = 1.5f;
                        speed = 1f;
                    }
                    useSkill2 = true;
                }
            }
        }

        if (time >= 2f + skillTime * 2f)
        {
            if (!useSkill3)
            {
                //스킬 시전중 1.5초대기
                if (waitTime > 0f)
                {
                    speed = 0f;
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    switch (bossType)
                    {
                        case 0:
                            SkillSnake();
                            break;
                        case 1:
                            SkillSilence();
                            break;
                        case 2:
                            SkillKnight();
                            break;
                    }

                    if (waitTime <= 0)
                    {
                        waitTime = 1.5f;
                        speed = 1f;
                    }
                    useSkill3 = true;
                }
            }
        }


        //독 디버프 상태일때 초당 데미지
        if (isPoision)
        {
            poisionTime += Time.deltaTime;
            if (poisionTime >= 1f)
            {
                hp -= poisionDamage;
                poisionTime = 0f;
            }
        }

        //락 걸리면 speed = 0
        if (isLock && lockTime >= 0)
        {
            lockTime -= Time.deltaTime;
            speed = 0f;
            if (lockTime <= 0) //시간이 다 되면 speed 재설정
            {
                isLock = false;
                speed = 1f;

                for (int i = 0; i < 4; i++)
                {
                    Color color = debuffImage[i].GetComponent<Image>().color;
                    color.a = 0f;
                    debuffImage[i].GetComponent<Image>().color = color;
                }
            }
        }

        //hp가 0이면 죽는다.
        if (hp <= 0)
            Die();
        HPTxT.text = hp.ToString();
        MoveEnemy();
    }

    //보스에게는 보너스hp가 있다. 0.5*(잔여 몬스터의 HP의 합)
    private int BonusHP()
    {
        int bonusHP = 0;
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            //현재 필드에 있는 적들만
            if (enemyManager.enemies[i].gameObject.activeSelf == true)
            {
                bonusHP += (int)(enemyManager.enemies[i].GetComponent<Enemy>().hp * 0.5);
            }

        }

        return bonusHP;
    }

    private void MoveEnemy() //enemy의 움직임
    {
        //처음 움직임, 위로 가다가 오른쪽
        if (transform.localPosition.y > 0)
        {
            move = 1;
            transform.localPosition = new Vector2(transform.localPosition.x, 0);
        }
        else if (transform.localPosition.x > 462) //오른쪽으로 가다가 아래로
        {
            move = 2;
            transform.localPosition = new Vector2(462, transform.localPosition.y);
        }

        if (move == 0)
            transform.localPosition += Vector3.up * speed * 100f * Time.deltaTime;
        else if (move == 1)
            transform.localPosition += Vector3.right * speed * 100f * Time.deltaTime;
        else if (move == 2)
            transform.localPosition += Vector3.down * speed * 100f * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) //끝에 다다른다면
    {
        //보스 처리 실패시
        if (collision.CompareTag("EndLocation"))
        {
            PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.life -= 2; //보스는 2차감
            playerStatus.setLife();

            transform.localPosition = originPos; //위치 초기화

            //디버프 관련 초기화
            isPoision = false;
            isIce = false;
            isLock = false;
            isCrack = false;
            iceDup = 0; //얼음 중복
            poisionDamage = 0;
            iceSlow = 0f; //얼음 이속 줄어드는 양
            lockTime = 0f;
            lockPer = 0f;
            lockCount = 0;
            poisionTime = 0f;
            crackDup = 0; //크랙 중복

            time = 0f;
            firstSkillTime = 2f;
            skillTime = 10f;
            waitTime = 1.5f;
            useSkill1 = false;
            useSkill2 = false;
            useSkill3 = false;

            for (int i = 0; i < 4; i++)
            {
                Color color = debuffImage[i].GetComponent<Image>().color;
                color.a = 0f;
                debuffImage[i].GetComponent<Image>().color = color;
            }

            //보스로 잠금된 다이스 풀어주기
            if (bossType == 1)
            {
                GameObject dices = GameObject.Find("Dice");

                for (int i = 0; i < dices.transform.childCount; i++)
                {
                    Color color = dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color; //이미지 8번이 lock
                    color.a = 0f;
                    dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = color;
                    dices.transform.GetChild(i).GetComponent<Dice>().isLock = false;
                }
            }

            isBossArrive = false;
            gameManager.isBoss = false;
            gameManager.isBossArrive = false;
            gameManager.setWave(++wave); //웨이브 단계 올려주기
            enemyManager.isBoss = false;
            enemyManager.isBossArrive = false;
            enemyManager.enemyNum = 0; //적 수 초기화
            enemyManager.enemies.Clear(); //적들은 모두 초기화
            enemyManager.ReStartMakeEnemy(); //다시 웨이브 시작

            int randomBoss = Random.Range(0, 3);
            gameManager.setBoss(randomBoss); //랜덤보스 재설정

            ObjectPool.instance.queue[bossType + 3].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        //bulletEffect에 닿고 Fire의 이펙트라면 범위딜로 데미지가 들어온다.
        if (collision.CompareTag("BulletEffectPlus") && gameObject.activeSelf == true)
        {
            if (collision.GetComponent<BulletEffectPlus>().diceType == 0)
            {
                //불주사위는 주변에 광역데미지
                //불주사위 정보받아오기
                Dice0Fire fireDice = null;
                if (GameObject.Find("Dice0_Fire(Clone)") != null)
                {
                    fireDice = GameObject.Find("Dice0_Fire(Clone)").GetComponent<Dice0Fire>();
                    //불데미지
                    int fireDamage = fireDice.fireDamage + (fireDice.fireDamageClassUp * fireDice.diceClassLV) +
                        (fireDice.fireDamagePowerUp * fireDice.dicePowerLV);

                    hp -= fireDamage;
                }
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            if (isCrack) //bullet에 맞았는데 균열상태일때
            {
                Dice11Crack crackDice = null;
                if (GameObject.Find("Dice11_Crack(Clone)") != null)
                {
                    crackDice = GameObject.Find("Dice11_Crack(Clone)").GetComponent<Dice11Crack>();

                    //추가 데미지 계산
                    hp -= (int)(collision.GetComponent<Bullet>().damage * crackDup *
                     (crackDice.plusDamage + (crackDice.plusDamageClassUp * crackDice.diceClassLV) + (crackDice.plusDamagePowerUp * crackDice.dicePowerLV)));
                }
            }
            if (collision.GetComponent<Bullet>().diceType == 4 && gameObject.activeSelf == true)
            {
                //얼음 디버프 상태일때 얼음중첩만큼 slow효과
                if (isIce)
                {
                    speed = 1f - iceDup * iceSlow;
                }
            }

            if (collision.GetComponent<Bullet>().diceType == 8 && gameObject.activeSelf == true)
            {
                if (lockCount == 0) //한번도 잠금된 적이 없다면
                {
                    //잠금 bullet을 맞으면 일정확률로 잠금
                    float range = Random.Range(0f, 1f);

                    if (range <= lockPer) //잠금 확률에 걸리면
                    {
                        isLock = true;
                        lockCount = 1;

                        Color color = debuffImage[2].GetComponent<Image>().color; //이미지 2번이 잠금
                        color.a = 1f;
                        debuffImage[2].GetComponent<Image>().color = color; //투명을 불투명으로
                    }
                }

            }
        }
    }

    private void Die()
    {
        //보스처리 성공시
        transform.localPosition = originPos; //위치 초기화

        //디버프 관련 초기화
        isPoision = false;
        isIce = false;
        isLock = false;
        isCrack = false;
        iceSlow = 0f; //얼음 이속 줄어드는 양
        poisionDamage = 0;
        iceDup = 0; //얼음 중복
        lockTime = 0f;
        lockPer = 0f;
        poisionTime = 0f;
        crackDup = 0; //크랙 중복

        time = 0f;
        firstSkillTime = 2f;
        skillTime = 10f;
        waitTime = 1.5f;
        useSkill1 = false;
        useSkill2 = false;
        useSkill3 = false;

        for (int i = 0; i < 4; i++)
        {
            Color color = debuffImage[i].GetComponent<Image>().color;
            color.a = 0f;
            debuffImage[i].GetComponent<Image>().color = color;
        }

        //보스로 잠금된 다이스 풀어주기
        if (bossType == 1)
        {
            GameObject dices = GameObject.Find("Dice");

            for (int i = 0; i < dices.transform.childCount; i++)
            {
                Color color = dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color; //이미지 8번이 lock
                color.a = 0f;
                dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = color;
                dices.transform.GetChild(i).GetComponent<Dice>().isLock = false;
            }
        }

        isBossArrive = false;
        gameManager.isBoss = false;
        gameManager.isBossArrive = false;
        gameManager.setWave(++wave); //웨이브 단계 올려주기
        enemyManager.isBoss = false;
        enemyManager.isBossArrive = false;
        enemyManager.enemyNum = 0; //적 수 초기화
        enemyManager.enemies.Clear(); //적들은 모두 초기화
        enemyManager.ReStartMakeEnemy(); //다시 웨이브 시작

        //sp획득
        diceCreateManager.remainSP += getSP;

        int randomBoss = Random.Range(0, 3);
        gameManager.setBoss(randomBoss); //랜덤보스 재설정

        ObjectPool.instance.queue[bossType + 3].Enqueue(gameObject);//오브젝트풀링 집어넣기
        gameObject.SetActive(false); //비활성화
    }

    //보스 스킬들 
    //0 = Snake, 일정 시간마다 정지한 뒤 사각형 몬스터 2기와 원 몬스터 1기를 소환한다.
    // 이 몬스터의 HP는 보스의 남은 HP에 비례하며(10%, 원 몬스터는 5%)
    public void SkillSnake()
    {
        enemyManager.CreateEnemy(0, (int)(hp * 0.1f), true);
        enemyManager.CreateEnemy(1, (int)(hp * 0.05f), true);
    }

    // 1 = Silence, 일정 시간마다 1.5초간 정지한 뒤 임의의 아군 주사위 두개를 봉인시킨다.
    //봉인된 주사위는 공격을 하지 못하며, 버프형 주사위는 버프가 사라진다. 해당 보스전이 끝나면 해제된다.
    //유니티 게임창에서는 잘 실행되나, 빌드 후에는 구간 반복??이 있다.
    public void SkillSilence()
    {
        GameObject dices = GameObject.Find("Dice");
        int random1 = 0;
        int random2 = 0;
        int count = 0;
        while (true)
        {
            count++;
            random1 = Random.Range(0, dices.transform.childCount);
            random2 = Random.Range(0, dices.transform.childCount);

            //일단 둘이 다른 숫자이고
            if (random1 != random2)
                //두 값이 잠기지않은 상태라면
                if (!dices.transform.GetChild(random1).GetComponent<Dice>().isLock &&
                    !dices.transform.GetChild(random2).GetComponent<Dice>().isLock)
                    break;

            if (count >= 50) //무한반복 방지
                break;
        }

        Color color = dices.transform.GetChild(random1).GetChild(8).GetComponent<Image>().color; //이미지 8번이 lock
        color.a = 1f;
        dices.transform.GetChild(random1).GetChild(8).GetComponent<Image>().color = color;
        dices.transform.GetChild(random1).GetComponent<Dice>().isLock = true;

        Color color2 = dices.transform.GetChild(random2).GetChild(8).GetComponent<Image>().color; //이미지 8번이 lock
        color2.a = 1f;
        dices.transform.GetChild(random2).GetChild(8).GetComponent<Image>().color = color2;
        dices.transform.GetChild(random2).GetComponent<Dice>().isLock = true;

    }

    //2 = Knight, 일정 시간마다 정지한 뒤 모든 아군 주사위의 종류를 랜덤으로 바꾼다. 
    //현재 버그 정지됨, 빌드 후에는 스킬 실행만 안되고 정지는 안됨
    public void SkillKnight()
    {
        GameObject dices = GameObject.Find("Dice");

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            //전부 지우고 위치와 눈금 수만 가지고 다시 만든다. createDice에 랜덤이 있으므로 랜덤된다.
            int diceLocation = dices.transform.GetChild(i).GetComponent<Dice>().diceLocation;
            int diceLv = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;
      
            diceCreateManager.dices.Remove(dices.transform.GetChild(i).gameObject);
            diceCreateManager.randomLocations.Remove(diceLocation);
            Destroy(dices.transform.GetChild(i).gameObject);

            diceCreateManager.CreateDice(diceLocation, diceLv);

        }
        
    }
}
