using UnityEngine;
using System.Collections;

public class BowBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_BOW;
        this.damage = 0;
    }
}