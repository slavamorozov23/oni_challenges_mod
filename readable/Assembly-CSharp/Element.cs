using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000912 RID: 2322
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
	// Token: 0x06004086 RID: 16518 RVA: 0x0016D6F4 File Offset: 0x0016B8F4
	public float GetRelativeHeatLevel(float currentTemperature)
	{
		float num = this.lowTemp - 3f;
		float num2 = this.highTemp + 3f;
		return Mathf.Clamp01((currentTemperature - num) / (num2 - num));
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x0016D727 File Offset: 0x0016B927
	public float PressureToMass(float pressure)
	{
		return pressure / this.defaultValues.pressure;
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06004088 RID: 16520 RVA: 0x0016D736 File Offset: 0x0016B936
	public bool IsSlippery
	{
		get
		{
			return this.HasTag(GameTags.Slippery);
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06004089 RID: 16521 RVA: 0x0016D743 File Offset: 0x0016B943
	public bool IsUnstable
	{
		get
		{
			return this.HasTag(GameTags.Unstable);
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x0600408A RID: 16522 RVA: 0x0016D750 File Offset: 0x0016B950
	public bool IsLiquid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Liquid;
		}
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x0600408B RID: 16523 RVA: 0x0016D75D File Offset: 0x0016B95D
	public bool IsGas
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Gas;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x0600408C RID: 16524 RVA: 0x0016D76A File Offset: 0x0016B96A
	public bool IsSolid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Solid;
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x0600408D RID: 16525 RVA: 0x0016D777 File Offset: 0x0016B977
	public bool IsVacuum
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Vacuum;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x0600408E RID: 16526 RVA: 0x0016D784 File Offset: 0x0016B984
	public bool IsTemperatureInsulated
	{
		get
		{
			return (this.state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
		}
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x0016D792 File Offset: 0x0016B992
	public bool IsState(Element.State expected_state)
	{
		return (this.state & Element.State.Solid) == expected_state;
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06004090 RID: 16528 RVA: 0x0016D79F File Offset: 0x0016B99F
	public bool HasTransitionUp
	{
		get
		{
			return this.highTempTransitionTarget != (SimHashes)0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06004091 RID: 16529 RVA: 0x0016D7CC File Offset: 0x0016B9CC
	// (set) Token: 0x06004092 RID: 16530 RVA: 0x0016D7D4 File Offset: 0x0016B9D4
	public string name { get; set; }

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06004093 RID: 16531 RVA: 0x0016D7DD File Offset: 0x0016B9DD
	// (set) Token: 0x06004094 RID: 16532 RVA: 0x0016D7E5 File Offset: 0x0016B9E5
	public string nameUpperCase { get; set; }

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06004095 RID: 16533 RVA: 0x0016D7EE File Offset: 0x0016B9EE
	// (set) Token: 0x06004096 RID: 16534 RVA: 0x0016D7F6 File Offset: 0x0016B9F6
	public string description { get; set; }

	// Token: 0x06004097 RID: 16535 RVA: 0x0016D7FF File Offset: 0x0016B9FF
	public string GetStateString()
	{
		return Element.GetStateString(this.state);
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x0016D80C File Offset: 0x0016BA0C
	public static string GetStateString(Element.State state)
	{
		if ((state & Element.State.Solid) == Element.State.Solid)
		{
			return ELEMENTS.STATE.SOLID;
		}
		if ((state & Element.State.Solid) == Element.State.Liquid)
		{
			return ELEMENTS.STATE.LIQUID;
		}
		if ((state & Element.State.Solid) == Element.State.Gas)
		{
			return ELEMENTS.STATE.GAS;
		}
		return ELEMENTS.STATE.VACUUM;
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x0016D84C File Offset: 0x0016BA4C
	public string FullDescription(bool addHardnessColor = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Clear();
		stringBuilder.Append(this.Description());
		if (this.IsSolid)
		{
			stringBuilder.Append("\n\n");
			stringBuilder.AppendFormat(ELEMENTS.ELEMENTDESCSOLID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetHardnessString(this, addHardnessColor));
		}
		else if (this.IsLiquid)
		{
			stringBuilder.Append("\n\n");
			stringBuilder.AppendFormat(ELEMENTS.ELEMENTDESCLIQUID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		else if (!this.IsVacuum)
		{
			stringBuilder.Append("\n\n");
			stringBuilder.AppendFormat(ELEMENTS.ELEMENTDESCGAS, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		StringBuilder stringBuilder2 = GlobalStringBuilderPool.Alloc();
		stringBuilder2.Append(ELEMENTS.THERMALPROPERTIES);
		stringBuilder2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
		stringBuilder2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
		stringBuilder.Append("\n");
		stringBuilder.Append(stringBuilder2.ToString());
		GlobalStringBuilderPool.Free(stringBuilder2);
		if (DlcManager.FeatureRadiationEnabled())
		{
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(ELEMENTS.RADIATIONPROPERTIES, this.radiationAbsorptionFactor, GameUtil.GetFormattedRads(this.radiationPer1000Mass * 1.1f / 600f, GameUtil.TimeSlice.PerCycle));
		}
		if (this.oreTags.Length != 0 && !this.IsVacuum)
		{
			stringBuilder.Append("\n\n");
			StringBuilder stringBuilder3 = GlobalStringBuilderPool.Alloc();
			for (int i = 0; i < this.oreTags.Length; i++)
			{
				Tag item = new Tag(this.oreTags[i]);
				if (!GameTags.HiddenElementTags.Contains(item))
				{
					stringBuilder3.Append(item.ProperName());
					if (i < this.oreTags.Length - 1)
					{
						stringBuilder3.Append(", ");
					}
				}
			}
			stringBuilder.AppendFormat(ELEMENTS.ELEMENTPROPERTIES, GlobalStringBuilderPool.ReturnAndFree(stringBuilder3));
		}
		if (this.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name, attributeModifier.GetFormattedString());
			}
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x0016DB24 File Offset: 0x0016BD24
	public string Description()
	{
		return this.description;
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x0016DB2C File Offset: 0x0016BD2C
	public bool HasTag(Tag search_tag)
	{
		return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x0016DB50 File Offset: 0x0016BD50
	public Tag GetMaterialCategoryTag()
	{
		return this.materialCategory;
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x0016DB58 File Offset: 0x0016BD58
	public int CompareTo(Element other)
	{
		return this.id - other.id;
	}

	// Token: 0x0400282F RID: 10287
	public const int INVALID_ID = 0;

	// Token: 0x04002830 RID: 10288
	public SimHashes id;

	// Token: 0x04002831 RID: 10289
	public Tag tag;

	// Token: 0x04002832 RID: 10290
	public ushort idx;

	// Token: 0x04002833 RID: 10291
	public float specificHeatCapacity;

	// Token: 0x04002834 RID: 10292
	public float thermalConductivity = 1f;

	// Token: 0x04002835 RID: 10293
	public float molarMass = 1f;

	// Token: 0x04002836 RID: 10294
	public float strength;

	// Token: 0x04002837 RID: 10295
	public float flow;

	// Token: 0x04002838 RID: 10296
	public float maxCompression;

	// Token: 0x04002839 RID: 10297
	public float viscosity;

	// Token: 0x0400283A RID: 10298
	public float minHorizontalFlow = float.PositiveInfinity;

	// Token: 0x0400283B RID: 10299
	public float minVerticalFlow = float.PositiveInfinity;

	// Token: 0x0400283C RID: 10300
	public float maxMass = 10000f;

	// Token: 0x0400283D RID: 10301
	public float solidSurfaceAreaMultiplier;

	// Token: 0x0400283E RID: 10302
	public float liquidSurfaceAreaMultiplier;

	// Token: 0x0400283F RID: 10303
	public float gasSurfaceAreaMultiplier;

	// Token: 0x04002840 RID: 10304
	public Element.State state;

	// Token: 0x04002841 RID: 10305
	public byte hardness;

	// Token: 0x04002842 RID: 10306
	public float lowTemp;

	// Token: 0x04002843 RID: 10307
	public SimHashes lowTempTransitionTarget;

	// Token: 0x04002844 RID: 10308
	public Element lowTempTransition;

	// Token: 0x04002845 RID: 10309
	public float highTemp;

	// Token: 0x04002846 RID: 10310
	public SimHashes highTempTransitionTarget;

	// Token: 0x04002847 RID: 10311
	public Element highTempTransition;

	// Token: 0x04002848 RID: 10312
	public SimHashes highTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x04002849 RID: 10313
	public float highTempTransitionOreMassConversion;

	// Token: 0x0400284A RID: 10314
	public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x0400284B RID: 10315
	public float lowTempTransitionOreMassConversion;

	// Token: 0x0400284C RID: 10316
	public SimHashes sublimateId;

	// Token: 0x0400284D RID: 10317
	public SimHashes convertId;

	// Token: 0x0400284E RID: 10318
	public SpawnFXHashes sublimateFX;

	// Token: 0x0400284F RID: 10319
	public float sublimateRate;

	// Token: 0x04002850 RID: 10320
	public float sublimateEfficiency;

	// Token: 0x04002851 RID: 10321
	public float sublimateProbability;

	// Token: 0x04002852 RID: 10322
	public float offGasPercentage;

	// Token: 0x04002853 RID: 10323
	public float lightAbsorptionFactor;

	// Token: 0x04002854 RID: 10324
	public float radiationAbsorptionFactor;

	// Token: 0x04002855 RID: 10325
	public float radiationPer1000Mass;

	// Token: 0x04002856 RID: 10326
	public Sim.PhysicsData defaultValues;

	// Token: 0x04002857 RID: 10327
	public SimHashes refinedMetalTarget;

	// Token: 0x04002858 RID: 10328
	public float toxicity;

	// Token: 0x04002859 RID: 10329
	public Substance substance;

	// Token: 0x0400285A RID: 10330
	public Tag materialCategory;

	// Token: 0x0400285B RID: 10331
	public int buildMenuSort;

	// Token: 0x0400285C RID: 10332
	public ElementLoader.ElementComposition[] elementComposition;

	// Token: 0x0400285D RID: 10333
	public Tag[] oreTags = new Tag[0];

	// Token: 0x0400285E RID: 10334
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x0400285F RID: 10335
	public bool disabled;

	// Token: 0x04002860 RID: 10336
	public string dlcId;

	// Token: 0x04002861 RID: 10337
	public const byte StateMask = 3;

	// Token: 0x02001910 RID: 6416
	[Serializable]
	public enum State : byte
	{
		// Token: 0x04007CCB RID: 31947
		Vacuum,
		// Token: 0x04007CCC RID: 31948
		Gas,
		// Token: 0x04007CCD RID: 31949
		Liquid,
		// Token: 0x04007CCE RID: 31950
		Solid,
		// Token: 0x04007CCF RID: 31951
		Unbreakable,
		// Token: 0x04007CD0 RID: 31952
		Unstable = 8,
		// Token: 0x04007CD1 RID: 31953
		TemperatureInsulated = 16
	}
}
