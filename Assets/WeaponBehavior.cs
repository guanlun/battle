using UnityEngine;
using System.Collections;

public class WeaponBehavior : MonoBehaviour {
    public string team;
    public bool blocked;
    public SoldierBehavior holder;

    public string type;
    public int damage;

    public const string TYPE_SWORD = "sword";
    public const string TYPE_SPEAR = "spear";
    public const string TYPE_SHIELD = "shield";
    public const string TYPE_BOW = "bow";
    public const string TYPE_ARROW = "arrow";
    public const string TYPE_LANCNE = "lance";

    protected const float BACK_BLOCK_CHANGE = 0.2f;

    void Start() {

    }

    void Update() {
        
    }

    protected float getDefendAngle(WeaponBehavior attackWeapon) {
        SoldierBehavior enemy = attackWeapon.holder;

        Vector3 direction = enemy.transform.position - this.holder.transform.position;
        return Vector3.Dot(Vector3.Normalize(this.holder.transform.forward), direction) / Vector3.Magnitude(direction);
    }

    public virtual int defend(WeaponBehavior attackWeapon, float rand) {
        return attackWeapon.damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (gameObject.name.Contains("ArrowPrefab")) {
            if (!this.blocked) {
                GameObject collidedObj = other.gameObject;

                this.transform.parent = collidedObj.transform;

                Rigidbody arrowRB = this.GetComponent<Rigidbody>();
                arrowRB.velocity = Vector3.zero;
                arrowRB.useGravity = false;

                this.blocked = true;
            }
        }
    }
}