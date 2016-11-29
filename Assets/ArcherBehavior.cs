using UnityEngine;
using System.Collections;

public class ArcherBehavior : SoldierBehavior
{
	// Use this for initialization
	void Start () {
        Vector3 pos = new Vector3();
        pos.x = transform.position.x;
        pos.y = transform.position.y + 1f;
        pos.z = transform.position.z + 0.5f;

        GameObject arrow = (GameObject)Instantiate(Resources.Load("ArrowPrefab"), pos, transform.rotation);
        arrow.GetComponent<Rigidbody>().velocity = new Vector3(-5f, 0, 0);
    }
	

}
