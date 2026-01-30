using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000774 RID: 1908
public class GravitasCreatureManipulator : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>
{
	// Token: 0x06003086 RID: 12422 RVA: 0x00118190 File Offset: 0x00116390
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.UpdateMeter();
		}).EventHandler(GameHashes.BuildingActivated, delegate(GravitasCreatureManipulator.Instance smi, object activated)
		{
			if (((Boxed<bool>)activated).value)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational.idle, (GravitasCreatureManipulator.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.inoperational, (GravitasCreatureManipulator.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(new StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback(GravitasCreatureManipulator.CheckForCritter)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWaiting, null).ParamTransition<GameObject>(this.creatureTarget, this.operational.capture, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && !smi.IsCritterStored).ParamTransition<GameObject>(this.creatureTarget, this.operational.working.pre, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && smi.IsCritterStored).ParamTransition<float>(this.cooldownTimer, this.operational.cooldown, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsGTZero);
		this.operational.capture.PlayAnim("working_capture").OnAnimQueueComplete(this.operational.working.pre);
		this.operational.working.DefaultState(this.operational.working.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWorking, null);
		this.operational.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.working.loop).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.StoreCreature();
		}).Exit(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.sm.workingTimer.Set(smi.def.workingDuration, smi, false);
		}).OnTargetLost(this.creatureTarget, this.operational.idle).Target(this.creatureTarget).ToggleStationaryIdling();
		this.operational.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.workingTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.workingTimer, this.operational.working.pst, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero).OnTargetLost(this.creatureTarget, this.operational.idle);
		this.operational.working.pst.PlayAnim("working_pst").Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).OnAnimQueueComplete(this.operational.cooldown);
		GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State state = this.operational.cooldown.PlayAnim("working_cooldown", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.cooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.cooldownTimer, this.operational.idle, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero);
		string name = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.NAME;
		string tooltip = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_string_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.Processing);
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_tooltip_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.ProcessingTooltip);
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, resolve_tooltip_callback, main);
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x001185F0 File Offset: 0x001167F0
	private static string Processing(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{percent}", GameUtil.GetFormattedPercent((1f - smi.sm.cooldownTimer.Get(smi) / smi.def.cooldownDuration) * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x0011862C File Offset: 0x0011682C
	private static string ProcessingTooltip(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{timeleft}", GameUtil.GetFormattedTime(smi.sm.cooldownTimer.Get(smi), "F0"));
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x00118654 File Offset: 0x00116854
	private static void CheckForCritter(GravitasCreatureManipulator.Instance smi)
	{
		if (smi.sm.creatureTarget.IsNull(smi))
		{
			GameObject gameObject = Grid.Objects[smi.pickupCell, 3];
			if (gameObject != null)
			{
				ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
				while (objectLayerListItem != null)
				{
					GameObject gameObject2 = objectLayerListItem.gameObject;
					Pickupable pickupable = objectLayerListItem.pickupable;
					objectLayerListItem = objectLayerListItem.nextItem;
					if (!(gameObject2 == null) && smi.IsAccepted(pickupable.KPrefabID))
					{
						smi.SetCritterTarget(gameObject2);
						return;
					}
				}
			}
		}
	}

	// Token: 0x04001CE8 RID: 7400
	public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State inoperational;

	// Token: 0x04001CE9 RID: 7401
	public GravitasCreatureManipulator.ActiveStates operational;

	// Token: 0x04001CEA RID: 7402
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.TargetParameter creatureTarget;

	// Token: 0x04001CEB RID: 7403
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter cooldownTimer;

	// Token: 0x04001CEC RID: 7404
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter workingTimer;

	// Token: 0x02001679 RID: 5753
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040074F2 RID: 29938
		public CellOffset pickupOffset;

		// Token: 0x040074F3 RID: 29939
		public CellOffset dropOffset;

		// Token: 0x040074F4 RID: 29940
		public int numSpeciesToUnlockMorphMode;

		// Token: 0x040074F5 RID: 29941
		public float workingDuration;

		// Token: 0x040074F6 RID: 29942
		public float cooldownDuration;
	}

	// Token: 0x0200167A RID: 5754
	public class WorkingStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x040074F7 RID: 29943
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pre;

		// Token: 0x040074F8 RID: 29944
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State loop;

		// Token: 0x040074F9 RID: 29945
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pst;
	}

	// Token: 0x0200167B RID: 5755
	public class ActiveStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x040074FA RID: 29946
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State idle;

		// Token: 0x040074FB RID: 29947
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State capture;

		// Token: 0x040074FC RID: 29948
		public GravitasCreatureManipulator.WorkingStates working;

		// Token: 0x040074FD RID: 29949
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State cooldown;
	}

	// Token: 0x0200167C RID: 5756
	public new class Instance : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.GameInstance
	{
		// Token: 0x06009754 RID: 38740 RVA: 0x00383F1C File Offset: 0x0038211C
		public Instance(IStateMachineTarget master, GravitasCreatureManipulator.Def def) : base(master, def)
		{
			this.pickupCell = Grid.OffsetCell(Grid.PosToCell(master.gameObject), base.smi.def.pickupOffset);
			this.m_partitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator", base.gameObject, this.pickupCell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectCreature));
			this.m_largeCreaturePartitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator.large", base.gameObject, Grid.CellLeft(this.pickupCell), GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectLargeCreature));
			this.m_progressMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TileFront, Array.Empty<string>());
		}

		// Token: 0x06009755 RID: 38741 RVA: 0x00383FF8 File Offset: 0x003821F8
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateStatusItems();
			this.UpdateMeter();
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.CreatureManipulator, base.gameObject.GetMyWorldId());
			if (this.ScannedSpecies.Count >= base.smi.def.numSpeciesToUnlockMorphMode)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
			this.TryShowCompletedNotification();
			this.onBuildingSelectHandle = base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.CreatureManipulator);
		}

		// Token: 0x06009756 RID: 38742 RVA: 0x003840AD File Offset: 0x003822AD
		public override void StopSM(string reason)
		{
			base.Unsubscribe(ref this.onBuildingSelectHandle);
			base.StopSM(reason);
		}

		// Token: 0x06009757 RID: 38743 RVA: 0x003840C4 File Offset: 0x003822C4
		private void OnBuildingSelect(object obj)
		{
			if (!((Boxed<bool>)obj).value)
			{
				return;
			}
			if (!this.m_introPopupSeen)
			{
				this.ShowIntroNotification();
			}
			if (this.m_endNotification != null)
			{
				this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06009758 RID: 38744 RVA: 0x00384110 File Offset: 0x00382310
		public bool IsMorphMode
		{
			get
			{
				return this.m_morphModeUnlocked;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06009759 RID: 38745 RVA: 0x00384118 File Offset: 0x00382318
		public bool IsCritterStored
		{
			get
			{
				return this.m_storage.Count > 0;
			}
		}

		// Token: 0x0600975A RID: 38746 RVA: 0x00384128 File Offset: 0x00382328
		private void UpdateStatusItems()
		{
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorProgress, !this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphMode, this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphModeLocked, !this.IsMorphMode, this);
		}

		// Token: 0x0600975B RID: 38747 RVA: 0x0038419C File Offset: 0x0038239C
		public void UpdateMeter()
		{
			this.m_progressMeter.SetPositionPercent(Mathf.Clamp01((float)this.ScannedSpecies.Count / (float)base.smi.def.numSpeciesToUnlockMorphMode));
		}

		// Token: 0x0600975C RID: 38748 RVA: 0x003841CC File Offset: 0x003823CC
		public bool IsAccepted(KPrefabID kpid)
		{
			return kpid.HasTag(GameTags.Creature) && !kpid.HasTag(GameTags.Robot) && kpid.PrefabTag != GameTags.Creature;
		}

		// Token: 0x0600975D RID: 38749 RVA: 0x003841FC File Offset: 0x003823FC
		private void DetectLargeCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable == null)
			{
				return;
			}
			if (pickupable.GetComponent<KCollider2D>().bounds.size.x > 1.5f)
			{
				this.DetectCreature(obj);
			}
		}

		// Token: 0x0600975E RID: 38750 RVA: 0x00384240 File Offset: 0x00382440
		private void DetectCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && this.IsAccepted(pickupable.KPrefabID) && base.smi.sm.creatureTarget.IsNull(base.smi) && base.smi.IsInsideState(base.smi.sm.operational.idle))
			{
				this.SetCritterTarget(pickupable.gameObject);
			}
		}

		// Token: 0x0600975F RID: 38751 RVA: 0x003842B6 File Offset: 0x003824B6
		public void SetCritterTarget(GameObject go)
		{
			base.smi.sm.creatureTarget.Set(go.gameObject, base.smi, false);
		}

		// Token: 0x06009760 RID: 38752 RVA: 0x003842DC File Offset: 0x003824DC
		public void StoreCreature()
		{
			GameObject go = base.smi.sm.creatureTarget.Get(base.smi);
			this.m_storage.Store(go, false, false, true, false);
		}

		// Token: 0x06009761 RID: 38753 RVA: 0x00384318 File Offset: 0x00382518
		public void DropCritter()
		{
			List<GameObject> list = new List<GameObject>();
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.smi), Grid.SceneLayer.Creatures);
			base.smi.def.dropOffset.ToVector3();
			if (this.m_storage.items.Count > 0 && this.m_storage.items[0] != null)
			{
				KBoxCollider2D component = this.m_storage.items[0].GetComponent<KBoxCollider2D>();
				if (component != null && component.size.x > 1.5f && component.PrefabID() != "Moo")
				{
					position.x += 0.5f;
				}
			}
			this.m_storage.DropAll(position, false, false, base.smi.def.dropOffset.ToVector3(), true, list);
			foreach (GameObject gameObject in list)
			{
				CreatureBrain component2 = gameObject.GetComponent<CreatureBrain>();
				if (!(component2 == null))
				{
					this.Scan(component2.species);
					if (component2.HasTag(GameTags.OriginalCreature) && this.IsMorphMode)
					{
						this.SpawnMorph(component2);
					}
					else
					{
						gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("idle_loop");
					}
				}
			}
			base.smi.sm.creatureTarget.Set(null, base.smi);
		}

		// Token: 0x06009762 RID: 38754 RVA: 0x003844AC File Offset: 0x003826AC
		private void Scan(Tag species)
		{
			if (this.ScannedSpecies.Add(species))
			{
				base.gameObject.Trigger(1980521255, null);
				this.UpdateStatusItems();
				this.UpdateMeter();
				this.ShowCritterScannedNotification(species);
			}
			this.TryShowCompletedNotification();
		}

		// Token: 0x06009763 RID: 38755 RVA: 0x003844E8 File Offset: 0x003826E8
		public void SpawnMorph(Brain brain)
		{
			Tag tag = Tag.Invalid;
			BabyMonitor.Instance smi = brain.GetSMI<BabyMonitor.Instance>();
			FertilityMonitor.Instance smi2 = brain.GetSMI<FertilityMonitor.Instance>();
			bool flag = smi != null;
			bool flag2 = smi2 != null;
			if (flag2)
			{
				tag = FertilityMonitor.EggBreedingRoll(smi2.breedingChances, true);
			}
			else if (flag)
			{
				FertilityMonitor.Def def = Assets.GetPrefab(smi.def.adultPrefab).GetDef<FertilityMonitor.Def>();
				if (def == null)
				{
					return;
				}
				tag = FertilityMonitor.EggBreedingRoll(def.initialBreedingWeights, true);
			}
			if (!tag.IsValid)
			{
				return;
			}
			Tag tag2 = Assets.GetPrefab(tag).GetDef<IncubationMonitor.Def>().spawnedCreature;
			if (flag2)
			{
				tag2 = Assets.GetPrefab(tag2).GetDef<BabyMonitor.Def>().adultPrefab;
			}
			Vector3 position = brain.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(tag2), position);
			gameObject.SetActive(true);
			gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			foreach (AmountInstance amountInstance in brain.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			gameObject.Trigger(-2027483228, brain.gameObject);
			KSelectable component = brain.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.sm.cooldownTimer.Set(base.smi.def.cooldownDuration, base.smi, false);
			brain.gameObject.DeleteObject();
		}

		// Token: 0x06009764 RID: 38756 RVA: 0x003846E8 File Offset: 0x003828E8
		public void ShowIntroNotification()
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_initial", true);
			this.m_introPopupSeen = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "crittermanipulatoractivate_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, null));
		}

		// Token: 0x06009765 RID: 38757 RVA: 0x00384744 File Offset: 0x00382944
		public void ShowCritterScannedNotification(Tag species)
		{
			GravitasCreatureManipulator.Instance.<>c__DisplayClass30_0 CS$<>8__locals1 = new GravitasCreatureManipulator.Instance.<>c__DisplayClass30_0();
			CS$<>8__locals1.species = species;
			CS$<>8__locals1.<>4__this = this;
			string unlockID = GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(CS$<>8__locals1.species);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowCritterScannedNotification>g__ShowCritterScannedNotificationAndWaitForClick|1().Then(delegate
			{
				GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(CS$<>8__locals1.species);
			});
		}

		// Token: 0x06009766 RID: 38758 RVA: 0x0038479C File Offset: 0x0038299C
		public static void ShowLoreUnlockedPopup(Tag species)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species)) != null;
			Option<string> bodyContentForSpeciesTag = GravitasCreatureManipulatorConfig.GetBodyContentForSpeciesTag(species);
			if (flag && bodyContentForSpeciesTag.HasValue)
			{
				infoDialogScreen.AddPlainText(bodyContentForSpeciesTag.Value).AddOption(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITCRITTERMANIPULATOR"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x06009767 RID: 38759 RVA: 0x0038481C File Offset: 0x00382A1C
		public void TryShowCompletedNotification()
		{
			if (this.ScannedSpecies.Count < base.smi.def.numSpeciesToUnlockMorphMode)
			{
				return;
			}
			if (this.IsMorphMode)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.BUTTON, "crittermanipulatormorphmode_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.UnlockMorphMode));
			base.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification, "");
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
		}

		// Token: 0x06009768 RID: 38760 RVA: 0x003848E0 File Offset: 0x00382AE0
		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_endNotification = null;
		}

		// Token: 0x06009769 RID: 38761 RVA: 0x00384934 File Offset: 0x00382B34
		public void UnlockMorphMode(object _)
		{
			if (this.m_morphModeUnlocked)
			{
				return;
			}
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_complete", true);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_morphModeUnlocked = true;
			this.UpdateStatusItems();
			this.ClearEndNotification();
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, base.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data
			{
				WorldId = base.smi.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnStorySequenceComplete),
				CanCompleteCB = null
			});
		}

		// Token: 0x0600976A RID: 38762 RVA: 0x00384A38 File Offset: 0x00382C38
		private void OnStorySequenceComplete()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, keepsakeSpawnPosition);
			this.eventInfo = null;
		}

		// Token: 0x0600976B RID: 38763 RVA: 0x00384A85 File Offset: 0x00382C85
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.m_partitionEntry);
			GameScenePartitioner.Instance.Free(ref this.m_largeCreaturePartitionEntry);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
		}

		// Token: 0x040074FE RID: 29950
		public int pickupCell;

		// Token: 0x040074FF RID: 29951
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04007500 RID: 29952
		[Serialize]
		public HashSet<Tag> ScannedSpecies = new HashSet<Tag>();

		// Token: 0x04007501 RID: 29953
		[Serialize]
		private bool m_introPopupSeen;

		// Token: 0x04007502 RID: 29954
		[Serialize]
		private bool m_morphModeUnlocked;

		// Token: 0x04007503 RID: 29955
		private EventInfoData eventInfo;

		// Token: 0x04007504 RID: 29956
		private Notification m_endNotification;

		// Token: 0x04007505 RID: 29957
		private MeterController m_progressMeter;

		// Token: 0x04007506 RID: 29958
		private HandleVector<int>.Handle m_partitionEntry;

		// Token: 0x04007507 RID: 29959
		private HandleVector<int>.Handle m_largeCreaturePartitionEntry;

		// Token: 0x04007508 RID: 29960
		private int onBuildingSelectHandle;
	}
}
