using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F1A RID: 3866
	public class Amounts : ResourceSet<Amount>
	{
		// Token: 0x06007BFF RID: 31743 RVA: 0x00301888 File Offset: 0x002FFA88
		public void Load()
		{
			this.Stamina = this.CreateAmount("Stamina", 0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.DUPLICANTS", "ui_icon_stamina", "attribute_stamina", "mod_stamina");
			this.Stamina.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Calories = this.CreateAmount("Calories", 0f, 0f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_calories", "attribute_calories", "mod_calories");
			this.Calories.SetDisplayer(new CaloriesDisplayer());
			this.Breath = this.CreateAmount("Breath", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_breath", null, "mod_breath");
			this.Breath.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Stress = this.CreateAmount("Stress", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_stress", "attribute_stress", "mod_stress");
			this.Stress.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Toxicity = this.CreateAmount("Toxicity", 0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", null, null, null);
			this.Toxicity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Bladder = this.CreateAmount("Bladder", 0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_bladder", null, "mod_bladder");
			this.Bladder.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Decor = this.CreateAmount("Decor", -1000f, 1000f, false, Units.Flat, 0.016666668f, true, "STRINGS.DUPLICANTS", "ui_icon_decor", null, "mod_decor");
			this.Decor.SetDisplayer(new DecorDisplayer());
			this.RadiationBalance = this.CreateAmount("RadiationBalance", 0f, 10000f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_radiation", null, "mod_health");
			this.RadiationBalance.SetDisplayer(new RadiationBalanceDisplayer());
			this.Temperature = this.CreateAmount("Temperature", 0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_temperature", null, null);
			this.Temperature.SetDisplayer(new DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
			this.CritterTemperature = this.CreateAmount("CritterTemperature", 0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.CREATURES", "ui_icon_temperature", null, null);
			this.CritterTemperature.SetDisplayer(new CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
			this.HitPoints = this.CreateAmount("HitPoints", 0f, 0f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_hitpoints", "attribute_hitpoints", "mod_health");
			this.HitPoints.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Possessive));
			this.AirPressure = this.CreateAmount("AirPressure", 0f, 1E+09f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.AirPressure.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Maturity = this.CreateAmount("Maturity", 0f, 0f, true, Units.Flat, 0.0009166667f, true, "STRINGS.CREATURES", "ui_icon_maturity", null, null);
			this.Maturity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Cycles, GameUtil.TimeSlice.None, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Maturity2 = this.CreateAmount("Maturity2", 0f, 0f, true, Units.Flat, 0.0009166667f, true, false, "STRINGS.CREATURES", "ui_icon_maturity", null, null);
			this.Maturity2.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Cycles, GameUtil.TimeSlice.None, null, GameUtil.IdentityDescriptorTense.Normal));
			this.OldAge = this.CreateAmount("OldAge", 0f, 0f, false, Units.Flat, 0f, false, "STRINGS.CREATURES", null, null, null);
			this.Fertilization = this.CreateAmount("Fertilization", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.Fertilization.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Fertility = this.CreateAmount("Fertility", 0f, 100f, true, Units.Flat, 0.008375f, true, "STRINGS.CREATURES", "ui_icon_fertility", null, null);
			this.Fertility.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Wildness = this.CreateAmount("Wildness", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_wildness", null, null);
			this.Wildness.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Incubation = this.CreateAmount("Incubation", 0f, 100f, true, Units.Flat, 0.01675f, true, "STRINGS.CREATURES", "ui_icon_incubation", null, null);
			this.Incubation.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Viability = this.CreateAmount("Viability", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_viability", null, null);
			this.Viability.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.PowerCharge = this.CreateAmount("PowerCharge", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.PowerCharge.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Age = this.CreateAmount("Age", 0f, 0f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_age", null, null);
			this.Age.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.Irrigation = this.CreateAmount("Irrigation", 0f, 1f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", null, null, null);
			this.Irrigation.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0f, DUPLICANTSTATS.STANDARD.BaseStats.IMMUNE_LEVEL_MAX, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_immunelevel", "attribute_immunelevel", null);
			this.ImmuneLevel.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Rot = this.CreateAmount("Rot", 0f, 0f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.Rot.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Illumination = this.CreateAmount("Illumination", 0f, 1f, false, Units.Flat, 0f, true, "STRINGS.CREATURES", null, null, null);
			this.Illumination.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None, null, GameUtil.IdentityDescriptorTense.Normal));
			this.ScaleGrowth = this.CreateAmount("ScaleGrowth", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth", null, null);
			this.ScaleGrowth.SetDisplayer(new ScaleGrowthDisplayer(GameUtil.TimeSlice.PerCycle));
			this.MilkProduction = this.CreateAmount("MilkProduction", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_milk_production", null, null);
			this.MilkProduction.SetDisplayer(new MilkProductionDisplayer(GameUtil.TimeSlice.PerCycle));
			this.ElementGrowth = this.CreateAmount("ElementGrowth", 0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth", null, null);
			this.ElementGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.Beckoning = this.CreateAmount("Beckoning", 0f, 100f, true, Units.Flat, 100.5f, true, "STRINGS.CREATURES", "ui_icon_moo", null, null);
			this.Beckoning.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
			this.BionicOxygenTank = this.CreateAmount("BionicOxygenTank", 0f, BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG, true, Units.Flat, 60f, true, "STRINGS.DUPLICANTS", "ui_icon_oxygentank", null, null);
			this.BionicOxygenTank.SetDisplayer(new BionicOxygenTankDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
			this.BionicOxygenTank.debugSetValue = delegate(AmountInstance instance, float val)
			{
				BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
				if (smi == null)
				{
					instance.SetValue(val);
					return;
				}
				float availableOxygen = smi.AvailableOxygen;
				if (val >= availableOxygen)
				{
					float mass = val - availableOxygen;
					smi.AddGas(SimHashes.Oxygen, mass, 6282.4497f, byte.MaxValue, 0);
					return;
				}
				float amount = Mathf.Min(availableOxygen - val, availableOxygen);
				float num;
				SimUtil.DiseaseInfo diseaseInfo;
				float num2;
				smi.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out num2);
			};
			this.BionicInternalBattery = this.CreateAmount("BionicInternalBattery", 0f, 480000f, true, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_battery", null, null);
			this.BionicInternalBattery.SetDisplayer(new BionicBatteryDisplayer());
			this.BionicInternalBattery.debugSetValue = delegate(AmountInstance instance, float val)
			{
				BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
				if (smi == null)
				{
					instance.SetValue(val);
					return;
				}
				float currentCharge = smi.CurrentCharge;
				if (val >= currentCharge)
				{
					float joules = val - currentCharge;
					smi.DebugAddCharge(joules);
					return;
				}
				float joules2 = currentCharge - val;
				smi.ConsumePower(joules2);
			};
			this.BionicOil = this.CreateAmount("BionicOil", 0f, 200f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_liquid", null, null);
			this.BionicOil.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal));
			this.BionicGunk = this.CreateAmount("BionicGunk", 0f, GunkMonitor.GUNK_CAPACITY, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_gunk", null, null);
			this.BionicGunk.SetDisplayer(new BionicGunkDisplayer(GameUtil.TimeSlice.PerCycle));
			this.InternalBattery = this.CreateAmount("InternalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalChemicalBattery = this.CreateAmount("InternalChemicalBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalChemicalBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalBioBattery = this.CreateAmount("InternalBioBattery", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalBioBattery.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
			this.InternalElectroBank = this.CreateAmount("InternalElectroBank", 0f, 0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery", null, null);
			this.InternalElectroBank.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal));
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x003022B0 File Offset: 0x003004B0
		public Amount CreateAmount(string id, float min, float max, bool show_max, Units units, float delta_threshold, bool show_in_ui, string string_root, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null)
		{
			return this.CreateAmount(id, min, max, show_max, units, delta_threshold, show_in_ui, true, string_root, uiSprite, thoughtSprite, uiFullColourSprite);
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x003022D8 File Offset: 0x003004D8
		public Amount CreateAmount(string id, float min, float max, bool show_max, Units units, float delta_threshold, bool show_in_ui, bool show_delta_in_ui, string string_root, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null)
		{
			string text = Strings.Get(string.Format("{1}.STATS.{0}.NAME", id.ToUpper(), string_root.ToUpper()));
			string description = Strings.Get(string.Format("{1}.STATS.{0}.TOOLTIP", id.ToUpper(), string_root.ToUpper()));
			Klei.AI.Attribute.Display show_in_ui2 = show_in_ui ? Klei.AI.Attribute.Display.Normal : Klei.AI.Attribute.Display.Never;
			Klei.AI.Attribute.Display show_in_ui3 = show_delta_in_ui ? Klei.AI.Attribute.Display.Normal : Klei.AI.Attribute.Display.Never;
			string text2 = id + "Min";
			StringEntry stringEntry;
			string name = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", text2.ToUpper(), string_root)), out stringEntry) ? stringEntry.String : ("Minimum" + text);
			StringEntry stringEntry2;
			string attribute_description = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", text2.ToUpper(), string_root)), out stringEntry2) ? stringEntry2.String : ("Minimum" + text);
			Klei.AI.Attribute attribute = new Klei.AI.Attribute(id + "Min", name, "", attribute_description, min, show_in_ui2, false, null, null, uiFullColourSprite);
			string text3 = id + "Max";
			StringEntry stringEntry3;
			string name2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", text3.ToUpper(), string_root)), out stringEntry3) ? stringEntry3.String : ("Maximum" + text);
			StringEntry stringEntry4;
			string attribute_description2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", text3.ToUpper(), string_root)), out stringEntry4) ? stringEntry4.String : ("Maximum" + text);
			Klei.AI.Attribute attribute2 = new Klei.AI.Attribute(id + "Max", name2, "", attribute_description2, max, show_in_ui2, false, null, null, uiFullColourSprite);
			string text4 = id + "Delta";
			string name3 = Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.NAME", text4.ToUpper(), string_root));
			string attribute_description3 = Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.DESC", text4.ToUpper(), string_root));
			Klei.AI.Attribute attribute3 = new Klei.AI.Attribute(text4, name3, "", attribute_description3, 0f, show_in_ui3, false, null, null, uiFullColourSprite);
			Amount amount = new Amount(id, text, description, attribute, attribute2, attribute3, show_max, units, delta_threshold, show_in_ui, uiSprite, thoughtSprite);
			Db.Get().Attributes.Add(attribute);
			Db.Get().Attributes.Add(attribute2);
			Db.Get().Attributes.Add(attribute3);
			base.Add(amount);
			return amount;
		}

		// Token: 0x04005661 RID: 22113
		public Amount Stamina;

		// Token: 0x04005662 RID: 22114
		public Amount Calories;

		// Token: 0x04005663 RID: 22115
		public Amount ImmuneLevel;

		// Token: 0x04005664 RID: 22116
		public Amount Breath;

		// Token: 0x04005665 RID: 22117
		public Amount Stress;

		// Token: 0x04005666 RID: 22118
		public Amount Toxicity;

		// Token: 0x04005667 RID: 22119
		public Amount Bladder;

		// Token: 0x04005668 RID: 22120
		public Amount Decor;

		// Token: 0x04005669 RID: 22121
		public Amount RadiationBalance;

		// Token: 0x0400566A RID: 22122
		public Amount BionicOxygenTank;

		// Token: 0x0400566B RID: 22123
		public Amount BionicOil;

		// Token: 0x0400566C RID: 22124
		public Amount BionicGunk;

		// Token: 0x0400566D RID: 22125
		public Amount BionicInternalBattery;

		// Token: 0x0400566E RID: 22126
		public Amount Temperature;

		// Token: 0x0400566F RID: 22127
		public Amount CritterTemperature;

		// Token: 0x04005670 RID: 22128
		public Amount HitPoints;

		// Token: 0x04005671 RID: 22129
		public Amount AirPressure;

		// Token: 0x04005672 RID: 22130
		public Amount Maturity;

		// Token: 0x04005673 RID: 22131
		public Amount Maturity2;

		// Token: 0x04005674 RID: 22132
		public Amount OldAge;

		// Token: 0x04005675 RID: 22133
		public Amount Age;

		// Token: 0x04005676 RID: 22134
		public Amount Fertilization;

		// Token: 0x04005677 RID: 22135
		public Amount Illumination;

		// Token: 0x04005678 RID: 22136
		public Amount Irrigation;

		// Token: 0x04005679 RID: 22137
		public Amount Fertility;

		// Token: 0x0400567A RID: 22138
		public Amount Viability;

		// Token: 0x0400567B RID: 22139
		public Amount PowerCharge;

		// Token: 0x0400567C RID: 22140
		public Amount Wildness;

		// Token: 0x0400567D RID: 22141
		public Amount Incubation;

		// Token: 0x0400567E RID: 22142
		public Amount ScaleGrowth;

		// Token: 0x0400567F RID: 22143
		public Amount ElementGrowth;

		// Token: 0x04005680 RID: 22144
		public Amount Beckoning;

		// Token: 0x04005681 RID: 22145
		public Amount MilkProduction;

		// Token: 0x04005682 RID: 22146
		public Amount InternalBattery;

		// Token: 0x04005683 RID: 22147
		public Amount InternalChemicalBattery;

		// Token: 0x04005684 RID: 22148
		public Amount InternalBioBattery;

		// Token: 0x04005685 RID: 22149
		public Amount InternalElectroBank;

		// Token: 0x04005686 RID: 22150
		public Amount Rot;
	}
}
