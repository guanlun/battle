using UnityEngine;
using System.Collections;

public class KnightBehavior : SoldierBehavior {
    private int frameCount = 0;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        anim.speed = 0.5f;

        Transform[] parts = GetComponentsInChildren<Transform>();
        foreach (Transform part in parts) {
            if (part.name == "Weapon") // Weapon name
            {
                this.weaponParent = part;
                attackRange = 0f;

                this.weapon = (GameObject)Instantiate(Resources.Load("LancePrefab"));
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

        init();
    }
	
	void Update () {
        if (!alive) {
            transform.rotation = Quaternion.Slerp(transform.rotation, fallenTargetRotation, 0.1f);
            return;
        }

        FindTarget();

        if (target == null) {
            anim.SetBool("isAttacking", false);
            return;
        }

        if (frameCount != 0) {
            frameCount--;
        }

        // if (frameCount == 0) {
        //     navMeshAgent.destination = target.position;
        // }

        Vector3 direction = target.position - this.transform.position;

        navMeshAgent.destination = target.position;// + Vector3.Normalize(direction) * 10;

        if (direction.magnitude > attackRange) {
            // navMeshAgent.Resume();
            anim.SetBool("isAttacking", false);
        } else {
            // navMeshAgent.destination = target.position + direction * 20;
            // frameCount = 100;

            // navMeshAgent.Stop();
            // transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.2f);
            anim.SetBool("isAttacking", true);
        }

        this.weaponBehavior.damage = 200;
    }
}
