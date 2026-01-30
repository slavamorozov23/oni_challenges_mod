using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C1B RID: 3099
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IGameObjectEffectDescriptor, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06005D2A RID: 23850 RVA: 0x0021B82F File Offset: 0x00219A2F
	// (set) Token: 0x06005D2B RID: 23851 RVA: 0x0021B838 File Offset: 0x00219A38
	public Tag ChosenBeverage
	{
		get
		{
			return this.chosenBeverage;
		}
		set
		{
			if (this.chosenBeverage != value)
			{
				this.chosenBeverage = value;
				base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
				this.storage.DropAll(false, false, default(Vector3), true, null);
			}
		}
	}

	// Token: 0x06005D2C RID: 23852 RVA: 0x0021B884 File Offset: 0x00219A84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.socializeOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.specificEffect = "Socialized";
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.chores = new Chore[this.socializeOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(this), this.socializeOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WaterCooler", this, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		base.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06005D2D RID: 23853 RVA: 0x0021B9C4 File Offset: 0x00219BC4
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.CancelDrinkChores();
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06005D2E RID: 23854 RVA: 0x0021BA28 File Offset: 0x00219C28
	public void UpdateDrinkChores(bool force = true)
	{
		if (!force && !this.choresDirty)
		{
			return;
		}
		float num = this.storage.GetMassAvailable(this.ChosenBeverage);
		int num2 = 0;
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			CellOffset offset = this.socializeOffsets[i];
			Chore chore = this.chores[i];
			if (num2 < this.choreCount && this.IsOffsetValid(offset) && num >= 1f)
			{
				num2++;
				num -= 1f;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = new WaterCoolerChore(this, this.workables[i], null, null, new Action<Chore>(this.OnChoreEnd));
				}
			}
			else if (chore != null)
			{
				chore.Cancel("invalid");
				this.chores[i] = null;
			}
		}
		this.choresDirty = false;
	}

	// Token: 0x06005D2F RID: 23855 RVA: 0x0021BB08 File Offset: 0x00219D08
	public void CancelDrinkChores()
	{
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (chore != null)
			{
				chore.Cancel("cancelled");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06005D30 RID: 23856 RVA: 0x0021BB48 File Offset: 0x00219D48
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x0021BB6F File Offset: 0x00219D6F
	private void OnChoreEnd(Chore chore)
	{
		this.choresDirty = true;
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x0021BB78 File Offset: 0x00219D78
	private void OnCellChanged(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x0021BB81 File Offset: 0x00219D81
	private void OnStorageChange(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x0021BB8A File Offset: 0x00219D8A
	public CellOffset[] GetOffsets()
	{
		return this.drinkOffsets;
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x0021BB92 File Offset: 0x00219D92
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x0021BB9C File Offset: 0x00219D9C
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06005D37 RID: 23863 RVA: 0x0021BC04 File Offset: 0x00219E04
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Socialized", true);
		foreach (global::Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
		{
			this.AddRequirementDesc(list, tuple.first, 1f);
		}
		return list;
	}

	// Token: 0x06005D38 RID: 23864 RVA: 0x0021BC84 File Offset: 0x00219E84
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true, "\n    • ", true);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first.ToString().ToUpper());
			if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second.IsNullOrWhiteSpace())
			{
				text = text + "\n\n" + Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second), false, "\n    • ", true);
			}
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first).name, Def.GetUISprite(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, "ui", false), text);
		}
		return array;
	}

	// Token: 0x06005D39 RID: 23865 RVA: 0x0021BD96 File Offset: 0x00219F96
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.ChosenBeverage = option.tag;
	}

	// Token: 0x06005D3A RID: 23866 RVA: 0x0021BDA4 File Offset: 0x00219FA4
	public Tag GetSelectedOption()
	{
		return this.ChosenBeverage;
	}

	// Token: 0x04003E17 RID: 15895
	public const float DRINK_MASS = 1f;

	// Token: 0x04003E18 RID: 15896
	public const string SPECIFIC_EFFECT = "Socialized";

	// Token: 0x04003E19 RID: 15897
	public CellOffset[] socializeOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(2, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04003E1A RID: 15898
	public int choreCount = 2;

	// Token: 0x04003E1B RID: 15899
	public float workTime = 5f;

	// Token: 0x04003E1C RID: 15900
	private CellOffset[] drinkOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04003E1D RID: 15901
	public static Action<GameObject, GameObject> OnDuplicantDrank;

	// Token: 0x04003E1E RID: 15902
	private Chore[] chores;

	// Token: 0x04003E1F RID: 15903
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04003E20 RID: 15904
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x04003E21 RID: 15905
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003E22 RID: 15906
	public bool choresDirty;

	// Token: 0x04003E23 RID: 15907
	[Serialize]
	private Tag chosenBeverage = GameTags.Water;

	// Token: 0x04003E24 RID: 15908
	private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>(delegate(WaterCooler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001DB2 RID: 7602
	public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
	{
		// Token: 0x0600B1E0 RID: 45536 RVA: 0x003DE544 File Offset: 0x003DC744
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass()).PlayAnim("off");
			this.dispensing.Enter("StartMeter", delegate(WaterCooler.StatesInstance smi)
			{
				smi.StartMeter();
			}).Enter("Set Active", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(true);
			}).Enter("UpdateDrinkChores.force", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.UpdateDrinkChores(true);
			}).Update("UpdateDrinkChores", delegate(WaterCooler.StatesInstance smi, float dt)
			{
				smi.master.UpdateDrinkChores(true);
			}, UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.CancelDrinkChores();
			}).Exit("Set Inactive", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (WaterCooler.StatesInstance smi) => !smi.HasMinimumMass()).PlayAnim("working");
		}

		// Token: 0x04008BFF RID: 35839
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;

		// Token: 0x04008C00 RID: 35840
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;

		// Token: 0x04008C01 RID: 35841
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;
	}

	// Token: 0x02001DB3 RID: 7603
	public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
	{
		// Token: 0x0600B1E2 RID: 45538 RVA: 0x003DE748 File Offset: 0x003DC948
		public StatesInstance(WaterCooler smi) : base(smi)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_bottle", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_bottle"
			});
			this.storage = base.master.GetComponent<Storage>();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x0600B1E3 RID: 45539 RVA: 0x003DE7B4 File Offset: 0x003DC9B4
		public void Drink(GameObject druplicant, bool triggerOnDrinkCallback = true)
		{
			if (!this.HasMinimumMass())
			{
				return;
			}
			Tag tag = this.storage.items[0].PrefabID();
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(tag, 1f, out num, out diseaseInfo, out num2);
			GermExposureMonitor.Instance smi = druplicant.GetSMI<GermExposureMonitor.Instance>();
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Digestion);
			}
			Effects component = druplicant.GetComponent<Effects>();
			if (tag == SimHashes.Milk.CreateTag())
			{
				component.Add("DuplicantGotMilk", true);
			}
			if (triggerOnDrinkCallback)
			{
				Action<GameObject, GameObject> onDuplicantDrank = WaterCooler.OnDuplicantDrank;
				if (onDuplicantDrank == null)
				{
					return;
				}
				onDuplicantDrank(druplicant, base.gameObject);
			}
		}

		// Token: 0x0600B1E4 RID: 45540 RVA: 0x003DE85C File Offset: 0x003DCA5C
		private void OnStorageChange(object data)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x0600B1E5 RID: 45541 RVA: 0x003DE892 File Offset: 0x003DCA92
		public void SetOperationalActiveState(bool isActive)
		{
			this.operational.SetActive(isActive, false);
		}

		// Token: 0x0600B1E6 RID: 45542 RVA: 0x003DE8A4 File Offset: 0x003DCAA4
		public void StartMeter()
		{
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(base.smi.master.ChosenBeverage, 0f);
			if (primaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), primaryElement.Element.substance.colour);
			this.OnStorageChange(null);
		}

		// Token: 0x0600B1E7 RID: 45543 RVA: 0x003DE908 File Offset: 0x003DCB08
		public bool HasMinimumMass()
		{
			return this.storage.GetMassAvailable(ElementLoader.GetElement(base.smi.master.ChosenBeverage).id) >= 1f;
		}

		// Token: 0x04008C02 RID: 35842
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04008C03 RID: 35843
		private Storage storage;

		// Token: 0x04008C04 RID: 35844
		private MeterController meter;
	}
}
