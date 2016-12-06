using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SoldierBehavior : MonoBehaviour {
    public ThirdPersonCharacter character { get; private set; }

    public string team;
    public string weaponType;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    public bool alive = true;

    private List<SoldierBehavior> enemyBehaviors;
    private Transform target;
    private int hp;
    private GameObject weapon;

    protected float attackRange;

    private Transform torso;

    protected void init()
    {
        hp = 100;
        alive = true;

        if (weaponType == "bow")
        {
            attackRange = 20f;

            navMeshAgent.stoppingDistance = 20f;
        } else
        {
            attackRange = 1f;

            navMeshAgent.stoppingDistance = 1f;
        }
    }

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        anim.speed = 0.5f;

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

                weapon = (GameObject)Instantiate(Resources.Load(weaponPrefabName));

                weapon.transform.parent = part.transform;

                weapon.transform.localPosition = new Vector3(0, 0, 0);
                weapon.transform.localScale = new Vector3(1, 1, 1);
                weapon.transform.localRotation = Quaternion.identity;

                WeaponBehavior weaponBehavior = weapon.GetComponent<WeaponBehavior>();

                if (weaponBehavior != null)
                {
                    weaponBehavior.team = this.team;
                }
            }
            else if (part.name == "Object02")
            {
                Renderer renderer = part.GetComponent<Renderer>();

                string matType = team == "red" ? "RedArmyMat" : "BlueArmyMat";
                renderer.material = Resources.Load(matType, typeof(Material)) as Material;
            }
            else if (part.name == "Object02")
            {
                torso = part;
            }
        }

        init();
    }

    private void FindTarget()
    {
        // TODO: move this back to the Start function?
        SoldierBehavior[] agentBehavior = GameObject.FindObjectsOfType(typeof(SoldierBehavior)) as SoldierBehavior[];

        enemyBehaviors = new List<SoldierBehavior>();
        foreach (SoldierBehavior behavior in agentBehavior)
        {
            if (behavior.team != team)
            {
                enemyBehaviors.Add(behavior);
            }
        }

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
        } else
        {
            target = null;
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
            anim.SetBool("isShooting", false);
            anim.SetBool("isAttacking", false);
            return;
        }

        navMeshAgent.destination = target.position;

        Vector3 direction = target.position - this.transform.position;

        transform.LookAt(target);

        if (direction.magnitude > attackRange)
        {
            // navMeshAgent.ResetPath();
            anim.SetBool("isShooting", false);
            anim.SetBool("isAttacking", false);
        } else
        {
            // navMeshAgent.Stop();

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

        GameObject collidedObj = other.gameObject;

        WeaponBehavior weaponBehavior = collidedObj.GetComponent<WeaponBehavior>();

        if (weaponBehavior != null)
        {
            if (weaponBehavior.team == this.team)
            {
                return;
            }
        }

        if (collidedObj.name.IndexOf("ArrowPrefab") != -1)
        {
            Quaternion worldRotation = collidedObj.transform.localRotation * collidedObj.transform.parent.rotation;
            Quaternion targetRotation = worldRotation * Quaternion.Inverse(this.transform.rotation);

            collidedObj.transform.parent = this.transform;

            collidedObj.transform.localRotation = targetRotation;

            Rigidbody arrowRB = collidedObj.GetComponent<Rigidbody>();
            arrowRB.velocity = Vector3.zero;
            arrowRB.useGravity = false;
        }

        hp -= 50;

        if (hp <= 0)
        {
            alive = false;
            anim.SetBool("isKilled", true);

            navMeshAgent.Stop();
            navMeshAgent.enabled = false;

            transform.Rotate(new Vector3(-90, 0, 0));

            // print(Quaternion.LookRotation(up));
            // transform.rotation = Quaternion


        }
    }

    void ArcherLoose()
    {
        Vector3 pos = new Vector3();
        pos.x = transform.position.x;
        pos.y = transform.position.y + 1.5f;
        pos.z = transform.position.z;

        Quaternion q = Quaternion.identity;
        
        q.y = 1f;

        GameObject arrow = (GameObject)Instantiate(Resources.Load("ArrowPrefab"), pos, Quaternion.identity);
        arrow.transform.parent = transform;
        arrow.transform.localRotation = q;

        WeaponBehavior weaponBehavior = arrow.GetComponent<WeaponBehavior>();
        weaponBehavior.team = this.team;

        // arrow.transform.rotation = transform.rotation;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.velocity = transform.forward * 30;
    }
}
