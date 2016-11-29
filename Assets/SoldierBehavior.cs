using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SoldierBehavior : MonoBehaviour {
    public ThirdPersonCharacter character { get; private set; }

    public string team;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private bool alive { get; set; }

    private List<SoldierBehavior> enemyBehaviors;
    private Transform target;
    private int hp;

    // Use this for initialization
    void Start () {
        hp = 100;
        alive = true;

        SoldierBehavior[] agentBehavior = GameObject.FindObjectsOfType(typeof(SoldierBehavior)) as SoldierBehavior[];

        enemyBehaviors = new List<SoldierBehavior>();
        foreach (SoldierBehavior behavior in agentBehavior)
        {
            if (behavior.team != team)
            {
                enemyBehaviors.Add(behavior);
            }
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void FindTarget()
    {
        float closestDist = float.MaxValue;
        SoldierBehavior closestAgent = null;

        foreach (SoldierBehavior behavior in enemyBehaviors)
        {
            if (!behavior.alive)
            {
                continue;
            }

            Vector3 enemyPosition = behavior.transform.position;
            float dist = Vector3.Distance(enemyPosition, transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestAgent = behavior;
            }
        }

        if (closestAgent != null)
        {
            target = closestAgent.transform;
        }

    }

    // Update is called once per frame
    void Update () {
        if (!alive)
        {
            return;
        }

        FindTarget();

        if (target == null)
        {
            return;
        }

        navMeshAgent.destination = target.position;

        /*
        Vector3 direction = target.position - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        
        if (direction.magnitude > 1.2)
        {
            this.transform.Translate(0, 0, 0.1f);
        } else
        {
            anim.SetBool("isAttacking", true);
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject);
        if (!alive)
        {
            return;
        }

        hp -= 20;

        if (hp <= 0)
        {
            print(this);
            alive = false;
            anim.SetBool("isKilled", true);
        }
    }
}
