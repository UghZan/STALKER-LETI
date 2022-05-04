using UnityEngine;

namespace Weapon.Equipments
{
    public class GrenadeBase : EquipmentBase
    {
        public GameObject grenadePrefab;
        public Vector3 readyOffset = Vector3.right * 0.5f + Vector3.up * 0.5f;

        private bool armed = false;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                armed = true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.safeToNextSwitch)
                    {
                        armed = false;
                        Debug.Log("secfire");
                        SecondaryFire();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.safeToNextSwitch)
                    {
                        Debug.Log("primfire");
                        PrimaryFire();
                    }
                }
            }

            if(armed)
                transform.localPosition = Vector3.Lerp(transform.localPosition, readyOffset, Time.deltaTime * 10);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10);
            
            if (Input.GetMouseButtonUp(1))
            {
                armed = false;
            }

            if (isExpired)
            {
                manager.ExpireEquipment();
                isExpired = false;
            }
        }
    }
}
