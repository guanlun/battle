using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Fight : MonoBehaviour {
    public Transform target;
    // public NavMeshAgent agent { get; private set; }
    public ThirdPersonCharacter character { get; private set; }

    static Animator anim;

	// Use this for initialization
	void Start () {
        // agent = GetComponentInChildren<NavMeshAgent>();

        // anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = target.position - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        
        // if (anim.GetBool("isMoving"))
        // {
            if (direction.magnitude > 1)
            {
                this.transform.Translate(0, 0, 0.1f);
            } else
            {
                // anim.SetBool("isMoving", false);
            }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hit");
    }
}
