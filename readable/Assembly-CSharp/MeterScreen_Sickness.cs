using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02000DA7 RID: 3495
public class MeterScreen_Sickness : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x06006CCD RID: 27853 RVA: 0x00292870 File Offset: 0x00290A70
	protected override void InternalRefresh()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Label.text = num.ToString();
	}

	// Token: 0x06006CCE RID: 27854 RVA: 0x002928A0 File Offset: 0x00290AA0
	protected override string OnTooltip()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_SICK_DUPES, num.ToString()), this.ToolTipStyle_Header);
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			if (!minionIdentity.IsNullOrDestroyed())
			{
				string text = minionIdentity.GetComponent<KSelectable>().GetName();
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					text += " (";
					int num2 = 0;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						text = text + ((num2 > 0) ? ", " : "") + sicknessInstance.modifier.Name;
						num2++;
					}
					text += ")";
				}
				bool selected = i == this.lastSelectedDuplicantIndex;
				base.AddToolTipLine(text, selected);
			}
		}
		return "";
	}

	// Token: 0x06006CCF RID: 27855 RVA: 0x002929DC File Offset: 0x00290BDC
	private int CountSickDupes(List<MinionIdentity> minions)
	{
		int num = 0;
		foreach (MinionIdentity minionIdentity in minions)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetComponent<MinionModifiers>().sicknesses.IsInfected())
			{
				num++;
			}
		}
		return num;
	}
}
