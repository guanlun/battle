using UnityEngine;
using System.Collections;

public class SpearBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_SPEAR;
        this.damage = 50;
    }
}