using System;
using System.Collections.Generic;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C9C RID: 3228
public class StandardAttributeFormatter : IAttributeFormatter
{
	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x060062DC RID: 25308 RVA: 0x0024AB39 File Offset: 0x00248D39
	// (set) Token: 0x060062DD RID: 25309 RVA: 0x0024AB41 File Offset: 0x00248D41
	public GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x060062DE RID: 25310 RVA: 0x0024AB4A File Offset: 0x00248D4A
	public StandardAttributeFormatter(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice)
	{
		this.unitClass = unitClass;
		this.DeltaTimeSlice = deltaTimeSlice;
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x0024AB60 File Offset: 0x00248D60
	public virtual string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x0024AB70 File Offset: 0x00248D70
	public virtual string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, (modifier.OverrideTimeSlice != null) ? modifier.OverrideTimeSlice.Value : this.DeltaTimeSlice);
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x0024ABB0 File Offset: 0x00248DB0
	public virtual string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		switch (this.unitClass)
		{
		case GameUtil.UnitClass.SimpleInteger:
			return GameUtil.GetFormattedInt(value, timeSlice);
		case GameUtil.UnitClass.Temperature:
			return GameUtil.GetFormattedTemperature(value, timeSlice, (timeSlice == GameUtil.TimeSlice.None) ? GameUtil.TemperatureInterpretation.Absolute : GameUtil.TemperatureInterpretation.Relative, true, false);
		case GameUtil.UnitClass.Mass:
			return GameUtil.GetFormattedMass(value, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.UnitClass.Calories:
			return GameUtil.GetFormattedCalories(value, timeSlice, true);
		case GameUtil.UnitClass.Percent:
			return GameUtil.GetFormattedPercent(value, timeSlice);
		case GameUtil.UnitClass.Distance:
			return GameUtil.GetFormattedDistance(value);
		case GameUtil.UnitClass.Disease:
			return GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(value), GameUtil.TimeSlice.None);
		case GameUtil.UnitClass.Radiation:
			return GameUtil.GetFormattedRads(value, timeSlice);
		case GameUtil.UnitClass.Energy:
			return GameUtil.GetFormattedJoules(value, "F1", timeSlice);
		case GameUtil.UnitClass.Power:
			return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
		case GameUtil.UnitClass.Lux:
			return GameUtil.GetFormattedLux(Mathf.FloorToInt(value));
		case GameUtil.UnitClass.Time:
			return GameUtil.GetFormattedCycles(value, "F1", false);
		case GameUtil.UnitClass.Seconds:
			return GameUtil.GetFormattedTime(value, "F0");
		case GameUtil.UnitClass.Cycles:
			return GameUtil.GetFormattedCycles(value * 600f, "F1", false);
		}
		return GameUtil.GetFormattedSimple(value, timeSlice, null);
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x0024ACB6 File Offset: 0x00248EB6
	public virtual string GetTooltipDescription(Klei.AI.Attribute master)
	{
		return master.Description;
	}

	// Token: 0x060062E3 RID: 25315 RVA: 0x0024ACC0 File Offset: 0x00248EC0
	public virtual string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		for (int i = 0; i < instance.Modifiers.Count; i++)
		{
			list.Add(instance.Modifiers[i]);
		}
		return this.GetTooltip(master, list, instance.GetComponent<AttributeConverters>());
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x0024AD0C File Offset: 0x00248F0C
	public string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Append(this.GetTooltipDescription(master));
		stringBuilder.AppendFormat(DUPLICANTS.ATTRIBUTES.TOTAL_VALUE, this.GetFormattedValue(AttributeInstance.GetTotalDisplayValue(master, modifiers), GameUtil.TimeSlice.None), master.Name);
		if (master.BaseValue != 0f)
		{
			stringBuilder.AppendFormat(DUPLICANTS.ATTRIBUTES.BASE_VALUE, master.BaseValue);
		}
		List<AttributeModifier> list = new List<AttributeModifier>(modifiers);
		list.Sort((AttributeModifier p1, AttributeModifier p2) => p2.Value.CompareTo(p1.Value));
		for (int num = 0; num != list.Count; num++)
		{
			AttributeModifier attributeModifier = list[num];
			string formattedString = attributeModifier.GetFormattedString();
			if (formattedString != null)
			{
				stringBuilder.AppendFormat(DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, attributeModifier.GetDescription(), formattedString);
			}
		}
		bool flag = true;
		if (converters != null && master.converters.Count > 0)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in converters.converters)
			{
				if (attributeConverterInstance.converter.attribute == master)
				{
					string text = attributeConverterInstance.DescriptionFromAttribute(attributeConverterInstance.Evaluate(), attributeConverterInstance.gameObject);
					if (text != null)
					{
						if (flag)
						{
							stringBuilder.AppendLine();
							flag = false;
						}
						stringBuilder.AppendLine();
						stringBuilder.Append(text);
					}
				}
			}
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x04004306 RID: 17158
	public GameUtil.UnitClass unitClass;
}
