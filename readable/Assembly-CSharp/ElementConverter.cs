using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000915 RID: 2325
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : StateMachineComponent<ElementConverter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060040C5 RID: 16581 RVA: 0x0016E723 File Offset: 0x0016C923
	public void SetWorkSpeedMultiplier(float speed)
	{
		this.workSpeedMultiplier = speed;
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x0016E72C File Offset: 0x0016C92C
	public void SetConsumedElementActive(Tag elementId, bool active)
	{
		int i = 0;
		while (i < this.consumedElements.Length)
		{
			if (!(this.consumedElements[i].Tag != elementId))
			{
				this.consumedElements[i].IsActive = active;
				if (!this.ShowInUI)
				{
					break;
				}
				ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, this.consumedElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<Tag>(consumedElement.Tag, this.consumedElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x0016E7C8 File Offset: 0x0016C9C8
	public void SetOutputElementActive(SimHashes element, bool active)
	{
		int i = 0;
		while (i < this.outputElements.Length)
		{
			if (this.outputElements[i].elementHash == element)
			{
				this.outputElements[i].IsActive = active;
				ElementConverter.OutputElement outputElement = this.outputElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, this.outputElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<SimHashes>(outputElement.elementHash, this.outputElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x0016E854 File Offset: 0x0016CA54
	public void SetStorage(Storage storage)
	{
		this.storage = storage;
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x060040C9 RID: 16585 RVA: 0x0016E85D File Offset: 0x0016CA5D
	// (set) Token: 0x060040CA RID: 16586 RVA: 0x0016E865 File Offset: 0x0016CA65
	public float OutputMultiplier
	{
		get
		{
			return this.outputMultiplier;
		}
		set
		{
			this.outputMultiplier = value;
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x060040CB RID: 16587 RVA: 0x0016E86E File Offset: 0x0016CA6E
	public float AverageConvertRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
		}
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x0016E890 File Offset: 0x0016CA90
	public bool HasEnoughMass(Tag tag, bool includeInactive = false)
	{
		bool result = false;
		List<GameObject> items = this.storage.items;
		foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
		{
			if (!(tag != consumedElement.Tag) && (includeInactive || consumedElement.IsActive))
			{
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				result = (num >= consumedElement.MassConsumptionRate);
				break;
			}
		}
		return result;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x0016E948 File Offset: 0x0016CB48
	public bool HasEnoughMassToStartConverting(bool includeInactive = false)
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = includeInactive || this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (includeInactive || consumedElement.IsActive)
			{
				float num2 = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						num2 += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num2 < consumedElement.MassConsumptionRate * num)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x0016EA30 File Offset: 0x0016CC30
	public bool CanConvertAtAll()
	{
		bool flag = this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				bool flag3 = false;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag) && gameObject.GetComponent<PrimaryElement>().Mass > 0f)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x0016EAEF File Offset: 0x0016CCEF
	private float GetSpeedMultiplier()
	{
		return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x0016EB04 File Offset: 0x0016CD04
	private void ConvertMass()
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = this.consumedElements.Length == 0;
		float num2 = 1f;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				float num3 = consumedElement.MassConsumptionRate * num * num2;
				if (num3 <= 0f)
				{
					num2 = 0f;
					break;
				}
				float num4 = 0f;
				for (int j = 0; j < this.storage.items.Count; j++)
				{
					GameObject gameObject = this.storage.items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
						float num5 = Mathf.Min(num3, component.Mass);
						num4 += num5 / num3;
					}
				}
				num2 = Mathf.Min(num2, num4);
			}
		}
		if (!flag || num2 <= 0f)
		{
			return;
		}
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		diseaseInfo.idx = byte.MaxValue;
		diseaseInfo.count = 0;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int k = 0; k < this.consumedElements.Length; k++)
		{
			ElementConverter.ConsumedElement consumedElement2 = this.consumedElements[k];
			if (consumedElement2.IsActive)
			{
				float num9 = consumedElement2.MassConsumptionRate * num * num2;
				Game.Instance.accumulators.Accumulate(consumedElement2.Accumulator, num9);
				for (int l = 0; l < this.storage.items.Count; l++)
				{
					GameObject gameObject2 = this.storage.items[l];
					if (!(gameObject2 == null))
					{
						if (gameObject2.HasTag(consumedElement2.Tag))
						{
							PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
							component2.KeepZeroMassObject = true;
							float num10 = Mathf.Min(num9, component2.Mass);
							int num11 = (int)(num10 / component2.Mass * (float)component2.DiseaseCount);
							float num12 = num10 * component2.Element.specificHeatCapacity;
							num8 += num12;
							num7 += num12 * component2.Temperature;
							component2.Mass -= num10;
							component2.ModifyDiseaseCount(-num11, "ElementConverter.ConvertMass");
							num6 += num10;
							diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component2.DiseaseIdx, num11);
							num9 -= num10;
							if (num9 <= 0f)
							{
								break;
							}
						}
						if (num9 <= 0f)
						{
							global::Debug.Assert(num9 <= 0f);
						}
					}
				}
			}
		}
		float num13 = (num8 > 0f) ? (num7 / num8) : 0f;
		if (this.onConvertMass != null && num6 > 0f)
		{
			this.onConvertMass(num6);
		}
		for (int m = 0; m < this.outputElements.Length; m++)
		{
			ElementConverter.OutputElement outputElement = this.outputElements[m];
			if (outputElement.IsActive)
			{
				SimUtil.DiseaseInfo diseaseInfo2 = diseaseInfo;
				if (this.totalDiseaseWeight <= 0f)
				{
					diseaseInfo2.idx = byte.MaxValue;
					diseaseInfo2.count = 0;
				}
				else
				{
					float num14 = outputElement.diseaseWeight / this.totalDiseaseWeight;
					diseaseInfo2.count = (int)((float)diseaseInfo2.count * num14);
				}
				if (outputElement.addedDiseaseIdx != 255)
				{
					diseaseInfo2 = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo2, new SimUtil.DiseaseInfo
					{
						idx = outputElement.addedDiseaseIdx,
						count = outputElement.addedDiseaseCount
					});
				}
				float num15 = outputElement.massGenerationRate * this.OutputMultiplier * num * num2;
				Game.Instance.accumulators.Accumulate(outputElement.accumulator, num15);
				float temperature;
				if (outputElement.useEntityTemperature || (num13 == 0f && outputElement.minOutputTemperature == 0f))
				{
					temperature = base.GetComponent<PrimaryElement>().Temperature;
				}
				else
				{
					temperature = Mathf.Max(outputElement.minOutputTemperature, num13);
				}
				Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
				if (outputElement.storeOutput)
				{
					PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num15, temperature);
					if (primaryElement == null)
					{
						if (element.IsGas)
						{
							this.storage.AddGasChunk(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else if (element.IsLiquid)
						{
							this.storage.AddLiquid(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else
						{
							GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false);
							this.storage.Store(go, true, false, true, false);
						}
					}
					else
					{
						primaryElement.AddDisease(diseaseInfo2.idx, diseaseInfo2.count, "ElementConverter.ConvertMass");
					}
				}
				else
				{
					Vector3 vector = new Vector3(base.transform.GetPosition().x + outputElement.outputElementOffset.x, base.transform.GetPosition().y + outputElement.outputElementOffset.y, 0f);
					int num16 = Grid.PosToCell(vector);
					if (element.IsLiquid)
					{
						FallingWater.instance.AddParticle(num16, element.idx, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false, false);
					}
					else if (element.IsSolid)
					{
						element.substance.SpawnResource(vector, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, false, false, false);
					}
					else
					{
						SimMessages.AddRemoveSubstance(num16, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, -1);
					}
				}
				if (outputElement.elementHash == SimHashes.Oxygen || outputElement.elementHash == SimHashes.ContaminatedOxygen)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num15, base.gameObject.GetProperName(), null);
				}
			}
		}
		this.storage.Trigger(-1697596308, base.gameObject);
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x0016F170 File Offset: 0x0016D370
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.machinerySpeedAttribute = attributes.Add(Db.Get().Attributes.MachinerySpeed);
		if (ElementConverter.ElementConverterInput == null)
		{
			ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement)data;
				str = str.Replace("{ElementTypes}", consumedElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.Tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
				return str;
			});
		}
		if (ElementConverter.ElementConverterOutput == null)
		{
			ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement)data;
				str = str.Replace("{ElementTypes}", outputElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			});
		}
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x0016F250 File Offset: 0x0016D450
	public void SetAllConsumedActive(bool active)
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].IsActive = active;
		}
		base.smi.sm.canConvert.Set(active, base.smi, false);
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x0016F2A0 File Offset: 0x0016D4A0
	public void SetConsumedActive(Tag id, bool active)
	{
		bool flag = this.consumedElements.Length == 0;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ref ElementConverter.ConsumedElement ptr = ref this.consumedElements[i];
			if (ptr.Tag == id)
			{
				ptr.IsActive = active;
				if (active)
				{
					flag = true;
					break;
				}
			}
			flag |= ptr.IsActive;
		}
		base.smi.sm.canConvert.Set(flag, base.smi, false);
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x0016F31C File Offset: 0x0016D51C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].Accumulator = Game.Instance.accumulators.Add("ElementsConsumed", this);
		}
		this.totalDiseaseWeight = 0f;
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			this.outputElements[j].accumulator = Game.Instance.accumulators.Add("OutputElements", this);
			this.totalDiseaseWeight += this.outputElements[j].diseaseWeight;
		}
		base.smi.StartSM();
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x0016F3D8 File Offset: 0x0016D5D8
	protected override void OnCleanUp()
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			Game.Instance.accumulators.Remove(this.consumedElements[i].Accumulator);
		}
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			Game.Instance.accumulators.Remove(this.outputElements[j].accumulator);
		}
		base.OnCleanUp();
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x0016F454 File Offset: 0x0016D654
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!this.showDescriptors)
		{
			return list;
		}
		if (this.consumedElements != null)
		{
			foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
					list.Add(item);
				}
			}
		}
		if (this.outputElements != null)
		{
			foreach (ElementConverter.OutputElement outputElement in this.outputElements)
			{
				if (outputElement.IsActive)
				{
					LocString loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
					LocString loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
					if (outputElement.useEntityTemperature)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
					}
					else if (outputElement.minOutputTemperature > 0f)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
					}
					Descriptor item2 = new Descriptor(string.Format(loc_string, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(loc_string2, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x04002880 RID: 10368
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002881 RID: 10369
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002882 RID: 10370
	public bool inputIsCategory;

	// Token: 0x04002883 RID: 10371
	public Action<float> onConvertMass;

	// Token: 0x04002884 RID: 10372
	private float totalDiseaseWeight = float.MaxValue;

	// Token: 0x04002885 RID: 10373
	public Operational.State OperationalRequirement = Operational.State.Active;

	// Token: 0x04002886 RID: 10374
	private AttributeInstance machinerySpeedAttribute;

	// Token: 0x04002887 RID: 10375
	private float workSpeedMultiplier = 1f;

	// Token: 0x04002888 RID: 10376
	public bool showDescriptors = true;

	// Token: 0x04002889 RID: 10377
	private const float BASE_INTERVAL = 1f;

	// Token: 0x0400288A RID: 10378
	public ElementConverter.ConsumedElement[] consumedElements;

	// Token: 0x0400288B RID: 10379
	public ElementConverter.OutputElement[] outputElements;

	// Token: 0x0400288C RID: 10380
	public bool ShowInUI = true;

	// Token: 0x0400288D RID: 10381
	private float outputMultiplier = 1f;

	// Token: 0x0400288E RID: 10382
	private Dictionary<Tag, Guid> consumedElementStatusHandles = new Dictionary<Tag, Guid>();

	// Token: 0x0400288F RID: 10383
	private Dictionary<SimHashes, Guid> outputElementStatusHandles = new Dictionary<SimHashes, Guid>();

	// Token: 0x04002890 RID: 10384
	private static StatusItem ElementConverterInput;

	// Token: 0x04002891 RID: 10385
	private static StatusItem ElementConverterOutput;

	// Token: 0x02001915 RID: 6421
	[DebuggerDisplay("{tag} {massConsumptionRate}")]
	[Serializable]
	public struct ConsumedElement
	{
		// Token: 0x0600A157 RID: 41303 RVA: 0x003AB7E1 File Offset: 0x003A99E1
		public ConsumedElement(Tag tag, float kgPerSecond, bool isActive = true)
		{
			this.Tag = tag;
			this.MassConsumptionRate = kgPerSecond;
			this.IsActive = isActive;
			this.Accumulator = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600A158 RID: 41304 RVA: 0x003AB803 File Offset: 0x003A9A03
		public string Name
		{
			get
			{
				return this.Tag.ProperName();
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600A159 RID: 41305 RVA: 0x003AB810 File Offset: 0x003A9A10
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.Accumulator);
			}
		}

		// Token: 0x04007CD9 RID: 31961
		public Tag Tag;

		// Token: 0x04007CDA RID: 31962
		public float MassConsumptionRate;

		// Token: 0x04007CDB RID: 31963
		public bool IsActive;

		// Token: 0x04007CDC RID: 31964
		public HandleVector<int>.Handle Accumulator;
	}

	// Token: 0x02001916 RID: 6422
	[Serializable]
	public struct OutputElement
	{
		// Token: 0x0600A15A RID: 41306 RVA: 0x003AB828 File Offset: 0x003A9A28
		public OutputElement(float kgPerSecond, SimHashes element, float minOutputTemperature, bool useEntityTemperature = false, bool storeOutput = false, float outputElementOffsetx = 0f, float outputElementOffsety = 0.5f, float diseaseWeight = 1f, byte addedDiseaseIdx = 255, int addedDiseaseCount = 0, bool isActive = true)
		{
			this.elementHash = element;
			this.minOutputTemperature = minOutputTemperature;
			this.useEntityTemperature = useEntityTemperature;
			this.storeOutput = storeOutput;
			this.massGenerationRate = kgPerSecond;
			this.outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
			this.accumulator = HandleVector<int>.InvalidHandle;
			this.diseaseWeight = diseaseWeight;
			this.addedDiseaseIdx = addedDiseaseIdx;
			this.addedDiseaseCount = addedDiseaseCount;
			this.IsActive = isActive;
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600A15B RID: 41307 RVA: 0x003AB894 File Offset: 0x003A9A94
		public string Name
		{
			get
			{
				return ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600A15C RID: 41308 RVA: 0x003AB8AB File Offset: 0x003A9AAB
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.accumulator);
			}
		}

		// Token: 0x04007CDD RID: 31965
		public bool IsActive;

		// Token: 0x04007CDE RID: 31966
		public SimHashes elementHash;

		// Token: 0x04007CDF RID: 31967
		public float minOutputTemperature;

		// Token: 0x04007CE0 RID: 31968
		public bool useEntityTemperature;

		// Token: 0x04007CE1 RID: 31969
		public float massGenerationRate;

		// Token: 0x04007CE2 RID: 31970
		public bool storeOutput;

		// Token: 0x04007CE3 RID: 31971
		public Vector2 outputElementOffset;

		// Token: 0x04007CE4 RID: 31972
		public HandleVector<int>.Handle accumulator;

		// Token: 0x04007CE5 RID: 31973
		public float diseaseWeight;

		// Token: 0x04007CE6 RID: 31974
		public byte addedDiseaseIdx;

		// Token: 0x04007CE7 RID: 31975
		public int addedDiseaseCount;
	}

	// Token: 0x02001917 RID: 6423
	public class StatesInstance : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
	{
		// Token: 0x0600A15D RID: 41309 RVA: 0x003AB8C2 File Offset: 0x003A9AC2
		public StatesInstance(ElementConverter smi) : base(smi)
		{
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x0600A15E RID: 41310 RVA: 0x003AB8D8 File Offset: 0x003A9AD8
		public void AddStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			foreach (ElementConverter.ConsumedElement consumedElement in base.master.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, base.master.consumedElementStatusHandles);
				}
			}
			foreach (ElementConverter.OutputElement outputElement in base.master.outputElements)
			{
				if (outputElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, base.master.outputElementStatusHandles);
				}
			}
		}

		// Token: 0x0600A15F RID: 41311 RVA: 0x003AB988 File Offset: 0x003A9B88
		public void RemoveStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			for (int i = 0; i < base.master.consumedElements.Length; i++)
			{
				ElementConverter.ConsumedElement consumedElement = base.master.consumedElements[i];
				this.RemoveStatusItem<Tag>(consumedElement.Tag, base.master.consumedElementStatusHandles);
			}
			for (int j = 0; j < base.master.outputElements.Length; j++)
			{
				ElementConverter.OutputElement outputElement = base.master.outputElements[j];
				this.RemoveStatusItem<SimHashes>(outputElement.elementHash, base.master.outputElementStatusHandles);
			}
			base.master.consumedElementStatusHandles.Clear();
			base.master.outputElementStatusHandles.Clear();
		}

		// Token: 0x0600A160 RID: 41312 RVA: 0x003ABA48 File Offset: 0x003A9C48
		public void AddStatusItem<ElementType, IDType>(ElementType element, IDType id, StatusItem status, Dictionary<IDType, Guid> collection)
		{
			Guid value = this.selectable.AddStatusItem(status, element);
			collection[id] = value;
		}

		// Token: 0x0600A161 RID: 41313 RVA: 0x003ABA74 File Offset: 0x003A9C74
		public void RemoveStatusItem<IDType>(IDType id, Dictionary<IDType, Guid> collection)
		{
			Guid guid;
			if (!collection.TryGetValue(id, out guid))
			{
				return;
			}
			this.selectable.RemoveStatusItem(guid, false);
		}

		// Token: 0x0600A162 RID: 41314 RVA: 0x003ABA9C File Offset: 0x003A9C9C
		public void OnOperationalRequirementChanged(object data)
		{
			Operational operational = data as Operational;
			bool value = (operational == null) ? ((Boxed<bool>)data).value : operational.IsActive;
			base.sm.canConvert.Set(value, this, false);
		}

		// Token: 0x04007CE8 RID: 31976
		private KSelectable selectable;

		// Token: 0x04007CE9 RID: 31977
		public int subscribedHandle;
	}

	// Token: 0x02001918 RID: 6424
	public class States : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
	{
		// Token: 0x0600A163 RID: 41315 RVA: 0x003ABAE4 File Offset: 0x003A9CE4
		private bool ValidateStateTransition(ElementConverter.StatesInstance smi, bool _)
		{
			bool flag = smi.GetCurrentState() == smi.sm.disabled;
			if (smi.master.operational == null)
			{
				return flag;
			}
			bool flag2 = smi.master.consumedElements.Length == 0;
			bool flag3 = this.canConvert.Get(smi);
			int num = 0;
			while (!flag2 && num < smi.master.consumedElements.Length)
			{
				flag2 = smi.master.consumedElements[num].IsActive;
				num++;
			}
			if (flag3 && !flag2)
			{
				this.canConvert.Set(false, smi, true);
				return false;
			}
			return smi.master.operational.MeetsRequirements(smi.master.OperationalRequirement) == flag;
		}

		// Token: 0x0600A164 RID: 41316 RVA: 0x003ABBA0 File Offset: 0x003A9DA0
		private void OnEnterRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Unsubscribe(ref smi.subscribedHandle);
			smi.subscribedHandle = smi.Subscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x0600A165 RID: 41317 RVA: 0x003ABBE3 File Offset: 0x003A9DE3
		private void OnExitRoot(ElementConverter.StatesInstance smi)
		{
			smi.Unsubscribe(ref smi.subscribedHandle);
		}

		// Token: 0x0600A166 RID: 41318 RVA: 0x003ABBF4 File Offset: 0x003A9DF4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.Enter(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnEnterRoot)).Exit(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnExitRoot));
			this.disabled.ParamTransition<bool>(this.canConvert, this.converting, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition));
			this.converting.Enter("AddStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.AddStatusItems();
			}).Exit("RemoveStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.RemoveStatusItems();
			}).ParamTransition<bool>(this.canConvert, this.disabled, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition)).Update("ConvertMass", delegate(ElementConverter.StatesInstance smi, float dt)
			{
				smi.master.ConvertMass();
			}, UpdateRate.SIM_1000ms, true);
		}

		// Token: 0x04007CEA RID: 31978
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;

		// Token: 0x04007CEB RID: 31979
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;

		// Token: 0x04007CEC RID: 31980
		public StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.BoolParameter canConvert;
	}
}
