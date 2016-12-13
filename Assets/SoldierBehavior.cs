using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SoldierBehavior : MonoBehaviour {
    public ThirdPersonCharacter character { get; private set; }

    public string team;
    public string weaponType;

    protected NavMeshAgent navMeshAgent;
    protected Animator anim;

    public bool alive = true;

    protected List<SoldierBehavior> enemyBehaviors;
    protected Transform target;
    protected int hp;
    protected GameObject weapon;
    protected WeaponBehavior weaponBehavior;

    protected float attackRange;

    protected Transform weaponParent;

    protected Quaternion fallenTargetRotation;

    public Vector3 initialPosition;

    protected virtual void init() {
        this.hp = 100;
    }

    // Use this for initialization
    void Start() {
        init();

        navMeshAgent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        anim.speed = 0.5f;
        // anim.enabled = false;

        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform part in parts) {
            if (part.name == "Weapon") // Weapon name
            {
                this.weaponParent = part;
                string weaponPrefabName = null;

                switch (weaponType) {
                    case "bow":
                        weaponPrefabName = "BowPrefab";
                        attackRange = 40f;
                        anim.runtimeAnimatorController = Resources.Load("ArcherAC") as RuntimeAnimatorController;
                        break;

                    case "sword":
                        weaponPrefabName = "SwordPrefab";
                        attackRange = 2f;
                        navMeshAgent.speed = 4;
                        navMeshAgent.radius = 0.15f;
                        anim.runtimeAnimatorController = Resources.Load("SwordManAC") as RuntimeAnimatorController;
                        break;

                    case "spear":
                        weaponPrefabName = "SpearPrefab";
                        attackRange = 4.6f;
                        anim.runtimeAnimatorController = Resources.Load("SpearManAC") as RuntimeAnimatorController;
                        break;

                    case "shield":
                        weaponPrefabName = "ShieldPrefab";
                        attackRange = 1;

                        anim.runtimeAnimatorController = Resources.Load("ShieldManAC") as RuntimeAnimatorController;
                        break;
                }

                this.weapon = (GameObject)Instantiate(Resources.Load(weaponPrefabName));
                this.weapon.transform.parent = part.transform;
                this.weapon.transform.localPosition = new Vector3(0, 0, 0);
                this.weapon.transform.localScale = new Vector3(1, 1, 1);
                this.weapon.transform.localRotation = Quaternion.identity;

                this.weaponBehavior = this.weapon.GetComponent<WeaponBehavior>();
                if (this.weaponBehavior != null) {
                    this.weaponBehavior.team = this.team;
                    this.weaponBehavior.holder = this;
                }
            } else if (part.name == "Object02") {
                Renderer renderer = part.GetComponent<Renderer>();

                string matType = team == "red" ? "RedArmyMat" : "BlueArmyMat";
                renderer.material = Resources.Load(matType, typeof(Material)) as Material;
            }
        }
    }

    protected virtual void FindTarget() {
        float closestDist = float.MaxValue;
        SoldierBehavior closestAgent = null;
        
        foreach (SoldierBehavior behavior in StateManager.soldierBehaviors) {
            if (!behavior.alive || behavior.team == team) {
                continue;
            }

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
        if (StateManager.paused) {
            return;
        }

        if (!alive) {
            transform.rotation = Quaternion.Slerp(transform.rotation, fallenTargetRotation, 0.1f);
            return;
        }

        FindTarget();

        if (target == null) {
            anim.SetBool("isAttacking", false);
            return;
        }

        this.navMeshAgent.destination = target.position;

        Vector3 direction = target.position - this.transform.position;

        if (this.weaponType == "spear" && direction.magnitude < 4) {
            this.navMeshAgent.Resume();
            this.navMeshAgent.destination = this.transform.position - direction;
            return;
        }

        if (direction.magnitude > attackRange) {
            this.navMeshAgent.Resume();

            this.anim.SetBool("isAttacking", false);
        } else {
            this.navMeshAgent.Stop();

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            this.anim.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!alive) {
            return;
        }

        GameObject collidedObj = other.gameObject;

        WeaponBehavior enemyWeaponBehavior = collidedObj.GetComponent<WeaponBehavior>();

        if (enemyWeaponBehavior != null) {
            if (enemyWeaponBehavior.type != WeaponBehavior.TYPE_ARROW) {
                if ((enemyWeaponBehavior.team == this.team) || (!enemyWeaponBehavior.holder.alive)) {
                    return;
                }

                if (enemyWeaponBehavior.holder.target != this.transform) {
                    return;
                }
            }
        }

        int damageInflicted = 0;

        if (enemyWeaponBehavior.type == WeaponBehavior.TYPE_ARROW) {
            if (enemyWeaponBehavior.blocked) {
                return;
            } else {
                damageInflicted = enemyWeaponBehavior.damage;
            }
        } else {
            damageInflicted = this.weaponBehavior.defend(enemyWeaponBehavior, Random.Range(0f, 1f));
        }

        hp -= damageInflicted;

        if (hp <= 0) {
            alive = false;
            anim.SetBool("isKilled", true);

            navMeshAgent.enabled = false;

            this.gettingKilled();
        }
    }

    protected virtual void gettingKilled() {
        transform.Rotate(new Vector3(-90, 0, 0));
        fallenTargetRotation = transform.rotation;

        // Transform back
        transform.Rotate(new Vector3(90, 0, 0));
    }

    void ArcherLoose() {
        if (target == null) {
            return;
        }

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

    public float getSpeed() {
        return Vector3.Magnitude(this.navMeshAgent.velocity);
    }

    public void pause() {
        this.anim.enabled = false;
        this.navMeshAgent.Stop();
    }

    public void resume() {
        this.anim.enabled = true;
        this.navMeshAgent.Resume();
    }
}
