using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class StressShockChore : Chore<StressShockChore.StatesInstance>
{
	// Token: 0x060019B2 RID: 6578 RVA: 0x0008FB58 File Offset: 0x0008DD58
	private static bool CheckBlocked(int sourceCell, int destinationCell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		Grid.CollectCellsInLine(sourceCell, destinationCell, hashSet);
		bool result = false;
		foreach (int i in hashSet)
		{
			if (Grid.Solid[i])
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x0008FBC4 File Offset: 0x0008DDC4
	public static void AddBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(true);
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x0008FBCD File Offset: 0x0008DDCD
	public static void RemoveBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(false);
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x0008FBD8 File Offset: 0x0008DDD8
	public static void ForceStressMonitorToTimeOut(StressShockChore.StatesInstance smi)
	{
		StressBehaviourMonitor.Instance smi2 = smi.GetSMI<StressBehaviourMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.ManualSetStressTier2TimeCounter(150f);
		}
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x0008FBFC File Offset: 0x0008DDFC
	public StressShockChore(ChoreType chore_type, IStateMachineTarget target, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.StressShock, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressShockChore.StatesInstance(this, target.gameObject, notification);
	}

	// Token: 0x04000EE1 RID: 3809
	public const float FaceBeamZOffset = 0.01f;

	// Token: 0x02001327 RID: 4903
	public class StatesInstance : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.GameInstance
	{
		// Token: 0x06008B0E RID: 35598 RVA: 0x0035C82C File Offset: 0x0035AA2C
		public StatesInstance(StressShockChore master, GameObject shocker, Notification notification) : base(master)
		{
			base.sm.shocker.Set(shocker, base.smi, false);
			this.notification = notification;
		}

		// Token: 0x06008B0F RID: 35599 RVA: 0x0035C8A4 File Offset: 0x0035AAA4
		public void SetDrainModifierActiveState(bool draining)
		{
			if (draining)
			{
				this.batteryMonitor.AddOrUpdateModifier(this.powerDrainModifier, true);
				return;
			}
			this.batteryMonitor.RemoveModifier(this.powerDrainModifier.id, true);
		}

		// Token: 0x06008B10 RID: 35600 RVA: 0x0035C8D8 File Offset: 0x0035AAD8
		public void FindDestination()
		{
			int num = this.FindIdleCell();
			if (num != -1 && num != Grid.PosToCell(base.gameObject))
			{
				base.sm.targetMoveLocation.Set(num, base.smi, false);
				this.GoTo(base.sm.shocking.runAroundShockingStuff);
				return;
			}
			num = this.FindMinionTarget();
			if (num != -1 && num != Grid.PosToCell(base.gameObject))
			{
				base.sm.targetMoveLocation.Set(num, base.smi, false);
				this.GoTo(base.sm.shocking.runAroundShockingStuff);
				return;
			}
			base.sm.targetMoveLocation.Set(Grid.PosToCell(base.gameObject), base.smi, false);
			this.GoTo(base.sm.shocking.standStillShockingStuff);
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x0035C9B0 File Offset: 0x0035ABB0
		private int FindMinionTarget()
		{
			Navigator component = base.smi.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return Grid.InvalidCell;
			}
			int num = int.MaxValue;
			int result = Grid.InvalidCell;
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.smi.gameObject.GetMyWorldId(), false);
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!worldItems[i].IsNullOrDestroyed() && !(worldItems[i].gameObject == base.gameObject))
				{
					int num2 = Grid.PosToCell(worldItems[i]);
					if (component.CanReach(num2))
					{
						int navigationCost = component.GetNavigationCost(num2);
						if (navigationCost < num)
						{
							num = navigationCost;
							result = num2;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x0035CA74 File Offset: 0x0035AC74
		private int FindIdleCell()
		{
			Navigator component = base.smi.master.GetComponent<Navigator>();
			MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)component.GetCurrentAbilities();
			minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
			IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(90, 180));
			component.RunQuery(idleCellQuery);
			if (idleCellQuery.GetResultCell() == Grid.PosToCell(base.gameObject))
			{
				idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(0, 90));
				component.RunQuery(idleCellQuery);
			}
			minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
			return idleCellQuery.GetResultCell();
		}

		// Token: 0x06008B13 RID: 35603 RVA: 0x0035CB0C File Offset: 0x0035AD0C
		public void ShockUpdateRender(StressShockChore.StatesInstance smi, float dt)
		{
			if (smi.sm.faceLightningFX.Get(smi) != null)
			{
				smi.sm.faceLightningFX.Get(smi).transform.SetPosition(smi.FaceOriginLocation());
			}
			if (smi.sm.beamTarget.Get(smi) != null)
			{
				Vector3 vector = smi.sm.beamTarget.Get(smi).transform.position + Vector3.up / 2f;
				if (smi.sm.beamFX.Get(smi) == null)
				{
					smi.MakeBeam();
				}
				if (!StressShockChore.CheckBlocked(Grid.PosToCell(smi.sm.beamFX.Get(smi).transform.position), Grid.PosToCell(vector)))
				{
					smi.AimBeam(vector, 0);
				}
			}
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x0035CBF4 File Offset: 0x0035ADF4
		public void ShockUpdate200(StressShockChore.StatesInstance smi, float dt)
		{
			float num = dt * STRESS.SHOCKER.POWER_CONSUMPTION_RATE;
			smi.sm.powerConsumed.Delta(num, smi);
			smi.batteryMonitor.ConsumePower(num);
			if (smi.sm.beamTarget.Get(smi) != null)
			{
				Health component = smi.sm.beamTarget.Get(smi).GetComponent<Health>();
				if (component != null)
				{
					component.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
					return;
				}
				Electrobank component2 = smi.sm.beamTarget.Get(smi).GetComponent<Electrobank>();
				if (component2 != null)
				{
					component2.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
					return;
				}
				if (smi.sm.beamTarget.Get(smi).HasTag(GameTags.Wires))
				{
					BuildingHP component3 = smi.sm.beamTarget.Get(smi).GetComponent<BuildingHP>();
					if (component3 != null)
					{
						component3.DoDamage(Mathf.RoundToInt(dt * STRESS.SHOCKER.DAMAGE_RATE));
					}
				}
			}
		}

		// Token: 0x06008B15 RID: 35605 RVA: 0x0035CCF0 File Offset: 0x0035AEF0
		public void PickShockTarget(StressShockChore.StatesInstance smi)
		{
			int num = Grid.PosToCell(smi.master.gameObject);
			int worldId = (int)Grid.WorldIdx[num];
			List<GameObject> list = new List<GameObject>();
			float num2 = UnityEngine.Random.Range(0f, 2f);
			foreach (Health health in Components.Health.GetWorldItems(worldId, false))
			{
				if (!health.IsNullOrDestroyed() && !(health.gameObject == smi.master.gameObject))
				{
					int num3 = Grid.PosToCell(health);
					float num4 = Vector2.Distance(Grid.CellToPos2D(num), Grid.CellToPos2D(num3));
					if (num4 <= (float)STRESS.SHOCKER.SHOCK_RADIUS && num4 > num2 && !StressShockChore.CheckBlocked(num, num3))
					{
						list.Add(health.gameObject);
					}
				}
			}
			if (list.Count == 0)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				List<ScenePartitionerEntry> list2 = new List<ScenePartitionerEntry>();
				GameScenePartitioner.Instance.GatherEntries(vector2I.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.completeBuildings, list2);
				foreach (ScenePartitionerEntry scenePartitionerEntry in list2)
				{
					if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry.x, (float)scenePartitionerEntry.y))))
					{
						BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
						if (buildingComplete != null)
						{
							list.Add(buildingComplete.gameObject);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				this.ClearBeam(false);
				return;
			}
			GameObject random = list.GetRandom<GameObject>();
			GameObject gameObject = random;
			float num5 = float.MaxValue;
			foreach (GameObject gameObject2 in list)
			{
				if (list.Count <= 1 || !(gameObject2 == base.sm.previousTarget.Get(smi)))
				{
					float num6 = Vector2.Distance(base.transform.position, gameObject2.transform.position);
					if (num6 < num5)
					{
						num5 = num6;
						gameObject = gameObject2;
					}
				}
			}
			if (random != null && gameObject != null && UnityEngine.Random.Range(0, 100) > 50)
			{
				base.sm.beamTarget.Set(gameObject, smi, false);
				return;
			}
			base.sm.beamTarget.Set(gameObject, smi, false);
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x0035CFBC File Offset: 0x0035B1BC
		public void MakeBeam()
		{
			GameObject gameObject = new GameObject("shockFX");
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			base.sm.beamFX.Set(kbatchedAnimController, base.smi, false);
			kbatchedAnimController.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_fx_kanim")
			});
			gameObject.SetActive(true);
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = base.transform.position.z + 0.01f;
			gameObject.transform.position = vector;
			kbatchedAnimController.Play("beam1", KAnim.PlayMode.Loop, 1f, 0f);
			if (base.sm.faceLightningFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.faceLightningFX.Get(base.smi).gameObject);
				base.sm.faceLightningFX.Set(null, base.smi, false);
			}
			GameObject gameObject2 = new GameObject("faceLightningFX");
			gameObject2.SetActive(false);
			KBatchedAnimController kbatchedAnimController2 = gameObject2.AddComponent<KBatchedAnimController>();
			base.sm.faceLightningFX.Set(kbatchedAnimController2, base.smi, false);
			kbatchedAnimController2.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_lightning_fx_kanim")
			});
			gameObject2.SetActive(true);
			gameObject2.transform.position = this.FaceOriginLocation();
			kbatchedAnimController2.Play("lightning", KAnim.PlayMode.Loop, 1f, 0f);
			GameObject gameObject3 = new GameObject("impactFX");
			gameObject3.SetActive(false);
			KBatchedAnimController kbatchedAnimController3 = gameObject3.AddComponent<KBatchedAnimController>();
			base.sm.impactFX.Set(kbatchedAnimController3, base.smi, false);
			kbatchedAnimController3.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_impact_fx_kanim")
			});
			gameObject3.SetActive(true);
			kbatchedAnimController3.Play("stress_beam_impact_fx", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06008B17 RID: 35607 RVA: 0x0035D204 File Offset: 0x0035B404
		public Vector3 FaceOriginLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
			return vector;
		}

		// Token: 0x06008B18 RID: 35608 RVA: 0x0035D25C File Offset: 0x0035B45C
		public void ClearBeam(bool clearFaceFX = true)
		{
			base.sm.previousTarget.Set(base.sm.beamTarget.Get(base.smi), base.smi, false);
			base.sm.beamTarget.Set(null, base.smi, false);
			if (base.sm.beamFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.beamFX.Get(base.smi).gameObject);
				base.sm.beamFX.Set(null, base.smi, false);
			}
			if (base.sm.impactFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.impactFX.Get(base.smi).gameObject);
				base.sm.impactFX.Set(null, base.smi, false);
			}
			if (clearFaceFX && base.sm.faceLightningFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.faceLightningFX.Get(base.smi).gameObject);
				base.sm.faceLightningFX.Set(null, base.smi, false);
			}
		}

		// Token: 0x06008B19 RID: 35609 RVA: 0x0035D3B8 File Offset: 0x0035B5B8
		public void AimBeam(Vector3 targetPosition, int beamIdx)
		{
			Vector3 position = this.FaceOriginLocation();
			position.z = base.transform.position.z + 0.01f;
			base.smi.sm.beamFX.Get(base.smi).transform.SetPosition(position);
			Vector3 v = Vector3.Normalize(targetPosition - base.smi.sm.beamFX.Get(base.smi).transform.position);
			float rotation = MathUtil.AngleSigned(Vector3.up, v, Vector3.forward) + 90f;
			base.smi.sm.beamFX.Get(base.smi).Rotation = rotation;
			base.smi.sm.impactFX.Get(base.smi).transform.position = targetPosition;
			base.smi.sm.faceLightningFX.Get(base.smi).FlipX = (targetPosition.x < base.smi.sm.faceLightningFX.Get(base.smi).transform.position.x);
			Vector3 position2 = base.smi.sm.beamFX.Get(base.smi).transform.position;
			position2.z = 0f;
			Vector3 b = targetPosition;
			b.z = 0f;
			float num = Vector3.Distance(position2, b);
			if (num > 3f)
			{
				if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam3")
				{
					base.smi.sm.beamFX.Get(base.smi).Play("beam3", KAnim.PlayMode.Loop, 1f, 0f);
				}
				base.smi.sm.beamFX.Get(base.smi).animWidth = num / 3f;
				return;
			}
			if (num > 2f)
			{
				if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam2")
				{
					base.smi.sm.beamFX.Get(base.smi).Play("beam2", KAnim.PlayMode.Loop, 1f, 0f);
				}
				base.smi.sm.beamFX.Get(base.smi).animWidth = num / 2f;
				return;
			}
			if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam1")
			{
				base.smi.sm.beamFX.Get(base.smi).Play("beam1", KAnim.PlayMode.Loop, 1f, 0f);
			}
			base.smi.sm.beamFX.Get(base.smi).animWidth = num;
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x0035D760 File Offset: 0x0035B960
		public void ShowBeam(bool show)
		{
			if (base.smi.sm.impactFX.Get(base.smi) != null)
			{
				base.smi.sm.impactFX.Get(base.smi).enabled = show;
			}
			if (base.smi.sm.beamFX.Get(base.smi) != null)
			{
				base.smi.sm.beamFX.Get(base.smi).enabled = show;
			}
		}

		// Token: 0x04006A67 RID: 27239
		public Notification notification;

		// Token: 0x04006A68 RID: 27240
		[MySmiReq]
		public BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04006A69 RID: 27241
		public BionicBatteryMonitor.WattageModifier powerDrainModifier = new BionicBatteryMonitor.WattageModifier("StressShockChore", string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.TRAITS.STRESSSHOCKER.DRAIN_ATTRIBUTE, "<b>+</b>" + GameUtil.GetFormattedWattage(STRESS.SHOCKER.POWER_CONSUMPTION_RATE, GameUtil.WattageFormatterUnit.Automatic, true)), STRESS.SHOCKER.POWER_CONSUMPTION_RATE, STRESS.SHOCKER.POWER_CONSUMPTION_RATE);
	}

	// Token: 0x02001328 RID: 4904
	public class States : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore>
	{
		// Token: 0x06008B1B RID: 35611 RVA: 0x0035D7F8 File Offset: 0x0035B9F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.shocking.findDestination;
			base.serializable = StateMachine.SerializeType.Never;
			base.Target(this.shocker);
			this.shocking.EventTransition(GameHashes.BionicOffline, this.offline, null).DefaultState(this.shocking.findDestination).ToggleAnims("anim_loco_stressshocker_kanim", 0f).ParamTransition<float>(this.powerConsumed, this.complete, (StressShockChore.StatesInstance smi, float p) => p >= STRESS.SHOCKER.MAX_POWER_USE).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.MakeBeam();
			}).Exit(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ClearBeam(true);
			});
			this.shocking.findDestination.Enter("FindDestination", delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(false);
				smi.FindDestination();
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				float delta_value = dt * STRESS.SHOCKER.FAKE_POWER_CONSUMPTION_RATE;
				smi.sm.powerConsumed.Delta(delta_value, smi);
				smi.FindDestination();
			}, UpdateRate.SIM_1000ms, false);
			this.shocking.runAroundShockingStuff.MoveTo((StressShockChore.StatesInstance smi) => smi.sm.targetMoveLocation.Get(smi), this.shocking.findDestination, this.delay, false).Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				smi.ShockUpdate200(smi, dt);
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.ShockUpdateRender(smi, dt);
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.shocking.standStillShockingStuff.Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).PlayAnim("interrupt_shocker", KAnim.PlayMode.Loop).ScheduleGoTo(2f, this.delay).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				smi.ShockUpdate200(smi, dt);
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.ShockUpdateRender(smi, dt);
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.delay.ScheduleGoTo(0.5f, this.shocking);
			this.complete.Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
			this.offline.Enter(new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.ForceStressMonitorToTimeOut)).ReturnSuccess();
		}

		// Token: 0x04006A6A RID: 27242
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.TargetParameter shocker;

		// Token: 0x04006A6B RID: 27243
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController[]> cosmeticBeamFXs;

		// Token: 0x04006A6C RID: 27244
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> beamFX;

		// Token: 0x04006A6D RID: 27245
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> impactFX;

		// Token: 0x04006A6E RID: 27246
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> faceLightningFX;

		// Token: 0x04006A6F RID: 27247
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> beamTarget;

		// Token: 0x04006A70 RID: 27248
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> previousTarget;

		// Token: 0x04006A71 RID: 27249
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.IntParameter targetMoveLocation;

		// Token: 0x04006A72 RID: 27250
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.FloatParameter powerConsumed;

		// Token: 0x04006A73 RID: 27251
		public StressShockChore.States.ShockStates shocking;

		// Token: 0x04006A74 RID: 27252
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State delay;

		// Token: 0x04006A75 RID: 27253
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State complete;

		// Token: 0x04006A76 RID: 27254
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State offline;

		// Token: 0x020027E9 RID: 10217
		public class ShockStates : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State
		{
			// Token: 0x0400B0EB RID: 45291
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State findDestination;

			// Token: 0x0400B0EC RID: 45292
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State runAroundShockingStuff;

			// Token: 0x0400B0ED RID: 45293
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State standStillShockingStuff;
		}
	}
}
