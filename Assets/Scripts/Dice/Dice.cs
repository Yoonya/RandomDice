using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler //다이스 부모 클래스
{
    /*
     * 0 : 불
     * 1 : 전기
     * 2 : 바람
     * 3 : 독
     * 4 : 얼음
     * 5 : 쇠
     * 6 : 고장
     * 7 : 도박
     * 8 : 잠금
     * 9 : 광산
     * 10 : 빛
     * 11 : 균열
     * 12 : 크리티컬
     * 13 : 에너지
     * 14 : 제물
     * 15 : 적응
    */
    public int diceType = 0; //다이스 타입
    public int diceLV = 1; //다이스 눈금 레벨
    public int diceClassLV = 0; //다이스 클래스 레벨 계산상 0부터 시작
    public int dicePowerLV = 0; //다이스 파워 레벨
    public bool isAttack = true; //공격을 하는 다이스인지
    public int damage = 0; //다이스 공격력
    public int damageClassUp = 0; //다이스 클래스 데미지 업
    public int damagePowerUp = 0; //다이스 파워 데미지 업
    public float defaultAttackTime = 0f;//다이스 공격주기 디폴트
    public float attackTime = 0f; //다이스 공격주기
    public float attackTimeClassUp = 0f; //다이스 클래스 공격주기 업
    public float attackTimePowerUp = 0f; //다이스 파워 공격주기 업

    public bool isLock = false; //보스에게 잠겼는지 여부

    //playerStatus에서 받아옴
    public float critical = 0.05f; //크리티컬 확률
    public float criticalDamage = 1.0f; //크리티컬 데미지
    public int sp = 100;

    public int diceLocation = 0; //다이스 생성 위치

    //드래그
    public Vector2 defaultposition;//드롭하면 다시 원위치로 보내기위한 변수
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

        //자꾸 하이라키에 1들어옴
        diceClassLV = 0;
        dicePowerLV = 0; 
}

    //드래그 기능
    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultposition = this.transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = new Vector2(Input.mousePosition.x - 540, Input.mousePosition.y - 960);
        transform.SetAsLastSibling(); //하이라키 순서를 변경해서 제일 위에 보이도록
        this.transform.localPosition = currentPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrop = true;
        this.transform.localPosition = defaultposition; //드래그가 끝나면 원래 장소로 돌아간다.
    }
    //원래 장소로 돌아갈때 트리거가 잡힌다.
    private void OnTriggerExit2D(Collider2D collision)
    {
        //드랍된 상태이고 Dice Tag라면
        if (isDrop && collision.gameObject.CompareTag("Dice"))
            //충돌 다이스나 이 다이스가 적응형 다이스거나 || 다이스 타입이 같다면
            if (collision.gameObject.GetComponent<Dice>().diceType == 15 ||
                diceType == 15 ||
                (collision.gameObject.GetComponent<Dice>().diceType == diceType))
                //눈금 수가 같고, 최대 눈금 수가 아니고
                if (collision.gameObject.GetComponent<Dice>().diceLV == diceLV && diceLV < 6)
                    //둘 다 잠겨져 있는게 아니라면 합쳐진다.
                    if(!collision.gameObject.GetComponent<Dice>().isLock && !isLock)
                    {
                        //나머지는 파괴
                        diceCreateManager.dices.Remove(collision.gameObject);
                        diceCreateManager.dices.Remove(gameObject);
                        diceCreateManager.randomLocations.Remove(diceLocation);

                        //랜덤으로 새로 만들 위치와 다이스레벨을 넘겨준다.
                        diceCreateManager.CreateDice(
                            collision.gameObject.GetComponent<Dice>().diceLocation,
                            ++collision.gameObject.GetComponent<Dice>().diceLV);

                        //나머지는 파괴
                        //sacrifce 때문에 그런것도 있지만 로그가 찍힐때 1성끼리 합칠때 2성 1성을 부수는 거로 되어있다. 1성 2개를 부수는 거로 한다.
                        --collision.gameObject.GetComponent<Dice>().diceLV;
                        Destroy(collision.gameObject);
                        Destroy(gameObject);
                    }


        if (isDrop) //아니면 제자리에 돌려두기
        {
            isDrop = false;
        }
    }

    //bullet 생성, bullet은 오브젝트풀링 관리인데 dice는 instantiate로 생성되기 때문에 자식으로 둘 수 없다.
    //bullet을 따로 관리하면서 빼와서 설정한다.
    public void CreateBullet()
    {
        //다이스 눈금에 따라 설정
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

    //다이스 bullet 세부설정
    public void SetDiceBullet(int x, int y)
    {
        GameObject bullet = ObjectPool.instance.queue[6].Dequeue(); //오브젝트 풀링 사용
        if (bullet.GetComponent<Bullet>().FowardCheck() != null || GameObject.FindWithTag("EnemyBoss") != null)
        {
            Transform transform = diceCreateManager.createLocations[diceLocation].transform; //bullet위치에 쓰인다
            bullet.transform.localPosition = new Vector2(transform.localPosition.x + x, transform.localPosition.y + y);
            bullet.GetComponent<Bullet>().damage = damage + (damageClassUp * diceClassLV) + (damagePowerUp * dicePowerLV); //데미지입력
            bullet.GetComponent<Bullet>().diceType = diceType; //주사위타입입력
            bullet.SetActive(true);
        }
        else
        {
            ObjectPool.instance.queue[6].Enqueue(bullet);
        }

    }
}
