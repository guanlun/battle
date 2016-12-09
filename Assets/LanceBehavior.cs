using UnityEngine;
using System.Collections;

public class LanceBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_LANCNE;
        this.damage = 100;
    }

    public override int defend(WeaponBehavior attackWeapon, float rand) {
        float defendAngle = this.getDefendAngle(attackWeapon);

        bool blocked = false;

        if (defendAngle > 0.8f) { // Facing the enemy
            switch (attackWeapon.type) {
                case TYPE_SPEAR:
                    blocked = (rand < 0.5f);
                    break;
                case TYPE_SWORD:
                    blocked = (rand < 0.3f);
                    break;
                case TYPE_SHIELD:
                    blocked = (rand < 0.1f);
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