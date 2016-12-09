using UnityEngine;
using System.Collections;

public class ShieldBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_SHIELD;
        this.damage = 5;
    }

    public override int defend(WeaponBehavior attackWeapon, float rand) {
        float defendAngle = this.getDefendAngle(attackWeapon);

        bool blocked = false;

        if (defendAngle > 0) { // Facing the enemy
            switch (attackWeapon.type) {
                case TYPE_SPEAR:
                    blocked = (rand < 0.95f);
                    break;
                case TYPE_SWORD:
                    blocked = (rand < 0.9f);
                    break;
                case TYPE_SHIELD:
                    blocked = (rand < 0.9f);
                    break;
                default:
                    blocked = false;
                    break;
            }
        } else {
            blocked = (rand < BACK_BLOCK_CHANGE);
        }

        return blocked ? 0 : attackWeapon.damage;
    }
}