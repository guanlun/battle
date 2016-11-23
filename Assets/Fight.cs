using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Fight : MonoBehaviour {
    public Transform target;

    static Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = target.position - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        
        if (anim.GetBool("isMoving"))
        {
            if (direction.magnitude > 1)
            {
                this.transform.Translate(0, 0, 0.02f);
            } else
            {
                anim.SetBool("isMoving", false);
            }
        }
	}
}
