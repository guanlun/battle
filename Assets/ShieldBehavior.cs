using UnityEngine;
using System.Collections;

public class ShieldBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_SHIELD;
        this.damage = 10;
    }
}