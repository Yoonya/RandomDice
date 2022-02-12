using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    private DiceCreateManager diceCreateManager;

    public Transform[] createLocations = new Transform[15]; //다이스 생성 장소
    public int[] AINums = new int[5] { -1, -1, -1, -1, -1 }; //-1로 해서 random검사에 걸리지 않게

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
