using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler //���̽� �θ� Ŭ����
{
    /*
     * 0 : ��
     * 1 : ����
     * 2 : �ٶ�
     * 3 : ��
     * 4 : ����
     * 5 : ��
     * 6 : ����
     * 7 : ����
     * 8 : ���
     * 9 : ����
     * 10 : ��
     * 11 : �տ�
     * 12 : ũ��Ƽ��
     * 13 : ������
     * 14 : ����
     * 15 : ����
    */
    public int diceType = 0; //���̽� Ÿ��
    public int diceLV = 1; //���̽� ���� ����
    public int diceClassLV = 0; //���̽� Ŭ���� ���� ���� 0���� ����
    public int dicePowerLV = 0; //���̽� �Ŀ� ����
    public bool isAttack = true; //������ �ϴ� ���̽�����
    public int damage = 0; //���̽� ���ݷ�
    public int damageClassUp = 0; //���̽� Ŭ���� ������ ��
    public int damagePowerUp = 0; //���̽� �Ŀ� ������ ��
    public float defaultAttackTime = 0f;//���̽� �����ֱ� ����Ʈ
    public float attackTime = 0f; //���̽� �����ֱ�
    public float attackTimeClassUp = 0f; //���̽� Ŭ���� �����ֱ� ��
    public float attackTimePowerUp = 0f; //���̽� �Ŀ� �����ֱ� ��

    public bool isLock = false; //�������� ������ ����

    //playerStatus���� �޾ƿ�
    public float critical = 0.05f; //ũ��Ƽ�� Ȯ��
    public float criticalDamage = 1.0f; //ũ��Ƽ�� ������
    public int sp = 100;

    public int diceLocation = 0; //���̽� ���� ��ġ

    //�巡��
    public Vector2 defaultposition;//����ϸ� �ٽ� ����ġ�� ���������� ����
    public bool isDrop = false;
    protected PlayerStatus playerStatus;
    protected DiceCreateManager diceCreateManager;

    protected float time = 0;

    public Dice(int diceType, bool isAttack, int damage, float attackTime, int damageClassUp,
        int damagePowerUp, float attackTimeClassUp, float attackTimePowerUp)
    {
        this.diceType = diceType;
        this.isAttack = isAttack;
        this.damage = damage;
        this.attackTime = attackTime;
        this.damageClassUp = damageClassUp;
        this.damagePowerUp = damagePowerUp;
        this.attackTimeClassUp = attackTimeClassUp;
        this.attackTimePowerUp = attackTimePowerUp;

        defaultAttackTime = attackTime - (attackTimeClassUp * diceClassLV) - (attackTimePowerUp - dicePowerLV);
    }

    protected void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        diceCreateManager = FindObjectOfType<DiceCreateManager>();

        critical = playerStatus.critical;
        criticalDamage = playerStatus.criticalDamage;

        //�ڲ� ���̶�Ű�� 1����
        diceClassLV = 0;
        dicePowerLV = 0; 
}

    //�巡�� ���
    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultposition = this.transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = new Vector2(Input.mousePosition.x - 540, Input.mousePosition.y - 960);
        transform.SetAsLastSibling(); //���̶�Ű ������ �����ؼ� ���� ���� ���̵���
        this.transform.localPosition = currentPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrop = true;
        this.transform.localPosition = defaultposition; //�巡�װ� ������ ���� ��ҷ� ���ư���.
    }
    //���� ��ҷ� ���ư��� Ʈ���Ű� ������.
    private void OnTriggerExit2D(Collider2D collision)
    {
        //����� �����̰� Dice Tag���
        if (isDrop && collision.gameObject.CompareTag("Dice"))
            //�浹 ���̽��� �� ���̽��� ������ ���̽��ų� || ���̽� Ÿ���� ���ٸ�
            if (collision.gameObject.GetComponent<Dice>().diceType == 15 ||
                diceType == 15 ||
                (collision.gameObject.GetComponent<Dice>().diceType == diceType))
                //���� ���� ����, �ִ� ���� ���� �ƴϰ�
                if (collision.gameObject.GetComponent<Dice>().diceLV == diceLV && diceLV < 6)
                    //�� �� ����� �ִ°� �ƴ϶�� ��������.
                    if(!collision.gameObject.GetComponent<Dice>().isLock && !isLock)
                    {
                        //�������� �ı�
                        diceCreateManager.dices.Remove(collision.gameObject);
                        diceCreateManager.dices.Remove(gameObject);
                        diceCreateManager.randomLocations.Remove(diceLocation);

                        //�������� ���� ���� ��ġ�� ���̽������� �Ѱ��ش�.
                        diceCreateManager.CreateDice(
                            collision.gameObject.GetComponent<Dice>().diceLocation,
                            ++collision.gameObject.GetComponent<Dice>().diceLV);

                        //�������� �ı�
                        //sacrifce ������ �׷��͵� ������ �αװ� ������ 1������ ��ĥ�� 2�� 1���� �μ��� �ŷ� �Ǿ��ִ�. 1�� 2���� �μ��� �ŷ� �Ѵ�.
                        --collision.gameObject.GetComponent<Dice>().diceLV;
                        Destroy(collision.gameObject);
                        Destroy(gameObject);
                    }


        if (isDrop) //�ƴϸ� ���ڸ��� �����α�
        {
            isDrop = false;
        }
    }

    //bullet ����, bullet�� ������ƮǮ�� �����ε� dice�� instantiate�� �����Ǳ� ������ �ڽ����� �� �� ����.
    //bullet�� ���� �����ϸ鼭 ���ͼ� �����Ѵ�.
    public void CreateBullet()
    {
        //���̽� ���ݿ� ���� ����
        switch (diceLV)
        {
            case 1:
                SetDiceBullet(0, 0);
                break;
            case 2:
                SetDiceBullet(30, 30);
                SetDiceBullet(-30, -30);
                break;
            case 3:
                SetDiceBullet(30, 30);
                SetDiceBullet(-30, -30);
                SetDiceBullet(0, 0);
                break;
            case 4:
                SetDiceBullet(30, 30);
                SetDiceBullet(-30, -30);
                SetDiceBullet(30, -30);
                SetDiceBullet(-30, 30);
                break;
            case 5:
                SetDiceBullet(30, 30);
                SetDiceBullet(-30, -30);
                SetDiceBullet(30, -30);
                SetDiceBullet(-30, 30);
                SetDiceBullet(0, 0);
                break;
            case 6:
                SetDiceBullet(30, 30);
                SetDiceBullet(-30, -30);
                SetDiceBullet(30, -30);
                SetDiceBullet(-30, 0);
                SetDiceBullet(30, 0);
                SetDiceBullet(-30, 30);
                break;

        }

    }

    //���̽� bullet ���μ���
    public void SetDiceBullet(int x, int y)
    {
        GameObject bullet = ObjectPool.instance.queue[6].Dequeue(); //������Ʈ Ǯ�� ���
        if (bullet.GetComponent<Bullet>().FowardCheck() != null || GameObject.FindWithTag("EnemyBoss") != null)
        {
            Transform transform = diceCreateManager.createLocations[diceLocation].transform; //bullet��ġ�� ���δ�
            bullet.transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
            bullet.GetComponent<Bullet>().damage = damage + (damageClassUp * diceClassLV) + (damagePowerUp * dicePowerLV); //�������Է�
            bullet.GetComponent<Bullet>().diceType = diceType; //�ֻ���Ÿ���Է�
            bullet.SetActive(true);
        }
        else
        {
            ObjectPool.instance.queue[6].Enqueue(bullet);
        }

    }
}
