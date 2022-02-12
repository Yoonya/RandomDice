using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;
    public Transform respawnLocation;

    public int wave; //���̺�
    public int enemyLV; //�� ����
    private float appearTime = 2.0f; //�⺻ ���� �ð�
    private float appearTime2 = 0.5f; //Ư�� �� ���� �ð�
    public int enemyNum = 0; //�� ���� �� ��
    public int randomBoss = 0;

    public bool isBoss = false; //����Ÿ���ΰ� �ƴѰ�
    public bool isBossArrive = false; //������ ����ִ°�

    public Coroutine enemyCoroutine;

    //���� �ʵ忡 �ִ� ���� ����, ���� ���� ������ �ʱ�ȭ
    public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //�Ŀ� ���̺� �ٲ� �� �ʱ�ȭ
        enemyCoroutine = StartCoroutine(StartMakeEnemy(0));                
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoss && !isBossArrive) //������ ������ ����� ����
        {
            StopCoroutine(enemyCoroutine); //�� ����� �ڷ�ƾ ����
            StartCoroutine(StartMakeBoss(randomBoss)); //������Ʈ Ǯ���� ����Ǿ��ִ� ���� ���� +3
            isBoss = false; //�ѹ��� ����ǵ���
            isBossArrive = true;
        }
    }

    IEnumerator StartMakeEnemy(int enemyType) // �� ���� �ڷ�ƾ
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(appearTime);
            CreateEnemy(enemyType);

            //���� ������ ���� ������ ���� �� ����
            if(enemyNum % 10 == 0 && enemyNum % 20 != 0)
            {
                yield return new WaitForSeconds(appearTime2);
                CreateEnemy(1);
            }
            //���� ������ ���� ������ �ߺ��� ����
            if (enemyNum % 20 == 0)
            {
                yield return new WaitForSeconds(appearTime2);
                CreateEnemy(1);
                CreateEnemy(2);
            }
        }
    }

    IEnumerator StartMakeBoss(int bossType) // �� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(appearTime);
        CreateBoss(bossType);
    }


    public void CreateEnemy(int enemyType, int hp = 0, bool isChild = false) //�� ����
    {
        GameObject tempEnemy = ObjectPool.instance.queue[enemyType].Dequeue(); //������Ʈ Ǯ�� ���
        tempEnemy.GetComponent<Enemy>().move = 0;
        tempEnemy.GetComponent<Enemy>().isChild = isChild;
        if (hp == 0)//hp�� ���������� �ʾҴٸ�
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

        tempEnemy.SetActive(true); //��üȭ

        enemies.Add(tempEnemy); //����Ʈ�� ����

        enemyNum += 1;
    }

    private void CreateBoss(int bossType) //�� ����
    {
        GameObject tempEnemy = ObjectPool.instance.queue[bossType + 3].Dequeue(); //������Ʈ Ǯ�� ���
        tempEnemy.GetComponent<EnemyBoss>().move = 0;
        tempEnemy.GetComponent<EnemyBoss>().wave = wave;
        tempEnemy.GetComponent<EnemyBoss>().isBossArrive = true;

        tempEnemy.transform.localPosition = respawnLocation.transform.localPosition;

        tempEnemy.SetActive(true); //��üȭ
    }

    public void ReStartMakeEnemy()
    {
        enemyCoroutine = StartCoroutine(StartMakeEnemy(0));
    }
}
