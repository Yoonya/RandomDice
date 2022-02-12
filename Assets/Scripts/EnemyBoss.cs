using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
    /*
     * ��� ������ ó�� ��ų ���� �ð��� 2���̸�, �� �������ʹ� 1.5�ʰ� ������ �� 10�ʸ��� ��ų�� ����. 100��
     * 0 = Snake, ���� �ð����� ������ �� �簢�� ���� 2��� �� ���� 1�⸦ ��ȯ�Ѵ�.
     * �� ������ HP�� ������ ���� HP�� ����ϸ�(10%, �� ���ʹ� 5%)
     * 1 = Silence, ���� �ð����� 1.5�ʰ� ������ �� ������ �Ʊ� �ֻ��� �ΰ��� ���ν�Ų��.
     * ���ε� �ֻ����� ������ ���� ���ϸ�, ������ �ֻ����� ������ �������. �ش� �������� ������ �����ȴ�.
     * 2 = Knight, ���� �ð����� ������ �� ��� �Ʊ� �ֻ����� ������ �������� �ٲ۴�. 
     */
    public int bossType = 0; //���� Ÿ��
    public int wave = 1; //���̺� ����
    public int maxHP = 25000;
    public int hp = 25000;
    public int getSP = 100;
    public float speed = 1f;

    public bool isBossArrive = false;

    //��ų �ð�
    public float time = 0f;
    public float firstSkillTime = 2f;
    public float skillTime = 10f;
    public float waitTime = 1.5f;
    //��ų ��Ÿ�ӻ� �ִ� 3������ �� ���̴�.
    public bool useSkill1 = false;
    public bool useSkill2 = false;
    public bool useSkill3 = false;

    public int move = 0; //0�� ����, 1�� ����������, 2�� �Ʒ���
    private Vector2 originPos;

    //����� Ÿ���� ����д�.
    //0 = poision, 1 = ice, 2 = lock, 3 = crack
    public bool isPoision = false;
    public bool isIce = false;
    public bool isLock = false;
    public bool isCrack = false;
    public int poisionDamage = 0;
    public float poisionTime = 0f;
    public int iceDup = 0; //���� �ߺ�
    public float iceSlow = 0f; //���� �̼� �پ��� ��
    public float lockTime = 0f;
    public float lockPer = 0f;
    public int lockCount = 0; //���ɸ� Ƚ��
    public int crackDup = 0; //ũ�� �ߺ�
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
        //���� ������ �غ��� ���� �־��Ѵ�.
        enemyManager = FindObjectOfType<EnemyManager>();
        //�������϶���
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
        //��ųó��
        if (time >= firstSkillTime)
        {
            if (!useSkill1)
            {
                //��ų ������ 1.5�ʴ��
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
                //��ų ������ 1.5�ʴ��
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
                //��ų ������ 1.5�ʴ��
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


        //�� ����� �����϶� �ʴ� ������
        if (isPoision)
        {
            poisionTime += Time.deltaTime;
            if (poisionTime >= 1f)
            {
                hp -= poisionDamage;
                poisionTime = 0f;
            }
        }

        //�� �ɸ��� speed = 0
        if (isLock && lockTime >= 0)
        {
            lockTime -= Time.deltaTime;
            speed = 0f;
            if (lockTime <= 0) //�ð��� �� �Ǹ� speed �缳��
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

        //hp�� 0�̸� �״´�.
        if (hp <= 0)
            Die();
        HPTxT.text = hp.ToString();
        MoveEnemy();
    }

    //�������Դ� ���ʽ�hp�� �ִ�. 0.5*(�ܿ� ������ HP�� ��)
    private int BonusHP()
    {
        int bonusHP = 0;
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            //���� �ʵ忡 �ִ� ���鸸
            if (enemyManager.enemies[i].gameObject.activeSelf == true)
            {
                bonusHP += (int)(enemyManager.enemies[i].GetComponent<Enemy>().hp * 0.5);
            }

        }

        return bonusHP;
    }

    private void MoveEnemy() //enemy�� ������
    {
        //ó�� ������, ���� ���ٰ� ������
        if (transform.localPosition.y > 0)
        {
            move = 1;
            transform.localPosition = new Vector2(transform.localPosition.x, 0);
        }
        else if (transform.localPosition.x > 462) //���������� ���ٰ� �Ʒ���
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

    private void OnTriggerEnter2D(Collider2D collision) //���� �ٴٸ��ٸ�
    {
        //���� ó�� ���н�
        if (collision.CompareTag("EndLocation"))
        {
            PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.life -= 2; //������ 2����
            playerStatus.setLife();

            transform.localPosition = originPos; //��ġ �ʱ�ȭ

            //����� ���� �ʱ�ȭ
            isPoision = false;
            isIce = false;
            isLock = false;
            isCrack = false;
            iceDup = 0; //���� �ߺ�
            poisionDamage = 0;
            iceSlow = 0f; //���� �̼� �پ��� ��
            lockTime = 0f;
            lockPer = 0f;
            lockCount = 0;
            poisionTime = 0f;
            crackDup = 0; //ũ�� �ߺ�

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

            //������ ��ݵ� ���̽� Ǯ���ֱ�
            if (bossType == 1)
            {
                GameObject dices = GameObject.Find("Dice");

                for (int i = 0; i < dices.transform.childCount; i++)
                {
                    Color color = dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color; //�̹��� 8���� lock
                    color.a = 0f;
                    dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = color;
                    dices.transform.GetChild(i).GetComponent<Dice>().isLock = false;
                }
            }

            isBossArrive = false;
            gameManager.isBoss = false;
            gameManager.isBossArrive = false;
            gameManager.setWave(++wave); //���̺� �ܰ� �÷��ֱ�
            enemyManager.isBoss = false;
            enemyManager.isBossArrive = false;
            enemyManager.enemyNum = 0; //�� �� �ʱ�ȭ
            enemyManager.enemies.Clear(); //������ ��� �ʱ�ȭ
            enemyManager.ReStartMakeEnemy(); //�ٽ� ���̺� ����

            int randomBoss = Random.Range(0, 3);
            gameManager.setBoss(randomBoss); //�������� �缳��

            ObjectPool.instance.queue[bossType + 3].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        //bulletEffect�� ��� Fire�� ����Ʈ��� �������� �������� ���´�.
        if (collision.CompareTag("BulletEffectPlus") && gameObject.activeSelf == true)
        {
            if (collision.GetComponent<BulletEffectPlus>().diceType == 0)
            {
                //���ֻ����� �ֺ��� ����������
                //���ֻ��� �����޾ƿ���
                Dice0Fire fireDice = null;
                if (GameObject.Find("Dice0_Fire(Clone)") != null)
                {
                    fireDice = GameObject.Find("Dice0_Fire(Clone)").GetComponent<Dice0Fire>();
                    //�ҵ�����
                    int fireDamage = fireDice.fireDamage + (fireDice.fireDamageClassUp * fireDice.diceClassLV) +
                        (fireDice.fireDamagePowerUp * fireDice.dicePowerLV);

                    hp -= fireDamage;
                }
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            if (isCrack) //bullet�� �¾Ҵµ� �տ������϶�
            {
                Dice11Crack crackDice = null;
                if (GameObject.Find("Dice11_Crack(Clone)") != null)
                {
                    crackDice = GameObject.Find("Dice11_Crack(Clone)").GetComponent<Dice11Crack>();

                    //�߰� ������ ���
                    hp -= (int)(collision.GetComponent<Bullet>().damage * crackDup *
                     (crackDice.plusDamage + (crackDice.plusDamageClassUp * crackDice.diceClassLV) + (crackDice.plusDamagePowerUp * crackDice.dicePowerLV)));
                }
            }
            if (collision.GetComponent<Bullet>().diceType == 4 && gameObject.activeSelf == true)
            {
                //���� ����� �����϶� ������ø��ŭ slowȿ��
                if (isIce)
                {
                    speed = 1f - iceDup * iceSlow;
                }
            }

            if (collision.GetComponent<Bullet>().diceType == 8 && gameObject.activeSelf == true)
            {
                if (lockCount == 0) //�ѹ��� ��ݵ� ���� ���ٸ�
                {
                    //��� bullet�� ������ ����Ȯ���� ���
                    float range = Random.Range(0f, 1f);

                    if (range <= lockPer) //��� Ȯ���� �ɸ���
                    {
                        isLock = true;
                        lockCount = 1;

                        Color color = debuffImage[2].GetComponent<Image>().color; //�̹��� 2���� ���
                        color.a = 1f;
                        debuffImage[2].GetComponent<Image>().color = color; //������ ����������
                    }
                }

            }
        }
    }

    private void Die()
    {
        //����ó�� ������
        transform.localPosition = originPos; //��ġ �ʱ�ȭ

        //����� ���� �ʱ�ȭ
        isPoision = false;
        isIce = false;
        isLock = false;
        isCrack = false;
        iceSlow = 0f; //���� �̼� �پ��� ��
        poisionDamage = 0;
        iceDup = 0; //���� �ߺ�
        lockTime = 0f;
        lockPer = 0f;
        poisionTime = 0f;
        crackDup = 0; //ũ�� �ߺ�

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

        //������ ��ݵ� ���̽� Ǯ���ֱ�
        if (bossType == 1)
        {
            GameObject dices = GameObject.Find("Dice");

            for (int i = 0; i < dices.transform.childCount; i++)
            {
                Color color = dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color; //�̹��� 8���� lock
                color.a = 0f;
                dices.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = color;
                dices.transform.GetChild(i).GetComponent<Dice>().isLock = false;
            }
        }

        isBossArrive = false;
        gameManager.isBoss = false;
        gameManager.isBossArrive = false;
        gameManager.setWave(++wave); //���̺� �ܰ� �÷��ֱ�
        enemyManager.isBoss = false;
        enemyManager.isBossArrive = false;
        enemyManager.enemyNum = 0; //�� �� �ʱ�ȭ
        enemyManager.enemies.Clear(); //������ ��� �ʱ�ȭ
        enemyManager.ReStartMakeEnemy(); //�ٽ� ���̺� ����

        //spȹ��
        diceCreateManager.remainSP += getSP;

        int randomBoss = Random.Range(0, 3);
        gameManager.setBoss(randomBoss); //�������� �缳��

        ObjectPool.instance.queue[bossType + 3].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
        gameObject.SetActive(false); //��Ȱ��ȭ
    }

    //���� ��ų�� 
    //0 = Snake, ���� �ð����� ������ �� �簢�� ���� 2��� �� ���� 1�⸦ ��ȯ�Ѵ�.
    // �� ������ HP�� ������ ���� HP�� ����ϸ�(10%, �� ���ʹ� 5%)
    public void SkillSnake()
    {
        enemyManager.CreateEnemy(0, (int)(hp * 0.1f), true);
        enemyManager.CreateEnemy(1, (int)(hp * 0.05f), true);
    }

    // 1 = Silence, ���� �ð����� 1.5�ʰ� ������ �� ������ �Ʊ� �ֻ��� �ΰ��� ���ν�Ų��.
    //���ε� �ֻ����� ������ ���� ���ϸ�, ������ �ֻ����� ������ �������. �ش� �������� ������ �����ȴ�.
    //����Ƽ ����â������ �� ����ǳ�, ���� �Ŀ��� ���� �ݺ�??�� �ִ�.
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

            //�ϴ� ���� �ٸ� �����̰�
            if (random1 != random2)
                //�� ���� ��������� ���¶��
                if (!dices.transform.GetChild(random1).GetComponent<Dice>().isLock &&
                    !dices.transform.GetChild(random2).GetComponent<Dice>().isLock)
                    break;

            if (count >= 50) //���ѹݺ� ����
                break;
        }

        Color color = dices.transform.GetChild(random1).GetChild(8).GetComponent<Image>().color; //�̹��� 8���� lock
        color.a = 1f;
        dices.transform.GetChild(random1).GetChild(8).GetComponent<Image>().color = color;
        dices.transform.GetChild(random1).GetComponent<Dice>().isLock = true;

        Color color2 = dices.transform.GetChild(random2).GetChild(8).GetComponent<Image>().color; //�̹��� 8���� lock
        color2.a = 1f;
        dices.transform.GetChild(random2).GetChild(8).GetComponent<Image>().color = color2;
        dices.transform.GetChild(random2).GetComponent<Dice>().isLock = true;

    }

    //2 = Knight, ���� �ð����� ������ �� ��� �Ʊ� �ֻ����� ������ �������� �ٲ۴�. 
    //���� ���� ������, ���� �Ŀ��� ��ų ���ุ �ȵǰ� ������ �ȵ�
    public void SkillKnight()
    {
        GameObject dices = GameObject.Find("Dice");

        for (int i = 0; i < dices.transform.childCount; i++)
        {
            //���� ����� ��ġ�� ���� ���� ������ �ٽ� �����. createDice�� ������ �����Ƿ� �����ȴ�.
            int diceLocation = dices.transform.GetChild(i).GetComponent<Dice>().diceLocation;
            int diceLv = dices.transform.GetChild(i).GetComponent<Dice>().diceLV;
      
            diceCreateManager.dices.Remove(dices.transform.GetChild(i).gameObject);
            diceCreateManager.randomLocations.Remove(diceLocation);
            Destroy(dices.transform.GetChild(i).gameObject);

            diceCreateManager.CreateDice(diceLocation, diceLv);

        }
        
    }
}
