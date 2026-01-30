using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007AE RID: 1966
public class LonelyMinionHouse : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>
{
	// Token: 0x060033D1 RID: 13265 RVA: 0x001267D8 File Offset: 0x001249D8
	private bool ValidateOperationalTransition(LonelyMinionHouse.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Active);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x00126815 File Offset: 0x00124A15
	private static bool AllQuestsComplete(LonelyMinionHouse.Instance smi, StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.SignalParameter param)
	{
		return 1f - smi.sm.QuestProgress.Get(smi) <= Mathf.Epsilon;
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x00126838 File Offset: 0x00124A38
	public static void EvaluateLights(LonelyMinionHouse.Instance smi, float dt)
	{
		bool flag = smi.IsInsideState(smi.sm.Active);
		QuestInstance instance = QuestManager.GetInstance(smi.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
		if (!flag || !smi.Light.enabled || instance.IsComplete)
		{
			return;
		}
		bool flag2;
		bool flag3;
		instance.TrackProgress(new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.PowerCriteriaId,
			CurrentValue = instance.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId, 0) + dt
		}, out flag2, out flag3);
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x001268C0 File Offset: 0x00124AC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<LonelyMinionHouse.Instance, float>(LonelyMinionHouse.EvaluateLights), UpdateRate.SIM_1000ms, false);
		this.Inactive.EventTransition(GameHashes.OperationalChanged, this.Active, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Active.Enter(delegate(LonelyMinionHouse.Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).Exit(delegate(LonelyMinionHouse.Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).OnSignal(this.CompleteStory, this.Active.StoryComplete, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Parameter<StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.SignalParameter>.Callback(LonelyMinionHouse.AllQuestsComplete)).EventTransition(GameHashes.OperationalChanged, this.Inactive, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Active.StoryComplete.Enter(new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State.Callback(LonelyMinionHouse.ActiveStates.OnEnterStoryComplete));
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x001269C4 File Offset: 0x00124BC4
	public static float CalculateAverageDecor(Extents area)
	{
		float num = 0f;
		int cell = Grid.XYToCell(area.x, area.y);
		for (int i = 0; i < area.width * area.height; i++)
		{
			int num2 = Grid.OffsetCell(cell, i % area.width, i / area.width);
			num += Grid.Decor[num2];
		}
		return num / (float)(area.width * area.height);
	}

	// Token: 0x04001F46 RID: 8006
	public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State Inactive;

	// Token: 0x04001F47 RID: 8007
	public LonelyMinionHouse.ActiveStates Active;

	// Token: 0x04001F48 RID: 8008
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal MailDelivered;

	// Token: 0x04001F49 RID: 8009
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal CompleteStory;

	// Token: 0x04001F4A RID: 8010
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.FloatParameter QuestProgress;

	// Token: 0x020016D5 RID: 5845
	public class Def : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitDef
	{
	}

	// Token: 0x020016D6 RID: 5846
	public new class Instance : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance, ICheckboxListGroupControl
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x060098A5 RID: 39077 RVA: 0x0038843A File Offset: 0x0038663A
		public HashedString QuestOwnerId
		{
			get
			{
				return this.questOwnerId;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060098A6 RID: 39078 RVA: 0x00388442 File Offset: 0x00386642
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animControllers[0];
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060098A7 RID: 39079 RVA: 0x0038844C File Offset: 0x0038664C
		public KBatchedAnimController LightsController
		{
			get
			{
				return this.animControllers[1];
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060098A8 RID: 39080 RVA: 0x00388456 File Offset: 0x00386656
		public KBatchedAnimController BlindsController
		{
			get
			{
				return this.blinds.meterController;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060098A9 RID: 39081 RVA: 0x00388463 File Offset: 0x00386663
		public Light2D Light
		{
			get
			{
				return this.light;
			}
		}

		// Token: 0x060098AA RID: 39082 RVA: 0x0038846B File Offset: 0x0038666B
		public Instance(StateMachineController master, LonelyMinionHouse.Def def) : base(master, def)
		{
		}

		// Token: 0x060098AB RID: 39083 RVA: 0x00388484 File Offset: 0x00386684
		public override void StartSM()
		{
			this.animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
			this.light = this.LightsController.GetComponent<Light2D>();
			this.light.transform.position += Vector3.forward * Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
			this.light.gameObject.SetActive(true);
			this.lightsLink = new KAnimLink(this.AnimController, this.LightsController);
			Activatable component = base.GetComponent<Activatable>();
			component.SetOffsets(new CellOffset[]
			{
				new CellOffset(-3, 0)
			});
			if (!component.IsActivated)
			{
				Activatable activatable = component;
				activatable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(activatable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
				Activatable activatable2 = component;
				activatable2.onActivate = (System.Action)Delegate.Combine(activatable2.onActivate, new System.Action(this.StartStoryTrait));
			}
			this.meter = new MeterController(this.AnimController, "meter_storage_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.METER_SYMBOLS);
			this.blinds = new MeterController(this.AnimController, "blinds_target", string.Format("{0}_{1}", "meter_blinds", 0), Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.BLINDS_SYMBOLS);
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			this.questOwnerId = new HashedString(component2.PrefabTag.GetHash());
			this.SpawnMinion();
			if (this.lonelyMinion != null && !this.TryFindMailbox())
			{
				GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingLayerChanged));
			}
			QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			QuestInstance questInstance = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			QuestInstance questInstance2 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			QuestInstance questInstance3 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			NonEssentialEnergyConsumer component3 = base.GetComponent<NonEssentialEnergyConsumer>();
			NonEssentialEnergyConsumer nonEssentialEnergyConsumer = component3;
			nonEssentialEnergyConsumer.PoweredStateChanged = (Action<bool>)Delegate.Combine(nonEssentialEnergyConsumer.PoweredStateChanged, new Action<bool>(this.OnPoweredStateChanged));
			this.OnPoweredStateChanged(component3.IsPowered);
			if (this.lonelyMinion == null)
			{
				base.StartSM();
				return;
			}
			this.onBuildingActivatedHandle = base.Subscribe(-592767678, new Action<object>(this.OnBuildingActivated));
			base.StartSM();
			QuestInstance questInstance4 = questInstance;
			questInstance4.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance4.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance questInstance5 = questInstance2;
			questInstance5.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance5.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance questInstance6 = questInstance3;
			questInstance6.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance6.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			float num = base.sm.QuestProgress.Get(this) * 3f;
			int num2 = Mathf.Approximately(num, Mathf.Ceil(num)) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 == 0)
			{
				return;
			}
			HashedString[] array = new HashedString[num2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = string.Format("{0}_{1}", "meter_blinds", i);
			}
			this.blinds.meterController.Play(array, KAnim.PlayMode.Once);
		}

		// Token: 0x060098AC RID: 39084 RVA: 0x003887F4 File Offset: 0x003869F4
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Activatable component = base.GetComponent<Activatable>();
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
			component.onActivate = (System.Action)Delegate.Remove(component.onActivate, new System.Action(this.StartStoryTrait));
			base.Unsubscribe(ref this.onBuildingActivatedHandle);
		}

		// Token: 0x060098AD RID: 39085 RVA: 0x00388860 File Offset: 0x00386A60
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State prevState, float delta)
		{
			float num = base.sm.QuestProgress.Get(this);
			num += delta / 3f;
			if (1f - num <= 0.001f)
			{
				num = 1f;
			}
			base.sm.QuestProgress.Set(Mathf.Clamp01(num), this, true);
			this.lonelyMinion.UnlockQuestIdle(quest, prevState, delta);
			this.lonelyMinion.ShowQuestCompleteNotification(quest, prevState, 0f);
			base.gameObject.Trigger(1980521255, null);
			if (!quest.IsComplete)
			{
				return;
			}
			if (num == 1f)
			{
				base.sm.CompleteStory.Trigger(this);
			}
			float num2 = num * 3f;
			int num3 = Mathf.Approximately(num2, Mathf.Ceil(num2)) ? Mathf.CeilToInt(num2) : Mathf.FloorToInt(num2);
			this.blinds.meterController.Queue(string.Format("{0}_{1}", "meter_blinds", num3 - 1), KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x060098AE RID: 39086 RVA: 0x00388965 File Offset: 0x00386B65
		public void MailboxContentChanged(GameObject item)
		{
			this.lonelyMinion.sm.Mail.Set(item, this.lonelyMinion, false);
		}

		// Token: 0x060098AF RID: 39087 RVA: 0x00388988 File Offset: 0x00386B88
		public override void CompleteEvent()
		{
			if (this.lonelyMinion == null)
			{
				base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop, 1f, 0f);
				base.gameObject.AddOrGet<TreeFilterable>();
				base.gameObject.AddOrGet<BuildingEnabledButton>();
				base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
				base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
				base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
				Storage component = base.GetComponent<Storage>();
				component.allowItemRemoval = true;
				component.showInUI = true;
				component.showDescriptor = true;
				component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
				this.storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, false, Db.Get().ChoreTypes.StorageFetch);
				this.storageFilter.SetMeter(this.meter);
				this.meter = null;
				RootMenu.Instance.Refresh();
				component.RenotifyAll();
				return;
			}
			List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
			list.Shuffle<MinionIdentity>();
			int num = 3;
			base.def.EventCompleteInfo.Minions = new GameObject[1 + Mathf.Min(num, list.Count)];
			base.def.EventCompleteInfo.Minions[0] = this.lonelyMinion.gameObject;
			int num2 = 0;
			while (num2 < list.Count && num > 0)
			{
				base.def.EventCompleteInfo.Minions[num2 + 1] = list[num2].gameObject;
				num--;
				num2++;
			}
			base.CompleteEvent();
		}

		// Token: 0x060098B0 RID: 39088 RVA: 0x00388B38 File Offset: 0x00386D38
		public override void OnCompleteStorySequence()
		{
			this.SpawnMinion();
			base.Unsubscribe(ref this.onBuildingActivatedHandle);
			base.OnCompleteStorySequence();
			QuestInstance instance = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance instance2 = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			instance2.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance2.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance instance3 = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			instance3.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance3.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.blinds.meterController.Play(this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode, 1f, 0f);
			base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop, 1f, 0f);
			base.gameObject.AddOrGet<TreeFilterable>();
			base.gameObject.AddOrGet<BuildingEnabledButton>();
			base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
			base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
			base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
			Storage component = base.GetComponent<Storage>();
			component.allowItemRemoval = true;
			component.showInUI = true;
			component.showDescriptor = true;
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
			this.storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, false, Db.Get().ChoreTypes.StorageFetch);
			this.storageFilter.SetMeter(this.meter);
			this.meter = null;
			RootMenu.Instance.Refresh();
		}

		// Token: 0x060098B1 RID: 39089 RVA: 0x00388D44 File Offset: 0x00386F44
		private void SpawnMinion()
		{
			if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.LonelyMinion))
			{
				return;
			}
			if (this.lonelyMinion == null)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(LonelyMinionConfig.ID), base.gameObject, null);
				global::Debug.Assert(gameObject != null);
				gameObject.transform.localPosition = new Vector3(0.54f, 0f, -0.01f);
				gameObject.SetActive(true);
				Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.gameObject));
				BuildingDef def = base.GetComponent<Building>().Def;
				this.lonelyMinion = gameObject.GetSMI<LonelyMinion.Instance>();
				this.lonelyMinion.def.QuestOwnerId = this.questOwnerId;
				this.lonelyMinion.def.DecorInspectionArea = new Extents(vector2I.x - Mathf.CeilToInt((float)def.WidthInCells / 2f) + 1, vector2I.y, def.WidthInCells, def.HeightInCells);
				return;
			}
			MinionStartingStats minionStartingStats = new MinionStartingStats(this.lonelyMinion.def.Personality, null, "AncientKnowledge", false);
			minionStartingStats.Traits.Add(Db.Get().traits.TryGet("Chatty"));
			minionStartingStats.voiceIdx = -2;
			string[] all_ATTRIBUTES = DUPLICANTSTATS.ALL_ATTRIBUTES;
			for (int i = 0; i < all_ATTRIBUTES.Length; i++)
			{
				Dictionary<string, int> startingLevels = minionStartingStats.StartingLevels;
				string key = all_ATTRIBUTES[i];
				startingLevels[key] += 7;
			}
			UnityEngine.Object.Destroy(this.lonelyMinion.gameObject);
			this.lonelyMinion = null;
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			MinionIdentity minionIdentity = Util.KInstantiate<MinionIdentity>(prefab, null, null);
			minionIdentity.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
			minionIdentity.gameObject.SetActive(true);
			minionStartingStats.Apply(minionIdentity.gameObject);
			minionIdentity.arrivalTime += (float)UnityEngine.Random.Range(2190, 3102);
			minionIdentity.arrivalTime *= -1f;
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			for (int j = 0; j < 3; j++)
			{
				component.ForceAddSkillPoint();
			}
			Vector3 position = base.transform.position + Vector3.left * Grid.CellSizeInMeters * 2f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			minionIdentity.transform.SetPosition(position);
		}

		// Token: 0x060098B2 RID: 39090 RVA: 0x00388FD8 File Offset: 0x003871D8
		private static Util.IterationInstruction tryFindMailboxVisitor(object obj, ref ValueTuple<LonelyMinionHouse.Instance, bool> param)
		{
			if ((obj as GameObject).GetComponent<KPrefabID>().PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
			{
				param.Item1.OnBuildingLayerChanged(0, obj);
				param.Item2 = true;
			}
			if (!param.Item2)
			{
				return Util.IterationInstruction.Continue;
			}
			return Util.IterationInstruction.Halt;
		}

		// Token: 0x060098B3 RID: 39091 RVA: 0x00389028 File Offset: 0x00387228
		private bool TryFindMailbox()
		{
			if (base.sm.QuestProgress.Get(this) == 1f)
			{
				return true;
			}
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), 10);
			ValueTuple<LonelyMinionHouse.Instance, bool> valueTuple = new ValueTuple<LonelyMinionHouse.Instance, bool>(this, false);
			GameScenePartitioner.Instance.VisitEntries<ValueTuple<LonelyMinionHouse.Instance, bool>>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.objectLayers[1], new GameScenePartitioner.VisitorRef<ValueTuple<LonelyMinionHouse.Instance, bool>>(LonelyMinionHouse.Instance.tryFindMailboxVisitor), ref valueTuple);
			return valueTuple.Item2;
		}

		// Token: 0x060098B4 RID: 39092 RVA: 0x003890B0 File Offset: 0x003872B0
		private void OnBuildingLayerChanged(int cell, object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component.PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
			{
				component.GetComponent<LonelyMinionMailbox>().Initialize(this);
				GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingLayerChanged));
			}
		}

		// Token: 0x060098B5 RID: 39093 RVA: 0x00389120 File Offset: 0x00387320
		public void OnPoweredStateChanged(bool isPowered)
		{
			this.light.enabled = (isPowered && base.GetComponent<Operational>().IsOperational);
			this.LightsController.Play(this.light.enabled ? LonelyMinionHouseConfig.LIGHTS_ON : LonelyMinionHouseConfig.LIGHTS_OFF, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x060098B6 RID: 39094 RVA: 0x00389178 File Offset: 0x00387378
		private void StartStoryTrait()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x060098B7 RID: 39095 RVA: 0x00389184 File Offset: 0x00387384
		protected override void OnBuildingActivated(object _)
		{
			if (!this.IsIntroSequenceComplete())
			{
				return;
			}
			bool isActivated = base.GetComponent<Activatable>().IsActivated;
			if (this.lonelyMinion != null)
			{
				this.lonelyMinion.sm.Active.Set(isActivated && base.GetComponent<Operational>().IsOperational, this.lonelyMinion, false);
			}
			if (isActivated && base.sm.QuestProgress.Get(this) < 1f)
			{
				base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.AllPower;
			}
		}

		// Token: 0x060098B8 RID: 39096 RVA: 0x00389204 File Offset: 0x00387404
		protected override void OnObjectSelect(object clicked)
		{
			if (!((Boxed<bool>)clicked).value)
			{
				return;
			}
			if (this.knockNotification != null)
			{
				this.knocker.gameObject.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				this.knockNotification.Clear();
				this.knockNotification = null;
				this.PlayIntroSequence(null);
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN))
			{
				int count = Components.LiveMinionIdentities.Count;
				int idx = UnityEngine.Random.Range(0, count);
				base.def.EventIntroInfo.Minions = new GameObject[]
				{
					this.lonelyMinion.gameObject,
					(count == 0) ? null : Components.LiveMinionIdentities[idx].gameObject
				};
			}
			base.OnObjectSelect(clicked);
		}

		// Token: 0x060098B9 RID: 39097 RVA: 0x003892D8 File Offset: 0x003874D8
		private void OnWorkStateChanged(Workable w, Workable.WorkableEvent state)
		{
			Activatable activatable = w as Activatable;
			if (activatable != null)
			{
				if (state == Workable.WorkableEvent.WorkStarted)
				{
					this.knocker = w.worker.GetComponent<KBatchedAnimController>();
					this.knocker.gameObject.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
					this.knockNotification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT, NotificationType.Event, null, null, false, 0f, new Notification.ClickCallback(this.PlayIntroSequence), null, null, true, true, false);
					base.gameObject.AddOrGet<Notifier>().Add(this.knockNotification, "");
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
				}
				if (state == Workable.WorkableEvent.WorkStopped)
				{
					if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
					{
						if (this.knockNotification != null)
						{
							base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
							this.knockNotification.Clear();
							this.knockNotification = null;
						}
						FocusTargetSequence.Cancel(base.master);
						this.knocker.gameObject.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
						this.knocker = null;
					}
					if (this.currentWorkState == Workable.WorkableEvent.WorkCompleted)
					{
						Activatable activatable2 = activatable;
						activatable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(activatable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
						Activatable activatable3 = activatable;
						activatable3.onActivate = (System.Action)Delegate.Remove(activatable3.onActivate, new System.Action(this.StartStoryTrait));
					}
				}
				this.currentWorkState = state;
				return;
			}
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORK_PST, KAnim.PlayMode.Once, 1f, 0f);
				this.AnimController.Queue(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			bool flag = this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[0] || this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[1];
			if (state == Workable.WorkableEvent.WorkStarted && !flag)
			{
				this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORKING, KAnim.PlayMode.Loop);
			}
		}

		// Token: 0x060098BA RID: 39098 RVA: 0x003894FC File Offset: 0x003876FC
		private void ReleaseKnocker(object _)
		{
			Navigator component = this.knocker.GetComponent<Navigator>();
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			this.knocker.RemoveAnimOverrides(base.GetComponent<Activatable>().overrideAnims[0]);
			this.knocker.Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.blinds.meterController.Play(this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode, 1f, 0f);
			this.lonelyMinion.AnimController.Play(this.lonelyMinion.AnimController.defaultAnim, this.lonelyMinion.AnimController.initialMode, 1f, 0f);
			this.knocker.gameObject.Unsubscribe(-1061186183, new Action<object>(this.ReleaseKnocker));
			this.knocker.GetComponent<Brain>().Reset("knock sequence");
			this.knocker = null;
		}

		// Token: 0x060098BB RID: 39099 RVA: 0x00389618 File Offset: 0x00387818
		private void PlayIntroSequence(object _ = null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.gameObject), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
			FocusTargetSequence.Start(base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 2f,
				TargetSize = 6f,
				Target = target,
				PopupData = null,
				CompleteCB = new System.Action(this.OnIntroSequenceComplete),
				CanCompleteCB = new Func<bool>(this.IsIntroSequenceComplete)
			});
			base.GetComponent<KnockKnock>().AnswerDoor();
			this.knockNotification = null;
		}

		// Token: 0x060098BC RID: 39100 RVA: 0x003896F0 File Offset: 0x003878F0
		private void OnIntroSequenceComplete()
		{
			this.OnBuildingActivated(null);
			bool flag;
			bool flag2;
			QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest).TrackProgress(new Quest.ItemData
			{
				CriteriaId = LonelyMinionConfig.GreetingCriteraId
			}, out flag, out flag2);
		}

		// Token: 0x060098BD RID: 39101 RVA: 0x00389740 File Offset: 0x00387940
		private bool IsIntroSequenceComplete()
		{
			if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
			{
				return false;
			}
			if (this.currentWorkState == Workable.WorkableEvent.WorkStopped && this.knocker != null && this.knocker.currentAnim != LonelyMinionHouseConfig.ANSWER)
			{
				this.knocker.GetComponent<Brain>().Stop("knock sequence");
				this.knocker.gameObject.Subscribe(-1061186183, new Action<object>(this.ReleaseKnocker));
				this.knocker.AddAnimOverrides(base.GetComponent<Activatable>().overrideAnims[0], 0f);
				this.knocker.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
				this.lonelyMinion.AnimController.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
				this.blinds.meterController.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
			}
			return this.currentWorkState == Workable.WorkableEvent.WorkStopped && this.knocker == null;
		}

		// Token: 0x060098BE RID: 39102 RVA: 0x00389854 File Offset: 0x00387A54
		public Vector3 GetParcelPosition()
		{
			int index = -1;
			KAnimFileData data = Assets.GetAnim("anim_interacts_lonely_dupe_kanim").GetData();
			for (int i = 0; i < data.animCount; i++)
			{
				if (data.GetAnim(i).hash == LonelyMinionConfig.CHECK_MAIL)
				{
					index = data.GetAnim(i).firstFrameIdx;
					break;
				}
			}
			List<KAnim.Anim.FrameElement> frameElements = this.lonelyMinion.AnimController.GetBatch().group.data.frameElements;
			KAnim.Anim.Frame frame;
			this.lonelyMinion.AnimController.GetBatch().group.data.TryGetFrame(index, out frame);
			bool flag = false;
			Matrix2x3 m = default(Matrix2x3);
			int num = 0;
			while (!flag && num < frame.numElements)
			{
				if (frameElements[frame.firstElementIdx + num].symbol == LonelyMinionConfig.PARCEL_SNAPTO)
				{
					flag = true;
					m = frameElements[frame.firstElementIdx + num].transform;
					break;
				}
				num++;
			}
			Vector3 result = Vector3.zero;
			if (flag)
			{
				Matrix4x4 lhs = this.lonelyMinion.AnimController.GetTransformMatrix();
				result = (lhs * m).GetColumn(3);
			}
			return result;
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x060098BF RID: 39103 RVA: 0x0038999B File Offset: 0x00387B9B
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.LONELYMINION.NAME;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060098C0 RID: 39104 RVA: 0x003899A7 File Offset: 0x00387BA7
		public string Description
		{
			get
			{
				return CODEX.STORY_TRAITS.LONELYMINION.DESCRIPTION_BUILDINGMENU;
			}
		}

		// Token: 0x060098C1 RID: 39105 RVA: 0x003899B4 File Offset: 0x00387BB4
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			QuestInstance greetingQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			if (!greetingQuest.IsComplete)
			{
				return new ICheckboxListGroupControl.ListGroup[]
				{
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(null), (string title) => this.ResolveQuestTitle(title, greetingQuest), null)
				};
			}
			QuestInstance foodQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			QuestInstance decorQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			QuestInstance powerQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			return new ICheckboxListGroupControl.ListGroup[]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(null), (string title) => this.ResolveQuestTitle(title, greetingQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionFoodQuest.Title, foodQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, foodQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionDecorQuest.Title, decorQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, decorQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionPowerQuest.Title, powerQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, powerQuest), null)
			};
		}

		// Token: 0x060098C2 RID: 39106 RVA: 0x00389BA8 File Offset: 0x00387DA8
		private string ResolveQuestTitle(string title, QuestInstance quest)
		{
			string str = GameUtil.FloatToString(quest.CurrentProgress * 100f, "##0") + UI.UNITSUFFIXES.PERCENT;
			return title + " - " + str;
		}

		// Token: 0x060098C3 RID: 39107 RVA: 0x00389BE8 File Offset: 0x00387DE8
		private string ResolveQuestToolTips(int criteriaId, string toolTip, QuestInstance quest)
		{
			if (criteriaId == LonelyMinionConfig.FoodCriteriaId.HashValue)
			{
				int quality = (int)quest.GetTargetValue(LonelyMinionConfig.FoodCriteriaId, 0);
				int targetCount = quest.GetTargetCount(LonelyMinionConfig.FoodCriteriaId);
				string text = string.Empty;
				for (int i = 0; i < targetCount; i++)
				{
					Tag satisfyingItem = quest.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, i);
					if (!satisfyingItem.IsValid)
					{
						break;
					}
					text = text + "    • " + TagManager.GetProperName(satisfyingItem, false);
					if (targetCount - i != 1)
					{
						text += "\n";
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = string.Format("{0}{1}", "    • ", CODEX.QUESTS.CRITERIA.FOODQUALITY.NONE);
				}
				return string.Format(toolTip, GameUtil.GetFormattedFoodQuality(quality), text);
			}
			if (criteriaId == LonelyMinionConfig.DecorCriteriaId.HashValue)
			{
				int num = (int)quest.GetTargetValue(LonelyMinionConfig.DecorCriteriaId, 0);
				float num2 = LonelyMinionHouse.CalculateAverageDecor(this.lonelyMinion.def.DecorInspectionArea);
				return string.Format(toolTip, num, num2);
			}
			float f = quest.GetTargetValue(LonelyMinionConfig.PowerCriteriaId, 0) - quest.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId, 0);
			return string.Format(toolTip, Mathf.CeilToInt(f));
		}

		// Token: 0x060098C4 RID: 39108 RVA: 0x00389D20 File Offset: 0x00387F20
		public bool SidescreenEnabled()
		{
			return StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN) && !StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion);
		}

		// Token: 0x060098C5 RID: 39109 RVA: 0x00389D5D File Offset: 0x00387F5D
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x040075EA RID: 30186
		private KAnimLink lightsLink;

		// Token: 0x040075EB RID: 30187
		private HashedString questOwnerId;

		// Token: 0x040075EC RID: 30188
		private LonelyMinion.Instance lonelyMinion;

		// Token: 0x040075ED RID: 30189
		private KBatchedAnimController[] animControllers;

		// Token: 0x040075EE RID: 30190
		private Light2D light;

		// Token: 0x040075EF RID: 30191
		private FilteredStorage storageFilter;

		// Token: 0x040075F0 RID: 30192
		private MeterController meter;

		// Token: 0x040075F1 RID: 30193
		private MeterController blinds;

		// Token: 0x040075F2 RID: 30194
		private int onBuildingActivatedHandle = -1;

		// Token: 0x040075F3 RID: 30195
		private Workable.WorkableEvent currentWorkState = Workable.WorkableEvent.WorkStopped;

		// Token: 0x040075F4 RID: 30196
		private Notification knockNotification;

		// Token: 0x040075F5 RID: 30197
		private KBatchedAnimController knocker;
	}

	// Token: 0x020016D7 RID: 5847
	public class ActiveStates : GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State
	{
		// Token: 0x060098C6 RID: 39110 RVA: 0x00389D61 File Offset: 0x00387F61
		public static void OnEnterStoryComplete(LonelyMinionHouse.Instance smi)
		{
			smi.CompleteEvent();
		}

		// Token: 0x040075F6 RID: 30198
		public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State StoryComplete;
	}
}
