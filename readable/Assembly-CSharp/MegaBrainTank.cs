using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
	// Token: 0x0600342B RID: 13355 RVA: 0x00127EE7 File Offset: 0x001260E7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x00127EF0 File Offset: 0x001260F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, base.gameObject.GetMyWorldId());
		base.smi.StartSM();
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
		base.GetComponent<Activatable>().SetWorkTime(5f);
		base.smi.JournalDelivery.refillMass = 25f;
		base.smi.JournalDelivery.FillToCapacity = true;
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x00127F80 File Offset: 0x00126180
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(-1503271301);
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x00127F94 File Offset: 0x00126194
	private void OnBuildingSelect(object obj)
	{
		if (!((Boxed<bool>)obj).value)
		{
			return;
		}
		if (!this.introDisplayed)
		{
			this.introDisplayed = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, new System.Action(this.DoInitialUnlock)));
		}
		base.smi.ShowEventCompleteUI(null);
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x00128007 File Offset: 0x00126207
	private void DoInitialUnlock()
	{
		Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial", true);
	}

	// Token: 0x04001F7F RID: 8063
	[Serialize]
	private bool introDisplayed;

	// Token: 0x020016E8 RID: 5864
	public class States : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank>
	{
		// Token: 0x060098FC RID: 39164 RVA: 0x0038AD40 File Offset: 0x00388F40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.root;
			this.root.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				if (!StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
				{
					smi.GoTo(this.common.dormant);
					return;
				}
				if (smi.IsHungry)
				{
					smi.GoTo(this.common.idle);
					return;
				}
				smi.GoTo(this.common.active);
			});
			this.common.Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.IncrementMeter(dt);
				if (smi.UnitsFromLastStore != 0)
				{
					smi.ShelveJournals(dt);
				}
				bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, true);
				smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag, null);
			}, UpdateRate.SIM_33ms, false);
			this.common.dormant.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.SetBonusActive(false);
				smi.ElementConverter.SetAllConsumedActive(false);
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, false);
				smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, false);
				smi.master.GetComponent<Light2D>().enabled = false;
			}).Exit(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, true);
				smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, true);
				RequireInputs component = smi.GetComponent<RequireInputs>();
				component.requireConduitHasMass = true;
				component.visualizeRequirements = RequireInputs.Requirements.All;
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.ActivateBrains(dt);
			}, UpdateRate.SIM_33ms, false).OnSignal(this.storyTraitCompleted, this.common.active);
			this.common.idle.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(false);
			}).UpdateTransition(this.common.active, (MegaBrainTank.StatesInstance smi, float _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.common.active.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(true);
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.Digest(dt);
			}, UpdateRate.SIM_33ms, false).UpdateTransition(this.common.idle, (MegaBrainTank.StatesInstance smi, float _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.StatBonus = new Effect("MegaBrainTankBonus", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
			object[,] stat_BONUSES = MegaBrainTankConfig.STAT_BONUSES;
			int length = stat_BONUSES.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				string attribute_id = stat_BONUSES[i, 0] as string;
				float? num = (float?)stat_BONUSES[i, 1];
				Units? units = (Units?)stat_BONUSES[i, 2];
				this.StatBonus.Add(new AttributeModifier(attribute_id, ModifierSet.ConvertValue(num.Value, units.Value), DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, false, false, true));
			}
		}

		// Token: 0x0400760F RID: 30223
		public MegaBrainTank.States.CommonState common;

		// Token: 0x04007610 RID: 30224
		public StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.Signal storyTraitCompleted;

		// Token: 0x04007611 RID: 30225
		public Effect StatBonus;

		// Token: 0x02002917 RID: 10519
		public class CommonState : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State
		{
			// Token: 0x0400B5A3 RID: 46499
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State dormant;

			// Token: 0x0400B5A4 RID: 46500
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State idle;

			// Token: 0x0400B5A5 RID: 46501
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State active;
		}
	}

	// Token: 0x020016E9 RID: 5865
	public class StatesInstance : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.GameInstance
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060098FF RID: 39167 RVA: 0x0038B03E File Offset: 0x0038923E
		public KBatchedAnimController BrainController
		{
			get
			{
				return this.controllers[0];
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06009900 RID: 39168 RVA: 0x0038B048 File Offset: 0x00389248
		public KBatchedAnimController ShelfController
		{
			get
			{
				return this.controllers[1];
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06009901 RID: 39169 RVA: 0x0038B052 File Offset: 0x00389252
		// (set) Token: 0x06009902 RID: 39170 RVA: 0x0038B05A File Offset: 0x0038925A
		public Storage BrainStorage { get; private set; }

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06009903 RID: 39171 RVA: 0x0038B063 File Offset: 0x00389263
		// (set) Token: 0x06009904 RID: 39172 RVA: 0x0038B06B File Offset: 0x0038926B
		public KSelectable Selectable { get; private set; }

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06009905 RID: 39173 RVA: 0x0038B074 File Offset: 0x00389274
		// (set) Token: 0x06009906 RID: 39174 RVA: 0x0038B07C File Offset: 0x0038927C
		public Operational Operational { get; private set; }

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06009907 RID: 39175 RVA: 0x0038B085 File Offset: 0x00389285
		// (set) Token: 0x06009908 RID: 39176 RVA: 0x0038B08D File Offset: 0x0038928D
		public ElementConverter ElementConverter { get; private set; }

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06009909 RID: 39177 RVA: 0x0038B096 File Offset: 0x00389296
		// (set) Token: 0x0600990A RID: 39178 RVA: 0x0038B09E File Offset: 0x0038929E
		public ManualDeliveryKG JournalDelivery { get; private set; }

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x0600990B RID: 39179 RVA: 0x0038B0A7 File Offset: 0x003892A7
		// (set) Token: 0x0600990C RID: 39180 RVA: 0x0038B0AF File Offset: 0x003892AF
		public LoopingSounds BrainSounds { get; private set; }

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x0600990D RID: 39181 RVA: 0x0038B0B8 File Offset: 0x003892B8
		public bool IsHungry
		{
			get
			{
				return !this.ElementConverter.HasEnoughMassToStartConverting(true);
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x0600990E RID: 39182 RVA: 0x0038B0C9 File Offset: 0x003892C9
		public int TimeTilDigested
		{
			get
			{
				return (int)this.timeTilDigested;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x0600990F RID: 39183 RVA: 0x0038B0D2 File Offset: 0x003892D2
		public int ActivationProgress
		{
			get
			{
				return (int)(25f * this.meterFill);
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06009910 RID: 39184 RVA: 0x0038B0E1 File Offset: 0x003892E1
		public HashedString CurrentActivationAnim
		{
			get
			{
				return MegaBrainTankConfig.ACTIVATION_ANIMS[(int)(this.nextActiveBrain - 1)];
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06009911 RID: 39185 RVA: 0x0038B0F8 File Offset: 0x003892F8
		private HashedString currentActivationLoop
		{
			get
			{
				int num = (int)(this.nextActiveBrain - 1 + 5);
				return MegaBrainTankConfig.ACTIVATION_ANIMS[num];
			}
		}

		// Token: 0x06009912 RID: 39186 RVA: 0x0038B11C File Offset: 0x0038931C
		public StatesInstance(MegaBrainTank master) : base(master)
		{
			this.BrainSounds = base.GetComponent<LoopingSounds>();
			this.BrainStorage = base.GetComponent<Storage>();
			this.ElementConverter = base.GetComponent<ElementConverter>();
			this.JournalDelivery = base.GetComponent<ManualDeliveryKG>();
			this.Operational = base.GetComponent<Operational>();
			this.Selectable = base.GetComponent<KSelectable>();
			this.notifier = base.GetComponent<Notifier>();
			this.controllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>();
			this.meter = new MeterController(this.BrainController, "meter_oxygen_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
			this.fxLink = new KAnimLink(this.BrainController, this.ShelfController);
		}

		// Token: 0x06009913 RID: 39187 RVA: 0x0038B1E4 File Offset: 0x003893E4
		public override void StartSM()
		{
			this.InitializeEffectsList();
			base.StartSM();
			this.BrainController.onAnimComplete += this.OnAnimComplete;
			this.ShelfController.onAnimComplete += this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			this.brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP", false);
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			if (this.GetCurrentState() == base.sm.common.dormant)
			{
				this.meterFill = (this.targetProgress = unitsAvailable / 25f);
				this.meter.SetPositionPercent(this.meterFill);
				short num = (short)(5f * this.meterFill);
				if (num > 0)
				{
					this.nextActiveBrain = num;
					this.BrainSounds.StartSound(this.brainHum);
					this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)num);
					this.CompleteBrainActivation();
				}
				return;
			}
			this.timeTilDigested = unitsAvailable * 60f;
			this.meterFill = this.timeTilDigested - this.timeTilDigested % 0.04f;
			this.meterFill /= 1500f;
			this.meter.SetPositionPercent(this.meterFill);
			StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
			this.nextActiveBrain = 5;
			this.CompleteBrainActivation();
		}

		// Token: 0x06009914 RID: 39188 RVA: 0x0038B38C File Offset: 0x0038958C
		public override void StopSM(string reason)
		{
			this.BrainController.onAnimComplete -= this.OnAnimComplete;
			this.ShelfController.onAnimComplete -= this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			base.StopSM(reason);
		}

		// Token: 0x06009915 RID: 39189 RVA: 0x0038B3F8 File Offset: 0x003895F8
		private void InitializeEffectsList()
		{
			Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
			liveMinionIdentities.OnAdd += this.OnLiveMinionIdAdded;
			liveMinionIdentities.OnRemove += this.OnLiveMinionIdRemoved;
			MegaBrainTank.StatesInstance.minionEffects = new List<Effects>((liveMinionIdentities.Count > 32) ? liveMinionIdentities.Count : 32);
			for (int i = 0; i < liveMinionIdentities.Count; i++)
			{
				this.OnLiveMinionIdAdded(liveMinionIdentities[i]);
			}
		}

		// Token: 0x06009916 RID: 39190 RVA: 0x0038B46C File Offset: 0x0038966C
		private void OnLiveMinionIdAdded(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Add(component);
			if (this.GetCurrentState() == base.sm.common.active)
			{
				component.Add(base.sm.StatBonus, false);
			}
		}

		// Token: 0x06009917 RID: 39191 RVA: 0x0038B4B8 File Offset: 0x003896B8
		private void OnLiveMinionIdRemoved(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Remove(component);
		}

		// Token: 0x06009918 RID: 39192 RVA: 0x0038B4D8 File Offset: 0x003896D8
		public void SetBonusActive(bool active)
		{
			for (int i = 0; i < MegaBrainTank.StatesInstance.minionEffects.Count; i++)
			{
				if (active)
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Add(base.sm.StatBonus, false);
				}
				else
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Remove(base.sm.StatBonus);
				}
			}
		}

		// Token: 0x06009919 RID: 39193 RVA: 0x0038B538 File Offset: 0x00389738
		private void OnAnimComplete(HashedString anim)
		{
			if (anim == MegaBrainTankConfig.KACHUNK)
			{
				this.StoreJournals();
				return;
			}
			if ((anim == base.smi.CurrentActivationAnim || anim == MegaBrainTankConfig.ACTIVATE_ALL) && this.GetCurrentState() != base.sm.common.idle)
			{
				this.CompleteBrainActivation();
			}
		}

		// Token: 0x0600991A RID: 39194 RVA: 0x0038B598 File Offset: 0x00389798
		private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (state != Workable.WorkableEvent.WorkStarted)
			{
				this.ShelfController.Play(MegaBrainTankConfig.KACHUNK, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			FetchAreaChore.StatesInstance smi = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
			if (smi.IsNullOrStopped())
			{
				return;
			}
			GameObject gameObject = smi.sm.deliveryObject.Get(smi);
			if (gameObject == null)
			{
				return;
			}
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.UnitsFromLastStore = (short)component.PrimaryElement.Units;
			float num = Mathf.Clamp01(component.PrimaryElement.Units / 5f);
			this.BrainStorage.SetWorkTime(num * this.BrainStorage.storageWorkTime);
		}

		// Token: 0x0600991B RID: 39195 RVA: 0x0038B644 File Offset: 0x00389844
		public void ShelveJournals(float dt)
		{
			float num = this.lastRemainingTime - this.BrainStorage.WorkTimeRemaining;
			if (num <= 0f)
			{
				num = this.BrainStorage.storageWorkTime - this.BrainStorage.WorkTimeRemaining;
			}
			this.lastRemainingTime = this.BrainStorage.WorkTimeRemaining;
			if (this.BrainStorage.storageWorkTime / 5f - this.journalActivationTimer > 0.001f)
			{
				this.journalActivationTimer += num;
				return;
			}
			int num2 = -1;
			this.journalActivationTimer = 0f;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				byte b = (byte)(1 << i);
				bool flag = (this.activatedJournals & b) == 0;
				if (flag && num2 == -1)
				{
					num2 = i;
				}
				if (flag & UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					num2 = -1;
					this.activatedJournals |= b;
					this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], true);
					break;
				}
			}
			if (num2 != -1)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[num2], true);
			}
			this.UnitsFromLastStore -= 1;
		}

		// Token: 0x0600991C RID: 39196 RVA: 0x0038B778 File Offset: 0x00389978
		public void StoreJournals()
		{
			this.lastRemainingTime = 0f;
			this.activatedJournals = 0;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
			}
			this.ShelfController.PlayMode = KAnim.PlayMode.Paused;
			this.ShelfController.SetPositionPercent(0f);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.targetProgress = Mathf.Clamp01(unitsAvailable / 25f);
		}

		// Token: 0x0600991D RID: 39197 RVA: 0x0038B804 File Offset: 0x00389A04
		public void ActivateBrains(float dt)
		{
			if (this.currentlyActivating)
			{
				return;
			}
			this.currentlyActivating = ((float)this.nextActiveBrain / 5f - this.meterFill <= 0.001f);
			if (!this.currentlyActivating)
			{
				return;
			}
			this.BrainController.QueueAndSyncTransition(this.CurrentActivationAnim, KAnim.PlayMode.Once, 1f, 0f);
			if (this.nextActiveBrain > 0)
			{
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
			}
		}

		// Token: 0x0600991E RID: 39198 RVA: 0x0038B8A0 File Offset: 0x00389AA0
		public void CompleteBrainActivation()
		{
			this.BrainController.Play(this.currentActivationLoop, KAnim.PlayMode.Loop, 1f, 0f);
			this.nextActiveBrain += 1;
			this.currentlyActivating = false;
			if (this.nextActiveBrain > 5)
			{
				float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
				this.timeTilDigested = unitsAvailable * 60f;
				this.CompleteEvent();
			}
		}

		// Token: 0x0600991F RID: 39199 RVA: 0x0038B90C File Offset: 0x00389B0C
		public void Digest(float dt)
		{
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (this.targetProgress - this.meterFill > Mathf.Epsilon)
			{
				return;
			}
			this.targetProgress = 0f;
			float num = this.meterFill - this.timeTilDigested / 1500f;
			if (num >= 0.04f)
			{
				this.meterFill -= num - num % 0.04f;
				this.meter.SetPositionPercent(this.meterFill);
			}
		}

		// Token: 0x06009920 RID: 39200 RVA: 0x0038B99C File Offset: 0x00389B9C
		public void CleanTank(bool active)
		{
			this.SetBonusActive(active);
			base.GetComponent<Light2D>().enabled = active;
			this.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, this);
			this.ElementConverter.SetAllConsumedActive(active);
			this.BrainController.ClearQueue();
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (active)
			{
				this.nextActiveBrain = 5;
				this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
				return;
			}
			if (this.timeTilDigested < 0.016666668f)
			{
				this.BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
				this.timeTilDigested = 0f;
				this.meterFill = 0f;
				this.meter.SetPositionPercent(this.meterFill);
			}
			this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
			this.BrainSounds.StopSound(this.brainHum);
		}

		// Token: 0x06009921 RID: 39201 RVA: 0x0038BAD8 File Offset: 0x00389CD8
		public bool IncrementMeter(float dt)
		{
			if (this.targetProgress - this.meterFill <= Mathf.Epsilon)
			{
				return false;
			}
			this.meterFill += Mathf.Lerp(0f, 1f, 0.04f * dt);
			if (1f - this.meterFill <= 0.001f)
			{
				this.meterFill = 1f;
			}
			this.meter.SetPositionPercent(this.meterFill);
			return this.targetProgress - this.meterFill > 0.001f;
		}

		// Token: 0x06009922 RID: 39202 RVA: 0x0038BB64 File Offset: 0x00389D64
		public void CompleteEvent()
		{
			this.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress, false);
			this.Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			base.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
			this.eventComplete = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.ShowEventCompleteUI));
			this.notifier.Add(this.eventComplete, "");
		}

		// Token: 0x06009923 RID: 39203 RVA: 0x0038BC68 File Offset: 0x00389E68
		public void ShowEventCompleteUI(object _ = null)
		{
			if (this.eventComplete == null)
			{
				return;
			}
			base.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			this.notifier.Remove(this.eventComplete);
			this.eventComplete = null;
			Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed", true);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnCompleteStorySequence),
				CanCompleteCB = null
			});
		}

		// Token: 0x06009924 RID: 39204 RVA: 0x0038BD70 File Offset: 0x00389F70
		private void OnCompleteStorySequence()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, keepsakeSpawnPosition);
			this.eventInfo = null;
			base.sm.storyTraitCompleted.Trigger(this);
		}

		// Token: 0x04007612 RID: 30226
		private static List<Effects> minionEffects;

		// Token: 0x04007619 RID: 30233
		public short UnitsFromLastStore;

		// Token: 0x0400761A RID: 30234
		private float meterFill = 0.04f;

		// Token: 0x0400761B RID: 30235
		private float targetProgress;

		// Token: 0x0400761C RID: 30236
		private float timeTilDigested;

		// Token: 0x0400761D RID: 30237
		private float journalActivationTimer;

		// Token: 0x0400761E RID: 30238
		private float lastRemainingTime;

		// Token: 0x0400761F RID: 30239
		private byte activatedJournals;

		// Token: 0x04007620 RID: 30240
		private bool currentlyActivating;

		// Token: 0x04007621 RID: 30241
		private short nextActiveBrain = 1;

		// Token: 0x04007622 RID: 30242
		private string brainHum;

		// Token: 0x04007623 RID: 30243
		private KBatchedAnimController[] controllers;

		// Token: 0x04007624 RID: 30244
		private KAnimLink fxLink;

		// Token: 0x04007625 RID: 30245
		private MeterController meter;

		// Token: 0x04007626 RID: 30246
		private EventInfoData eventInfo;

		// Token: 0x04007627 RID: 30247
		private Notification eventComplete;

		// Token: 0x04007628 RID: 30248
		private Notifier notifier;
	}
}
