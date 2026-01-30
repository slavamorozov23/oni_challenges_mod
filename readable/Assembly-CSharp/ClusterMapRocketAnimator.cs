using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BB8 RID: 3000
public class ClusterMapRocketAnimator : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060059FC RID: 23036 RVA: 0x0020A630 File Offset: 0x00208830
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDrillConeSymbol)).Transition(null, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.entityTarget.IsNull), UpdateRate.SIM_200ms).Target(this.entityTarget).EventHandlerTransition(GameHashes.RocketSelfDestructRequested, this.exploding, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).TagTransition(GameTags.RocketCollectingResources, this.utility.collecting, false).EventHandlerTransition(GameHashes.RocketLaunched, this.moving.takeoff, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true);
		this.idle.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Target(this.masterTarget).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop);
		}).Target(this.entityTarget).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Transition(this.moving.traveling, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling), UpdateRate.SIM_200ms).Transition(this.grounded, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.moving.landing, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsLanding), UpdateRate.SIM_200ms).Transition(this.utility.collecting, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsCollectingResourcesFromHexCell), UpdateRate.SIM_200ms);
		this.grounded.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(false, smi);
			smi.ToggleVisAnim(false);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
			smi.ToggleVisAnim(true);
			ClusterMapRocketAnimator.RefreshDrillConeSymbol(smi);
		}).Target(this.entityTarget).EventTransition(GameHashes.RocketLaunched, this.moving.takeoff, null);
		this.moving.takeoff.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Target(this.entityTarget).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations));
		this.moving.landing.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.traveling.DefaultState(this.moving.traveling.regular).Target(this.entityTarget).EventTransition(GameHashes.ClusterLocationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)));
		this.moving.traveling.regular.Target(this.entityTarget).Transition(this.moving.traveling.boosted, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.moving.traveling.boosted.Target(this.entityTarget).Transition(this.moving.traveling.regular, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("boosted", KAnim.PlayMode.Loop);
		});
		this.utility.Target(this.entityTarget).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations));
		this.utility.collecting.DefaultState(this.utility.collecting.pre).Target(this.entityTarget).TagTransition(GameTags.RocketCollectingResources, this.utility.collecting.pst, true).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHexCellInventoryItems, null);
		this.utility.collecting.pre.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pre", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.utility.collecting.loop);
			});
		});
		this.utility.collecting.loop.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_loop", KAnim.PlayMode.Loop);
		});
		this.utility.collecting.pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pst", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.idle);
			});
		});
		this.exploding.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("rocket_self_destruct_kanim")
			});
			smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.exploding_pst);
			});
		});
		this.exploding_pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().Stop();
			smi.entity.gameObject.Trigger(-1311384361, null);
		});
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x0020AB8D File Offset: 0x00208D8D
	private static void RefreshDillingAnimations(ClusterMapRocketAnimator.StatesInstance smi)
	{
		if (ClusterMapRocketAnimator.IsDrilling(smi))
		{
			smi.PlayDrillingAnimation();
			return;
		}
		smi.PlayIdleDrillConeAnimation();
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x0020ABA4 File Offset: 0x00208DA4
	private static void RefreshDrillConeSymbol(ClusterMapRocketAnimator.StatesInstance smi)
	{
		smi.RefreshDrillConeVisibility();
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x0020ABAC File Offset: 0x00208DAC
	private bool ClusterChangedAtMyLocation(ClusterMapRocketAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x06005A00 RID: 23040 RVA: 0x0020ABF0 File Offset: 0x00208DF0
	private bool IsTraveling(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling() && ((Clustercraft)smi.entity).HasResourcesToMove(1, Clustercraft.CombustionResource.All);
	}

	// Token: 0x06005A01 RID: 23041 RVA: 0x0020AC18 File Offset: 0x00208E18
	private bool IsBoosted(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).controlStationBuffTimeRemaining > 0f;
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x0020AC31 File Offset: 0x00208E31
	private bool IsGrounded(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x0020AC46 File Offset: 0x00208E46
	private bool IsLanding(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Landing;
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x0020AC5B File Offset: 0x00208E5B
	private static bool IsDrilling(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).HasTag(GameTags.RocketDrilling);
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x0020AC72 File Offset: 0x00208E72
	private bool IsCollectingResourcesFromHexCell(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).HasTag(GameTags.RocketCollectingResources);
	}

	// Token: 0x06005A06 RID: 23046 RVA: 0x0020AC8C File Offset: 0x00208E8C
	private bool IsSurfaceTransitioning(ClusterMapRocketAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x06005A07 RID: 23047 RVA: 0x0020ACC4 File Offset: 0x00208EC4
	private void ToggleSelectable(bool isSelectable, ClusterMapRocketAnimator.StatesInstance smi)
	{
		if (smi.entity.IsNullOrDestroyed())
		{
			return;
		}
		KSelectable component = smi.entity.GetComponent<KSelectable>();
		component.IsSelectable = isSelectable;
		if (!isSelectable && component.IsSelected && ClusterMapScreen.Instance.GetMode() != ClusterMapScreen.Mode.SelectDestination)
		{
			ClusterMapSelectTool.Instance.Select(null, true);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x04003C39 RID: 15417
	public StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003C3A RID: 15418
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04003C3B RID: 15419
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x04003C3C RID: 15420
	public ClusterMapRocketAnimator.MovingStates moving;

	// Token: 0x04003C3D RID: 15421
	public ClusterMapRocketAnimator.UtilityStates utility;

	// Token: 0x04003C3E RID: 15422
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding;

	// Token: 0x04003C3F RID: 15423
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding_pst;

	// Token: 0x04003C40 RID: 15424
	public const string DRILLCONE_METER_TARGET_NAME = "nose_target";

	// Token: 0x04003C41 RID: 15425
	public const string DRILLCONE_DEFAULT_ANIM_NAME = "drill_cone_idle";

	// Token: 0x04003C42 RID: 15426
	public const string DRILLCONE_DRILL_ANIM_NAME = "drilling_loop";

	// Token: 0x02001D55 RID: 7509
	public class TravelingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04008B01 RID: 35585
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State regular;

		// Token: 0x04008B02 RID: 35586
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State boosted;
	}

	// Token: 0x02001D56 RID: 7510
	public class MovingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04008B03 RID: 35587
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State takeoff;

		// Token: 0x04008B04 RID: 35588
		public ClusterMapRocketAnimator.TravelingStates traveling;

		// Token: 0x04008B05 RID: 35589
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;
	}

	// Token: 0x02001D57 RID: 7511
	public class UtilityStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04008B06 RID: 35590
		public ClusterMapRocketAnimator.UtilityStates.CollectingStates collecting;

		// Token: 0x02002A40 RID: 10816
		public class CollectingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
		{
			// Token: 0x0400BAB0 RID: 47792
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;

			// Token: 0x0400BAB1 RID: 47793
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State loop;

			// Token: 0x0400BAB2 RID: 47794
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pst;
		}
	}

	// Token: 0x02001D58 RID: 7512
	public class StatesInstance : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600B0E3 RID: 45283 RVA: 0x003DBB27 File Offset: 0x003D9D27
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600B0E4 RID: 45284 RVA: 0x003DBB50 File Offset: 0x003D9D50
		public override void StartSM()
		{
			base.GetComponent<ClusterMapVisualizer>().GetFirstAnimController();
			base.StartSM();
		}

		// Token: 0x0600B0E5 RID: 45285 RVA: 0x003DBB64 File Offset: 0x003D9D64
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x0600B0E6 RID: 45286 RVA: 0x003DBB74 File Offset: 0x003D9D74
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600B0E7 RID: 45287 RVA: 0x003DBBAC File Offset: 0x003D9DAC
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600B0E8 RID: 45288 RVA: 0x003DBBEE File Offset: 0x003D9DEE
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClusterMapRocketAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600B0E9 RID: 45289 RVA: 0x003DBC28 File Offset: 0x003D9E28
		public void RefreshDrillConeVisibility()
		{
			List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = ((Clustercraft)base.smi.entity).GetAllResourceHarvestModules();
			bool drillConeVisibility = allResourceHarvestModules != null && allResourceHarvestModules.Count > 0;
			this.SetDrillConeVisibility(drillConeVisibility);
		}

		// Token: 0x0600B0EA RID: 45290 RVA: 0x003DBC64 File Offset: 0x003D9E64
		private void SetDrillConeVisibility(bool shouldBeVisible)
		{
			if (shouldBeVisible)
			{
				if (this.drillConeSubAnim == null)
				{
					this.drillConeSubAnim = this.CreateSymbolController("nose_target", true);
				}
				this.drillConeSubAnim.gameObject.SetActive(true);
				return;
			}
			if (this.drillConeSubAnim != null)
			{
				this.drillConeSubAnim.gameObject.SetActive(false);
			}
			base.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SetSymbolVisiblity("nose_target", false);
		}

		// Token: 0x0600B0EB RID: 45291 RVA: 0x003DBCE0 File Offset: 0x003D9EE0
		public void PlayDrillingAnimation()
		{
			if (this.drillConeSubAnim != null)
			{
				this.drillConeSubAnim.Play("drilling_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600B0EC RID: 45292 RVA: 0x003DBD10 File Offset: 0x003D9F10
		public void PlayIdleDrillConeAnimation()
		{
			if (this.drillConeSubAnim != null)
			{
				this.drillConeSubAnim.Play("drill_cone_idle", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600B0ED RID: 45293 RVA: 0x003DBD40 File Offset: 0x003D9F40
		private void DeleteDrillConeSubAnim()
		{
			if (this.drillConeSubAnim != null)
			{
				this.drillConeSubAnim.gameObject.DeleteObject();
				this.drillConeSubAnim = null;
			}
		}

		// Token: 0x0600B0EE RID: 45294 RVA: 0x003DBD67 File Offset: 0x003D9F67
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.DeleteDrillConeSubAnim();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x0600B0EF RID: 45295 RVA: 0x003DBD7C File Offset: 0x003D9F7C
		private KBatchedAnimController CreateSymbolController(string symbolName, bool require_sound = false)
		{
			KBatchedAnimController firstAnimController = base.GetComponent<ClusterMapVisualizer>().GetFirstAnimController();
			KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolName, firstAnimController);
			kbatchedAnimController.transform.SetParent(firstAnimController.transform, false);
			kbatchedAnimController.initialAnim = "drill_cone_idle";
			KBatchedAnimTracker kbatchedAnimTracker = kbatchedAnimController.gameObject.AddComponent<KBatchedAnimTracker>();
			HashedString symbol = new HashedString(symbolName);
			kbatchedAnimTracker.controller = firstAnimController;
			kbatchedAnimTracker.symbol = symbol;
			kbatchedAnimTracker.forceAlwaysVisible = false;
			if (require_sound)
			{
				kbatchedAnimController.gameObject.AddComponent<LoopingSounds>();
			}
			kbatchedAnimController.gameObject.SetActive(false);
			firstAnimController.SetSymbolVisiblity(symbolName, false);
			return kbatchedAnimController;
		}

		// Token: 0x0600B0F0 RID: 45296 RVA: 0x003DBE0C File Offset: 0x003DA00C
		private KBatchedAnimController CreateEmptyKAnimController(string name, KBatchedAnimController animController)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("rocket01_kanim")
			};
			kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
			kbatchedAnimController.animScale = ((animController == null) ? 0.08f : animController.animScale);
			kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.NoLayer;
			kbatchedAnimController.forceUseGameTime = true;
			return kbatchedAnimController;
		}

		// Token: 0x04008B07 RID: 35591
		public ClusterGridEntity entity;

		// Token: 0x04008B08 RID: 35592
		private KBatchedAnimController drillConeSubAnim;

		// Token: 0x04008B09 RID: 35593
		private int animCompleteHandle = -1;

		// Token: 0x04008B0A RID: 35594
		private GameObject animCompleteSubscriber;
	}
}
