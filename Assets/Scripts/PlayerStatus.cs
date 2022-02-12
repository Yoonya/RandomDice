using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int life = 3;
    public float critical = 0.05f;
    public float criticalDamage = 1.0f;

    public Image[] lifes = new Image[3];

    // Start is called before the first frame update
    void Start()
    {
        setLife();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //life �̹��� ����
    public void setLife()
    {
        if (life <= 0)
        {
            //GameOver
        }
        else
        {
            //�������� �־ �ִ� �������� ���̸� ���ؼ� �̹��� ����ȭ
            if (life == 1)
            {
                Color color = lifes[0].GetComponent<Image>().color;
                color.a = 0.2f;
                lifes[0].GetComponent<Image>().color = color;
                color = lifes[1].GetComponent<Image>().color;
                color.a = 0.2f;
                lifes[1].GetComponent<Image>().color = color;
            }
            else if (life == 2)
            {
                Color color = lifes[0].GetComponent<Image>().color;
                color.a = 0.2f;
                lifes[0].GetComponent<Image>().color = color;
            }
        }

    }
}
