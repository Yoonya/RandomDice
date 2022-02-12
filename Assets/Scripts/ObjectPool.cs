using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알, 적들이 사용
[System.Serializable]
public class ObjectInfo //오브젝트 내용 클래스
{
    public GameObject prefab;
    public int maxCount;
    public Transform parent;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfos = null;

    public Queue<GameObject>[] queue = new Queue<GameObject>[10];

    public static ObjectPool instance; //공유자원으로 설정

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        queue[0] = InsertQueue(objectInfos[0]);
        queue[1] = InsertQueue(objectInfos[1]);
        queue[2] = InsertQueue(objectInfos[2]);
        queue[3] = InsertQueue(objectInfos[3]);
        queue[4] = InsertQueue(objectInfos[4]);
        queue[5] = InsertQueue(objectInfos[5]);
        queue[6] = InsertQueue(objectInfos[6]);
        queue[7] = InsertQueue(objectInfos[7]);
        queue[8] = InsertQueue(objectInfos[8]);
        queue[9] = InsertQueue(objectInfos[9]);
    }

    //오브젝트 풀링 생성
    private Queue<GameObject> InsertQueue(ObjectInfo objectInfo) 
    {
        Queue<GameObject> tempQueue = new Queue<GameObject>();
        for (int i = 0; i < objectInfo.maxCount; i++) //갯수만큼 채워두기
        {
            GameObject clone = Instantiate(objectInfo.prefab, transform.position, Quaternion.identity); //객체생성
            clone.SetActive(false);

            //부모객체설정
            if (objectInfo.parent != null)
                clone.transform.SetParent(objectInfo.parent, false); //스케일을 false로 막음
            else
                clone.transform.SetParent(this.transform, false);

            tempQueue.Enqueue(clone);
        }

        return tempQueue;
    }
}
