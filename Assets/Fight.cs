using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Fight : MonoBehaviour {
    
    // public NavMeshAgent agent { get; private set; }
    public ThirdPersonCharacter character { get; private set; }

    public string team;

    static Animator anim;

    private bool alive { get; set; }

    private List<Fight> enemyBehaviors;
    private Transform target;
    private int hp;

    // Use this for initialization
    void Start () {
        hp = 100;
        alive = true;

        Fight[] agentBehavior = GameObject.FindObjectsOfType(typeof(Fight)) as Fight[];

        enemyBehaviors = new List<Fight>();
        foreach (Fight behavior in agentBehavior)
        {
            if (behavior.team != team)
            {
                enemyBehaviors.Add(behavior);
            }
        }

        anim = GetComponent<Animator>();
    }

    private void FindTarget()
    {
        float closestDist = float.MaxValue;
        Fight closestAgent = null;

        foreach (Fight behavior in enemyBehaviors)
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

        Vector3 direction = target.position - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        
        // if (anim.GetBool("isMoving"))
        // {
            if (direction.magnitude > 1)
            {
                this.transform.Translate(0, 0, 0.1f);
            } else
            {
                // anim.SetBool("isMoving", false);
            }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alive)
        {
            return;
        }

        hp -= 50;

        if (hp <= 0)
        {
            alive = false;
            anim.SetBool("isKilled", true);
        }
    }
}
