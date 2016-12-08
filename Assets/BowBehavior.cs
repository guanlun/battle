using UnityEngine;
using System.Collections;

public class BowBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_BOW;
        this.damage = 0;
    }

    public override int defend(WeaponBehavior attackWeapon, float rand) {
        float defendAngle = this.getDefendAngle(attackWeapon);

        bool blocked = false;

        if (defendAngle > 0.8f) { // Facing the enemy
            switch (attackWeapon.type) {
                case TYPE_SPEAR:
                    blocked = (rand < 0.2f);
                    break;
                case TYPE_SWORD:
                    blocked = (rand < 0.2f);
                    break;
                case TYPE_SHIELD:
                    blocked = (rand < 0.1f);
                    break;
            }
        } else {
            blocked = (rand < BACK_BLOCK_CHANGE);
        }

        return blocked ? 0 : attackWeapon.damage;
    }
}