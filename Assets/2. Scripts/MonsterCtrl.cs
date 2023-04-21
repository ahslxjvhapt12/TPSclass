using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class MonsterCtrl : MonoBehaviour
{
    public UnityEvent OnDamageCast;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float traceDist = 10f;
    public float attackDist = 2f;
    public bool isDie = false;

    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDie = Animator.StringToHash("Death");

    private void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        //agent.destination = playerTr.position;
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    {
                        agent.isStopped = true;
                        anim.SetBool(hashTrace, true);
                        anim.SetBool(hashAttack, false);
                    }
                    break;
                case State.TRACE:
                    {
                        agent.SetDestination(playerTr.position);
                        agent.isStopped = false;
                        anim.SetBool(hashTrace, true);
                        anim.SetBool(hashAttack, false);
                    }
                    break;
                case State.ATTACK:
                    {
                        anim.SetBool(hashAttack, true);
                    }
                    break;
                case State.DIE:
                    {
                        isDie = true;
                        agent.isStopped = true;
                        anim.SetTrigger(hashDie);
                        GetComponent<CapsuleCollider>().enabled = false;
                    }
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private bool CheckPlayer()
    {
        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;
        pos.y += 1;
        for (int i = -60; i < 60; i += 10)
        {
            Vector3 dir = Quaternion.Euler(0, i, 0) * bias;

            Ray ray = new Ray(pos, dir.normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, traceDist))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {

        Vector3 bias = transform.forward;
        Vector3 pos = transform.position;
        pos.y += 1;
        Gizmos.color = Color.red;
        for (int i = -60; i < 60; i += 10)
        {
            Vector3 dir = Quaternion.Euler(0, i, 0) * bias;
            Gizmos.DrawRay(pos, dir * traceDist);
        }
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            if (state == State.DIE)
            {
                yield break;
            }

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            if ((dist <= attackDist * attackDist) && CheckPlayer())
            {
                state = State.ATTACK;
            }
            else if ((dist <= traceDist * traceDist) && CheckPlayer())
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void OnAnimationHit()
    {
        OnDamageCast?.Invoke();
    }
}