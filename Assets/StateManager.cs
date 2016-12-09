using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {
    public static string currWeaponType = "sword";
    public static bool paused = true;

    public static List<SoldierBehavior> soldierBehaviors;

    private int weaponTypeIdx;
    private string[] weaponTypes = { "sword", "bow", "spear", "shield", "horse" };
    private Text[] weaponTexts;

    private Vector3 instantiationOffset = new Vector3(20, 2, 30);

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

        soldierBehaviors = new List<SoldierBehavior>();
    }

    public static void instantiateSoldier(Vector3 position, string weaponType, string team) {
        SoldierBehavior behavior;

        if (weaponType == "horse") {
            GameObject knight = (GameObject)Instantiate(Resources.Load("KnightPrefab"), position, Quaternion.identity);
            behavior = knight.GetComponent<KnightBehavior>();
        } else {
            GameObject soldier = (GameObject)Instantiate(Resources.Load("SoldierPrefab"), position, Quaternion.identity);
            behavior = soldier.GetComponent<SoldierBehavior>();
        }

        behavior.team = team;
        behavior.weaponType = weaponType;
        behavior.initialPosition = new Vector3(position.x, position.y, position.z);

        soldierBehaviors.Add(behavior);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return)) {
            paused = !paused;
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 1f, 0, -20);
                instantiateSoldier(pos, "shield", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 1f + 0.5f, 0, -25);
                instantiateSoldier(pos, "spear", "red");
            }

            for (int i = 0; i < 80; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(Random.Range(0f, 15f), 0, Random.Range(0f, 35f));
                instantiateSoldier(pos, Random.Range(0f, 1f) < 0.8 ? "sword" : "bow", "blue");
            }
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
