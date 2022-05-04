using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class ArtifactBeltController : MonoBehaviour
{
    [SerializeField] private InventoryController inv;
    [SerializeField] private Player p;

    [SerializeField] private List<Buff> buffs;
    private ArtifactBase[] arts = new ArtifactBase[3];

    private void Start()
    {
        buffs = new List<Buff>();
        StartCoroutine(BuffTick());
    }

    public void AddArtifact(ArtifactBase art, int index)
    {
        arts[index] = art;
        art.OnEquip(this, p);
    }

    public void RemoveArtifact(int index)
    {
        arts[index].OnUnequip();
        arts[index] = null;
    }

    public void AddBuff(Buff b)
    {
        buffs.Add(b);
    }

    public void RemoveBuff(Buff b)
    {
        buffs.Remove(b);
    }

    IEnumerator BuffTick()
    {
        for (;;)
        {
            foreach (Buff b in buffs)
            {
                b.OnTick();
                
            }
            
            yield return new WaitForSeconds(0.1f);
        } 
    }
}
