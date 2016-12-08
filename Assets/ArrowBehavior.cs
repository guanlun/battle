using UnityEngine;
using System.Collections;

public class ArrowBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_ARROW;
        this.damage = 30;
    }

    private void OnTriggerEnter(Collider other) {
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