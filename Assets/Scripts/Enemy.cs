using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /*
     * 0 = 일반적, 2초마다 소환, 죽으면 상대에게 한번 소환 , 10점
     * 1 = 소형적, 체력이 절반, 이동속도가 1.5배, 10마리마다 소환 , 10점
     * 2 = 준보스, 체력이 5배, 이동속도 8/9, 20마리마다 소환 , 50점
     */
    public int enemyType = 0; //적 타입
    public int enemyNum = 0; //적 고유넘버, 리스트관리
    public int enemyLV = 1; //적 레벨, 오를수록 체력 증가, 10초마다 증가
    public int wave = 1; //웨이브가 높을수록 체력 증가 폭이 높아진다.
    public int maxHP = 100;
    public int hp = 100;
    public int getSP = 10;
    public float speed = 1f;

    public bool isFirst = true; //true일때 죽으면 상대방에게 소환, 죽어서 소환된 적은 false  
    public bool isChild = false; //보스의 부하일 때
    public int move = 0; //0은 위로, 1은 오른쪽으로, 2는 아래로
    private Vector2 originPos;

    //디버프 타입을 적어둔다.
    //0 = poision, 1 = ice, 2 = lock, 3 = crack
    public bool isPoision = false;
    public bool isIce = false;
    public bool isLock = false;
    public bool isCrack = false;
    public float poisionTime = 0f;
    public int poisionDamage = 0;
    public int iceDup = 0; //얼음 중복
    public float iceSlow = 0f; //얼음 이속 줄어드는 양
    public float lockTime = 0f;
    public float lockPer = 0f;
    public int lockCount = 0; //잠금 걸린횟수
    public int crackDup = 0; //크랙 중복
    public Image[] debuffImage = new Image[4];

    public Text HPTxT;

    private EnemyManager enemyManager;
    private DiceCreateManager diceCreateManager;


    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        diceCreateManager = FindObjectOfType<DiceCreateManager>();
        originPos = transform.localPosition;
    }

    void OnEnable()
    {
        if (enemyType == 0)
        {
            //적의 HP설정
            maxHP = enemyLV * wave * 100;
            if(maxHP >= hp) //enemy의 체력을 직접 조정할경우 wave에 0을 넣을것이다.
                hp = maxHP;
            speed = 1f;
        }
        else if (enemyType == 1)
        {
            maxHP = enemyLV * wave * 100 / 2;
            if (maxHP >= hp) //enemy의 체력을 직접 조정할경우 wave에 0을 넣을것이다.
                hp = maxHP;
            speed = 1.5f;
        }
        else if (enemyType == 2)
        {
            maxHP = enemyLV * wave * 100 * 5;
            if (maxHP >= hp) //enemy의 체력을 직접 조정할경우 wave에 0을 넣을것이다.
                hp = maxHP;
            speed = 1.0f * 8 / 9;
        }
    }

    void Update()
    {
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

        //hp가 0이면 죽는다.
        if (hp <= 0)
            Die();

        HPTxT.text = hp.ToString();

        //락 걸리면 speed = 0
        if (isLock && lockTime >= 0)
        {
            lockTime -= Time.deltaTime;
            speed = 0f;
            if (lockTime <= 0) //시간이 다 되면 speed 재설정
            {
                isLock = false;
                if (enemyType == 0)
                    speed = 1f;
                else if (enemyType == 1)
                    speed = 1.5f;
                else if (enemyType == 2)
                    speed = 1.0f * 8 / 9;

                for (int i = 0; i < 4; i++)
                {
                    Color color = debuffImage[i].GetComponent<Image>().color;
                    color.a = 0f;
                    debuffImage[i].GetComponent<Image>().color = color;
                }
            }             
        }

        MoveEnemy();
    }

    private void MoveEnemy() //enemy의 움직임
    {
        //보스전이 아니라면
        if (!enemyManager.isBossArrive || isChild)
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
        else //보스전이라면 보스에게 몰려서 합체연출
        {
            GameObject boss = GameObject.FindWithTag("EnemyBoss");
            if (boss != null)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, boss.transform.localPosition,
                    500f * Time.deltaTime);
            }
        }
    }

    private void Die()
    {
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
        isChild = false;

        for (int i = 0; i < 4; i++)
        {
            Color color = debuffImage[i].GetComponent<Image>().color;
            color.a = 0f;
            debuffImage[i].GetComponent<Image>().color = color;
        }

        //sp획득
        if (enemyType == 0 || enemyType == 1)
            diceCreateManager.remainSP += getSP;
        else if (enemyType == 2)
            diceCreateManager.remainSP += getSP * 5;

        ObjectPool.instance.queue[enemyType].Enqueue(gameObject);//오브젝트풀링 집어넣기
        gameObject.SetActive(false); //비활성화
    }

    private void OnTriggerEnter2D(Collider2D collision) //끝에 다다른다면, 보스와 합쳐지면, 범위딜에 닿으면
    {
        //못 죽일때
        if (collision.CompareTag("EndLocation"))
        {
            PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.life -= 1;
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
            isChild = false;
            for (int i = 0; i < 4; i++)
            {
                Color color = debuffImage[i].GetComponent<Image>().color;
                color.a = 0f;
                debuffImage[i].GetComponent<Image>().color = color;
            }

            ObjectPool.instance.queue[enemyType].Enqueue(this.gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("EnemyBoss") && !isChild)
        {
            if (gameObject.activeSelf == true)
            {
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

                for (int i = 0; i < 4; i++)
                {
                    Color color = debuffImage[i].GetComponent<Image>().color;
                    color.a = 0f;
                    debuffImage[i].GetComponent<Image>().color = color;
                }

                transform.localPosition = originPos; //위치 초기화
                ObjectPool.instance.queue[enemyType].Enqueue(this.gameObject);//오브젝트풀링 집어넣기
                gameObject.SetActive(false); //비활성화
            }
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
                Dice11Crack crackDice = GameObject.Find("Dice11_Crack(Clone)").GetComponent<Dice11Crack>();
                
                //추가 데미지 계산
                hp -= (int)(collision.GetComponent<Bullet>().damage * crackDup *
                 (crackDice.plusDamage + (crackDice.plusDamageClassUp * crackDice.diceClassLV) + (crackDice.plusDamagePowerUp * crackDice.dicePowerLV)));
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

}
