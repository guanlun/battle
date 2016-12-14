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

            foreach (SoldierBehavior behavior in soldierBehaviors) {
                if (paused) {
                    behavior.pause();
                } else {
                    behavior.resume();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.H)) {
            Cursor.visible = !Cursor.visible;
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

            for (int i = 0; i < 40; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(Random.Range(0f, 15f), 0, Random.Range(0f, 35f));
                instantiateSoldier(pos, "sword", "blue");
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 2f, 0, -80 + Random.Range(-0.3f, 0.3f));
                instantiateSoldier(pos, "horse", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 2f, 0, -85 + Random.Range(-0.3f, 0.3f));
                instantiateSoldier(pos, "horse", "red");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, 30 + Random.Range(-0.1f, 0.1f));
                instantiateSoldier(pos, "shield", "blue");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i + 0.5f, 0, 35 + Random.Range(-0.1f, 0.1f));
                instantiateSoldier(pos, "spear", "blue");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, 37 + Random.Range(-0.1f, 0.1f));
                instantiateSoldier(pos, "spear", "blue");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i + 0.5f, 0, 39 + Random.Range(-0.1f, 0.1f));
                instantiateSoldier(pos, "spear", "blue");
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha3)) {
            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, -60);
                instantiateSoldier(pos, "shield", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, -62);
                instantiateSoldier(pos, "shield", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, -66);
                instantiateSoldier(pos, "spear", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, -68);
                instantiateSoldier(pos, "spear", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i, 0, -68);
                instantiateSoldier(pos, "spear", "red");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i - 5, 0, -75);
                instantiateSoldier(pos, "bow", "red");
            }

            for (int i = 0; i < 15; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i - 70 + Random.Range(-10, 10), 0, -15 + Random.Range(-10, 10));
                instantiateSoldier(pos, "bow", "red");
            }

            for (int i = 0; i < 2; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 45, 0, -60);
                instantiateSoldier(pos, "horse", "red");
            }

            for (int i = 0; i < 4; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 42, 0, -65);
                instantiateSoldier(pos, "horse", "red");
            }

            for (int i = 0; i < 6; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 39, 0, -70);
                instantiateSoldier(pos, "horse", "red");
            }

            for (int i = 0; i < 25; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i + 3f, 0, 28);
                instantiateSoldier(pos, "shield", "blue");
            }

            for (int i = 0; i < 20; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i + 5, 0, 30);
                instantiateSoldier(pos, "sword", "blue");
            }

            for (int i = 0; i < 20; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i + 5, 0, 32);
                instantiateSoldier(pos, "sword", "blue");
            }

            for (int i = 0; i < 10; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 35, 0, 50);
                instantiateSoldier(pos, "horse", "blue");
            }

            for (int i = 0; i < 10; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 35, 0, 55);
                instantiateSoldier(pos, "horse", "blue");
            }

            for (int i = 0; i < 10; i++) {
                Vector3 pos = this.instantiationOffset + new Vector3(i * 3 + 35, 0, 60);
                instantiateSoldier(pos, "horse", "blue");
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
