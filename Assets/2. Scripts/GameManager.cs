using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    public List<Transform> points = new List<Transform>();
    public List<GameObject> monsterPool = new List<GameObject>();
    public GameObject monster;
    public float createTime = 2.0f;
    public int maxMonster = 10;

    private void Start()
    {
        HideCursor(true);

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }

    private void HideCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }
    public void CreateMonsterPool()
    {
        // 최대로 만들 수 있는 몬스터의 수 만큼 몬스터 오브젝트를 생성
        for (int i = 0; i < maxMonster; i++)
        {
            var _monster = Instantiate<GameObject>(monster);
            _monster.name = $"Monster_{i:00}";
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);
        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    private GameObject GetMonsterInPool()
    {
        foreach (var _monster in monsterPool)
        {
            if(_monster.activeSelf == false)
            {
                return _monster;
            }
        }
        return null;
    }
}
