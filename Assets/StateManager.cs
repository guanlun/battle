using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {
    public static string currWeaponType;
    public static bool paused = true;

    private int weaponTypeIdx;
    private string[] weaponTypes = { "sword", "bow", "spear", "shield", "horse" };
    private Text[] weaponTexts;

    // Use this for initialization
    void Start () {
        Transform canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;

        weaponTexts = new Text[weaponTypes.Length];
        weaponTexts[0] = canvasTransform.Find("SwordText").GetComponent<Text>();
        weaponTexts[1] = canvasTransform.Find("BowText").GetComponent<Text>();
        weaponTexts[2] = canvasTransform.Find("SpearText").GetComponent<Text>();
        weaponTexts[3] = canvasTransform.Find("ShieldText").GetComponent<Text>();
        weaponTexts[4] = canvasTransform.Find("LanceText").GetComponent<Text>();

        weaponTexts[0].color = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return)) {
            paused = !paused;
        }

        if (Input.GetKeyUp(KeyCode.P)) {
            GameObject soldier = (GameObject)Instantiate(Resources.Load("SoldierPrefab"), new Vector3(20, 2, 20), Quaternion.identity);
            SoldierBehavior behavior = soldier.GetComponent<SoldierBehavior>();

            behavior.team = "red";
            behavior.weaponType = "sword";
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            weaponTexts[weaponTypeIdx].color = Color.black;

            if (scroll < 0) {
                weaponTypeIdx++;

                if (weaponTypeIdx == weaponTypes.Length) {
                    weaponTypeIdx = 0;
                }
            } else if (scroll > 0) {
                weaponTypeIdx--;

                if (weaponTypeIdx == -1) {
                    weaponTypeIdx = weaponTypes.Length - 1;
                }
            }

            currWeaponType = weaponTypes[weaponTypeIdx];

            weaponTexts[weaponTypeIdx].color = Color.red;
        }
    }
}
