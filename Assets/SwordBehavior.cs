using UnityEngine;
using System.Collections;

public class SwordBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_SHIELD;
        this.damage = 30;
    }

    public override int defend(WeaponBehavior attackWeapon) {
        float rand = Random.Range(0, 1);

        if (attackWeapon.GetType() == typeof(SpearBehavior)) {
            print("sword against spear");
        }

        return attackWeapon.damage;
    }
}