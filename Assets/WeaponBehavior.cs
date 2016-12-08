using UnityEngine;
using System.Collections;

public class WeaponBehavior : MonoBehaviour {
    public string team;
    public bool blocked;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void defend(WeaponBehavior attackWeapon) {

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