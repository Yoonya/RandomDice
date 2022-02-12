using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private EnemyManager enemyManager;
    private PlayerStatus playerStatus;

    private Vector2 originPos;

    public int diceType;
    public int damage;
    public bool isBrokenDice = false; //무작위 주사위를 한번만 적용시키기 위해

    public bool isCol = false;

    //color
    private float[,] colors = new float[16, 3] //bullet의 색을 정하기 위해, 다이스 눈금의 색과 같음
    {
       { 255f, 0f, 0f},
       { 255f, 255f, 0f},
       { 0f, 207f, 126f},
       { 0f, 195f, 0f},
       { 69f, 91f, 255f},
       { 120f, 120f, 120f},
       { 153f, 0f, 156f},
       { 87f, 0f, 255f},
       { 67f, 67f, 67f},
       { 0f, 255f, 255f},
       { 233f, 245f, 0f},
       { 228f, 0f, 229f},
       { 255f, 89f, 0f},
       { 0f, 245f, 193f},
       { 52f, 87f, 255f},
       { 0f, 27f, 255f}
    };

    private void OnEnable()
    {
        gameObject.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        enemyManager = FindObjectOfType<EnemyManager>(); //실행 순서상
    }

    // Start is called before the first frame update
    void Start()
    {

        playerStatus = FindObjectOfType<PlayerStatus>();

        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCol)
        {
            if (!enemyManager.isBossArrive) //보스전이 아니라면
            {
                //bullet을 가장 앞쪽의 적에게 이동
                GameObject forward = null;

                if (diceType == 3) //독주사위라면
                    forward = FowardPoisionCheck(); //전방이자 독 여부체크
                else if (diceType == 5)
                    forward = HPCheck(); //체력이 가장 많은 적 체크
                else if (diceType == 6 && !isBrokenDice)
                {
                    isBrokenDice = true;
                    forward = RandomCheck(); //무작위 적에게 날아간다.
                }
                else if (diceType == 11)
                    forward = FowardCrackCheck();
                else //일반주사위라면
                    forward = FowardCheck(); //전방체크

                if (forward != null) //null이 반환된 것이 아니라면
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                        forward.transform.localPosition,
                        1500f * Time.deltaTime);
                }
                else //forward가 null이라면 bullet삭제
                {
                    //bullet관리
                    //BulletEffect(); //null 애니메이션처리
                    transform.localPosition = originPos; //위치 초기화
                    ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기                 
                    gameObject.SetActive(false); //비활성화
                }
            }
            else //보스전이라면
            {
                GameObject boss = GameObject.FindWithTag("EnemyBoss"); //전방체크, 보스도 list에 들어있고 부하들 처리를 위해
                GameObject forward = FowardCheck(); //전방체크
                if (forward != null)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                    forward.transform.localPosition,
                    1500f * Time.deltaTime);
                }
                else if (boss != null)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                    boss.transform.localPosition,
                    1500f * Time.deltaTime);
                }
                else //boss가 null이라면 bullet삭제
                {
                    //bullet관리
                    //BulletEffect(); //null 애니메이션처리
                    transform.localPosition = originPos; //위치 초기화
                    ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
                    gameObject.SetActive(false); //비활성화
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //적과 닿는다면
    {
        if (collision.CompareTag("Enemy"))
        {
            isCol = true;
            isBrokenDice = false;

            //bulletEffect효과, 다이스 능력을 적용
            DiceAbility(collision.gameObject);

            //크리티컬 계산
            float critical = playerStatus.critical;
            float criticalDamage = playerStatus.criticalDamage;

            float random = Random.Range(0f, 1f);

            if (random <= critical)
            {
                damage += (int)(damage * criticalDamage);
            }

            //적과 상호작용 크리티컬 데미지는 Enemy에서
            collision.gameObject.GetComponent<Enemy>().hp -= damage;


        }
        if(collision.CompareTag("EnemyBoss"))
        {
            isCol = true;
            isBrokenDice = false;

            //bulletEffect효과, 다이스 능력을 적용
            DiceAbility(collision.gameObject);

            //크리티컬 계산
            float critical = playerStatus.critical;
            float criticalDamage = playerStatus.criticalDamage;

            float random = Random.Range(0f, 1f);

            if (random <= critical)
            {
                damage += (int)(damage * criticalDamage);
            }

            //적과 상호작용 크리티컬 데미지는 Enemy에서
            collision.gameObject.GetComponent<EnemyBoss>().hp -= damage;

        }
    }

    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //다이스 능력을 적용하는부분
    private void DiceAbility(GameObject collision)
    {
        switch (diceType)
        {
            //없는 번호는 추가이펙트가 없음
            case 0:               
                FireDice();
                break;
            case 1:
                ElectricDice(collision);
                break;
            case 3:
                PoisionDice(collision);
                break;
            case 4:
                IceDice(collision);
                break;
            case 5:
                IronDice(collision);
                break;
            case 7:
                GambleDice(collision);
                break;
            case 8:
                LockDice(collision);
                break;
            case 11:
                CrackDice(collision);
                break;
            case 13:
                EnergyDice(collision);
                break;
            default:
                BulletEffect();
                break;
        }
    }


    /*
     * FireDice()의 범위 딜은 Enemy에
     * WindDice()는 Dice2Wind에
     * MineDice()는 Dice9Mine에
     * LightDice()는 Dice10Light에
     * CriticalDice()는 Dice12Critical에
     * SacrificeDice()는 Dice14Sacrifice에
    */
    private void FireDice()
    {
        //불주사위는 주변에 광역데미지, Enemy에서 처리
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        GameObject bulletEffectPlus = ObjectPool.instance.queue[8].Dequeue();
        bulletEffectPlus.SetActive(true);
        bulletEffectPlus.transform.localPosition = transform.localPosition;
        bulletEffectPlus.GetComponent<BulletEffectPlus>().diceType = diceType;
        bulletEffectPlus.GetComponent<Animator>().SetTrigger("Fire");

        StartCoroutine(EndDelay(0.07f, bulletEffect, bulletEffectPlus));
    }

    //타겟을 포함한 3개의 몬스터에게 각각 100%, 70%, 30% [전기]데미지를 입힌다.
    private void ElectricDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        GameObject bulletEffectPlus = ObjectPool.instance.queue[8].Dequeue();
        bulletEffectPlus.SetActive(true);
        bulletEffectPlus.transform.localPosition = transform.localPosition;
        bulletEffectPlus.GetComponent<BulletEffectPlus>().diceType = diceType;
        bulletEffectPlus.GetComponent<Animator>().SetTrigger("Electric");

        float min1 = 1000f; //첫번째로 가까운적 거리
        float min2 = 1000f; //두번째로 가까운적 거리
        GameObject minObject1 = null; //첫번째로 가까운적
        GameObject minObject2 = null; //두번째로 가까운적
        GameObject bulletEffectPlus2 = null;  //첫번째로 가까운적 이펙트
        GameObject bulletEffectPlus3 = null;  //두번째로 가까운적 이펙트

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].activeSelf)
            {
                float distance = Vector2.Distance(collision.gameObject.transform.localPosition, enemyManager.enemies[i].transform.localPosition);
                if (distance != 0 && min2 >= distance) //두번째로 가까운적보다 가깝다면
                {
                    if (min1 >= distance) //첫번째보다 가깝다면
                    {
                        min1 = distance; //첫번째
                        minObject1 = enemyManager.enemies[i];
                    }
                    else
                    {
                        min2 = distance; //아니면 두번째
                        minObject2 = enemyManager.enemies[i];
                    }

                }
            }

        }

        Dice1Electric electricDice = null;
        if (GameObject.Find("Dice1_Electric(Clone)") != null)
            electricDice = GameObject.Find("Dice1_Electric(Clone)").GetComponent<Dice1Electric>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            //BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        int electricDamage1 = (int)(0.7 * (electricDice.electricDamage + (electricDice.electricDamageClassUp * electricDice.diceClassLV)
                  + (electricDice.electricDamagePowerUp * electricDice.dicePowerLV)));
        int electricDamage2 = (int)(0.3 * (electricDice.electricDamage + (electricDice.electricDamageClassUp * electricDice.diceClassLV)
                          + (electricDice.electricDamagePowerUp * electricDice.dicePowerLV)));

        if (minObject1 != null)
        {
            //적과 상호작용
            minObject1.transform.GetComponent<Enemy>().hp -= electricDamage1;
            bulletEffectPlus2 = ObjectPool.instance.queue[8].Dequeue();
            bulletEffectPlus2.SetActive(true);
            bulletEffectPlus2.transform.localPosition = minObject1.transform.localPosition;
            bulletEffectPlus2.GetComponent<BulletEffectPlus>().diceType = diceType;
            bulletEffectPlus2.GetComponent<Animator>().SetTrigger("Electric");
           
        }
        if(minObject2 != null)
        {
            //적과 상호작용
            minObject2.transform.GetComponent<Enemy>().hp -= electricDamage2;
            bulletEffectPlus3 = ObjectPool.instance.queue[8].Dequeue();
            bulletEffectPlus3.SetActive(true);
            bulletEffectPlus3.transform.localPosition = minObject2.transform.localPosition;
            bulletEffectPlus3.GetComponent<BulletEffectPlus>().diceType = diceType;
            bulletEffectPlus3.GetComponent<Animator>().SetTrigger("Electric");
           
        }

        StartCoroutine(EndDelayElectric(0.14f, bulletEffect, bulletEffectPlus, bulletEffectPlus2, bulletEffectPlus3));
    }
    //몬스터 공격 시[독] 디버프를 남겨 초당 독 데미지를 입힌다. [독] 디버프가 없는 몬스터를 우선 공격한다.
    private void PoisionDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        GameObject bulletEffectPlus = ObjectPool.instance.queue[8].Dequeue();
        bulletEffectPlus.SetActive(true);
        bulletEffectPlus.transform.localPosition = transform.localPosition;
        bulletEffectPlus.GetComponent<BulletEffectPlus>().diceType = diceType;
        bulletEffectPlus.GetComponent<Animator>().SetTrigger("Poision");

        Dice3Poision poisionDice = null;
        if(GameObject.Find("Dice3_Poision(Clone)") != null)
            poisionDice = GameObject.Find("Dice3_Poision(Clone)").GetComponent<Dice3Poision>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            // BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("Enemy"))
        {
            //독 주사위는 초당 데미지
            collision.GetComponent<Enemy>().isPoision = true; //디버프 설정
            collision.GetComponent<Enemy>().poisionDamage = poisionDice.poisionDamage + (poisionDice.poisionDamageClassUp * poisionDice.diceClassLV) +
                (poisionDice.poisionDamagePowerUp * poisionDice.dicePowerLV);
            Color color = collision.GetComponent<Enemy>().debuffImage[0].GetComponent<Image>().color; //이미지 0번이 독
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[0].GetComponent<Image>().color = color;
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //독 주사위는 초당 데미지
            collision.GetComponent<EnemyBoss>().isPoision = true; //디버프 설정
            collision.GetComponent<EnemyBoss>().poisionDamage = poisionDice.poisionDamage + (poisionDice.poisionDamageClassUp * poisionDice.diceClassLV) +
                    (poisionDice.poisionDamagePowerUp * poisionDice.dicePowerLV);
            Color color = collision.GetComponent<EnemyBoss>().debuffImage[0].GetComponent<Image>().color; //이미지 0번이 독
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[0].GetComponent<Image>().color = color;
        }


        StartCoroutine(EndDelay(0.09f, bulletEffect, bulletEffectPlus));
    }
    //몬스터 공격 시 [얼음] 디버프를 남겨 이동속도를 감소시킨다. [얼음] 디버프는 최대 3회 중첩된다.
    private void IceDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        GameObject bulletEffectPlus = ObjectPool.instance.queue[8].Dequeue();
        bulletEffectPlus.SetActive(true);
        bulletEffectPlus.transform.localPosition = transform.localPosition;
        bulletEffectPlus.GetComponent<BulletEffectPlus>().diceType = diceType;
        bulletEffectPlus.GetComponent<Animator>().SetTrigger("Ice");

        Dice4Ice iceDice = null;
        if(GameObject.Find("Dice4_Ice(Clone)") != null)
            iceDice = GameObject.Find("Dice4_Ice(Clone)").GetComponent<Dice4Ice>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            //BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("Enemy"))
        {
            //얼음 주사위는 중첩 여부
            collision.GetComponent<Enemy>().isIce = true; //디버프 설정
            collision.GetComponent<Enemy>().iceSlow = iceDice.speedDown + (iceDice.speedDownClassUp * iceDice.diceClassLV) +
                (iceDice.speedDownPowerUp * iceDice.dicePowerLV);
            collision.GetComponent<Enemy>().iceDup++;//얼음 중첩을 늘린다.
            if (collision.GetComponent<Enemy>().iceDup > 3) //3중첩이 넘었다면 그냥 3중첩으로 둔다.
                collision.GetComponent<Enemy>().iceDup = 3;
            Color color = collision.GetComponent<Enemy>().debuffImage[1].GetComponent<Image>().color; //이미지 1번이 얼음
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[1].GetComponent<Image>().color = color; //투명을 불투명으로
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //얼음 주사위는 중첩 여부
            collision.GetComponent<EnemyBoss>().isIce = true; //디버프 설정
            collision.GetComponent<EnemyBoss>().iceSlow = iceDice.speedDown + (iceDice.speedDownClassUp * iceDice.diceClassLV) +
                (iceDice.speedDownPowerUp * iceDice.dicePowerLV);
            collision.GetComponent<EnemyBoss>().iceDup++;//얼음 중첩을 늘린다.
            if (collision.GetComponent<EnemyBoss>().iceDup > 3) //3중첩이 넘었다면 그냥 3중첩으로 둔다.
                collision.GetComponent<EnemyBoss>().iceDup = 3;
            Color color = collision.GetComponent<EnemyBoss>().debuffImage[1].GetComponent<Image>().color; //이미지 1번이 얼음
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[1].GetComponent<Image>().color = color; //투명을 불투명으로
        }

        StartCoroutine(EndDelay(0.10f, bulletEffect, bulletEffectPlus));
    }

    //보스, 중보스 공격 시 2배 데미지를 입힌다. 체력이 가장 많은 몬스터를 우선 공격한다.
    private void IronDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        if (collision.CompareTag("EnemyBoss") || collision.name == "Enemy_Big(Clone)")
            damage *= 2;

        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    //주사위의 기본 공격력부터 크리티컬 데미지까지 무작위 데미지를 입힌다.
    private void GambleDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        Dice7Gamble gambleDice = null;
        if (GameObject.Find("Dice7_Gamble(Clone)") != null)
        {
            gambleDice = GameObject.Find("Dice7_Gamble(Clone)").GetComponent<Dice7Gamble>();
            int minDamage = damage + (gambleDice.damageClassUp * gambleDice.diceClassLV) + (gambleDice.damagePowerUp * gambleDice.dicePowerLV);
            int maxDamage = minDamage;
            maxDamage += (int)(minDamage * gambleDice.criticalDamage);
            damage = Random.Range(minDamage, maxDamage);
        }
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            //BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }


        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    //몬스터 공격 시 일정 확률로[잠금] 디버프를 걸어 일정 시간동안 움직이지 못하게 한다. [잠금] 디버프는 몬스터당 1회만 걸린다.
    private void LockDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        Dice8Lock lockDice = null;
        if(GameObject.Find("Dice8_Lock(Clone)") != null)
            lockDice = GameObject.Find("Dice8_Lock(Clone)").GetComponent<Dice8Lock>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            //BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("Enemy"))
        {
            //잠금 주사위 적용
            if(collision.GetComponent<Enemy>().lockCount == 0)
                collision.GetComponent<Enemy>().lockTime = lockDice.lockTime + (lockDice.lockTimeClassUp * lockDice.diceClassLV) +
                    (lockDice.lockTimePowerUp * lockDice.dicePowerLV); //잠금 지속시간 설정
            collision.GetComponent<Enemy>().lockPer = lockDice.lockPer + (lockDice.lockPerClassUp * lockDice.diceClassLV) +
                (lockDice.lockPerPowerUp * lockDice.dicePowerLV);//잠금 확률 설정
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //잠금 주사위 적용
            if (collision.GetComponent<EnemyBoss>().lockCount == 0)
                collision.GetComponent<EnemyBoss>().lockTime = lockDice.lockTime + (lockDice.lockTimeClassUp * lockDice.diceClassLV) +
                    (lockDice.lockTimePowerUp * lockDice.dicePowerLV); //잠금 지속시간 설정
            collision.GetComponent<EnemyBoss>().lockPer = lockDice.lockPer + (lockDice.lockPerClassUp * lockDice.diceClassLV) +
                (lockDice.lockPerPowerUp * lockDice.dicePowerLV);//잠금 확률 설정
        }


        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    //몬스터 공격 시 [균열] 표식을 남긴다. [균열] 표식이 남은 몬스터는 데미지를 받을 때마다 추가 데미지를 입는다. 
    //[균열] 표식은 최대 3회 중첩되고, 3회 중첩되지 않은 몬스터를 우선 공격한다.
    private void CrackDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        GameObject bulletEffectPlus = ObjectPool.instance.queue[8].Dequeue();
        bulletEffectPlus.SetActive(true);
        bulletEffectPlus.transform.localPosition = transform.localPosition;
        bulletEffectPlus.GetComponent<BulletEffectPlus>().diceType = diceType;
        bulletEffectPlus.GetComponent<Animator>().SetTrigger("Crack");

        Dice11Crack crackDice = null;
        if(GameObject.Find("Dice11_Crack(Clone)") != null)
            crackDice = GameObject.Find("Dice11_Crack(Clone)").GetComponent<Dice11Crack>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            // BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("Enemy"))
        {
            //균열 주사위는 중첩 여부
            collision.GetComponent<Enemy>().isCrack = true; //디버프 설정
            
            collision.GetComponent<Enemy>().crackDup++;//균열 중첩을 늘린다.
            if (collision.GetComponent<Enemy>().crackDup > 3) //3중첩이 넘었다면 그냥 3중첩으로 둔다.
                collision.GetComponent<Enemy>().crackDup = 3;

            Color color = collision.GetComponent<Enemy>().debuffImage[3].GetComponent<Image>().color; //이미지 3번이 균열
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[3].GetComponent<Image>().color = color; //투명을 불투명으로
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //균열 주사위는 중첩 여부
            collision.GetComponent<EnemyBoss>().isCrack = true; //디버프 설정

            collision.GetComponent<EnemyBoss>().crackDup++;//균열 중첩을 늘린다.
            if (collision.GetComponent<EnemyBoss>().crackDup > 3) //3중첩이 넘었다면 그냥 3중첩으로 둔다.
                collision.GetComponent<EnemyBoss>().crackDup = 3;

            Color color = collision.GetComponent<EnemyBoss>().debuffImage[3].GetComponent<Image>().color; //이미지 3번이 균열
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[3].GetComponent<Image>().color = color; //투명을 불투명으로
        }

        StartCoroutine(EndDelay(0.07f, bulletEffect, bulletEffectPlus));
    }

    //몬스터 공격 시 현재 자신이 보유한 SP에 비례하여 추가 데미지를 입힌다.
    private void EnergyDice(GameObject collision)
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        Dice13Energy energyDice = null;
        if(GameObject.Find("Dice13_Energy(Clone)") != null)
            energyDice = GameObject.Find("Dice13_Energy(Clone)").GetComponent<Dice13Energy>();
        else //중간에 합성해서 사라졌거나 하는 이유
        {
            //bullet관리
            //BulletEffect(); //null 애니메이션처리
            transform.localPosition = originPos; //위치 초기화
            ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
            gameObject.SetActive(false); //비활성화
        }
        DiceCreateManager diceCreate = GameObject.Find("DiceCreate").GetComponent<DiceCreateManager>();

        //데미지 계산
        damage += (int)(diceCreate.remainSP * 
            (energyDice.spDamage + (energyDice.spDamageClassUp * energyDice.diceClassLV) + (energyDice.spDamagePowerUp * energyDice.dicePowerLV)));

        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }
    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //애니메이션 끝나기 대기 후 비활성화
    IEnumerator EndDelay(float time, GameObject bulletEffect, GameObject bulletEffectPlus)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//오브젝트풀링 집어넣기
        bulletEffect.SetActive(false);
        ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus);//오브젝트풀링 집어넣기
        bulletEffectPlus.SetActive(false);

        
        transform.localPosition = originPos; //위치 초기화
        
        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
        gameObject.SetActive(false);
    }

    //애니메이션 끝나기 대기 후 비활성화
    IEnumerator EndDelayElectric(float time, GameObject bulletEffect, GameObject bulletEffectPlus, GameObject bulletEffectPlus2, GameObject bulletEffectPlus3)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//오브젝트풀링 집어넣기
        bulletEffect.SetActive(false);
        ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus);//오브젝트풀링 집어넣기
        bulletEffectPlus.SetActive(false);
        if (bulletEffectPlus2 != null)
        {
            ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus2);//오브젝트풀링 집어넣기
            bulletEffectPlus2.SetActive(false);
        }
        if (bulletEffectPlus3 != null)
        {
            ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus3);//오브젝트풀링 집어넣기
            bulletEffectPlus3.SetActive(false);
        }


        transform.localPosition = originPos; //위치 초기화

        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
        gameObject.SetActive(false);
    }

    //bullet 충돌이펙트, null이펙트
    public void BulletEffect()
    {
        GameObject bulletEffect = ObjectPool.instance.queue[7].Dequeue();
        bulletEffect.GetComponent<Image>().color = new Color(colors[diceType, 0] / 255, colors[diceType, 1] / 255, colors[diceType, 2] / 255);
        bulletEffect.transform.localPosition = transform.localPosition;
        bulletEffect.SetActive(true);
        bulletEffect.GetComponent<Animator>().SetTrigger("bulletEffect");

        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    IEnumerator BulletEffectDelay(float time, GameObject bulletEffect)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//오브젝트풀링 집어넣기
        bulletEffect.SetActive(false);

        transform.localPosition = originPos; //위치 초기화
        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//오브젝트풀링 집어넣기
        gameObject.SetActive(false);
    }


    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //원하는 적의 위치를 정하는 부분
    //가장 앞쪽에 있는 적을 체크한다. 가장 기본이므로 이걸로 적이 있는지도 검사
    public GameObject FowardCheck()
    {
        /*
         enemy에서 move를 통해 이동 경로를 알 수 있다. move가 클 수록 가장 앞쪽에 있는 적이다.
         모두 if(move == 0) 일때 y값이 가장 큰 것이 앞쪽이다.
         모두 if(move == 1) 일때 x값이 가장 큰 것이 앞쪽이다.
         모두 if(move == 2) 일때 y값이 가장 작은 것이 앞쪽이다.
         */
        int move0 = 0;
        int move1 = 0;
        int move2 = 0;
        List<GameObject> moves0 = new List<GameObject>();
        List<GameObject> moves1 = new List<GameObject>();
        List<GameObject> moves2 = new List<GameObject>();

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //활성화된 것만
            {
                //move값을 계산하고 각각 리스트에 저장
                if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 0)
                {
                    move0++;
                    moves0.Add(enemyManager.enemies[i]);
                }

                else if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 1)
                {
                    move1++;
                    moves1.Add(enemyManager.enemies[i]);
                }
                else
                {
                    move2++;
                    moves2.Add(enemyManager.enemies[i]);
                }
            }
        }

        //각 move에 1개일 때 그것이 앞쪽
        if (move0 == 1 && move1 == 0 && move2 == 0)
            return moves0[0];
        else if (move1 == 1 && move2 == 0)
            return moves1[0];
        else if (move2 == 1)
            return moves2[0];

        //move0에만 적들이 있을 때 앞쪽을 골라준다.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (max <= moves0[i].transform.localPosition.y)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1끼지만 적들이 있을 때 앞쪽을 골라준다.
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (max <= moves1[i].transform.localPosition.x)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            return forward;
        }

        //move2까지 적들이 있을 때 앞쪽을 골라준다.
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (min >= moves2[i].transform.localPosition.y)
                {
                    min = moves2[i].transform.localPosition.y;
                    forward = moves2[i];
                }
            }
            return forward;
        }

        return null;
    }
    //가장 앞쪽이면서 독이 중독되지 않은 적을 체크한다.
    private GameObject FowardPoisionCheck()
    {
        int move0 = 0;
        int move1 = 0;
        int move2 = 0;
        List<GameObject> moves0 = new List<GameObject>();
        List<GameObject> moves1 = new List<GameObject>();
        List<GameObject> moves2 = new List<GameObject>();

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //활성화된 것만
            {
                //move값을 계산하고 각각 리스트에 저장
                if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 0)
                {
                    move0++;
                    moves0.Add(enemyManager.enemies[i]);
                }

                else if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 1)
                {
                    move1++;
                    moves1.Add(enemyManager.enemies[i]);
                }
                else
                {
                    move2++;
                    moves2.Add(enemyManager.enemies[i]);
                }
            }
        }

        //각 move에 1개일 때 그것이 앞쪽, 그리고 중독이 아닐때
        if (move0 == 1 && move1 == 0 && move2 == 0 && !moves0[0].GetComponent<Enemy>().isPoision)
            return moves0[0];
        else if (move1 == 1 && move2 == 0 && !moves1[0].GetComponent<Enemy>().isPoision)
            return moves1[0];
        else if (move2 == 1 && !moves2[0].GetComponent<Enemy>().isPoision)
            return moves2[0];

        //move0에만 적들이 있을 때 앞쪽을 골라준다.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 중독이 아니면 골라낸다
                if (max <= moves0[i].transform.localPosition.y && !moves0[i].GetComponent<Enemy>().isPoision)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1끼지만 적들이 있을 때 앞쪽을 골라준다. 그리고 중독이 아니면 골라낸다
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (max <= moves1[i].transform.localPosition.x && !moves1[i].GetComponent<Enemy>().isPoision)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            if (forward != null) //값이 입력되었으면 반환
                return forward;
            else //아니라면 move0에서 조사
            {
                float max2 = -576f;
                for (int i = 0; i < moves0.Count; i++)
                {
                    //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 중독이 아니면 골라낸다
                    if (max2 <= moves0[i].transform.localPosition.y && !moves0[i].GetComponent<Enemy>().isPoision)
                    {
                        max2 = moves0[i].transform.localPosition.y;
                        forward = moves0[i];
                    }
                }
                return forward;
            }
        }

        //move2까지 적들이 있을 때 앞쪽을 골라준다. 그리고 중독이 아니면 골라낸다
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (min >= moves2[i].transform.localPosition.y && !moves2[i].GetComponent<Enemy>().isPoision)
                {
                    min = moves2[i].transform.localPosition.y;
                    forward = moves2[i];
                }
            }
            if (forward != null) //값이 입력되었으면 반환
                return forward;
            else //아니라면 move1에서 조사
            {
                float max = -463f;
                for (int i = 0; i < moves1.Count; i++)
                {
                    //x값을 비교해서 가장 앞쪽의 적을 골라낸다.
                    if (max <= moves1[i].transform.localPosition.x && !moves1[i].GetComponent<Enemy>().isPoision)
                    {
                        max = moves1[i].transform.localPosition.x;
                        forward = moves1[i];
                    }
                }
                if (forward != null) //값이 입력되었으면 반환
                    return forward;
                else //아니라면 move0에서 조사
                {
                    float max2 = -576f;
                    for (int i = 0; i < moves0.Count; i++)
                    {
                        //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 중독이 아니면 골라낸다
                        if (max2 <= moves0[i].transform.localPosition.y && !moves0[i].GetComponent<Enemy>().isPoision)
                        {
                            max2 = moves0[i].transform.localPosition.y;
                            forward = moves0[i];
                        }
                    }
                    return forward;
                }
            }
        }
        return null;
    }


    //가장 체력이 많은 적을 체크한다.
    private GameObject HPCheck()
    {  
        int max = 0;
        GameObject maxEnemy = null;
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //활성화된 것만
            {
                if (max <= enemyManager.enemies[i].gameObject.GetComponent<Enemy>().hp)
                {
                    max = enemyManager.enemies[i].gameObject.GetComponent<Enemy>().hp;
                    maxEnemy = enemyManager.enemies[i];
                }

            }
        }
        if (maxEnemy != null) //가장 체력이 많은 적 반환
            return maxEnemy;
        else
            return null;
    }

    //무작위로 적을 체크한다.
    private GameObject RandomCheck()
    {
        GameObject randomEnemy = null;
        int max = 0; //혹시 운 안좋아서 계속 돌 수 있으니 반복 최대 횟수를 넘지 않도록
        while (true)
        {
            if (max == 50) //50번이나 반복했다면 종료
                break;
            if (enemyManager.enemies.Count <= 0) //최소 개수가 안된다면 break
                break;

            int random = Random.Range(0, enemyManager.enemies.Count);
            if (enemyManager.enemies[random].gameObject.activeSelf == true)
            {
                randomEnemy = enemyManager.enemies[random];
                break;
            }
               
            max++;
        }
        if (randomEnemy != null)
            return randomEnemy;
        else
            return null;
    }

    //가장 앞쪽이면서 균열 중복이 3이하인 적을 체크한다.
    private GameObject FowardCrackCheck()
    {
        int move0 = 0;
        int move1 = 0;
        int move2 = 0;
        List<GameObject> moves0 = new List<GameObject>();
        List<GameObject> moves1 = new List<GameObject>();
        List<GameObject> moves2 = new List<GameObject>();

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //활성화된 것만
            {
                //move값을 계산하고 각각 리스트에 저장
                if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 0)
                {
                    move0++;
                    moves0.Add(enemyManager.enemies[i]);
                }

                else if (enemyManager.enemies[i].gameObject.GetComponent<Enemy>().move == 1)
                {
                    move1++;
                    moves1.Add(enemyManager.enemies[i]);
                }
                else
                {
                    move2++;
                    moves2.Add(enemyManager.enemies[i]);
                }
            }
        }

        //각 move에 1개일 때 그것이 앞쪽, 그리고 3중첩이 아닐 때
        if (move0 == 1 && move1 == 0 && move2 == 0 && moves0[0].GetComponent<Enemy>().crackDup < 3)
            return moves0[0];
        else if (move1 == 1 && move2 == 0 && moves1[0].GetComponent<Enemy>().crackDup < 3)
            return moves1[0];
        else if (move2 == 1 && moves2[0].GetComponent<Enemy>().crackDup < 3)
            return moves2[0];

        //move0에만 적들이 있을 때 앞쪽을 골라준다.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 3중첩이 아니면 골라낸다
                if (max <= moves0[i].transform.localPosition.y && moves0[i].GetComponent<Enemy>().crackDup < 3)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1끼지만 적들이 있을 때 앞쪽을 골라준다. 그리고 3중첩이 아니면 골라낸다
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (max <= moves1[i].transform.localPosition.x && moves1[i].GetComponent<Enemy>().crackDup < 3)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            if (forward != null) //값이 입력되었으면 반환
                return forward;
            else //아니라면 move0에서 조사
            {
                float max2 = -576f;
                for (int i = 0; i < moves0.Count; i++)
                {
                    //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 3중첩이 아니면 골라낸다
                    if (max2 <= moves0[i].transform.localPosition.y && moves0[i].GetComponent<Enemy>().crackDup < 3)
                    {
                        max2 = moves0[i].transform.localPosition.y;
                        forward = moves0[i];
                    }
                }
                return forward;
            }
        }

        //move2까지 적들이 있을 때 앞쪽을 골라준다. 그리고 3중첩이 아니면 골라낸다
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y값을 비교해서 가장 앞쪽의 적을 골라낸다.
                if (min >= moves2[i].transform.localPosition.y && moves2[i].GetComponent<Enemy>().crackDup < 3)
                {
                    min = moves2[i].transform.localPosition.y;
                    forward = moves2[i];
                }
            }
            if (forward != null) //값이 입력되었으면 반환
                return forward;
            else //아니라면 move1에서 조사
            {
                float max = -463f;
                for (int i = 0; i < moves1.Count; i++)
                {
                    //x값을 비교해서 가장 앞쪽의 적을 골라낸다.
                    if (max <= moves1[i].transform.localPosition.x && moves1[i].GetComponent<Enemy>().crackDup < 3)
                    {
                        max = moves1[i].transform.localPosition.x;
                        forward = moves1[i];
                    }
                }
                if (forward != null) //값이 입력되었으면 반환
                    return forward;
                else //아니라면 move0에서 조사
                {
                    float max2 = -576f;
                    for (int i = 0; i < moves0.Count; i++)
                    {
                        //y값을 비교해서 가장 앞쪽의 적을 골라낸다. 그리고 중독이 아니면 골라낸다
                        if (max2 <= moves0[i].transform.localPosition.y && moves0[i].GetComponent<Enemy>().crackDup < 3)
                        {
                            max2 = moves0[i].transform.localPosition.y;
                            forward = moves0[i];
                        }
                    }
                    return forward;
                }
            }
        }
        return null;
    }


    //************************************************************************************
    //************************************************************************************
    //************************************************************************************

}
