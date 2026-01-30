using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class MissileLauncher : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>
{
	// Token: 0x06003479 RID: 13433 RVA: 0x00129468 File Offset: 0x00127668
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Off;
		this.root.Update(delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.HasLineOfSight();
		}, UpdateRate.SIM_200ms, false);
		this.Off.PlayAnim("inoperational").EventTransition(GameHashes.OperationalChanged, this.On, (MissileLauncher.Instance smi) => smi.Operational.IsOperational).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		});
		this.On.DefaultState(this.On.opening).EventTransition(GameHashes.OperationalChanged, this.On.shutdown, (MissileLauncher.Instance smi) => !smi.Operational.IsOperational).ParamTransition<bool>(this.fullyBlocked, this.Nosurfacesight, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue).ScheduleGoTo(this.shutdownDuration, this.On.idle).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(smi.Operational.IsOperational, false);
		});
		this.On.opening.PlayAnim("working_pre").OnAnimQueueComplete(this.On.searching).Target(this.cannonTarget).PlayAnim("Cannon_working_pre");
		this.On.searching.PlayAnim("on", KAnim.PlayMode.Loop).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.sm.rotationComplete.Set(false, smi, false);
			smi.sm.meteorTarget.Set(null, smi, false);
			smi.cannonRotation = smi.def.scanningAngle;
		}).Update("FindMeteor", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.Searching(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OnStorageChange, this.NoAmmo, (MissileLauncher.Instance smi) => smi.MissileStorage.Count <= 0 && smi.LongRangeStorage.Count <= 0).ParamTransition<GameObject>(this.meteorTarget, this.Launch.targeting, (MissileLauncher.Instance smi, GameObject meteor) => meteor != null).ParamTransition<GameObject>(this.longRangeTarget, this.Launch.targetingLongRange, (MissileLauncher.Instance smi, GameObject longrange) => smi.ShouldRotateToLongRange()).Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.sm.rotationComplete.Set(false, smi, false);
		});
		this.On.idle.Target(this.masterTarget).PlayAnim("idle", KAnim.PlayMode.Loop).UpdateTransition(this.On, (MissileLauncher.Instance smi, float dt) => smi.Operational.IsOperational && smi.MeteorDetected(), UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ClusterDestinationChanged, this.On.searching, (MissileLauncher.Instance smi) => smi.LongRangeStorage.Count > 0).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
		this.On.shutdown.Target(this.masterTarget).PlayAnim("working_pst").OnAnimQueueComplete(this.Off).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
		this.Launch.PlayAnim("target_detected", KAnim.PlayMode.Loop).Update("Rotate", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.Launch.targeting.Update("Targeting", delegate(MissileLauncher.Instance smi, float dt)
		{
			if (smi.sm.meteorTarget.Get(smi).IsNullOrDestroyed())
			{
				smi.GoTo(this.On.searching);
				return;
			}
			if (smi.cannonAnimController.Rotation < smi.def.maxAngle * -1f || smi.cannonAnimController.Rotation > smi.def.maxAngle)
			{
				smi.sm.meteorTarget.Get(smi).GetComponent<Comet>().Targeted = false;
				smi.sm.meteorTarget.Set(null, smi, false);
				smi.GoTo(this.On.searching);
			}
		}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<bool>(this.rotationComplete, this.Launch.shoot, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue);
		this.Launch.targetingLongRange.Update("TargetingLongRange", delegate(MissileLauncher.Instance smi, float dt)
		{
		}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<bool>(this.rotationComplete, this.Launch.shoot, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue);
		this.Launch.shoot.ScheduleGoTo(this.shootDelayDuration, this.Launch.pst).Exit("LaunchMissile", delegate(MissileLauncher.Instance smi)
		{
			if (smi.sm.meteorTarget.Get(smi) != null)
			{
				smi.LaunchMissile();
			}
			else if (smi.sm.longRangeTarget.Get(smi) != null)
			{
				smi.LaunchLongRangeMissile();
			}
			this.cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play("Cannon_shooting_pre", KAnim.PlayMode.Once, 1f, 0f);
		});
		this.Launch.pst.Target(this.masterTarget).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.SetOreChunk();
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component.Play("base_shooting_pst_last", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("base_shooting_pst", KAnim.PlayMode.Once, 1f, 0f);
		}).Target(this.cannonTarget).PlayAnim("Cannon_shooting_pst").OnAnimQueueComplete(this.Cooldown);
		this.Cooldown.Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.SpawnOre();
		}).Enter(delegate(MissileLauncher.Instance smi)
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			if (smi.GetComponent<Storage>().Count <= 0)
			{
				component.Play("base_ejecting_last", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play("base_ejecting", KAnim.PlayMode.Once, 1f, 0f);
			}
			smi.sm.rotationComplete.Set(false, smi, false);
			smi.sm.meteorTarget.Set(null, smi, false);
			smi.GoTo(smi.CooldownGoToState);
		});
		this.Cooldown.basic.Update("Rotate", delegate(MissileLauncher.Instance smi, float dt)
		{
			smi.RotateToMeteor(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).OnAnimQueueComplete(this.On.searching);
		this.Cooldown.longrange.QueueAnim("cooldown", true, null).ToggleStatusItem(MissileLauncher.LongRangeCooldown, null).Target(this.cannonTarget).QueueAnim("cooldown_cannon_pre", false, null).QueueAnim("cooldown_cannon", true, null).ScheduleGoTo(MissileLauncher.longrangeCooldownTime, this.On.searching).Exit(delegate(MissileLauncher.Instance smi)
		{
			this.cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play("cooldown_cannon_pst", KAnim.PlayMode.Once, 1f, 0f);
		});
		this.Nosurfacesight.Target(this.masterTarget).PlayAnim("working_pst").QueueAnim("error", false, null).ParamTransition<bool>(this.fullyBlocked, this.On, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsFalse).Target(this.cannonTarget).PlayAnim("Cannon_working_pst").Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		});
		this.NoAmmo.PlayAnim("off_open").EventTransition(GameHashes.OnStorageChange, this.On, (MissileLauncher.Instance smi) => smi.MissileStorage.Count > 0 || smi.LongRangeStorage.Count > 0).Enter(delegate(MissileLauncher.Instance smi)
		{
			smi.Operational.SetActive(false, false);
		}).Exit(delegate(MissileLauncher.Instance smi)
		{
			smi.GetComponent<KAnimControllerBase>().Play("off_closing", KAnim.PlayMode.Once, 1f, 0f);
		}).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
	}

	// Token: 0x04001FA3 RID: 8099
	private static StatusItem NoSurfaceSight = new StatusItem("MissileLauncher_NoSurfaceSight", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);

	// Token: 0x04001FA4 RID: 8100
	private static StatusItem PartiallyBlockedStatus = new StatusItem("MissileLauncher_PartiallyBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001FA5 RID: 8101
	private static StatusItem LongRangeCooldown = new StatusItem("MissileLauncher_LongRangeCooldown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001FA6 RID: 8102
	public float shutdownDuration = 50f;

	// Token: 0x04001FA7 RID: 8103
	public float shootDelayDuration = 0.25f;

	// Token: 0x04001FA8 RID: 8104
	public static float SHELL_MASS = 2.5f;

	// Token: 0x04001FA9 RID: 8105
	public static float SHELL_TEMPERATURE = 353.15f;

	// Token: 0x04001FAA RID: 8106
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter rotationComplete;

	// Token: 0x04001FAB RID: 8107
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject> meteorTarget = new StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject>();

	// Token: 0x04001FAC RID: 8108
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.TargetParameter cannonTarget;

	// Token: 0x04001FAD RID: 8109
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter fullyBlocked;

	// Token: 0x04001FAE RID: 8110
	public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject> longRangeTarget = new StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject>();

	// Token: 0x04001FAF RID: 8111
	public static float longrangeCooldownTime = 10f;

	// Token: 0x04001FB0 RID: 8112
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Off;

	// Token: 0x04001FB1 RID: 8113
	public MissileLauncher.OnState On;

	// Token: 0x04001FB2 RID: 8114
	public MissileLauncher.LaunchState Launch;

	// Token: 0x04001FB3 RID: 8115
	public MissileLauncher.CooldownState Cooldown;

	// Token: 0x04001FB4 RID: 8116
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Nosurfacesight;

	// Token: 0x04001FB5 RID: 8117
	public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State NoAmmo;

	// Token: 0x020016FA RID: 5882
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400765A RID: 30298
		public static readonly CellOffset LaunchOffset = new CellOffset(0, 4);

		// Token: 0x0400765B RID: 30299
		public float launchSpeed = 30f;

		// Token: 0x0400765C RID: 30300
		public float rotationSpeed = 100f;

		// Token: 0x0400765D RID: 30301
		public static readonly Vector2I launchRange = new Vector2I(16, 32);

		// Token: 0x0400765E RID: 30302
		public float scanningAngle = 50f;

		// Token: 0x0400765F RID: 30303
		public float maxAngle = 80f;
	}

	// Token: 0x020016FB RID: 5883
	public new class Instance : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.GameInstance, IMissileSelectionInterface
	{
		// Token: 0x06009962 RID: 39266 RVA: 0x0038C76C File Offset: 0x0038A96C
		public bool IsAnyCosmicBlastShotAllowed()
		{
			return MissileLauncherConfig.CosmicBlastShotTypes.Any((Tag match) => this.AmmunitionIsAllowed(match));
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06009963 RID: 39267 RVA: 0x0038C784 File Offset: 0x0038A984
		public WorldContainer myWorld
		{
			get
			{
				if (this.worldContainer == null)
				{
					this.worldContainer = this.GetMyWorld();
				}
				return this.worldContainer;
			}
		}

		// Token: 0x06009964 RID: 39268 RVA: 0x0038C7A8 File Offset: 0x0038A9A8
		public Instance(IStateMachineTarget master, MissileLauncher.Def def) : base(master, def)
		{
			Components.MissileLaunchers.Add(this);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			string name = component.name + ".cannon";
			base.smi.cannonGameObject = new GameObject(name);
			base.smi.cannonGameObject.SetActive(false);
			base.smi.cannonGameObject.transform.parent = component.transform;
			base.smi.cannonGameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
			base.smi.cannonAnimController = base.smi.cannonGameObject.AddComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.AnimFiles = new KAnimFile[]
			{
				component.AnimFiles[0]
			};
			base.smi.cannonAnimController.initialAnim = "Cannon_off";
			base.smi.cannonAnimController.isMovable = true;
			base.smi.cannonAnimController.SetSceneLayer(Grid.SceneLayer.Building);
			component.SetSymbolVisiblity("cannon_target", false);
			bool flag;
			Vector3 position = component.GetSymbolTransform(new HashedString("cannon_target"), out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Building);
			base.smi.cannonGameObject.transform.SetPosition(position);
			this.launchPosition = position;
			Grid.PosToXY(this.launchPosition, out this.launchXY);
			base.smi.cannonGameObject.SetActive(true);
			base.smi.sm.cannonTarget.Set(base.smi.cannonGameObject, base.smi, false);
			KAnim.Anim anim = component.AnimFiles[0].GetData().GetAnim("Cannon_shooting_pre");
			if (anim != null)
			{
				this.launchAnimTime = anim.totalTime / 2f;
			}
			else
			{
				global::Debug.LogWarning("MissileLauncher anim data is missing");
				this.launchAnimTime = 1f;
			}
			this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.longRangemeter = new MeterController(component, "meter_target_longrange", "meter_longrange", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			base.Subscribe(-1201923725, new Action<object>(this.OnHighlight));
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			foreach (Storage storage in base.smi.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == "MissileBasic")
				{
					this.MissileStorage = storage;
				}
				else if (storage.storageID == "MissileLongRange")
				{
					this.LongRangeStorage = storage;
				}
				else if (storage.storageID == "CondiutStorage")
				{
					this.LoadingStorage = storage;
				}
			}
			base.Subscribe(-1697596308, new Action<object>(this.OnStorage));
			FlatTagFilterable component2 = base.smi.master.GetComponent<FlatTagFilterable>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Comet))
			{
				if (!go.HasTag(GameTags.DeprecatedContent))
				{
					if (!component2.tagOptions.Contains(go.PrefabID()))
					{
						component2.tagOptions.Add(go.PrefabID());
						component2.selectedTags.Add(go.PrefabID());
					}
					component2.selectedTags.Remove(GassyMooCometConfig.ID);
					component2.selectedTags.Remove(DieselMooCometConfig.ID);
				}
			}
			foreach (ManualDeliveryKG manualDeliveryKG in base.smi.gameObject.GetComponents<ManualDeliveryKG>())
			{
				if (manualDeliveryKG.RequestedItemTag == "MissileBasic")
				{
					this.ManualDeliveryMissile = manualDeliveryKG;
				}
				else
				{
					this.ManualDeliveryLongRange = manualDeliveryKG;
				}
			}
		}

		// Token: 0x06009965 RID: 39269 RVA: 0x0038CBF8 File Offset: 0x0038ADF8
		public override void StartSM()
		{
			base.StartSM();
			this.OnStorage(null);
			base.smi.master.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed("MissileBasic");
			this.clusterDestinationSelector = base.smi.master.GetComponent<EntityClusterDestinationSelector>();
			if (this.clusterDestinationSelector != null)
			{
				this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
			}
			this.UpdateAmmunitionDelivery();
			this.UpdateMeterVisibility();
		}

		// Token: 0x06009966 RID: 39270 RVA: 0x0038CC78 File Offset: 0x0038AE78
		protected override void OnCleanUp()
		{
			Components.MissileLaunchers.Remove(this);
			base.Unsubscribe(-1201923725, new Action<object>(this.OnHighlight));
			base.OnCleanUp();
		}

		// Token: 0x06009967 RID: 39271 RVA: 0x0038CCA4 File Offset: 0x0038AEA4
		private void OnHighlight(object _)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.smi.cannonAnimController.HighlightColour = component.HighlightColour;
		}

		// Token: 0x06009968 RID: 39272 RVA: 0x0038CCD0 File Offset: 0x0038AED0
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				MissileLauncher.Instance smi = gameObject.GetSMI<MissileLauncher.Instance>();
				if (smi != null)
				{
					this.ammunitionPermissions.Clear();
					foreach (KeyValuePair<Tag, bool> keyValuePair in smi.ammunitionPermissions)
					{
						this.ChangeAmmunition(keyValuePair.Key, smi.AmmunitionIsAllowed(keyValuePair.Key));
					}
					base.smi.master.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed("MissileBasic");
					this.clusterDestinationSelector = base.smi.master.GetComponent<EntityClusterDestinationSelector>();
					if (this.clusterDestinationSelector != null)
					{
						this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
					}
					if (smi.sm.longRangeTarget != null)
					{
						base.sm.longRangeTarget.Set(smi.sm.longRangeTarget.Get(smi), this, false);
					}
				}
			}
		}

		// Token: 0x06009969 RID: 39273 RVA: 0x0038CDEC File Offset: 0x0038AFEC
		private void OnStorage(object data)
		{
			if (this.LoadingStorage.items.Count > 0)
			{
				KPrefabID component = this.LoadingStorage.items[0].GetComponent<KPrefabID>();
				if (this.AmmunitionIsAllowed(component.PrefabTag))
				{
					Pickupable component2 = component.GetComponent<Pickupable>();
					Storage storage = null;
					if (component.PrefabTag == "MissileBasic")
					{
						storage = this.MissileStorage;
					}
					else if (MissileLauncherConfig.CosmicBlastShotTypes.Contains(component.PrefabTag))
					{
						storage = this.LongRangeStorage;
					}
					if (storage != null && storage.Capacity() - storage.MassStored() >= component2.PrimaryElement.Mass)
					{
						this.LoadingStorage.Transfer(component2.gameObject, storage, true, true);
					}
				}
			}
			this.meter.SetPositionPercent(Mathf.Clamp01(this.MissileStorage.MassStored() / this.MissileStorage.capacityKg));
			this.longRangemeter.SetPositionPercent(Mathf.Clamp01(this.LongRangeStorage.MassStored() / this.LongRangeStorage.capacityKg));
		}

		// Token: 0x0600996A RID: 39274 RVA: 0x0038CEFC File Offset: 0x0038B0FC
		private void UpdateMeterVisibility()
		{
			this.meter.gameObject.SetActive(this.AmmunitionIsAllowed("MissileBasic"));
			this.longRangemeter.gameObject.SetActive(this.IsAnyCosmicBlastShotAllowed());
		}

		// Token: 0x0600996B RID: 39275 RVA: 0x0038CF34 File Offset: 0x0038B134
		public void Searching(float dt)
		{
			if (!this.FindMeteor())
			{
				this.FindLongRangeTarget();
			}
			this.RotateCannon(dt, base.def.rotationSpeed / 2f);
			if (base.smi.sm.rotationComplete.Get(base.smi))
			{
				this.cannonRotation *= -1f;
				base.smi.sm.rotationComplete.Set(false, base.smi, false);
			}
		}

		// Token: 0x0600996C RID: 39276 RVA: 0x0038CFB8 File Offset: 0x0038B1B8
		private bool FindMeteor()
		{
			if (this.MissileStorage.items.Count > 0)
			{
				GameObject gameObject = this.ChooseClosestInterceptionPoint(this.myWorld.id);
				if (gameObject != null)
				{
					base.smi.sm.meteorTarget.Set(gameObject, base.smi, false);
					gameObject.GetComponent<Comet>().Targeted = true;
					base.smi.cannonRotation = this.CalculateLaunchAngle(gameObject.transform.position);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600996D RID: 39277 RVA: 0x0038D03C File Offset: 0x0038B23C
		private bool FindLongRangeTarget()
		{
			if (this.LongRangeStorage.items.Count > 0)
			{
				GameObject gameObject = null;
				if (this.clusterDestinationSelector != null)
				{
					if (this.clusterDestinationSelector.GetDestination() != this.myWorld.GetComponent<ClusterGridEntity>().Location)
					{
						ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.clusterDestinationSelector.GetDestination(), EntityLayer.Meteor);
						gameObject = ((visibleEntityOfLayerAtCell != null) ? visibleEntityOfLayerAtCell.gameObject : null);
					}
				}
				else
				{
					GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash, -1);
					if (gameplayEventInstance != null)
					{
						GameObject impactorInstance = ((LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi).impactorInstance;
						gameObject = ((impactorInstance != null) ? impactorInstance.gameObject : null);
					}
				}
				if (gameObject != null)
				{
					Vector3 position = base.transform.position;
					position.y += 50f;
					if (this.IsPathClear(this.launchPosition, position))
					{
						base.smi.sm.longRangeTarget.Set(gameObject, base.smi, false);
						base.smi.cannonRotation = this.CalculateLaunchAngle(position);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600996E RID: 39278 RVA: 0x0038D170 File Offset: 0x0038B370
		private float CalculateLaunchAngle(Vector3 targetPosition)
		{
			Vector3 v = Vector3.Normalize(targetPosition - this.launchPosition);
			return MathUtil.AngleSigned(Vector3.up, v, Vector3.forward);
		}

		// Token: 0x0600996F RID: 39279 RVA: 0x0038D1A0 File Offset: 0x0038B3A0
		public void LaunchMissile()
		{
			GameObject gameObject = this.MissileStorage.FindFirst("MissileBasic");
			if (gameObject != null)
			{
				Pickupable pickupable = gameObject.GetComponent<Pickupable>();
				if (pickupable.TotalAmount <= 1f)
				{
					this.MissileStorage.Drop(pickupable.gameObject, true);
				}
				else
				{
					pickupable = EntitySplitter.Split(pickupable, 1f, null);
				}
				pickupable.allowedChoreTypes = MissileLauncher.Instance.empty_chore_list;
				this.SetMissileElement(gameObject);
				GameObject gameObject2 = base.smi.sm.meteorTarget.Get(base.smi);
				if (!gameObject2.IsNullOrDestroyed())
				{
					pickupable.GetSMI<MissileProjectile.StatesInstance>().PrepareLaunch(gameObject2.GetComponent<Comet>(), base.def.launchSpeed, this.launchPosition, base.smi.cannonRotation);
					this.CooldownGoToState = base.sm.Cooldown.basic;
				}
			}
		}

		// Token: 0x06009970 RID: 39280 RVA: 0x0038D280 File Offset: 0x0038B480
		public void LaunchLongRangeMissile()
		{
			GameObject gameObject = null;
			foreach (Tag tag in MissileLauncherConfig.CosmicBlastShotTypes)
			{
				gameObject = this.LongRangeStorage.FindFirst(tag);
				if (gameObject != null)
				{
					break;
				}
			}
			if (gameObject != null)
			{
				Pickupable pickupable = gameObject.GetComponent<Pickupable>();
				if (pickupable.TotalAmount <= 1f)
				{
					this.LongRangeStorage.Drop(pickupable.gameObject, true);
				}
				else
				{
					pickupable = EntitySplitter.Split(pickupable, 1f, null);
				}
				pickupable.allowedChoreTypes = MissileLauncher.Instance.empty_chore_list;
				this.SetMissileElement(gameObject);
				GameObject gameObject2 = base.smi.sm.longRangeTarget.Get(base.smi);
				if (!gameObject2.IsNullOrDestroyed())
				{
					pickupable.GetSMI<MissileLongRangeProjectile.StatesInstance>().PrepareLaunch(gameObject2, base.def.launchSpeed, this.launchPosition, base.smi.cannonRotation);
					this.CooldownGoToState = base.sm.Cooldown.longrange;
					base.smi.sm.longRangeTarget.Set(null, base.smi, false);
				}
			}
		}

		// Token: 0x06009971 RID: 39281 RVA: 0x0038D3BC File Offset: 0x0038B5BC
		private void SetMissileElement(GameObject missile)
		{
			this.missileElement = missile.GetComponent<PrimaryElement>().Element.tag;
			if (Assets.GetPrefab(this.missileElement) == null)
			{
				global::Debug.LogWarning(string.Format("Missing element {0} for missile launcher. Defaulting to IronOre", this.missileElement));
				this.missileElement = GameTags.IronOre;
			}
		}

		// Token: 0x06009972 RID: 39282 RVA: 0x0038D418 File Offset: 0x0038B618
		public GameObject ChooseClosestInterceptionPoint(int world_id)
		{
			GameObject result = null;
			List<Comet> items = Components.Meteors.GetItems(world_id);
			float num = (float)MissileLauncher.Def.launchRange.y;
			foreach (Comet comet in items)
			{
				if (!comet.IsNullOrDestroyed() && !comet.Targeted && this.TargetFilter.selectedTags.Contains(comet.typeID))
				{
					Vector3 targetPosition = comet.TargetPosition;
					float num2;
					Vector3 vector = this.CalculateCollisionPoint(targetPosition, comet.Velocity, out num2);
					Grid.PosToCell(vector);
					float num3 = Vector3.Distance(vector, this.launchPosition);
					if (num3 < num && num2 > this.launchAnimTime && this.IsMeteorInRange(vector) && this.IsPathClear(this.launchPosition, targetPosition))
					{
						result = comet.gameObject;
						num = num3;
					}
				}
			}
			return result;
		}

		// Token: 0x06009973 RID: 39283 RVA: 0x0038D51C File Offset: 0x0038B71C
		private bool IsMeteorInRange(Vector3 interception_point)
		{
			Vector2I vector2I;
			Grid.PosToXY(interception_point, out vector2I);
			return Math.Abs(vector2I.X - this.launchXY.X) <= MissileLauncher.Def.launchRange.X && vector2I.Y - this.launchXY.Y > 0 && vector2I.Y - this.launchXY.Y <= MissileLauncher.Def.launchRange.Y;
		}

		// Token: 0x06009974 RID: 39284 RVA: 0x0038D598 File Offset: 0x0038B798
		public bool IsPathClear(Vector3 startPoint, Vector3 endPoint)
		{
			Vector2I vector2I = Grid.PosToXY(startPoint);
			Vector2I vector2I2 = Grid.PosToXY(endPoint);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, new Func<int, bool>(this.IsCellBlockedFromSky), false, true);
		}

		// Token: 0x06009975 RID: 39285 RVA: 0x0038D5E0 File Offset: 0x0038B7E0
		public bool IsCellBlockedFromSky(int cell)
		{
			if (Grid.IsValidCell(cell) && (int)Grid.WorldIdx[cell] == this.myWorld.id)
			{
				return Grid.Solid[cell];
			}
			int num;
			int num2;
			Grid.CellToXY(cell, out num, out num2);
			return num2 <= this.launchXY.Y;
		}

		// Token: 0x06009976 RID: 39286 RVA: 0x0038D630 File Offset: 0x0038B830
		public Vector3 CalculateCollisionPoint(Vector3 targetPosition, Vector3 targetVelocity, out float timeToCollision)
		{
			Vector3 vector = targetVelocity - base.smi.def.launchSpeed * (targetPosition - this.launchPosition).normalized;
			timeToCollision = (targetPosition - this.launchPosition).magnitude / vector.magnitude;
			return targetPosition + targetVelocity * timeToCollision;
		}

		// Token: 0x06009977 RID: 39287 RVA: 0x0038D69C File Offset: 0x0038B89C
		public void HasLineOfSight()
		{
			bool flag = false;
			bool flag2 = true;
			Extents extents = base.GetComponent<Building>().GetExtents();
			int val = this.launchXY.x - MissileLauncher.Def.launchRange.X;
			int val2 = this.launchXY.x + MissileLauncher.Def.launchRange.X;
			int y = extents.y + extents.height;
			int num = Grid.XYToCell(Math.Max((int)this.myWorld.minimumBounds.x, val), y);
			int num2 = Grid.XYToCell(Math.Min((int)this.myWorld.maximumBounds.x, val2), y);
			for (int i = num; i <= num2; i++)
			{
				flag = (flag || Grid.ExposedToSunlight[i] <= 0);
				flag2 = (flag2 && Grid.ExposedToSunlight[i] <= 0);
			}
			this.Selectable.ToggleStatusItem(MissileLauncher.PartiallyBlockedStatus, flag && !flag2, null);
			this.Selectable.ToggleStatusItem(MissileLauncher.NoSurfaceSight, flag2, null);
			base.smi.sm.fullyBlocked.Set(flag2, base.smi, false);
		}

		// Token: 0x06009978 RID: 39288 RVA: 0x0038D7CD File Offset: 0x0038B9CD
		public bool MeteorDetected()
		{
			return Components.Meteors.GetItems(this.myWorld.id).Count > 0;
		}

		// Token: 0x06009979 RID: 39289 RVA: 0x0038D7EC File Offset: 0x0038B9EC
		public void SetOreChunk()
		{
			if (!this.missileElement.IsValid)
			{
				global::Debug.LogWarning(string.Format("Missing element {0} for missile launcher. Defaulting to IronOre", this.missileElement));
				this.missileElement = GameTags.IronOre;
			}
			KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(this.missileElement).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
			base.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("Shell", symbolByIndex, 0);
		}

		// Token: 0x0600997A RID: 39290 RVA: 0x0038D870 File Offset: 0x0038BA70
		public void SpawnOre()
		{
			bool flag;
			Vector3 position = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("Shell", out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Assets.GetPrefab(this.missileElement).GetComponent<PrimaryElement>().Element.substance.SpawnResource(position, MissileLauncher.SHELL_MASS, MissileLauncher.SHELL_TEMPERATURE, byte.MaxValue, 0, false, false, false);
		}

		// Token: 0x0600997B RID: 39291 RVA: 0x0038D8E8 File Offset: 0x0038BAE8
		public void RotateCannon(float dt, float rotation_speed)
		{
			float num = this.cannonRotation - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = rotation_speed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			this.simpleAngle = this.cannonRotation;
			this.cannonAnimController.Rotation = this.simpleAngle;
			base.smi.sm.rotationComplete.Set(true, base.smi, false);
		}

		// Token: 0x0600997C RID: 39292 RVA: 0x0038D9C0 File Offset: 0x0038BBC0
		public bool ShouldRotateToLongRange()
		{
			return !base.smi.sm.longRangeTarget.Get(base.smi).IsNullOrDestroyed() && this.LongRangeStorage.items.Count > 0 && this.IsPathClear(this.launchPosition, this.launchPosition + new Vector3(0f, 50f, 0f));
		}

		// Token: 0x0600997D RID: 39293 RVA: 0x0038DA30 File Offset: 0x0038BC30
		public void RotateToMeteor(float dt)
		{
			GameObject gameObject = base.sm.meteorTarget.Get(this);
			float num;
			if (!gameObject.IsNullOrDestroyed())
			{
				num = this.CalculateLaunchAngle(gameObject.transform.position);
			}
			else
			{
				if (!this.ShouldRotateToLongRange())
				{
					return;
				}
				Vector3 position = base.transform.position;
				position.y += 50f;
				num = this.CalculateLaunchAngle(position);
			}
			float num2 = num - this.simpleAngle;
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			else if (num2 < -180f)
			{
				num2 += 360f;
			}
			float num3 = base.def.rotationSpeed * dt;
			if (num2 > 0f && num3 < num2)
			{
				this.simpleAngle += num3;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			if (num2 < 0f && -num3 > num2)
			{
				this.simpleAngle -= num3;
				this.cannonAnimController.Rotation = this.simpleAngle;
				return;
			}
			base.smi.sm.rotationComplete.Set(true, base.smi, false);
		}

		// Token: 0x0600997E RID: 39294 RVA: 0x0038DB4C File Offset: 0x0038BD4C
		public void ChangeAmmunition(Tag tag, bool allowed)
		{
			if (!this.ammunitionPermissions.ContainsKey(tag))
			{
				this.ammunitionPermissions.Add(tag, false);
			}
			this.ammunitionPermissions[tag] = allowed;
			this.UpdateAmmunitionDelivery();
			this.OnStorage(null);
			this.DropAmmunitionFromStorage(this.MissileStorage);
			this.DropAmmunitionFromStorage(this.LongRangeStorage);
			this.UpdateMeterVisibility();
		}

		// Token: 0x0600997F RID: 39295 RVA: 0x0038DBAC File Offset: 0x0038BDAC
		public void OnRowToggleClick()
		{
			if (this.clusterDestinationSelector != null)
			{
				this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
			}
			base.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed("MissileBasic");
		}

		// Token: 0x06009980 RID: 39296 RVA: 0x0038DBE8 File Offset: 0x0038BDE8
		public List<Tag> GetValidAmmunitionTags()
		{
			List<Tag> list = new List<Tag>
			{
				"MissileBasic",
				"MissileLongRange"
			};
			if (GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash, -1) == null)
			{
				list.Remove("MissileLongRange");
			}
			return list;
		}

		// Token: 0x06009981 RID: 39297 RVA: 0x0038DC4E File Offset: 0x0038BE4E
		public bool AmmunitionIsAllowed(Tag tag)
		{
			return this.ammunitionPermissions.ContainsKey(tag) && this.ammunitionPermissions[tag];
		}

		// Token: 0x06009982 RID: 39298 RVA: 0x0038DC6C File Offset: 0x0038BE6C
		private void UpdateAmmunitionDelivery()
		{
			bool pause = false;
			bool flag = this.AmmunitionIsAllowed("MissileLongRange");
			if (flag)
			{
				this.ManualDeliveryLongRange.RequestedItemTag = GameTags.LongRangeMissile;
			}
			else if (flag)
			{
				this.ManualDeliveryLongRange.RequestedItemTag = "MissileLongRange";
			}
			else
			{
				pause = true;
			}
			this.ManualDeliveryLongRange.Pause(pause, "ammunitionnotallowed");
			this.ManualDeliveryMissile.Pause(!this.AmmunitionIsAllowed("MissileBasic"), "ammunitionnotallowed");
		}

		// Token: 0x06009983 RID: 39299 RVA: 0x0038DCF4 File Offset: 0x0038BEF4
		private void DropAmmunitionFromStorage(Storage targetStorage)
		{
			for (int i = targetStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = targetStorage.items[i];
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!this.AmmunitionIsAllowed(component.PrefabTag))
					{
						targetStorage.Drop(gameObject, true);
					}
				}
			}
		}

		// Token: 0x04007660 RID: 30304
		[MyCmpReq]
		public Operational Operational;

		// Token: 0x04007661 RID: 30305
		public Storage MissileStorage;

		// Token: 0x04007662 RID: 30306
		public Storage LongRangeStorage;

		// Token: 0x04007663 RID: 30307
		private Storage LoadingStorage;

		// Token: 0x04007664 RID: 30308
		public ManualDeliveryKG ManualDeliveryMissile;

		// Token: 0x04007665 RID: 30309
		public ManualDeliveryKG ManualDeliveryLongRange;

		// Token: 0x04007666 RID: 30310
		[MyCmpReq]
		public KSelectable Selectable;

		// Token: 0x04007667 RID: 30311
		[MyCmpReq]
		public FlatTagFilterable TargetFilter;

		// Token: 0x04007668 RID: 30312
		private EntityClusterDestinationSelector clusterDestinationSelector;

		// Token: 0x04007669 RID: 30313
		[Serialize]
		private Dictionary<Tag, bool> ammunitionPermissions = new Dictionary<Tag, bool>
		{
			{
				"MissileBasic",
				true
			}
		};

		// Token: 0x0400766A RID: 30314
		private Vector3 launchPosition;

		// Token: 0x0400766B RID: 30315
		private Vector2I launchXY;

		// Token: 0x0400766C RID: 30316
		private float launchAnimTime;

		// Token: 0x0400766D RID: 30317
		public KBatchedAnimController cannonAnimController;

		// Token: 0x0400766E RID: 30318
		public GameObject cannonGameObject;

		// Token: 0x0400766F RID: 30319
		public float cannonRotation;

		// Token: 0x04007670 RID: 30320
		public float simpleAngle;

		// Token: 0x04007671 RID: 30321
		private Tag missileElement;

		// Token: 0x04007672 RID: 30322
		private MeterController meter;

		// Token: 0x04007673 RID: 30323
		private MeterController longRangemeter;

		// Token: 0x04007674 RID: 30324
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State CooldownGoToState;

		// Token: 0x04007675 RID: 30325
		private WorldContainer worldContainer;

		// Token: 0x04007676 RID: 30326
		private static List<ChoreType> empty_chore_list = new List<ChoreType>();
	}

	// Token: 0x020016FC RID: 5884
	public class OnState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x04007677 RID: 30327
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State searching;

		// Token: 0x04007678 RID: 30328
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State opening;

		// Token: 0x04007679 RID: 30329
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shutdown;

		// Token: 0x0400767A RID: 30330
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State idle;
	}

	// Token: 0x020016FD RID: 5885
	public class LaunchState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x0400767B RID: 30331
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State targeting;

		// Token: 0x0400767C RID: 30332
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State targetingLongRange;

		// Token: 0x0400767D RID: 30333
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shoot;

		// Token: 0x0400767E RID: 30334
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State pst;
	}

	// Token: 0x020016FE RID: 5886
	public class CooldownState : GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
	{
		// Token: 0x0400767F RID: 30335
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State longrange;

		// Token: 0x04007680 RID: 30336
		public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State basic;
	}
}
