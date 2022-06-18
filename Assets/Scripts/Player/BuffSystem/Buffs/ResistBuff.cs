using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Resist Buff")] 
public class ResistBuff : Buff
{
    public float normalResist;
    public float anomalResist;
    public float electricResist;
    public float hotResist;
    public float freezeResist;
    public float radResist;
    public float bleedResist;
    public float mentalResist;
        
    public override void OnApply(GenericStats stats)
    {
        stats.UpdateMultiplier("normalVul", -normalResist);
        stats.UpdateMultiplier("anomalVul", -anomalResist);
        stats.UpdateMultiplier("electricVul", -electricResist);
        stats.UpdateMultiplier("hotVul", -hotResist);
        stats.UpdateMultiplier("freezeVul", -freezeResist);
        stats.UpdateMultiplier("radVul", -radResist);
        stats.UpdateMultiplier("bleedVul", -bleedResist);
        stats.UpdateMultiplier("mentalVul", -mentalResist);
    }
        
    public override void OnRemove(GenericStats stats)
    {
        stats.UpdateMultiplier("normalVul", normalResist);
        stats.UpdateMultiplier("anomalVul", anomalResist);
        stats.UpdateMultiplier("electricVul", electricResist);
        stats.UpdateMultiplier("hotVul", hotResist);
        stats.UpdateMultiplier("freezeVul", freezeResist);
        stats.UpdateMultiplier("radVul", radResist);
        stats.UpdateMultiplier("bleedVul", bleedResist);
        stats.UpdateMultiplier("mentalVul", mentalResist);
    }
}