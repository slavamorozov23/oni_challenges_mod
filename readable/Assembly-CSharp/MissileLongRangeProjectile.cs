using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
[SerializationConfig(MemberSerialization.OptIn)]
public class MissileLongRangeProjectile : GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>
{
	// Token: 0x0600347F RID: 13439 RVA: 0x00129D88 File Offset: 0x00127F88
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ParamTransition<GameObject>(this.asteroidTarget, this.launch, (MissileLongRangeProjectile.StatesInstance smi, GameObject target) => !target.IsNullOrDestroyed());
		this.launch.Update("Launch", delegate(MissileLongRangeProjectile.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<bool>(this.triggeroutofworld, this.leaveworld, GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.IsTrue).Enter(delegate(MissileLongRangeProjectile.StatesInstance smi)
		{
			Vector3 position = smi.master.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
			smi.smokeTrailFX = Util.KInstantiate(EffectPrefabs.Instance.LongRangeMissileSmokeTrailFX, position);
			smi.smokeTrailFX.transform.SetParent(smi.master.transform);
			smi.smokeTrailFX.SetActive(true);
			smi.StartTakeoff();
			KFMOD.PlayOneShot(GlobalAssets.GetSound("MissileLauncher_Missile_ignite", false), CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		});
		this.leaveworld.Enter(delegate(MissileLongRangeProjectile.StatesInstance smi)
		{
			smi.ExitWorldEnterStarmap();
		});
	}

	// Token: 0x04001FB6 RID: 8118
	public GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State launch;

	// Token: 0x04001FB7 RID: 8119
	public GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.State leaveworld;

	// Token: 0x04001FB8 RID: 8120
	public StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.BoolParameter triggeroutofworld = new StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.BoolParameter(false);

	// Token: 0x04001FB9 RID: 8121
	public StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.ObjectParameter<GameObject> asteroidTarget = new StateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.ObjectParameter<GameObject>();

	// Token: 0x02001700 RID: 5888
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007699 RID: 30361
		public string starmapOverrideSymbol = "payload";

		// Token: 0x0400769A RID: 30362
		public string missileName = "STRINGS.ITEMS.MISSILE_LONGRANGE.NAME";

		// Token: 0x0400769B RID: 30363
		public string missileDesc = "STRINGS.ITEMS.MISSILE_LONGRANGE.DESC";
	}

	// Token: 0x02001701 RID: 5889
	public class StatesInstance : GameStateMachine<MissileLongRangeProjectile, MissileLongRangeProjectile.StatesInstance, IStateMachineTarget, MissileLongRangeProjectile.Def>.GameInstance
	{
		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060099A3 RID: 39331 RVA: 0x0038E024 File Offset: 0x0038C224
		private Vector3 Position
		{
			get
			{
				return base.transform.position + this.animController.Offset;
			}
		}

		// Token: 0x060099A4 RID: 39332 RVA: 0x0038E041 File Offset: 0x0038C241
		public StatesInstance(IStateMachineTarget master, MissileLongRangeProjectile.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x060099A5 RID: 39333 RVA: 0x0038E06C File Offset: 0x0038C26C
		public override void StartSM()
		{
			base.StartSM();
			if (this.launchedTarget.Get() != null)
			{
				base.sm.asteroidTarget.Set(this.launchedTarget.Get().gameObject, this, false);
				this.myWorld = ClusterManager.Instance.GetWorld(this.myWorldId);
			}
		}

		// Token: 0x060099A6 RID: 39334 RVA: 0x0038E0CB File Offset: 0x0038C2CB
		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			base.GetComponent<Pickupable>().handleFallerComponents = false;
		}

		// Token: 0x060099A7 RID: 39335 RVA: 0x0038E0FC File Offset: 0x0038C2FC
		public void UpdateLaunch(float dt)
		{
			float rotation = MathUtil.AngleSigned(Vector3.up, Vector3.up, Vector3.forward);
			this.animController.Rotation = rotation;
			int cell = Grid.PosToCell(this.Position);
			Vector2I vector2I = Grid.CellToXY(cell);
			if (!Grid.IsValidCell(cell))
			{
				base.smi.sm.triggeroutofworld.Set(true, base.smi, false);
				return;
			}
			if (Grid.IsValidCellInWorld(Grid.PosToCell(this.Position), this.myWorldId) && (float)vector2I.y < this.myWorld.maximumBounds.y)
			{
				base.transform.SetPosition(base.transform.position + Vector3.up * (this.launchSpeed * dt));
			}
			else
			{
				this.animController.Offset += Vector3.up * (this.launchSpeed * dt);
			}
			ParticleSystem[] componentsInChildren = base.smi.smokeTrailFX.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.transform.SetPositionAndRotation(this.Position, Quaternion.identity);
			}
		}

		// Token: 0x060099A8 RID: 39336 RVA: 0x0038E228 File Offset: 0x0038C428
		public void PrepareLaunch(GameObject asteroid_target, float speed, Vector3 launchPos, float launchAngle)
		{
			base.gameObject.transform.SetParent(null);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			launchPos.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
			base.gameObject.transform.SetLocalPosition(launchPos);
			this.animController.Rotation = launchAngle;
			this.animController.Offset = Vector3.back;
			this.animController.SetVisiblity(true);
			FetchableMonitor.Instance smi = base.gameObject.GetSMI<FetchableMonitor.Instance>();
			if (smi != null)
			{
				smi.SetForceUnfetchable(true);
			}
			base.sm.triggeroutofworld.Set(false, base.smi, false);
			base.sm.asteroidTarget.Set(asteroid_target, base.smi, false);
			this.launchedTarget = new Ref<KPrefabID>(asteroid_target.GetComponent<KPrefabID>());
			this.launchSpeed = speed;
			this.myWorld = base.gameObject.GetMyWorld();
			this.myWorldId = this.myWorld.id;
			ClusterGridEntity component = this.myWorld.GetComponent<ClusterGridEntity>();
			if (component != null)
			{
				this.myLocation = component.Location;
			}
		}

		// Token: 0x060099A9 RID: 39337 RVA: 0x0038E348 File Offset: 0x0038C548
		public void ExitWorldEnterStarmap()
		{
			GameObject gameObject = base.sm.asteroidTarget.Get(base.smi);
			if (gameObject != null)
			{
				ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
				if (component != null)
				{
					GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("ClusterMapLongRangeMissile"), Grid.SceneLayer.NoLayer, null, 0);
					gameObject2.SetActive(true);
					gameObject2.GetSMI<ClusterMapLongRangeMissile.StatesInstance>().Setup(this.myLocation, component, base.def);
				}
				else
				{
					gameObject.Trigger(-2056344675, MissileLongRangeConfig.DamageEventPayload.sharedInstance);
				}
			}
			Util.KDestroyGameObject(base.gameObject);
		}

		// Token: 0x0400769C RID: 30364
		public KBatchedAnimController animController;

		// Token: 0x0400769D RID: 30365
		[Serialize]
		private float launchSpeed;

		// Token: 0x0400769E RID: 30366
		public GameObject smokeTrailFX;

		// Token: 0x0400769F RID: 30367
		private WorldContainer myWorld;

		// Token: 0x040076A0 RID: 30368
		[Serialize]
		private AxialI myLocation;

		// Token: 0x040076A1 RID: 30369
		[Serialize]
		private int myWorldId = -1;

		// Token: 0x040076A2 RID: 30370
		[Serialize]
		private Ref<KPrefabID> launchedTarget = new Ref<KPrefabID>();
	}
}
