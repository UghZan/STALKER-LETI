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
        
    public override void OnApply()
    {
        plr.UpdateMultiplier("normalVul", -normalResist);
        plr.UpdateMultiplier("anomalVul", -anomalResist);
        plr.UpdateMultiplier("electricVul", -electricResist);
        plr.UpdateMultiplier("hotVul", -hotResist);
        plr.UpdateMultiplier("freezeVul", -freezeResist);
        plr.UpdateMultiplier("radVul", -radResist);
        plr.UpdateMultiplier("bleedVul", -bleedResist);
        plr.UpdateMultiplier("mentalVul", -mentalResist);
    }
        
    public override void OnRemove()
    {
        plr.UpdateMultiplier("normalVul", normalResist);
        plr.UpdateMultiplier("anomalVul", anomalResist);
        plr.UpdateMultiplier("electricVul", electricResist);
        plr.UpdateMultiplier("hotVul", hotResist);
        plr.UpdateMultiplier("freezeVul", freezeResist);
        plr.UpdateMultiplier("radVul", radResist);
        plr.UpdateMultiplier("bleedVul", bleedResist);
        plr.UpdateMultiplier("mentalVul", mentalResist);
    }
}