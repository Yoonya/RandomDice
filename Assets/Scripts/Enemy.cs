using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /*
     * 0 = �Ϲ���, 2�ʸ��� ��ȯ, ������ ��뿡�� �ѹ� ��ȯ , 10��
     * 1 = ������, ü���� ����, �̵��ӵ��� 1.5��, 10�������� ��ȯ , 10��
     * 2 = �غ���, ü���� 5��, �̵��ӵ� 8/9, 20�������� ��ȯ , 50��
     */
    public int enemyType = 0; //�� Ÿ��
    public int enemyNum = 0; //�� �����ѹ�, ����Ʈ����
    public int enemyLV = 1; //�� ����, �������� ü�� ����, 10�ʸ��� ����
    public int wave = 1; //���̺갡 �������� ü�� ���� ���� ��������.
    public int maxHP = 100;
    public int hp = 100;
    public int getSP = 10;
    public float speed = 1f;

    public bool isFirst = true; //true�϶� ������ ���濡�� ��ȯ, �׾ ��ȯ�� ���� false  
    public bool isChild = false; //������ ������ ��
    public int move = 0; //0�� ����, 1�� ����������, 2�� �Ʒ���
    private Vector2 originPos;

    //����� Ÿ���� ����д�.
    //0 = poision, 1 = ice, 2 = lock, 3 = crack
    public bool isPoision = false;
    public bool isIce = false;
    public bool isLock = false;
    public bool isCrack = false;
    public float poisionTime = 0f;
    public int poisionDamage = 0;
    public int iceDup = 0; //���� �ߺ�
    public float iceSlow = 0f; //���� �̼� �پ��� ��
    public float lockTime = 0f;
    public float lockPer = 0f;
    public int lockCount = 0; //��� �ɸ�Ƚ��
    public int crackDup = 0; //ũ�� �ߺ�
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
            //���� HP����
            maxHP = enemyLV * wave * 100;
            if(maxHP >= hp) //enemy�� ü���� ���� �����Ұ�� wave�� 0�� �������̴�.
                hp = maxHP;
            speed = 1f;
        }
        else if (enemyType == 1)
        {
            maxHP = enemyLV * wave * 100 / 2;
            if (maxHP >= hp) //enemy�� ü���� ���� �����Ұ�� wave�� 0�� �������̴�.
                hp = maxHP;
            speed = 1.5f;
        }
        else if (enemyType == 2)
        {
            maxHP = enemyLV * wave * 100 * 5;
            if (maxHP >= hp) //enemy�� ü���� ���� �����Ұ�� wave�� 0�� �������̴�.
                hp = maxHP;
            speed = 1.0f * 8 / 9;
        }
    }

    void Update()
    {
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

        //hp�� 0�̸� �״´�.
        if (hp <= 0)
            Die();

        HPTxT.text = hp.ToString();

        //�� �ɸ��� speed = 0
        if (isLock && lockTime >= 0)
        {
            lockTime -= Time.deltaTime;
            speed = 0f;
            if (lockTime <= 0) //�ð��� �� �Ǹ� speed �缳��
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

    private void MoveEnemy() //enemy�� ������
    {
        //�������� �ƴ϶��
        if (!enemyManager.isBossArrive || isChild)
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
        else //�������̶�� �������� ������ ��ü����
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
        isChild = false;

        for (int i = 0; i < 4; i++)
        {
            Color color = debuffImage[i].GetComponent<Image>().color;
            color.a = 0f;
            debuffImage[i].GetComponent<Image>().color = color;
        }

        //spȹ��
        if (enemyType == 0 || enemyType == 1)
            diceCreateManager.remainSP += getSP;
        else if (enemyType == 2)
            diceCreateManager.remainSP += getSP * 5;

        ObjectPool.instance.queue[enemyType].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
        gameObject.SetActive(false); //��Ȱ��ȭ
    }

    private void OnTriggerEnter2D(Collider2D collision) //���� �ٴٸ��ٸ�, ������ ��������, �������� ������
    {
        //�� ���϶�
        if (collision.CompareTag("EndLocation"))
        {
            PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.life -= 1;
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
            isChild = false;
            for (int i = 0; i < 4; i++)
            {
                Color color = debuffImage[i].GetComponent<Image>().color;
                color.a = 0f;
                debuffImage[i].GetComponent<Image>().color = color;
            }

            ObjectPool.instance.queue[enemyType].Enqueue(this.gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        if (collision.CompareTag("EnemyBoss") && !isChild)
        {
            if (gameObject.activeSelf == true)
            {
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

                for (int i = 0; i < 4; i++)
                {
                    Color color = debuffImage[i].GetComponent<Image>().color;
                    color.a = 0f;
                    debuffImage[i].GetComponent<Image>().color = color;
                }

                transform.localPosition = originPos; //��ġ �ʱ�ȭ
                ObjectPool.instance.queue[enemyType].Enqueue(this.gameObject);//������ƮǮ�� ����ֱ�
                gameObject.SetActive(false); //��Ȱ��ȭ
            }
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
                Dice11Crack crackDice = GameObject.Find("Dice11_Crack(Clone)").GetComponent<Dice11Crack>();
                
                //�߰� ������ ���
                hp -= (int)(collision.GetComponent<Bullet>().damage * crackDup *
                 (crackDice.plusDamage + (crackDice.plusDamageClassUp * crackDice.diceClassLV) + (crackDice.plusDamagePowerUp * crackDice.dicePowerLV)));
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

}
