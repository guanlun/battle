using UnityEngine;
using System.Collections;

public class LanceBehavior : WeaponBehavior {
    private void Start() {
        this.type = TYPE_LANCNE;
        this.damage = 150;
    }

    public override int defend(WeaponBehavior attackWeapon, float rand) {
        float defendAngle = this.getDefendAngle(attackWeapon);

        bool blocked = false;

        if (defendAngle > 0.5f) { // Facing the enemy
            switch (attackWeapon.type) {
                case TYPE_SPEAR:
                    blocked = (rand < 0.3f);
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
            if (this.holder.getSpeed() > 3) {
                blocked = (rand < 0.2f);
            } else {
                blocked = (rand < BACK_BLOCK_CHANGE);
            }
        }

        if (blocked) {
            return 0;
        }

        if (attackWeapon.type == "spear" || attackWeapon.type == "lance") {
            return (int)(attackWeapon.damage * 1.5f);
        } else {
            return (int)(attackWeapon.damage / 1.5f);
        }
    }
}