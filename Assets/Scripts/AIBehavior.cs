using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    private DiceCreateManager diceCreateManager;

    public Transform[] createLocations = new Transform[15]; //���̽� ���� ���
    public int[] AINums = new int[5] { -1, -1, -1, -1, -1 }; //-1�� �ؼ� random�˻翡 �ɸ��� �ʰ�

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
