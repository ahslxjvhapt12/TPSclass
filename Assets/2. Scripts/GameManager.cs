using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    public List<Transform> points = new List<Transform>(); // ���� ���� ��ġ ����
    public List<GameObject> monsterPool = new List<GameObject>(); // ���� ������Ʈ Ǯ
    public GameObject monster; //  ���� ������ ������ ����
    public float createTime = 2.0f; // 
    public int maxMonster = 10; // ������Ʈ Ǯ�� ������ ������ �ִ� ����

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