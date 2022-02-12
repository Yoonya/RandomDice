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
    public bool isBrokenDice = false; //������ �ֻ����� �ѹ��� �����Ű�� ����

    public bool isCol = false;

    //color
    private float[,] colors = new float[16, 3] //bullet�� ���� ���ϱ� ����, ���̽� ������ ���� ����
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
        enemyManager = FindObjectOfType<EnemyManager>(); //���� ������
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
            if (!enemyManager.isBossArrive) //�������� �ƴ϶��
            {
                //bullet�� ���� ������ ������ �̵�
                GameObject forward = null;

                if (diceType == 3) //���ֻ������
                    forward = FowardPoisionCheck(); //�������� �� ����üũ
                else if (diceType == 5)
                    forward = HPCheck(); //ü���� ���� ���� �� üũ
                else if (diceType == 6 && !isBrokenDice)
                {
                    isBrokenDice = true;
                    forward = RandomCheck(); //������ ������ ���ư���.
                }
                else if (diceType == 11)
                    forward = FowardCrackCheck();
                else //�Ϲ��ֻ������
                    forward = FowardCheck(); //����üũ

                if (forward != null) //null�� ��ȯ�� ���� �ƴ϶��
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                        forward.transform.localPosition,
                        1500f * Time.deltaTime);
                }
                else //forward�� null�̶�� bullet����
                {
                    //bullet����
                    //BulletEffect(); //null �ִϸ��̼�ó��
                    transform.localPosition = originPos; //��ġ �ʱ�ȭ
                    ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�                 
                    gameObject.SetActive(false); //��Ȱ��ȭ
                }
            }
            else //�������̶��
            {
                GameObject boss = GameObject.FindWithTag("EnemyBoss"); //����üũ, ������ list�� ����ְ� ���ϵ� ó���� ����
                GameObject forward = FowardCheck(); //����üũ
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
                else //boss�� null�̶�� bullet����
                {
                    //bullet����
                    //BulletEffect(); //null �ִϸ��̼�ó��
                    transform.localPosition = originPos; //��ġ �ʱ�ȭ
                    ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
                    gameObject.SetActive(false); //��Ȱ��ȭ
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //���� ��´ٸ�
    {
        if (collision.CompareTag("Enemy"))
        {
            isCol = true;
            isBrokenDice = false;

            //bulletEffectȿ��, ���̽� �ɷ��� ����
            DiceAbility(collision.gameObject);

            //ũ��Ƽ�� ���
            float critical = playerStatus.critical;
            float criticalDamage = playerStatus.criticalDamage;

            float random = Random.Range(0f, 1f);

            if (random <= critical)
            {
                damage += (int)(damage * criticalDamage);
            }

            //���� ��ȣ�ۿ� ũ��Ƽ�� �������� Enemy����
            collision.gameObject.GetComponent<Enemy>().hp -= damage;


        }
        if(collision.CompareTag("EnemyBoss"))
        {
            isCol = true;
            isBrokenDice = false;

            //bulletEffectȿ��, ���̽� �ɷ��� ����
            DiceAbility(collision.gameObject);

            //ũ��Ƽ�� ���
            float critical = playerStatus.critical;
            float criticalDamage = playerStatus.criticalDamage;

            float random = Random.Range(0f, 1f);

            if (random <= critical)
            {
                damage += (int)(damage * criticalDamage);
            }

            //���� ��ȣ�ۿ� ũ��Ƽ�� �������� Enemy����
            collision.gameObject.GetComponent<EnemyBoss>().hp -= damage;

        }
    }

    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //���̽� �ɷ��� �����ϴºκ�
    private void DiceAbility(GameObject collision)
    {
        switch (diceType)
        {
            //���� ��ȣ�� �߰�����Ʈ�� ����
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
     * FireDice()�� ���� ���� Enemy��
     * WindDice()�� Dice2Wind��
     * MineDice()�� Dice9Mine��
     * LightDice()�� Dice10Light��
     * CriticalDice()�� Dice12Critical��
     * SacrificeDice()�� Dice14Sacrifice��
    */
    private void FireDice()
    {
        //���ֻ����� �ֺ��� ����������, Enemy���� ó��
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

    //Ÿ���� ������ 3���� ���Ϳ��� ���� 100%, 70%, 30% [����]�������� ������.
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

        float min1 = 1000f; //ù��°�� ������� �Ÿ�
        float min2 = 1000f; //�ι�°�� ������� �Ÿ�
        GameObject minObject1 = null; //ù��°�� �������
        GameObject minObject2 = null; //�ι�°�� �������
        GameObject bulletEffectPlus2 = null;  //ù��°�� ������� ����Ʈ
        GameObject bulletEffectPlus3 = null;  //�ι�°�� ������� ����Ʈ

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].activeSelf)
            {
                float distance = Vector2.Distance(collision.gameObject.transform.localPosition, enemyManager.enemies[i].transform.localPosition);
                if (distance != 0 && min2 >= distance) //�ι�°�� ����������� �����ٸ�
                {
                    if (min1 >= distance) //ù��°���� �����ٸ�
                    {
                        min1 = distance; //ù��°
                        minObject1 = enemyManager.enemies[i];
                    }
                    else
                    {
                        min2 = distance; //�ƴϸ� �ι�°
                        minObject2 = enemyManager.enemies[i];
                    }

                }
            }

        }

        Dice1Electric electricDice = null;
        if (GameObject.Find("Dice1_Electric(Clone)") != null)
            electricDice = GameObject.Find("Dice1_Electric(Clone)").GetComponent<Dice1Electric>();
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            //BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        int electricDamage1 = (int)(0.7 * (electricDice.electricDamage + (electricDice.electricDamageClassUp * electricDice.diceClassLV)
                  + (electricDice.electricDamagePowerUp * electricDice.dicePowerLV)));
        int electricDamage2 = (int)(0.3 * (electricDice.electricDamage + (electricDice.electricDamageClassUp * electricDice.diceClassLV)
                          + (electricDice.electricDamagePowerUp * electricDice.dicePowerLV)));

        if (minObject1 != null)
        {
            //���� ��ȣ�ۿ�
            minObject1.transform.GetComponent<Enemy>().hp -= electricDamage1;
            bulletEffectPlus2 = ObjectPool.instance.queue[8].Dequeue();
            bulletEffectPlus2.SetActive(true);
            bulletEffectPlus2.transform.localPosition = minObject1.transform.localPosition;
            bulletEffectPlus2.GetComponent<BulletEffectPlus>().diceType = diceType;
            bulletEffectPlus2.GetComponent<Animator>().SetTrigger("Electric");
           
        }
        if(minObject2 != null)
        {
            //���� ��ȣ�ۿ�
            minObject2.transform.GetComponent<Enemy>().hp -= electricDamage2;
            bulletEffectPlus3 = ObjectPool.instance.queue[8].Dequeue();
            bulletEffectPlus3.SetActive(true);
            bulletEffectPlus3.transform.localPosition = minObject2.transform.localPosition;
            bulletEffectPlus3.GetComponent<BulletEffectPlus>().diceType = diceType;
            bulletEffectPlus3.GetComponent<Animator>().SetTrigger("Electric");
           
        }

        StartCoroutine(EndDelayElectric(0.14f, bulletEffect, bulletEffectPlus, bulletEffectPlus2, bulletEffectPlus3));
    }
    //���� ���� ��[��] ������� ���� �ʴ� �� �������� ������. [��] ������� ���� ���͸� �켱 �����Ѵ�.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            // BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        if (collision.CompareTag("Enemy"))
        {
            //�� �ֻ����� �ʴ� ������
            collision.GetComponent<Enemy>().isPoision = true; //����� ����
            collision.GetComponent<Enemy>().poisionDamage = poisionDice.poisionDamage + (poisionDice.poisionDamageClassUp * poisionDice.diceClassLV) +
                (poisionDice.poisionDamagePowerUp * poisionDice.dicePowerLV);
            Color color = collision.GetComponent<Enemy>().debuffImage[0].GetComponent<Image>().color; //�̹��� 0���� ��
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[0].GetComponent<Image>().color = color;
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //�� �ֻ����� �ʴ� ������
            collision.GetComponent<EnemyBoss>().isPoision = true; //����� ����
            collision.GetComponent<EnemyBoss>().poisionDamage = poisionDice.poisionDamage + (poisionDice.poisionDamageClassUp * poisionDice.diceClassLV) +
                    (poisionDice.poisionDamagePowerUp * poisionDice.dicePowerLV);
            Color color = collision.GetComponent<EnemyBoss>().debuffImage[0].GetComponent<Image>().color; //�̹��� 0���� ��
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[0].GetComponent<Image>().color = color;
        }


        StartCoroutine(EndDelay(0.09f, bulletEffect, bulletEffectPlus));
    }
    //���� ���� �� [����] ������� ���� �̵��ӵ��� ���ҽ�Ų��. [����] ������� �ִ� 3ȸ ��ø�ȴ�.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            //BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        if (collision.CompareTag("Enemy"))
        {
            //���� �ֻ����� ��ø ����
            collision.GetComponent<Enemy>().isIce = true; //����� ����
            collision.GetComponent<Enemy>().iceSlow = iceDice.speedDown + (iceDice.speedDownClassUp * iceDice.diceClassLV) +
                (iceDice.speedDownPowerUp * iceDice.dicePowerLV);
            collision.GetComponent<Enemy>().iceDup++;//���� ��ø�� �ø���.
            if (collision.GetComponent<Enemy>().iceDup > 3) //3��ø�� �Ѿ��ٸ� �׳� 3��ø���� �д�.
                collision.GetComponent<Enemy>().iceDup = 3;
            Color color = collision.GetComponent<Enemy>().debuffImage[1].GetComponent<Image>().color; //�̹��� 1���� ����
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[1].GetComponent<Image>().color = color; //������ ����������
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //���� �ֻ����� ��ø ����
            collision.GetComponent<EnemyBoss>().isIce = true; //����� ����
            collision.GetComponent<EnemyBoss>().iceSlow = iceDice.speedDown + (iceDice.speedDownClassUp * iceDice.diceClassLV) +
                (iceDice.speedDownPowerUp * iceDice.dicePowerLV);
            collision.GetComponent<EnemyBoss>().iceDup++;//���� ��ø�� �ø���.
            if (collision.GetComponent<EnemyBoss>().iceDup > 3) //3��ø�� �Ѿ��ٸ� �׳� 3��ø���� �д�.
                collision.GetComponent<EnemyBoss>().iceDup = 3;
            Color color = collision.GetComponent<EnemyBoss>().debuffImage[1].GetComponent<Image>().color; //�̹��� 1���� ����
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[1].GetComponent<Image>().color = color; //������ ����������
        }

        StartCoroutine(EndDelay(0.10f, bulletEffect, bulletEffectPlus));
    }

    //����, �ߺ��� ���� �� 2�� �������� ������. ü���� ���� ���� ���͸� �켱 �����Ѵ�.
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

    //�ֻ����� �⺻ ���ݷº��� ũ��Ƽ�� ���������� ������ �������� ������.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            //BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }


        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    //���� ���� �� ���� Ȯ����[���] ������� �ɾ� ���� �ð����� �������� ���ϰ� �Ѵ�. [���] ������� ���ʹ� 1ȸ�� �ɸ���.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            //BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        if (collision.CompareTag("Enemy"))
        {
            //��� �ֻ��� ����
            if(collision.GetComponent<Enemy>().lockCount == 0)
                collision.GetComponent<Enemy>().lockTime = lockDice.lockTime + (lockDice.lockTimeClassUp * lockDice.diceClassLV) +
                    (lockDice.lockTimePowerUp * lockDice.dicePowerLV); //��� ���ӽð� ����
            collision.GetComponent<Enemy>().lockPer = lockDice.lockPer + (lockDice.lockPerClassUp * lockDice.diceClassLV) +
                (lockDice.lockPerPowerUp * lockDice.dicePowerLV);//��� Ȯ�� ����
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //��� �ֻ��� ����
            if (collision.GetComponent<EnemyBoss>().lockCount == 0)
                collision.GetComponent<EnemyBoss>().lockTime = lockDice.lockTime + (lockDice.lockTimeClassUp * lockDice.diceClassLV) +
                    (lockDice.lockTimePowerUp * lockDice.dicePowerLV); //��� ���ӽð� ����
            collision.GetComponent<EnemyBoss>().lockPer = lockDice.lockPer + (lockDice.lockPerClassUp * lockDice.diceClassLV) +
                (lockDice.lockPerPowerUp * lockDice.dicePowerLV);//��� Ȯ�� ����
        }


        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }

    //���� ���� �� [�տ�] ǥ���� �����. [�տ�] ǥ���� ���� ���ʹ� �������� ���� ������ �߰� �������� �Դ´�. 
    //[�տ�] ǥ���� �ִ� 3ȸ ��ø�ǰ�, 3ȸ ��ø���� ���� ���͸� �켱 �����Ѵ�.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            // BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }

        if (collision.CompareTag("Enemy"))
        {
            //�տ� �ֻ����� ��ø ����
            collision.GetComponent<Enemy>().isCrack = true; //����� ����
            
            collision.GetComponent<Enemy>().crackDup++;//�տ� ��ø�� �ø���.
            if (collision.GetComponent<Enemy>().crackDup > 3) //3��ø�� �Ѿ��ٸ� �׳� 3��ø���� �д�.
                collision.GetComponent<Enemy>().crackDup = 3;

            Color color = collision.GetComponent<Enemy>().debuffImage[3].GetComponent<Image>().color; //�̹��� 3���� �տ�
            color.a = 1f;
            collision.GetComponent<Enemy>().debuffImage[3].GetComponent<Image>().color = color; //������ ����������
        }
        else if (collision.CompareTag("EnemyBoss"))
        {
            //�տ� �ֻ����� ��ø ����
            collision.GetComponent<EnemyBoss>().isCrack = true; //����� ����

            collision.GetComponent<EnemyBoss>().crackDup++;//�տ� ��ø�� �ø���.
            if (collision.GetComponent<EnemyBoss>().crackDup > 3) //3��ø�� �Ѿ��ٸ� �׳� 3��ø���� �д�.
                collision.GetComponent<EnemyBoss>().crackDup = 3;

            Color color = collision.GetComponent<EnemyBoss>().debuffImage[3].GetComponent<Image>().color; //�̹��� 3���� �տ�
            color.a = 1f;
            collision.GetComponent<EnemyBoss>().debuffImage[3].GetComponent<Image>().color = color; //������ ����������
        }

        StartCoroutine(EndDelay(0.07f, bulletEffect, bulletEffectPlus));
    }

    //���� ���� �� ���� �ڽ��� ������ SP�� ����Ͽ� �߰� �������� ������.
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
        else //�߰��� �ռ��ؼ� ������ų� �ϴ� ����
        {
            //bullet����
            //BulletEffect(); //null �ִϸ��̼�ó��
            transform.localPosition = originPos; //��ġ �ʱ�ȭ
            ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
            gameObject.SetActive(false); //��Ȱ��ȭ
        }
        DiceCreateManager diceCreate = GameObject.Find("DiceCreate").GetComponent<DiceCreateManager>();

        //������ ���
        damage += (int)(diceCreate.remainSP * 
            (energyDice.spDamage + (energyDice.spDamageClassUp * energyDice.diceClassLV) + (energyDice.spDamagePowerUp * energyDice.dicePowerLV)));

        StartCoroutine(BulletEffectDelay(0.07f, bulletEffect));
    }
    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //�ִϸ��̼� ������ ��� �� ��Ȱ��ȭ
    IEnumerator EndDelay(float time, GameObject bulletEffect, GameObject bulletEffectPlus)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//������ƮǮ�� ����ֱ�
        bulletEffect.SetActive(false);
        ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus);//������ƮǮ�� ����ֱ�
        bulletEffectPlus.SetActive(false);

        
        transform.localPosition = originPos; //��ġ �ʱ�ȭ
        
        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
        gameObject.SetActive(false);
    }

    //�ִϸ��̼� ������ ��� �� ��Ȱ��ȭ
    IEnumerator EndDelayElectric(float time, GameObject bulletEffect, GameObject bulletEffectPlus, GameObject bulletEffectPlus2, GameObject bulletEffectPlus3)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//������ƮǮ�� ����ֱ�
        bulletEffect.SetActive(false);
        ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus);//������ƮǮ�� ����ֱ�
        bulletEffectPlus.SetActive(false);
        if (bulletEffectPlus2 != null)
        {
            ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus2);//������ƮǮ�� ����ֱ�
            bulletEffectPlus2.SetActive(false);
        }
        if (bulletEffectPlus3 != null)
        {
            ObjectPool.instance.queue[8].Enqueue(bulletEffectPlus3);//������ƮǮ�� ����ֱ�
            bulletEffectPlus3.SetActive(false);
        }


        transform.localPosition = originPos; //��ġ �ʱ�ȭ

        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
        gameObject.SetActive(false);
    }

    //bullet �浹����Ʈ, null����Ʈ
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
        ObjectPool.instance.queue[7].Enqueue(bulletEffect);//������ƮǮ�� ����ֱ�
        bulletEffect.SetActive(false);

        transform.localPosition = originPos; //��ġ �ʱ�ȭ
        isCol = false;
        ObjectPool.instance.queue[6].Enqueue(gameObject);//������ƮǮ�� ����ֱ�
        gameObject.SetActive(false);
    }


    //************************************************************************************
    //************************************************************************************
    //************************************************************************************
    //���ϴ� ���� ��ġ�� ���ϴ� �κ�
    //���� ���ʿ� �ִ� ���� üũ�Ѵ�. ���� �⺻�̹Ƿ� �̰ɷ� ���� �ִ����� �˻�
    public GameObject FowardCheck()
    {
        /*
         enemy���� move�� ���� �̵� ��θ� �� �� �ִ�. move�� Ŭ ���� ���� ���ʿ� �ִ� ���̴�.
         ��� if(move == 0) �϶� y���� ���� ū ���� �����̴�.
         ��� if(move == 1) �϶� x���� ���� ū ���� �����̴�.
         ��� if(move == 2) �϶� y���� ���� ���� ���� �����̴�.
         */
        int move0 = 0;
        int move1 = 0;
        int move2 = 0;
        List<GameObject> moves0 = new List<GameObject>();
        List<GameObject> moves1 = new List<GameObject>();
        List<GameObject> moves2 = new List<GameObject>();

        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //Ȱ��ȭ�� �͸�
            {
                //move���� ����ϰ� ���� ����Ʈ�� ����
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

        //�� move�� 1���� �� �װ��� ����
        if (move0 == 1 && move1 == 0 && move2 == 0)
            return moves0[0];
        else if (move1 == 1 && move2 == 0)
            return moves1[0];
        else if (move2 == 1)
            return moves2[0];

        //move0���� ������ ���� �� ������ ����ش�.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (max <= moves0[i].transform.localPosition.y)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1������ ������ ���� �� ������ ����ش�.
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (max <= moves1[i].transform.localPosition.x)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            return forward;
        }

        //move2���� ������ ���� �� ������ ����ش�.
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�.
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
    //���� �����̸鼭 ���� �ߵ����� ���� ���� üũ�Ѵ�.
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
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //Ȱ��ȭ�� �͸�
            {
                //move���� ����ϰ� ���� ����Ʈ�� ����
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

        //�� move�� 1���� �� �װ��� ����, �׸��� �ߵ��� �ƴҶ�
        if (move0 == 1 && move1 == 0 && move2 == 0 && !moves0[0].GetComponent<Enemy>().isPoision)
            return moves0[0];
        else if (move1 == 1 && move2 == 0 && !moves1[0].GetComponent<Enemy>().isPoision)
            return moves1[0];
        else if (move2 == 1 && !moves2[0].GetComponent<Enemy>().isPoision)
            return moves2[0];

        //move0���� ������ ���� �� ������ ����ش�.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
                if (max <= moves0[i].transform.localPosition.y && !moves0[i].GetComponent<Enemy>().isPoision)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1������ ������ ���� �� ������ ����ش�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (max <= moves1[i].transform.localPosition.x && !moves1[i].GetComponent<Enemy>().isPoision)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            if (forward != null) //���� �ԷµǾ����� ��ȯ
                return forward;
            else //�ƴ϶�� move0���� ����
            {
                float max2 = -576f;
                for (int i = 0; i < moves0.Count; i++)
                {
                    //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
                    if (max2 <= moves0[i].transform.localPosition.y && !moves0[i].GetComponent<Enemy>().isPoision)
                    {
                        max2 = moves0[i].transform.localPosition.y;
                        forward = moves0[i];
                    }
                }
                return forward;
            }
        }

        //move2���� ������ ���� �� ������ ����ش�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (min >= moves2[i].transform.localPosition.y && !moves2[i].GetComponent<Enemy>().isPoision)
                {
                    min = moves2[i].transform.localPosition.y;
                    forward = moves2[i];
                }
            }
            if (forward != null) //���� �ԷµǾ����� ��ȯ
                return forward;
            else //�ƴ϶�� move1���� ����
            {
                float max = -463f;
                for (int i = 0; i < moves1.Count; i++)
                {
                    //x���� ���ؼ� ���� ������ ���� ��󳽴�.
                    if (max <= moves1[i].transform.localPosition.x && !moves1[i].GetComponent<Enemy>().isPoision)
                    {
                        max = moves1[i].transform.localPosition.x;
                        forward = moves1[i];
                    }
                }
                if (forward != null) //���� �ԷµǾ����� ��ȯ
                    return forward;
                else //�ƴ϶�� move0���� ����
                {
                    float max2 = -576f;
                    for (int i = 0; i < moves0.Count; i++)
                    {
                        //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
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


    //���� ü���� ���� ���� üũ�Ѵ�.
    private GameObject HPCheck()
    {  
        int max = 0;
        GameObject maxEnemy = null;
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //Ȱ��ȭ�� �͸�
            {
                if (max <= enemyManager.enemies[i].gameObject.GetComponent<Enemy>().hp)
                {
                    max = enemyManager.enemies[i].gameObject.GetComponent<Enemy>().hp;
                    maxEnemy = enemyManager.enemies[i];
                }

            }
        }
        if (maxEnemy != null) //���� ü���� ���� �� ��ȯ
            return maxEnemy;
        else
            return null;
    }

    //�������� ���� üũ�Ѵ�.
    private GameObject RandomCheck()
    {
        GameObject randomEnemy = null;
        int max = 0; //Ȥ�� �� �����Ƽ� ��� �� �� ������ �ݺ� �ִ� Ƚ���� ���� �ʵ���
        while (true)
        {
            if (max == 50) //50���̳� �ݺ��ߴٸ� ����
                break;
            if (enemyManager.enemies.Count <= 0) //�ּ� ������ �ȵȴٸ� break
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

    //���� �����̸鼭 �տ� �ߺ��� 3������ ���� üũ�Ѵ�.
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
            if (enemyManager.enemies[i].gameObject.activeSelf == true) //Ȱ��ȭ�� �͸�
            {
                //move���� ����ϰ� ���� ����Ʈ�� ����
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

        //�� move�� 1���� �� �װ��� ����, �׸��� 3��ø�� �ƴ� ��
        if (move0 == 1 && move1 == 0 && move2 == 0 && moves0[0].GetComponent<Enemy>().crackDup < 3)
            return moves0[0];
        else if (move1 == 1 && move2 == 0 && moves1[0].GetComponent<Enemy>().crackDup < 3)
            return moves1[0];
        else if (move2 == 1 && moves2[0].GetComponent<Enemy>().crackDup < 3)
            return moves2[0];

        //move0���� ������ ���� �� ������ ����ش�.
        if (move1 == 0 && move2 == 0)
        {
            float max = -576f;
            GameObject forward = null;
            for (int i = 0; i < moves0.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� 3��ø�� �ƴϸ� ��󳽴�
                if (max <= moves0[i].transform.localPosition.y && moves0[i].GetComponent<Enemy>().crackDup < 3)
                {
                    max = moves0[i].transform.localPosition.y;
                    forward = moves0[i];
                }
            }
            return forward;
        }

        //move1������ ������ ���� �� ������ ����ش�. �׸��� 3��ø�� �ƴϸ� ��󳽴�
        if (move1 != 0 && move2 == 0)
        {
            float max = -463f;
            GameObject forward = null;
            for (int i = 0; i < moves1.Count; i++)
            {
                //x���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (max <= moves1[i].transform.localPosition.x && moves1[i].GetComponent<Enemy>().crackDup < 3)
                {
                    max = moves1[i].transform.localPosition.x;
                    forward = moves1[i];
                }
            }
            if (forward != null) //���� �ԷµǾ����� ��ȯ
                return forward;
            else //�ƴ϶�� move0���� ����
            {
                float max2 = -576f;
                for (int i = 0; i < moves0.Count; i++)
                {
                    //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� 3��ø�� �ƴϸ� ��󳽴�
                    if (max2 <= moves0[i].transform.localPosition.y && moves0[i].GetComponent<Enemy>().crackDup < 3)
                    {
                        max2 = moves0[i].transform.localPosition.y;
                        forward = moves0[i];
                    }
                }
                return forward;
            }
        }

        //move2���� ������ ���� �� ������ ����ش�. �׸��� 3��ø�� �ƴϸ� ��󳽴�
        if (move2 != 0)
        {
            float min = -1f;
            GameObject forward = null;
            for (int i = 0; i < moves2.Count; i++)
            {
                //y���� ���ؼ� ���� ������ ���� ��󳽴�.
                if (min >= moves2[i].transform.localPosition.y && moves2[i].GetComponent<Enemy>().crackDup < 3)
                {
                    min = moves2[i].transform.localPosition.y;
                    forward = moves2[i];
                }
            }
            if (forward != null) //���� �ԷµǾ����� ��ȯ
                return forward;
            else //�ƴ϶�� move1���� ����
            {
                float max = -463f;
                for (int i = 0; i < moves1.Count; i++)
                {
                    //x���� ���ؼ� ���� ������ ���� ��󳽴�.
                    if (max <= moves1[i].transform.localPosition.x && moves1[i].GetComponent<Enemy>().crackDup < 3)
                    {
                        max = moves1[i].transform.localPosition.x;
                        forward = moves1[i];
                    }
                }
                if (forward != null) //���� �ԷµǾ����� ��ȯ
                    return forward;
                else //�ƴ϶�� move0���� ����
                {
                    float max2 = -576f;
                    for (int i = 0; i < moves0.Count; i++)
                    {
                        //y���� ���ؼ� ���� ������ ���� ��󳽴�. �׸��� �ߵ��� �ƴϸ� ��󳽴�
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
