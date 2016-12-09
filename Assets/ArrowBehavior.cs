using UnityEngine;
using System.Collections;

public class ArrowBehavior : WeaponBehavior {
    private Rigidbody rigidBody;

    private void Start() {
        this.type = TYPE_ARROW;
        this.damage = 30;

        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void Update() {
        // this.transform.rotation = Quaternion.LookRotation(this.rigidBody.velocity);
    }

    private void OnTriggerEnter(Collider other) {
        if (!this.blocked) {
            GameObject collidedObj = other.gameObject;

            this.transform.parent = collidedObj.transform;

            rigidBody.velocity = Vector3.zero;
            rigidBody.useGravity = false;

            this.blocked = true;
        }
    }
}