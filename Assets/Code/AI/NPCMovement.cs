using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    NavMeshAgent m_agent;
    [SerializeField] protected float m_moveRadius;
    PhotonView m_PV;

    // Start is called before the first frame update
    void Start()
    {
        m_PV = GetComponent<PhotonView>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
        {
            MoveRandomPosition();
        }
    }

    void MoveRandomPosition()
    {
        Vector3 m_randomDirection = Random.insideUnitSphere * m_moveRadius;
        m_randomDirection += transform.position;

        NavMeshHit m_hit;

        if(NavMesh.SamplePosition(m_randomDirection, out m_hit, m_moveRadius, NavMesh.AllAreas))
        {
            m_agent.SetDestination(m_hit.position);
        }
    }
}
