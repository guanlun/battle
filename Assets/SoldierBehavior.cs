using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SoldierBehavior : MonoBehaviour {
    public ThirdPersonCharacter character { get; private set; }

    public string team;
    public string weaponType;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private bool alive { get; set; }

    private List<SoldierBehavior> enemyBehaviors;
    private Transform target;
    private int hp;

    protected float attackRange;

    protected void init()
    {
        hp = 100;
        alive = true;

        if (weaponType == "bow")
        {
            attackRange = 20f;
        } else
        {
            attackRange = 1f;
        }
    }

    // Use this for initialization
    void Start () {
        init();

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

        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform part in parts)
        {
            if (part.name == "Bip001_Prop1") // Weapon name
            {
                string weaponPrefabName;
                if (this.weaponType == "sword")
                {
                    weaponPrefabName = "SwordPrefab";
                } else
                {
                    weaponPrefabName = "BowPrefab";
                }

                GameObject weapon = (GameObject)Instantiate(Resources.Load(weaponPrefabName));

                weapon.transform.parent = part.transform;

                weapon.transform.localPosition = new Vector3(0, 0, 0);
                weapon.transform.localScale = new Vector3(1, 1, 1);
                weapon.transform.localRotation = Quaternion.identity;
            }
        }
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

        Vector3 direction = target.position - this.transform.position;
        
        if (direction.magnitude > attackRange)
        {
        } else
        {
            navMeshAgent.Stop();
            transform.LookAt(target);

            if (weaponType == "bow")
            {
                anim.SetBool("isShooting", true);
            }
            else
            {
                anim.SetBool("isAttacking", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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

    void ArcherLoose()
    {
        Vector3 pos = new Vector3();
        pos.x = transform.position.x;
        pos.y = transform.position.y + 1.5f;
        pos.z = transform.position.z;

        Quaternion q = Quaternion.identity;
        // print(q);
        q.y += 1f;

        GameObject arrow = (GameObject)Instantiate(Resources.Load("ArrowPrefab"), pos, Quaternion.identity);
        arrow.transform.parent = transform;
        arrow.transform.localRotation = q;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.velocity = transform.forward * 10;
    }
}
