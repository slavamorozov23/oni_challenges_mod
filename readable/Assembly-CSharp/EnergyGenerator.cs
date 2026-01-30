using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200091E RID: 2334
[SerializationConfig(MemberSerialization.OptIn)]
public class EnergyGenerator : Generator, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06004139 RID: 16697 RVA: 0x0017092B File Offset: 0x0016EB2B
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x0600413A RID: 16698 RVA: 0x00170932 File Offset: 0x0016EB32
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x0017093E File Offset: 0x0016EB3E
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x0600413C RID: 16700 RVA: 0x00170941 File Offset: 0x0016EB41
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x00170948 File Offset: 0x0016EB48
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x0017094F File Offset: 0x0016EB4F
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x0017095D File Offset: 0x0016EB5D
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x0017096C File Offset: 0x0016EB6C
	string ISliderControl.GetSliderTooltip(int index)
	{
		ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), component.RequestedItemTag.ProperName(), this.batteryRefillPercent * 100f);
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x001709B0 File Offset: 0x0016EBB0
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x001709B8 File Offset: 0x0016EBB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EnergyGenerator.EnsureStatusItemAvailable();
		base.Subscribe<EnergyGenerator>(824508782, EnergyGenerator.OnActiveChangedDelegate);
		if (!this.ignoreBatteryRefillPercent)
		{
			base.gameObject.AddOrGet<CopyBuildingSettings>();
			base.Subscribe<EnergyGenerator>(-905833192, EnergyGenerator.OnCopySettingsDelegate);
		}
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x00170A08 File Offset: 0x0016EC08
	private void OnCopySettings(object data)
	{
		EnergyGenerator component = ((GameObject)data).GetComponent<EnergyGenerator>();
		if (component != null)
		{
			this.batteryRefillPercent = component.batteryRefillPercent;
		}
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x00170A38 File Offset: 0x0016EC38
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x00170A90 File Offset: 0x0016EC90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasMeter)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
		}
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x00170AF4 File Offset: 0x0016ECF4
	private bool IsConvertible(float dt)
	{
		bool flag = true;
		foreach (EnergyGenerator.InputItem inputItem in this.formula.inputs)
		{
			float massAvailable = this.storage.GetMassAvailable(inputItem.tag);
			float num = inputItem.consumptionRate * dt;
			flag = (flag && massAvailable >= num);
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x06004147 RID: 16711 RVA: 0x00170B58 File Offset: 0x0016ED58
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.hasMeter)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[0];
			float positionPercent = this.storage.GetMassAvailable(inputItem.tag) / inputItem.maxStoredMass;
			this.meter.SetPositionPercent(positionPercent);
		}
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		bool value = false;
		if (this.operational.IsOperational)
		{
			bool flag = false;
			List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
			if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
			{
				using (List<Battery>.Enumerator enumerator = batteriesOnCircuit.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Battery battery = enumerator.Current;
						if (this.batteryRefillPercent <= 0f && battery.PercentFull <= 0f)
						{
							flag = true;
							break;
						}
						if (battery.PercentFull < this.batteryRefillPercent)
						{
							flag = true;
							break;
						}
					}
					goto IL_105;
				}
			}
			flag = true;
			IL_105:
			if (!this.ignoreBatteryRefillPercent)
			{
				this.selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !flag, null);
			}
			if (this.delivery != null)
			{
				this.delivery.Pause(!flag, "Circuit has sufficient energy");
			}
			if (this.formula.inputs != null)
			{
				bool flag2 = this.IsConvertible(dt);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag2, this.formula);
				if (flag2)
				{
					foreach (EnergyGenerator.InputItem inputItem2 in this.formula.inputs)
					{
						float amount = inputItem2.consumptionRate * dt;
						this.storage.ConsumeIgnoringDisease(inputItem2.tag, amount);
					}
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					foreach (EnergyGenerator.OutputItem output in this.formula.outputs)
					{
						this.Emit(output, dt, component);
					}
					base.GenerateJoules(base.WattageRating * dt, false);
					this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
					value = true;
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x06004148 RID: 16712 RVA: 0x00170DD8 File Offset: 0x0016EFD8
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.inputs == null || this.formula.inputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.inputs.Length; i++)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[i];
			string arg = inputItem.tag.ProperName();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06004149 RID: 16713 RVA: 0x00170EA4 File Offset: 0x0016F0A4
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.outputs == null || this.formula.outputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.outputs.Length; i++)
		{
			EnergyGenerator.OutputItem outputItem = this.formula.outputs[i];
			string arg = ElementLoader.FindElementByHash(outputItem.element).tag.ProperName();
			Descriptor item = default(Descriptor);
			if (outputItem.minTemperature > 0f)
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
			}
			else
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600414A RID: 16714 RVA: 0x00170FF4 File Offset: 0x0016F1F4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x0600414B RID: 16715 RVA: 0x00171090 File Offset: 0x0016F290
	public static StatusItem BatteriesSufficientlyFull
	{
		get
		{
			return EnergyGenerator.batteriesSufficientlyFull;
		}
	}

	// Token: 0x0600414C RID: 16716 RVA: 0x00171098 File Offset: 0x0016F298
	public static void EnsureStatusItemAvailable()
	{
		if (EnergyGenerator.batteriesSufficientlyFull == null)
		{
			EnergyGenerator.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x0600414D RID: 16717 RVA: 0x001710D4 File Offset: 0x0016F2D4
	public static EnergyGenerator.Formula CreateSimpleFormula(Tag input_element, float input_mass_rate, float max_stored_input_mass, SimHashes output_element = SimHashes.Void, float output_mass_rate = 0f, bool store_output_mass = true, CellOffset output_offset = default(CellOffset), float min_output_temperature = 0f)
	{
		EnergyGenerator.Formula result = default(EnergyGenerator.Formula);
		result.inputs = new EnergyGenerator.InputItem[]
		{
			new EnergyGenerator.InputItem(input_element, input_mass_rate, max_stored_input_mass)
		};
		if (output_element != SimHashes.Void)
		{
			result.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(output_element, output_mass_rate, store_output_mass, output_offset, min_output_temperature)
			};
		}
		else
		{
			result.outputs = null;
		}
		return result;
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x0017113C File Offset: 0x0016F33C
	private void Emit(EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
	{
		Element element = ElementLoader.FindElementByHash(output.element);
		float num = output.creationRate * dt;
		if (output.store)
		{
			if (element.IsGas)
			{
				this.storage.AddGasChunk(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			if (element.IsLiquid)
			{
				this.storage.AddLiquid(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num, root_pe.Temperature, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, true, false, true, false);
			return;
		}
		else
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), output.emitOffset);
			float temperature = Mathf.Max(root_pe.Temperature, output.minTemperature);
			if (element.IsGas)
			{
				SimMessages.ModifyMass(num2, num, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temperature, output.element);
				return;
			}
			if (element.IsLiquid)
			{
				ushort elementIndex = ElementLoader.GetElementIndex(output.element);
				FallingWater.instance.AddParticle(num2, elementIndex, num, temperature, byte.MaxValue, 0, true, false, false, false);
				return;
			}
			element.substance.SpawnResource(Grid.CellToPosCCC(num2, Grid.SceneLayer.Front), num, temperature, byte.MaxValue, 0, true, false, false);
			return;
		}
	}

	// Token: 0x040028C2 RID: 10434
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040028C3 RID: 10435
	[MyCmpGet]
	private ManualDeliveryKG delivery;

	// Token: 0x040028C4 RID: 10436
	[SerializeField]
	[Serialize]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x040028C5 RID: 10437
	public bool ignoreBatteryRefillPercent;

	// Token: 0x040028C6 RID: 10438
	public bool hasMeter = true;

	// Token: 0x040028C7 RID: 10439
	private static StatusItem batteriesSufficientlyFull;

	// Token: 0x040028C8 RID: 10440
	public Meter.Offset meterOffset;

	// Token: 0x040028C9 RID: 10441
	[SerializeField]
	public EnergyGenerator.Formula formula;

	// Token: 0x040028CA RID: 10442
	private MeterController meter;

	// Token: 0x040028CB RID: 10443
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040028CC RID: 10444
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200191C RID: 6428
	[DebuggerDisplay("{tag} -{consumptionRate} kg/s")]
	[Serializable]
	public struct InputItem
	{
		// Token: 0x0600A172 RID: 41330 RVA: 0x003ABDE4 File Offset: 0x003A9FE4
		public InputItem(Tag tag, float consumption_rate, float max_stored_mass)
		{
			this.tag = tag;
			this.consumptionRate = consumption_rate;
			this.maxStoredMass = max_stored_mass;
		}

		// Token: 0x04007CF2 RID: 31986
		public Tag tag;

		// Token: 0x04007CF3 RID: 31987
		public float consumptionRate;

		// Token: 0x04007CF4 RID: 31988
		public float maxStoredMass;
	}

	// Token: 0x0200191D RID: 6429
	[DebuggerDisplay("{element} {creationRate} kg/s")]
	[Serializable]
	public struct OutputItem
	{
		// Token: 0x0600A173 RID: 41331 RVA: 0x003ABDFB File Offset: 0x003A9FFB
		public OutputItem(SimHashes element, float creation_rate, bool store, float min_temperature = 0f)
		{
			this = new EnergyGenerator.OutputItem(element, creation_rate, store, CellOffset.none, min_temperature);
		}

		// Token: 0x0600A174 RID: 41332 RVA: 0x003ABE0D File Offset: 0x003AA00D
		public OutputItem(SimHashes element, float creation_rate, bool store, CellOffset emit_offset, float min_temperature = 0f)
		{
			this.element = element;
			this.creationRate = creation_rate;
			this.store = store;
			this.emitOffset = emit_offset;
			this.minTemperature = min_temperature;
		}

		// Token: 0x04007CF5 RID: 31989
		public SimHashes element;

		// Token: 0x04007CF6 RID: 31990
		public float creationRate;

		// Token: 0x04007CF7 RID: 31991
		public bool store;

		// Token: 0x04007CF8 RID: 31992
		public CellOffset emitOffset;

		// Token: 0x04007CF9 RID: 31993
		public float minTemperature;
	}

	// Token: 0x0200191E RID: 6430
	[Serializable]
	public struct Formula
	{
		// Token: 0x04007CFA RID: 31994
		public EnergyGenerator.InputItem[] inputs;

		// Token: 0x04007CFB RID: 31995
		public EnergyGenerator.OutputItem[] outputs;

		// Token: 0x04007CFC RID: 31996
		public Tag meterTag;
	}
}
