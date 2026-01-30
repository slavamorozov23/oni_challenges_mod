using System;
using UnityEngine;

// Token: 0x02000658 RID: 1624
public class BatteryTracker : WorldTracker
{
	// Token: 0x06002778 RID: 10104 RVA: 0x000E2CC6 File Offset: 0x000E0EC6
	public BatteryTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x000E2CD0 File Offset: 0x000E0ED0
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
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num2);
					foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
					{
						num += battery.JoulesAvailable;
					}
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x000E2DDC File Offset: 0x000E0FDC
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
