using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedObj = other.gameObject;

        WeaponBehavior weaponBehavior = collidedObj.GetComponent<WeaponBehavior>();

        if (weaponBehavior != null)
        {
            // if (weaponBehavior.team == this.team)
            // {
            //     return;
            // }
        }

        if (collidedObj.name.IndexOf("ArrowPrefab") != -1)
        {
            WeaponBehavior arrowBehavior = collidedObj.GetComponent<WeaponBehavior>();
            arrowBehavior.blocked = true;

            Quaternion worldRotation = collidedObj.transform.localRotation * collidedObj.transform.parent.rotation;
            Quaternion targetRotation = worldRotation * Quaternion.Inverse(this.transform.rotation);

            collidedObj.transform.parent = this.transform;
            collidedObj.transform.localRotation = targetRotation;

            Rigidbody arrowRB = collidedObj.GetComponent<Rigidbody>();
            arrowRB.velocity = Vector3.zero;
            arrowRB.useGravity = false;
        }
    }
}