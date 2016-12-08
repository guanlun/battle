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
    private WeaponBehavior weaponBehavior;

    protected float attackRange;

    private Transform weaponParent;

    private Quaternion fallenTargetRotation;

    protected void init() {
        hp = 100;
        alive = true;
    }

    // Use this for initialization
    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        anim.speed = 0.5f;

        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform part in parts) {
            if (part.name == "Weapon") // Weapon name
            {
                weaponParent = part;
                string weaponPrefabName = null;

                switch (weaponType) {
                    case "bow":
                        weaponPrefabName = "BowPrefab";
                        attackRange = 20f;
                        navMeshAgent.stoppingDistance = 20f;
                        anim.runtimeAnimatorController = Resources.Load("ArcherAC") as RuntimeAnimatorController;
                        break;

                    case "sword":
                        weaponPrefabName = "SwordPrefab";
                        attackRange = 2f;
                        navMeshAgent.stoppingDistance = 2f;
                        anim.runtimeAnimatorController = Resources.Load("SwordManAC") as RuntimeAnimatorController;
                        break;

                    case "spear":
                        weaponPrefabName = "SpearPrefab";
                        attackRange = 5f;
                        navMeshAgent.stoppingDistance = 5f;
                        anim.runtimeAnimatorController = Resources.Load("SpearManAC") as RuntimeAnimatorController;
                        break;

                    case "shield":
                        weaponPrefabName = "ShieldPrefab";
                        attackRange = 1;
                        navMeshAgent.stoppingDistance = 1;

                        anim.runtimeAnimatorController = Resources.Load("ShieldManAC") as RuntimeAnimatorController;
                        break;
                }

                weapon = (GameObject)Instantiate(Resources.Load(weaponPrefabName));
                this.weaponBehavior = weapon.GetComponent<WeaponBehavior>();

                weapon.transform.parent = part.transform;

                weapon.transform.localPosition = new Vector3(0, 0, 0);
                weapon.transform.localScale = new Vector3(1, 1, 1);
                weapon.transform.localRotation = Quaternion.identity;

                WeaponBehavior weaponBehavior = weapon.GetComponent<WeaponBehavior>();

                if (weaponBehavior != null) {
                    weaponBehavior.team = this.team;
                }
            } else if (part.name == "Object02") {
                Renderer renderer = part.GetComponent<Renderer>();

                string matType = team == "red" ? "RedArmyMat" : "BlueArmyMat";
                renderer.material = Resources.Load(matType, typeof(Material)) as Material;
            }
        }

        init();
    }

    private void FindTarget() {
        // TODO: move this back to the Start function?
        SoldierBehavior[] agentBehavior = GameObject.FindObjectsOfType(typeof(SoldierBehavior)) as SoldierBehavior[];

        enemyBehaviors = new List<SoldierBehavior>();
        foreach (SoldierBehavior behavior in agentBehavior) {
            if (behavior.team != team) {
                enemyBehaviors.Add(behavior);
            }
        }

        float closestDist = float.MaxValue;
        SoldierBehavior closestAgent = null;

        foreach (SoldierBehavior behavior in enemyBehaviors) {
            if (!behavior.alive) {
                continue;
            }

            Vector3 enemyPosition = behavior.transform.position;
            float dist = Vector3.Distance(enemyPosition, transform.position);

            if (dist < closestDist) {
                closestDist = dist;
                closestAgent = behavior;
            }
        }

        if (closestAgent != null) {
            target = closestAgent.transform;
        } else {
            target = null;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!alive) {
            transform.rotation = Quaternion.Slerp(transform.rotation, fallenTargetRotation, 0.1f);
            return;
        }

        FindTarget();

        if (target == null) {
            anim.SetBool("isAttacking", false);
            return;
        }

        navMeshAgent.destination = target.position;

        Vector3 direction = target.position - this.transform.position;

        if (direction.magnitude > attackRange) {
            anim.SetBool("isAttacking", false);
        } else {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            anim.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!alive) {
            return;
        }

        GameObject collidedObj = other.gameObject;

        WeaponBehavior enemyWeaponBehavior = collidedObj.GetComponent<WeaponBehavior>();

        if (enemyWeaponBehavior != null) {
            if (enemyWeaponBehavior.team == this.team) {
                return;
            }
        }

        if (collidedObj.name.IndexOf("ArrowPrefab") != -1) {
            if (enemyWeaponBehavior.blocked) {
                return;
            }
        }

        int damageInflicted = this.weaponBehavior.defend(enemyWeaponBehavior);

        hp -= damageInflicted;

        if (hp <= 0) {
            alive = false;
            anim.SetBool("isKilled", true);

            navMeshAgent.enabled = false;

            transform.Rotate(new Vector3(-90, 0, 0));
            fallenTargetRotation = transform.rotation;

            // Transform back
            transform.Rotate(new Vector3(90, 0, 0));
        }
    }

    void ArcherLoose() {
        Vector3 pos = new Vector3();
        pos.x = transform.position.x;
        pos.y = transform.position.y + 1.5f;
        pos.z = transform.position.z;

        GameObject arrow = (GameObject)Instantiate(Resources.Load("ArrowPrefab"), pos, Quaternion.identity);

        WeaponBehavior weaponBehavior = arrow.GetComponent<WeaponBehavior>();
        weaponBehavior.team = this.team;

        arrow.transform.rotation = transform.rotation;
        arrow.transform.Rotate(new Vector3(0, 90, 0));
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        float speed = 25;
        float coeff = 5;
        float dist = Vector3.Distance(target.position, this.transform.position);

        float angle = Mathf.Asin(coeff * dist / (speed * speed));

        Vector3 vel = transform.forward * speed * Mathf.Cos(angle) + transform.up * speed * Mathf.Sin(angle);

        float randScale = 0.5f;

        Vector3 random = Random.Range(-randScale, randScale) * transform.forward +
            Random.Range(-randScale, randScale) * transform.up +
            Random.Range(-randScale, randScale) * transform.right;

        rb.velocity = vel + random;
    }
}
