using UnityEngine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

    private List<Camera> soldierCameras;
    private Camera activeFirstPersonCamera;
    private Camera mainCamera;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        SoldierBehavior[] agentBehavior = GameObject.FindObjectsOfType(typeof(SoldierBehavior)) as SoldierBehavior[];

        this.soldierCameras = new List<Camera>();
        foreach (SoldierBehavior behavior in agentBehavior) {
            if (behavior.alive) {
                soldierCameras.Add(behavior.firstPersonCamera);
            }
        }

        GhostFreeRoamCamera mainCamScript = GameObject.FindObjectOfType(typeof(GhostFreeRoamCamera)) as GhostFreeRoamCamera;
        mainCamera = mainCamScript.gameObject.GetComponent<Camera>();

        if (Input.GetKeyUp(KeyCode.C)) {
            this.mainCamera.enabled = false;

            int camNum = this.soldierCameras.Count;
            int idx = Random.Range(0, camNum);

            this.activeFirstPersonCamera = this.soldierCameras[idx];

            this.activeFirstPersonCamera.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.M)) {
            this.activeFirstPersonCamera.enabled = false;

            this.mainCamera.enabled = true;
        }
    }
}
