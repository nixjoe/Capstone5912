using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakofWhoosh : ActiveItem
{
    private ActiveTimeout timeout;
    private ActiveCooldown cooldown;

    public override void ActivateHold() { }
    public override void ActivateRelease() { }

    public override void ActiveDown() {
        ActivateCloak();
    }

    public override void OnEquip() {
        GetComponent<Cloth>().capsuleColliders = new CapsuleCollider[] { Owner.GetComponent<CapsuleCollider>() };
        timeout = GetComponent<ActiveTimeout>();
        cooldown = GetComponent<ActiveCooldown>();
        timeout.OnTimeout += DeactivateCloak;
    }

    // TODO: Need on dequip to remove effects that are currently active.

    private void ActivateCloak() {
        if (timeout.InTimeout || !cooldown.Ready) return;

        Owner.GetComponent<PlayerStatsController>().state.Speed += 1;

        timeout.StartTimeout();
    }

    private void DeactivateCloak() {
        Owner.GetComponent<PlayerStatsController>().state.Speed -= 1;

        cooldown.ResetCooldown();
    }
}