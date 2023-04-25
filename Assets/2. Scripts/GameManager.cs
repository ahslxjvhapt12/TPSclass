using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    public List<Transform> points = new List<Transform>(); // 몬스터 출현 위치 저장
    public List<GameObject> monsterPool = new List<GameObject>(); // 몬스터 오브젝트 풀
    public GameObject monster; //  몬스터 프리팹 연결할 변수
    public float createTime = 2.0f; // 
    public int maxMonster = 10; // 오브젝트 풀에 생성할 몬스터의 최대 개수

    [SerializeField] private PoolListSO initList;

    private void Awake()
    {
    }

    private void Start()
    {
        HideCursor(true);
        CreatePool();

        //CreateMonsterPool(); 

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            HideCursor(false);
        }
        if (Input.GetMouseButton(1))
        {
            HideCursor(true);
        }
    }

    private void HideCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

    public void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonster; i++)
        {
            var _monster = Instantiate<GameObject>(monster);

            _monster.name = $"Monster_{i}";

            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public void CreateMonster()
    {
        //int idx = Random.Range(0, points.Count);

        //GameObject _moster = GetMonsterInPool();

        //_moster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        //_moster?.SetActive(true);

        MonsterCtrl m = PoolManager.Instance.Pop("Monster") as MonsterCtrl;

        int idx = Random.Range(0, points.Count);
        m?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
    }

    private GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if (!_monster.activeSelf)
            {
                return _monster;
            }
        }

        return null;
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        initList.Pairs.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.count);
        });
    }
}