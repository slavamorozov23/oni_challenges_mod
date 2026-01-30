using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200071F RID: 1823
public class ClusterTelescope : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>
{
	// Token: 0x06002D8F RID: 11663 RVA: 0x00107F78 File Offset: 0x00106178
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.ready.no_visibility;
		this.root.Update(delegate(ClusterTelescope.Instance smi, float dt)
		{
			KSelectable component = smi.GetComponent<KSelectable>();
			bool flag = Mathf.Approximately(0f, smi.PercentClear) || smi.PercentClear < 0f;
			bool flag2 = Mathf.Approximately(1f, smi.PercentClear) || smi.PercentClear > 1f;
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, flag, smi);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, !flag && !flag2, smi);
		}, UpdateRate.SIM_200ms, false);
		this.ready.DoNothing();
		this.ready.no_visibility.UpdateTransition(this.ready.ready_to_work, (ClusterTelescope.Instance smi, float dt) => smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false);
		this.ready.ready_to_work.UpdateTransition(this.ready.no_visibility, (ClusterTelescope.Instance smi, float dt) => !smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false).DefaultState(this.ready.ready_to_work.decide);
		this.ready.ready_to_work.decide.EnterTransition(this.ready.ready_to_work.identifyMeteorShower, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EnterTransition(this.ready.ready_to_work.revealTile, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnRevealingTile()).EnterTransition(this.all_work_complete, (ClusterTelescope.Instance smi) => !smi.IsAnyAvailableWorkToBeDone());
		this.ready.ready_to_work.identifyMeteorShower.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi, StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter param) => !smi.ShouldBeWorkingOnMeteorIdentification()).ParamTransition<GameObject>(this.meteorShowerTarget, this.ready.ready_to_work.decide, GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.IsNull).EventTransition(GameHashes.ClusterMapMeteorShowerIdentified, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateIdentifyMeteorChore(), this.ready.no_visibility);
		this.ready.ready_to_work.revealTile.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi, StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter param) => smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterFogOfWarRevealed, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnRevealingTile()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateRevealTileChore(), this.ready.no_visibility);
		this.all_work_complete.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.no_visibility, (ClusterTelescope.Instance smi, StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter param) => smi.IsAnyAvailableWorkToBeDone()).ToggleMainStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeAllWorkComplete, null).EventTransition(GameHashes.ClusterLocationChanged, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification());
	}

	// Token: 0x04001B14 RID: 6932
	public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State all_work_complete;

	// Token: 0x04001B15 RID: 6933
	public ClusterTelescope.ReadyStates ready;

	// Token: 0x04001B16 RID: 6934
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.TargetParameter meteorShowerTarget;

	// Token: 0x04001B17 RID: 6935
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Signal MeteorIdenificationPriorityChangeSignal;

	// Token: 0x020015F0 RID: 5616
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400732F RID: 29487
		public int clearScanCellRadius = 15;

		// Token: 0x04007330 RID: 29488
		public int analyzeClusterRadius = 3;

		// Token: 0x04007331 RID: 29489
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04007332 RID: 29490
		public bool providesOxygen;

		// Token: 0x04007333 RID: 29491
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x020015F1 RID: 5617
	public class WorkStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04007334 RID: 29492
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State decide;

		// Token: 0x04007335 RID: 29493
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State identifyMeteorShower;

		// Token: 0x04007336 RID: 29494
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State revealTile;
	}

	// Token: 0x020015F2 RID: 5618
	public class ReadyStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04007337 RID: 29495
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State no_visibility;

		// Token: 0x04007338 RID: 29496
		public ClusterTelescope.WorkStates ready_to_work;
	}

	// Token: 0x020015F3 RID: 5619
	public new class Instance : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.GameInstance, ICheckboxControl, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06009526 RID: 38182 RVA: 0x0037B218 File Offset: 0x00379418
		public float PercentClear
		{
			get
			{
				return this.m_percentClear;
			}
		}

		// Token: 0x06009527 RID: 38183 RVA: 0x0037B220 File Offset: 0x00379420
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.m_percentClear;
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06009528 RID: 38184 RVA: 0x0037B228 File Offset: 0x00379428
		private bool hasMeteorShowerTarget
		{
			get
			{
				return this.meteorShowerTarget != null;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06009529 RID: 38185 RVA: 0x0037B233 File Offset: 0x00379433
		private ClusterMapMeteorShower.Instance meteorShowerTarget
		{
			get
			{
				GameObject gameObject = base.sm.meteorShowerTarget.Get(this);
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetSMI<ClusterMapMeteorShower.Instance>();
			}
		}

		// Token: 0x0600952A RID: 38186 RVA: 0x0037B251 File Offset: 0x00379451
		public Instance(IStateMachineTarget smi, ClusterTelescope.Def def) : base(smi, def)
		{
			this.workableOverrideAnims = def.workableOverrideAnims;
			this.providesOxygen = def.providesOxygen;
		}

		// Token: 0x0600952B RID: 38187 RVA: 0x0037B27A File Offset: 0x0037947A
		public bool ShouldBeWorkingOnRevealingTile()
		{
			return this.CheckHasAnalyzeTarget() && (!this.allowMeteorIdentification || !this.CheckHasValidMeteorTarget());
		}

		// Token: 0x0600952C RID: 38188 RVA: 0x0037B299 File Offset: 0x00379499
		public bool ShouldBeWorkingOnMeteorIdentification()
		{
			return this.allowMeteorIdentification && this.CheckHasValidMeteorTarget();
		}

		// Token: 0x0600952D RID: 38189 RVA: 0x0037B2AB File Offset: 0x003794AB
		public bool IsAnyAvailableWorkToBeDone()
		{
			return this.CheckHasAnalyzeTarget() || this.ShouldBeWorkingOnMeteorIdentification();
		}

		// Token: 0x0600952E RID: 38190 RVA: 0x0037B2C0 File Offset: 0x003794C0
		public bool CheckHasValidMeteorTarget()
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.HasValidMeteor())
			{
				return true;
			}
			ClusterMapMeteorShower.Instance instance = null;
			AxialI myWorldLocation = this.GetMyWorldLocation();
			ClusterGrid.Instance.GetVisibleUnidentifiedMeteorShowerWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out instance);
			base.sm.meteorShowerTarget.Set((instance == null) ? null : instance.gameObject, this, false);
			return instance != null;
		}

		// Token: 0x0600952F RID: 38191 RVA: 0x0037B328 File Offset: 0x00379528
		public bool CheckHasAnalyzeTarget()
		{
			ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.m_hasAnalyzeTarget && !smi.IsLocationRevealed(this.m_analyzeTarget))
			{
				return true;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			this.m_hasAnalyzeTarget = smi.GetUnrevealedLocationWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out this.m_analyzeTarget);
			return this.m_hasAnalyzeTarget;
		}

		// Token: 0x06009530 RID: 38192 RVA: 0x0037B384 File Offset: 0x00379584
		private bool HasValidMeteor()
		{
			if (!this.hasMeteorShowerTarget)
			{
				return false;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			bool flag = ClusterGrid.Instance.IsInRange(this.meteorShowerTarget.ClusterGridPosition(), myWorldLocation, base.def.analyzeClusterRadius);
			bool flag2 = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().IsLocationRevealed(this.meteorShowerTarget.ClusterGridPosition());
			bool hasBeenIdentified = this.meteorShowerTarget.HasBeenIdentified;
			return flag && flag2 && !hasBeenIdentified;
		}

		// Token: 0x06009531 RID: 38193 RVA: 0x0037B3FC File Offset: 0x003795FC
		public Chore CreateRevealTileChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeWorkable>(Db.Get().ChoreTypes.Research, this.m_workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06009532 RID: 38194 RVA: 0x0037B44C File Offset: 0x0037964C
		public Chore CreateIdentifyMeteorChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable>(Db.Get().ChoreTypes.Research, this.m_identifyMeteorWorkable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06009533 RID: 38195 RVA: 0x0037B49A File Offset: 0x0037969A
		public ClusterMapMeteorShower.Instance GetMeteorTarget()
		{
			return this.meteorShowerTarget;
		}

		// Token: 0x06009534 RID: 38196 RVA: 0x0037B4A2 File Offset: 0x003796A2
		public AxialI GetAnalyzeTarget()
		{
			global::Debug.Assert(this.m_hasAnalyzeTarget, "GetAnalyzeTarget called but this telescope has no target assigned.");
			return this.m_analyzeTarget;
		}

		// Token: 0x06009535 RID: 38197 RVA: 0x0037B4BC File Offset: 0x003796BC
		public bool HasSkyVisibility()
		{
			ValueTuple<bool, float> visibilityOf = base.def.skyVisibilityInfo.GetVisibilityOf(base.gameObject);
			bool item = visibilityOf.Item1;
			float item2 = visibilityOf.Item2;
			this.m_percentClear = item2;
			return item;
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06009536 RID: 38198 RVA: 0x0037B4F4 File Offset: 0x003796F4
		public string CheckboxTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06009537 RID: 38199 RVA: 0x0037B4FB File Offset: 0x003796FB
		public string CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_METEORS;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06009538 RID: 38200 RVA: 0x0037B507 File Offset: 0x00379707
		public string CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_TOOLTIP_METEORS;
			}
		}

		// Token: 0x06009539 RID: 38201 RVA: 0x0037B513 File Offset: 0x00379713
		public bool GetCheckboxValue()
		{
			return this.allowMeteorIdentification;
		}

		// Token: 0x0600953A RID: 38202 RVA: 0x0037B51B File Offset: 0x0037971B
		public void SetCheckboxValue(bool value)
		{
			this.allowMeteorIdentification = value;
			base.sm.MeteorIdenificationPriorityChangeSignal.Trigger(this);
		}

		// Token: 0x04007339 RID: 29497
		private float m_percentClear;

		// Token: 0x0400733A RID: 29498
		[Serialize]
		public bool allowMeteorIdentification = true;

		// Token: 0x0400733B RID: 29499
		[Serialize]
		private bool m_hasAnalyzeTarget;

		// Token: 0x0400733C RID: 29500
		[Serialize]
		private AxialI m_analyzeTarget;

		// Token: 0x0400733D RID: 29501
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeWorkable m_workable;

		// Token: 0x0400733E RID: 29502
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable m_identifyMeteorWorkable;

		// Token: 0x0400733F RID: 29503
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04007340 RID: 29504
		public bool providesOxygen;
	}

	// Token: 0x020015F4 RID: 5620
	public class ClusterTelescopeWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x0600953B RID: 38203 RVA: 0x0037B538 File Offset: 0x00379738
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x0600953C RID: 38204 RVA: 0x0037B5E3 File Offset: 0x003797E3
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x0600953D RID: 38205 RVA: 0x0037B604 File Offset: 0x00379804
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x0600953E RID: 38206 RVA: 0x0037B668 File Offset: 0x00379868
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(() => this.m_fowManager.GetRevealCompleteFraction(this.currentTarget));
				this.currentTarget = this.m_telescope.GetAnalyzeTarget();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().SetTargetMeteorShower(null);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					if (component2 != null)
					{
						component2.AddGasProvider(this);
					}
					worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				if (component2 != null)
				{
					component2.RemoveGasProvider(this);
				}
				worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
		}

		// Token: 0x0600953F RID: 38207 RVA: 0x0037B84C File Offset: 0x00379A4C
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06009540 RID: 38208 RVA: 0x0037B8A7 File Offset: 0x00379AA7
		public override float GetEfficiencyMultiplier(WorkerBase worker)
		{
			return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.m_telescope.PercentClear);
		}

		// Token: 0x06009541 RID: 38209 RVA: 0x0037B8C4 File Offset: 0x00379AC4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			AxialI analyzeTarget = this.m_telescope.GetAnalyzeTarget();
			bool flag = false;
			if (analyzeTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(analyzeTarget);
				}
				this.currentTarget = analyzeTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
				}
			}
			this.checkMarkerTimer += dt;
			float num = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL / ROCKETRY.CLUSTER_FOW.DEFAULT_CYCLES_PER_REVEAL / 600f;
			float points = dt * num;
			if (this.m_fowManager.EarnRevealPointsForLocation(this.currentTarget, points))
			{
				Util.KInstantiate(Assets.GetPrefab(DatabankHelper.ID), base.transform.position + Vector3.up * 2f + Vector3.left, Quaternion.identity, null, null, true, 23).SetActive(true);
				Util.KInstantiate(Assets.GetPrefab(DatabankHelper.ID), base.transform.position + Vector3.up * 2f + Vector3.right, Quaternion.identity, null, null, true, 23).SetActive(true);
				Util.KInstantiate(Assets.GetPrefab(DatabankHelper.ID), base.transform.position + Vector3.up * 2f + Vector3.zero, Quaternion.identity, null, null, true, 23).SetActive(true);
				PopFXManager.Instance.SpawnFX(global::Def.GetUISprite(DatabankHelper.ID, "ui", false).first, DatabankHelper.NAME_PLURAL, base.transform, Vector3.up * 2f, 1.5f, true, false);
			}
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06009542 RID: 38210 RVA: 0x0037BAFF File Offset: 0x00379CFF
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
		}

		// Token: 0x06009543 RID: 38211 RVA: 0x0037BB08 File Offset: 0x00379D08
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009544 RID: 38212 RVA: 0x0037BB0A File Offset: 0x00379D0A
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009545 RID: 38213 RVA: 0x0037BB0C File Offset: 0x00379D0C
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06009546 RID: 38214 RVA: 0x0037BB0F File Offset: 0x00379D0F
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06009547 RID: 38215 RVA: 0x0037BB14 File Offset: 0x00379D14
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			float mass = gameObject.GetComponent<PrimaryElement>().Mass;
			float num = 0f;
			float temperature = 0f;
			SimHashes elementConsumed = SimHashes.Vacuum;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			bool result = num >= amount;
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
			return result;
		}

		// Token: 0x06009548 RID: 38216 RVA: 0x0037BBA8 File Offset: 0x00379DA8
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement == null || primaryElement.Mass == 0f;
		}

		// Token: 0x06009549 RID: 38217 RVA: 0x0037BBF8 File Offset: 0x00379DF8
		public bool HasOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement != null && primaryElement.Mass > 0f;
		}

		// Token: 0x0600954A RID: 38218 RVA: 0x0037BC48 File Offset: 0x00379E48
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x04007341 RID: 29505
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04007342 RID: 29506
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04007343 RID: 29507
		private GameObject telescopeTargetMarker;

		// Token: 0x04007344 RID: 29508
		private AxialI currentTarget;

		// Token: 0x04007345 RID: 29509
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04007346 RID: 29510
		private AttributeModifier radiationShielding;

		// Token: 0x04007347 RID: 29511
		private float checkMarkerTimer;

		// Token: 0x04007348 RID: 29512
		private float checkMarkerFrequency = 1f;
	}

	// Token: 0x020015F5 RID: 5621
	public class ClusterTelescopeIdentifyMeteorWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x0600954D RID: 38221 RVA: 0x0037BC74 File Offset: 0x00379E74
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x0600954E RID: 38222 RVA: 0x0037BD1F File Offset: 0x00379F1F
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x0600954F RID: 38223 RVA: 0x0037BD40 File Offset: 0x00379F40
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06009550 RID: 38224 RVA: 0x0037BDA4 File Offset: 0x00379FA4
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(delegate
				{
					if (this.currentTarget == null)
					{
						return 0f;
					}
					return this.currentTarget.IdentifyingProgress;
				});
				this.currentTarget = this.m_telescope.GetMeteorTarget();
				AxialI axialI = this.currentTarget.ClusterGridPosition();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					TelescopeTarget component4 = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component4.Init(axialI);
					component4.SetTargetMeteorShower(this.currentTarget);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					component2.AddGasProvider(this);
					component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				component2.RemoveGasProvider(this);
				component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
		}

		// Token: 0x06009551 RID: 38225 RVA: 0x0037BF78 File Offset: 0x0037A178
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06009552 RID: 38226 RVA: 0x0037BFD4 File Offset: 0x0037A1D4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			ClusterMapMeteorShower.Instance meteorTarget = this.m_telescope.GetMeteorTarget();
			AxialI axialI = meteorTarget.ClusterGridPosition();
			bool flag = false;
			if (meteorTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					TelescopeTarget component = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component.Init(axialI);
					component.SetTargetMeteorShower(meteorTarget);
				}
				this.currentTarget = meteorTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(axialI);
				}
			}
			this.checkMarkerTimer += dt;
			float num = 20f;
			float points = dt / num;
			this.currentTarget.ProgressIdentifiction(points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06009553 RID: 38227 RVA: 0x0037C0D1 File Offset: 0x0037A2D1
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009554 RID: 38228 RVA: 0x0037C0D3 File Offset: 0x0037A2D3
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009555 RID: 38229 RVA: 0x0037C0D5 File Offset: 0x0037A2D5
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06009556 RID: 38230 RVA: 0x0037C0D8 File Offset: 0x0037A2D8
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06009557 RID: 38231 RVA: 0x0037C0DC File Offset: 0x0037A2DC
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			float mass = gameObject.GetComponent<PrimaryElement>().Mass;
			float num = 0f;
			float temperature = 0f;
			SimHashes elementConsumed = SimHashes.Vacuum;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			bool result = num >= amount;
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
			return result;
		}

		// Token: 0x06009558 RID: 38232 RVA: 0x0037C170 File Offset: 0x0037A370
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x06009559 RID: 38233 RVA: 0x0037C1C4 File Offset: 0x0037A3C4
		public bool HasOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x0600955A RID: 38234 RVA: 0x0037C215 File Offset: 0x0037A415
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x04007349 RID: 29513
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x0400734A RID: 29514
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x0400734B RID: 29515
		private GameObject telescopeTargetMarker;

		// Token: 0x0400734C RID: 29516
		private ClusterMapMeteorShower.Instance currentTarget;

		// Token: 0x0400734D RID: 29517
		[MyCmpGet]
		private Storage storage;

		// Token: 0x0400734E RID: 29518
		private AttributeModifier radiationShielding;

		// Token: 0x0400734F RID: 29519
		private float checkMarkerTimer;

		// Token: 0x04007350 RID: 29520
		private float checkMarkerFrequency = 1f;
	}
}
