using UnityEngine;
using System.Collections;

public class WeaponBehavior : MonoBehaviour {
    public string team;
    public bool blocked;

    public string type;
    public int damage;

    public static string TYPE_SWORD = "sword";
    public static string TYPE_SPEAR = "spear";
    public static string TYPE_SHIELD = "shield";
    public static string TYPE_BOW = "bow";
    public static string TYPE_ARROW = "arrow";

    void Start() {

    }

    void Update() {
        
    }

    public virtual int defend(WeaponBehavior attackWeapon) {
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