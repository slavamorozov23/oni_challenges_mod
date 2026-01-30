using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200098E RID: 2446
[SerializationConfig(MemberSerialization.OptIn)]
public class HotTub : StateMachineComponent<HotTub.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06004662 RID: 18018 RVA: 0x001960EC File Offset: 0x001942EC
	public float PercentFull
	{
		get
		{
			return 100f * this.waterStorage.GetMassAvailable(SimHashes.Water) / this.hotTubCapacity;
		}
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x0019610C File Offset: 0x0019430C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new HotTubWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("HotTubWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			HotTubWorkable hotTubWorkable = go.AddOrGet<HotTubWorkable>();
			int player_index = i;
			HotTubWorkable hotTubWorkable2 = hotTubWorkable;
			hotTubWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(hotTubWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			this.workables[i] = hotTubWorkable;
			this.workables[i].hotTub = this;
		}
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_water_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_water_target"
		});
		base.smi.UpdateWaterMeter();
		this.tempMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_temperature_target", "meter_temp", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_temperature_target"
		});
		base.smi.TestWaterTemperature();
		base.smi.StartSM();
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x001962A4 File Offset: 0x001944A4
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
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

	// Token: 0x06004665 RID: 18021 RVA: 0x001962F8 File Offset: 0x001944F8
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<HotTubWorkable> workChore = new WorkChore<HotTubWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, workable);
		return workChore;
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x00196371 File Offset: 0x00194571
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x0019638C File Offset: 0x0019458C
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x001963EC File Offset: 0x001945EC
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.occupants.Add(player);
		}
		else
		{
			this.occupants.Remove(player);
		}
		base.smi.sm.userCount.Set(this.occupants.Count, base.smi, false);
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x00196444 File Offset: 0x00194644
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x04002F5C RID: 12124
	public string specificEffect;

	// Token: 0x04002F5D RID: 12125
	public string trackingEffect;

	// Token: 0x04002F5E RID: 12126
	public int basePriority;

	// Token: 0x04002F5F RID: 12127
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(0, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04002F60 RID: 12128
	private HotTubWorkable[] workables;

	// Token: 0x04002F61 RID: 12129
	private Chore[] chores;

	// Token: 0x04002F62 RID: 12130
	public HashSet<int> occupants = new HashSet<int>();

	// Token: 0x04002F63 RID: 12131
	public float waterCoolingRate;

	// Token: 0x04002F64 RID: 12132
	public float hotTubCapacity = 100f;

	// Token: 0x04002F65 RID: 12133
	public float minimumWaterTemperature;

	// Token: 0x04002F66 RID: 12134
	public float bleachStoneConsumption;

	// Token: 0x04002F67 RID: 12135
	public float maxOperatingTemperature;

	// Token: 0x04002F68 RID: 12136
	[MyCmpGet]
	public Storage waterStorage;

	// Token: 0x04002F69 RID: 12137
	private MeterController waterMeter;

	// Token: 0x04002F6A RID: 12138
	private MeterController tempMeter;

	// Token: 0x020019F9 RID: 6649
	public class States : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub>
	{
		// Token: 0x0600A385 RID: 41861 RVA: 0x003B1F50 File Offset: 0x003B0150
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			this.root.Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.SapHeatFromWater(dt);
				smi.TestWaterTemperature();
			}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.OnStorageChange, delegate(HotTub.StatesInstance smi)
			{
				smi.UpdateWaterMeter();
				smi.TestWaterTemperature();
			});
			this.unoperational.TagTransition(GameTags.Operational, this.off, false).PlayAnim("off");
			this.off.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.off.filling);
			this.off.filling.DefaultState(this.off.filling.normal).Transition(this.ready, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) >= smi.master.hotTubCapacity, UpdateRate.SIM_200ms).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubFilling, (HotTub.StatesInstance smi) => smi.master);
			this.off.filling.normal.ParamTransition<bool>(this.waterTooCold, this.off.filling.too_cold, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue);
			this.off.filling.too_cold.ParamTransition<bool>(this.waterTooCold, this.off.filling.normal, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master);
			this.off.draining.Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) <= 0f, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(false);
			});
			this.off.too_hot.Transition(this.ready, (HotTub.StatesInstance smi) => !smi.IsTubTooHot(), UpdateRate.SIM_200ms).PlayAnim("overheated").ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubTooHot, (HotTub.StatesInstance smi) => smi.master);
			this.off.awaiting_delivery.EventTransition(GameHashes.OnStorageChange, this.ready, (HotTub.StatesInstance smi) => smi.HasBleachStone());
			this.ready.DefaultState(this.ready.idle).Enter("CreateChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).ParamTransition<bool>(this.waterTooCold, this.off.draining, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.off.awaiting_delivery, (HotTub.StatesInstance smi) => !smi.HasBleachStone()).Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.IsEmpty(), UpdateRate.SIM_200ms).Transition(this.off.too_hot, (HotTub.StatesInstance smi) => smi.IsTubTooHot(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.ready.idle.PlayAnim("on").ParamTransition<int>(this.userCount, this.ready.on.pre, (HotTub.StatesInstance smi, int p) => p > 0);
			this.ready.on.Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.ConsumeBleachstone(dt);
			}, UpdateRate.SIM_4000ms, false).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(false);
			});
			this.ready.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.ready.on.relaxing);
			this.ready.on.relaxing.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing_together, (HotTub.StatesInstance smi, int p) => p > 1);
			this.ready.on.relaxing_together.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing, (HotTub.StatesInstance smi, int p) => p == 1);
			this.ready.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready.idle);
		}

		// Token: 0x0600A386 RID: 41862 RVA: 0x003B2660 File Offset: 0x003B0860
		private string GetRelaxingAnim(HotTub.StatesInstance smi)
		{
			bool flag = smi.master.occupants.Contains(0);
			bool flag2 = smi.master.occupants.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x04007FBC RID: 32700
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IntParameter userCount;

		// Token: 0x04007FBD RID: 32701
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.BoolParameter waterTooCold;

		// Token: 0x04007FBE RID: 32702
		public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State unoperational;

		// Token: 0x04007FBF RID: 32703
		public HotTub.States.OffStates off;

		// Token: 0x04007FC0 RID: 32704
		public HotTub.States.ReadyStates ready;

		// Token: 0x020029C1 RID: 10689
		public class OffStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400B8A7 RID: 47271
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State draining;

			// Token: 0x0400B8A8 RID: 47272
			public HotTub.States.FillingStates filling;

			// Token: 0x0400B8A9 RID: 47273
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_hot;

			// Token: 0x0400B8AA RID: 47274
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State awaiting_delivery;
		}

		// Token: 0x020029C2 RID: 10690
		public class OnStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400B8AB RID: 47275
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State pre;

			// Token: 0x0400B8AC RID: 47276
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing;

			// Token: 0x0400B8AD RID: 47277
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing_together;

			// Token: 0x0400B8AE RID: 47278
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State post;
		}

		// Token: 0x020029C3 RID: 10691
		public class ReadyStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400B8AF RID: 47279
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State idle;

			// Token: 0x0400B8B0 RID: 47280
			public HotTub.States.OnStates on;
		}

		// Token: 0x020029C4 RID: 10692
		public class FillingStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400B8B1 RID: 47281
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State normal;

			// Token: 0x0400B8B2 RID: 47282
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_cold;
		}
	}

	// Token: 0x020019FA RID: 6650
	public class StatesInstance : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.GameInstance
	{
		// Token: 0x0600A388 RID: 41864 RVA: 0x003B26B6 File Offset: 0x003B08B6
		public StatesInstance(HotTub smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x0600A389 RID: 41865 RVA: 0x003B26D0 File Offset: 0x003B08D0
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x0600A38A RID: 41866 RVA: 0x003B26EC File Offset: 0x003B08EC
		public void UpdateWaterMeter()
		{
			base.smi.master.waterMeter.SetPositionPercent(Mathf.Clamp(base.smi.master.waterStorage.GetMassAvailable(SimHashes.Water) / base.smi.master.hotTubCapacity, 0f, 1f));
		}

		// Token: 0x0600A38B RID: 41867 RVA: 0x003B2748 File Offset: 0x003B0948
		public void UpdateTemperatureMeter(float waterTemp)
		{
			Element element = ElementLoader.GetElement(SimHashes.Water.CreateTag());
			base.smi.master.tempMeter.SetPositionPercent(Mathf.Clamp((waterTemp - base.smi.master.minimumWaterTemperature) / (element.highTemp - base.smi.master.minimumWaterTemperature), 0f, 1f));
		}

		// Token: 0x0600A38C RID: 41868 RVA: 0x003B27B4 File Offset: 0x003B09B4
		public void TestWaterTemperature()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(1836671383));
			float num = 0f;
			if (!gameObject)
			{
				this.UpdateTemperatureMeter(num);
				base.smi.sm.waterTooCold.Set(false, base.smi, false);
				return;
			}
			num = gameObject.GetComponent<PrimaryElement>().Temperature;
			this.UpdateTemperatureMeter(num);
			if (num < base.smi.master.minimumWaterTemperature)
			{
				base.smi.sm.waterTooCold.Set(true, base.smi, false);
				return;
			}
			base.smi.sm.waterTooCold.Set(false, base.smi, false);
		}

		// Token: 0x0600A38D RID: 41869 RVA: 0x003B2878 File Offset: 0x003B0A78
		public bool IsTubTooHot()
		{
			return base.smi.master.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxOperatingTemperature;
		}

		// Token: 0x0600A38E RID: 41870 RVA: 0x003B28A4 File Offset: 0x003B0AA4
		public bool HasBleachStone()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(-839728230));
			return gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x0600A38F RID: 41871 RVA: 0x003B28F0 File Offset: 0x003B0AF0
		public void SapHeatFromWater(float dt)
		{
			float num = base.smi.master.waterCoolingRate * dt / (float)base.smi.master.waterStorage.items.Count;
			foreach (GameObject gameObject in base.smi.master.waterStorage.items)
			{
				GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, base.smi.master.minimumWaterTemperature);
				GameUtil.DeltaThermalEnergy(base.GetComponent<PrimaryElement>(), num, base.GetComponent<PrimaryElement>().Element.highTemp);
			}
		}

		// Token: 0x0600A390 RID: 41872 RVA: 0x003B29B4 File Offset: 0x003B0BB4
		public void ConsumeBleachstone(float dt)
		{
			base.smi.master.waterStorage.ConsumeIgnoringDisease(new Tag(-839728230), base.smi.master.bleachStoneConsumption * dt);
		}

		// Token: 0x04007FC1 RID: 32705
		private Operational operational;
	}
}
