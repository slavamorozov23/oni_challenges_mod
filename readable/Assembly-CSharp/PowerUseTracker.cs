using System;
using UnityEngine;

// Token: 0x02000657 RID: 1623
public class PowerUseTracker : WorldTracker
{
	// Token: 0x06002775 RID: 10101 RVA: 0x000E2BF2 File Offset: 0x000E0DF2
	public PowerUseTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000E2BFC File Offset: 0x000E0DFC
	public override void UpdateData()
	{
		float num = 0f;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num2] == base.WorldID)
				{
					num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(num2));
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000E2CBC File Offset: 0x000E0EBC
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
	}
}
