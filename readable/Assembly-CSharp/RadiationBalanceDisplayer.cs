using System;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C96 RID: 3222
public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062C9 RID: 25289 RVA: 0x0024A43B File Offset: 0x0024863B
	public RadiationBalanceDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new RadiationBalanceDisplayer.RadiationAttributeFormatter();
	}

	// Token: 0x060062CA RID: 25290 RVA: 0x0024A452 File Offset: 0x00248652
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance) + UI.UNITSUFFIXES.RADIATION.RADS;
	}

	// Token: 0x060062CB RID: 25291 RVA: 0x0024A46C File Offset: 0x0024866C
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
		{
			int num = Grid.PosToCell(instance.gameObject);
			if (Grid.IsValidCell(num))
			{
				stringBuilder.Append(DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE);
			}
			stringBuilder.Append("\n\n");
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
			stringBuilder.AppendFormat(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, Mathf.RoundToInt(Grid.Radiation[num] * num2));
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x02001EC8 RID: 7880
	public class RadiationAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600B4AB RID: 46251 RVA: 0x003EC132 File Offset: 0x003EA332
		public RadiationAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
