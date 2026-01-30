using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000047 RID: 71
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterMapLongRangeMissile : GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>
{
	// Token: 0x06000157 RID: 343 RVA: 0x0000A5BC File Offset: 0x000087BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialization;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.ToggleTag(GameTags.EntityInSpace);
		this.initialization.Enter(delegate(ClusterMapLongRangeMissile.StatesInstance smi)
		{
			if (smi.exploded)
			{
				smi.GoTo(smi.sm.cleanup);
				return;
			}
			if (this.targetObject.Get(smi) != null)
			{
				smi.GoTo(smi.sm.travelling.moving);
				return;
			}
			smi.GoTo(smi.sm.contact);
		});
		this.travelling.ToggleStatusItem(Db.Get().MiscStatusItems.LongRangeMissileTTI, null).OnTargetLost(this.targetObject, this.contact).Target(this.targetObject).EventHandler(GameHashes.ClusterLocationChanged, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback(ClusterMapLongRangeMissile.UpdatePath)).Target(this.masterTarget);
		this.travelling.moving.ToggleTag(GameTags.LongRangeMissileMoving).EnterTransition(this.travelling.idle, (ClusterMapLongRangeMissile.StatesInstance smi) => !smi.IsTraveling()).EventTransition(GameHashes.ClusterDestinationReached, this.travelling.idle, null);
		this.travelling.idle.ToggleTag(GameTags.LongRangeMissileIdle).Transition(this.contact, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HitTarget), UpdateRate.SIM_1000ms).Transition(this.contact, GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Not(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.CanHitTarget)), UpdateRate.SIM_1000ms);
		this.contact.Enter(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State.Callback(ClusterMapLongRangeMissile.TriggerDamage)).EnterTransition(this.exploding_with_visual, new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HasVisualizer)).EnterTransition(this.cleanup, GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Not(new StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.Transition.ConditionCallback(ClusterMapLongRangeMissile.HasVisualizer)));
		this.exploding_with_visual.ToggleTag(GameTags.LongRangeMissileExploding).EventTransition(GameHashes.RocketExploded, this.cleanup, null);
		this.cleanup.Enter(delegate(ClusterMapLongRangeMissile.StatesInstance smi)
		{
			smi.gameObject.DeleteObject();
		}).GoTo(null);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000A79E File Offset: 0x0000899E
	private static bool HasVisualizer(ClusterMapLongRangeMissile.StatesInstance smi)
	{
		return smi != null && ClusterMapScreen.Instance.GetEntityVisAnim(smi.GetComponent<ClusterGridEntity>()) != null;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000A7BC File Offset: 0x000089BC
	public static void TriggerDamage(ClusterMapLongRangeMissile.StatesInstance smi)
	{
		GameObject gameObject = smi.sm.targetObject.Get(smi);
		if (gameObject != null && ClusterMapLongRangeMissile.CanHitTarget(smi))
		{
			gameObject.Trigger(-2056344675, MissileLongRangeConfig.DamageEventPayload.sharedInstance);
		}
		smi.exploded = true;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000A804 File Offset: 0x00008A04
	public static bool HitTarget(ClusterMapLongRangeMissile.StatesInstance smi)
	{
		ClusterGridEntity clusterGridEntity = smi.sm.targetObject.Get<ClusterGridEntity>(smi);
		return !(clusterGridEntity == null) && clusterGridEntity.Location == smi.sm.destinationHex.Get(smi);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000A84A File Offset: 0x00008A4A
	public static bool CanHitTarget(ClusterMapLongRangeMissile.StatesInstance smi)
	{
		return smi.sm.targetObject.Get(smi) != null;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000A864 File Offset: 0x00008A64
	private static void UpdatePath(ClusterMapLongRangeMissile.StatesInstance smi)
	{
		ClusterDestinationSelector component = smi.GetComponent<ClusterDestinationSelector>();
		if (component == null)
		{
			return;
		}
		ClusterGridEntity clusterGridEntity = smi.sm.targetObject.Get<ClusterGridEntity>(smi);
		if (clusterGridEntity == null)
		{
			return;
		}
		ClusterGridEntity component2 = smi.GetComponent<ClusterGridEntity>();
		AxialI axialI = ClusterMapLongRangeMissile.StatesInstance.FindInterceptPoint(component2.Location, clusterGridEntity, component, 99999);
		if (axialI != smi.sm.destinationHex.Get(smi))
		{
			smi.Travel(component2.Location, axialI);
		}
	}

	// Token: 0x040000D4 RID: 212
	public StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.TargetParameter targetObject;

	// Token: 0x040000D5 RID: 213
	public StateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.AxialIParameter destinationHex;

	// Token: 0x040000D6 RID: 214
	public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State initialization;

	// Token: 0x040000D7 RID: 215
	public ClusterMapLongRangeMissile.TravellingStates travelling;

	// Token: 0x040000D8 RID: 216
	public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State contact;

	// Token: 0x040000D9 RID: 217
	public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State exploding_with_visual;

	// Token: 0x040000DA RID: 218
	public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State cleanup;

	// Token: 0x0200107E RID: 4222
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200107F RID: 4223
	public class TravellingStates : GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State
	{
		// Token: 0x040062A0 RID: 25248
		public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State moving;

		// Token: 0x040062A1 RID: 25249
		public GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.State idle;
	}

	// Token: 0x02001080 RID: 4224
	public class StatesInstance : GameStateMachine<ClusterMapLongRangeMissile, ClusterMapLongRangeMissile.StatesInstance, IStateMachineTarget, ClusterMapLongRangeMissile.Def>.GameInstance
	{
		// Token: 0x06008236 RID: 33334 RVA: 0x003418F0 File Offset: 0x0033FAF0
		public StatesInstance(IStateMachineTarget master, ClusterMapLongRangeMissile.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x06008237 RID: 33335 RVA: 0x00341908 File Offset: 0x0033FB08
		public void Setup(AxialI source, ClusterGridEntity target, MissileLongRangeProjectile.Def projectile_def)
		{
			BallisticClusterGridEntity component = base.GetComponent<BallisticClusterGridEntity>();
			component.nameKey = new StringKey(projectile_def.missileName);
			base.GetComponent<InfoDescription>().description = Strings.Get(projectile_def.missileDesc);
			component.SwapSymbolFromSameAnim("payload", projectile_def.starmapOverrideSymbol);
			KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(projectile_def.starmapOverrideSymbol);
			this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride("payload", symbol, 0);
			base.sm.targetObject.Set(target.gameObject, this, false);
			this.Travel(source, ClusterMapLongRangeMissile.StatesInstance.FindInterceptPoint(source, target, base.GetComponent<ClusterDestinationSelector>(), 99999));
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x003419D0 File Offset: 0x0033FBD0
		public static AxialI FindInterceptPoint(AxialI source, ClusterGridEntity target, ClusterDestinationSelector selector, int maxGridRange = 99999)
		{
			ClusterTraveler component = target.GetComponent<ClusterTraveler>();
			if (component != null)
			{
				List<AxialI> currentPath = component.CurrentPath;
				AxialI result = target.Location;
				foreach (AxialI axialI in currentPath)
				{
					float num = component.TravelETA(axialI);
					List<AxialI> path = ClusterGrid.Instance.GetPath(source, axialI, selector);
					if (path != null && path.Count != 0 && path.Count <= maxGridRange && (float)path.Count * 600f / 10f < num)
					{
						return result;
					}
					result = axialI;
				}
			}
			return target.Location;
		}

		// Token: 0x06008239 RID: 33337 RVA: 0x00341A90 File Offset: 0x0033FC90
		public float InterceptETA()
		{
			ClusterTraveler component = base.GetComponent<ClusterTraveler>();
			float a = 0f;
			float b = component.TravelETA();
			GameObject gameObject = base.sm.targetObject.Get(this);
			if (gameObject != null)
			{
				ClusterTraveler component2 = gameObject.GetComponent<ClusterTraveler>();
				if (component2 != null && component.CurrentPath != null)
				{
					a = component2.TravelETA(component.Destination);
				}
			}
			return Mathf.Max(a, b);
		}

		// Token: 0x0600823A RID: 33338 RVA: 0x00341AFB File Offset: 0x0033FCFB
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			base.sm.destinationHex.Set(destination, this, false);
			this.GoTo(base.sm.travelling.moving);
		}

		// Token: 0x0600823B RID: 33339 RVA: 0x00341B34 File Offset: 0x0033FD34
		public bool IsTraveling()
		{
			ClusterTraveler component = base.GetComponent<ClusterTraveler>();
			return component.CurrentPath != null && component.CurrentPath.Count != 0;
		}

		// Token: 0x040062A2 RID: 25250
		[Serialize]
		public bool exploded;

		// Token: 0x040062A3 RID: 25251
		public KBatchedAnimController animController;
	}
}
