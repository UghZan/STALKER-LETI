using System;
using System.Collections;
using UnityEngine;

namespace Weapon.Equipments.Equipment_specific
{
    public class BuffConsumable : ConsumableBase
    {
        public Buff[] buffs;
        public int[] timers;
        public AnimationType animationType;
        public enum AnimationType
        {
            DRINKING,
            FAKE_HIDE
        }

        protected override void PrimaryFire()
        {
            StartCoroutine(Apply());
        }

        IEnumerator Apply()
        {
            switch (animationType)
            {
                case AnimationType.DRINKING: StartCoroutine(manager.PlayDrinkingAnimation(false)); break;
                case AnimationType.FAKE_HIDE: StartCoroutine(manager.FakeHideEquipmentAnimation(false)); break;
            }
            manager.safeToNextSwitch = false;
            yield return new WaitForSeconds(applyTime);
            for (int i = 0; i < buffs.Length; i++)
            {
                p.AddBuff(buffs[i], timers[i]);
            }
            manager.safeToNextSwitch = true;
            isExpired = true;
            yield return null;
        }
    }
}