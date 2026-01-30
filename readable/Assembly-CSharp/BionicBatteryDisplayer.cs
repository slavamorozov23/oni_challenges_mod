using System;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C92 RID: 3218
public class BionicBatteryDisplayer : StandardAmountDisplayer
{
	// Token: 0x060062C0 RID: 25280 RVA: 0x00249B84 File Offset: 0x00247D84
	private string GetIconForState(BionicBatteryDisplayer.ElectrobankState state)
	{
		switch (state)
		{
		case BionicBatteryDisplayer.ElectrobankState.Unexistent:
			return BionicBatteryMonitor.EmptySlotBatteryIcon;
		case BionicBatteryDisplayer.ElectrobankState.Charged:
			return BionicBatteryMonitor.ChargedBatteryIcon;
		}
		return BionicBatteryMonitor.DischargedBatteryIcon;
	}

	// Token: 0x060062C1 RID: 25281 RVA: 0x00249BC8 File Offset: 0x00247DC8
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float wattage = smi.Wattage;
			num += wattage;
		}
		if (master.description.IndexOf("{1}") > -1)
		{
			stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		if (smi != null)
		{
			int electrobankCount = smi.ElectrobankCount;
			int electrobankCountCapacity = smi.ElectrobankCountCapacity;
			stringBuilder.Append("\n\n");
			stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_DETAILS_LABEL, GameUtil.GetFormattedInt((float)electrobankCount, GameUtil.TimeSlice.None), GameUtil.GetFormattedInt((float)electrobankCountCapacity, GameUtil.TimeSlice.None));
			if (electrobankCount > 0)
			{
				for (int i = 0; i < smi.storage.items.Count; i++)
				{
					GameObject gameObject = smi.storage.items[i];
					Electrobank component = gameObject.GetComponent<Electrobank>();
					BionicBatteryDisplayer.ElectrobankState state = (component == null) ? BionicBatteryDisplayer.ElectrobankState.Damaged : ((component.Charge <= 0f) ? BionicBatteryDisplayer.ElectrobankState.Depleated : BionicBatteryDisplayer.ElectrobankState.Charged);
					string iconForState = this.GetIconForState(state);
					float joules = (component == null) ? 0f : component.Charge;
					stringBuilder.Append("\n");
					stringBuilder.Append("    • ");
					stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_ROW, iconForState, gameObject.GetProperName(), GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None));
				}
			}
			if (electrobankCount < electrobankCountCapacity)
			{
				for (int j = 0; j < electrobankCountCapacity - electrobankCount; j++)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("    • ");
					stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_EMPTY_ROW, this.GetIconForState(BionicBatteryDisplayer.ElectrobankState.Unexistent));
				}
			}
		}
		stringBuilder.Append("\n\n");
		stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.CURRENT_WATTAGE_LABEL, this.formatter.GetFormattedValue(num, this.formatter.DeltaTimeSlice));
		if (smi != null)
		{
			StringBuilder stringBuilder2 = GlobalStringBuilderPool.Alloc();
			stringBuilder2.Append("<b>+</b>");
			GameUtil.AppendFormattedWattage(stringBuilder2, smi.GetBaseWattage(), GameUtil.WattageFormatterUnit.Automatic, true);
			stringBuilder.Append("\n");
			stringBuilder.Append("    • ");
			stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.MODIFIERS.BIONIC_WATTS.BASE_NAME, stringBuilder2.ToString());
			stringBuilder2.Clear();
			float num2 = 0f;
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in smi.Modifiers)
			{
				if (wattageModifier.value != 0f)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("    • ");
					stringBuilder.Append(wattageModifier.name);
				}
				else if (wattageModifier.potentialValue > 0f)
				{
					stringBuilder2.Append("\n");
					stringBuilder2.Append("    • ");
					stringBuilder2.Append(wattageModifier.name);
					num2 += wattageModifier.potentialValue;
				}
			}
			if (stringBuilder2.Length != 0)
			{
				stringBuilder.Append("\n\n");
				stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.POTENTIAL_EXTRA_WATTAGE_LABEL, this.formatter.GetFormattedValue(num2, this.formatter.DeltaTimeSlice));
				stringBuilder.Append(stringBuilder2.ToString());
			}
			GlobalStringBuilderPool.Free(stringBuilder2);
		}
		global::Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, "Bionic Battery Displayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, instead, use BionicBatteryMonitor.Instance.Modifiers");
		float seconds = (num == 0f) ? 0f : (smi.CurrentCharge / num);
		stringBuilder.Append("\n\n");
		stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ESTIMATED_LIFE_TIME_REMAINING, GameUtil.GetFormattedCycles(seconds, "F1", false));
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060062C2 RID: 25282 RVA: 0x00249FE0 File Offset: 0x002481E0
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance);
	}

	// Token: 0x060062C3 RID: 25283 RVA: 0x00249FEA File Offset: 0x002481EA
	public BionicBatteryDisplayer() : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new BionicBatteryDisplayer.BionicBatteryAttributeFormatter();
	}

	// Token: 0x04004305 RID: 17157
	private const float criticalIconFlashFrequency = 0.45f;

	// Token: 0x02001EC5 RID: 7877
	private enum ElectrobankState
	{
		// Token: 0x04009099 RID: 37017
		Unexistent,
		// Token: 0x0400909A RID: 37018
		Damaged,
		// Token: 0x0400909B RID: 37019
		Depleated,
		// Token: 0x0400909C RID: 37020
		Charged
	}

	// Token: 0x02001EC6 RID: 7878
	public class BionicBatteryAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x0600B4A8 RID: 46248 RVA: 0x003EC0F8 File Offset: 0x003EA2F8
		public BionicBatteryAttributeFormatter() : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond)
		{
		}
	}
}
