using System;
using UnityEngine;

// Token: 0x02000BB9 RID: 3001
public class ClusterMapTravelAnimator : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06005A12 RID: 23058 RVA: 0x0020AEA4 File Offset: 0x002090A4
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.OnTargetLost(this.entityTarget, null);
		this.idle.Target(this.entityTarget).Transition(this.grounded, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Target(this.masterTarget);
		this.grounded.Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms);
		this.surfaceTransitioning.Update(delegate(ClusterMapTravelAnimator.StatesInstance smi, float dt)
		{
			this.DoOrientToPath(smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.repositioning, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms);
		this.repositioning.Transition(this.traveling.orientToIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoReposition), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.DefaultState(this.traveling.orientToPath);
		this.traveling.travelIdle.Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToIdle, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToPath, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath))).EventTransition(GameHashes.ClusterLocationChanged, this.traveling.move, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove))).Target(this.masterTarget);
		this.traveling.orientToPath.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath), UpdateRate.RENDER_EVERY_TICK).Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).Target(this.masterTarget);
		this.traveling.move.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove), UpdateRate.RENDER_EVERY_TICK).Exit(delegate(ClusterMapTravelAnimator.StatesInstance smi)
		{
			smi.gameObject.Trigger(-408710611, null);
		});
		this.traveling.orientToIdle.Transition(this.idle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToIdle), UpdateRate.RENDER_EVERY_TICK);
	}

	// Token: 0x06005A13 RID: 23059 RVA: 0x0020B1BB File Offset: 0x002093BB
	private bool IsTraveling(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling();
	}

	// Token: 0x06005A14 RID: 23060 RVA: 0x0020B1D0 File Offset: 0x002093D0
	private bool IsSurfaceTransitioning(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x06005A15 RID: 23061 RVA: 0x0020B208 File Offset: 0x00209408
	private bool IsGrounded(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06005A16 RID: 23062 RVA: 0x0020B238 File Offset: 0x00209438
	private bool DoReposition(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x06005A17 RID: 23063 RVA: 0x0020B264 File Offset: 0x00209464
	private bool DoMove(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x06005A18 RID: 23064 RVA: 0x0020B290 File Offset: 0x00209490
	private bool DoOrientToPath(ClusterMapTravelAnimator.StatesInstance smi)
	{
		float pathAngle = smi.GetComponent<ClusterMapVisualizer>().GetPathAngle();
		return smi.RotateTowards(pathAngle, Time.unscaledDeltaTime);
	}

	// Token: 0x06005A19 RID: 23065 RVA: 0x0020B2B5 File Offset: 0x002094B5
	private bool DoOrientToIdle(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.keepRotationOnIdle || smi.RotateTowards(0f, Time.unscaledDeltaTime);
	}

	// Token: 0x06005A1A RID: 23066 RVA: 0x0020B2D4 File Offset: 0x002094D4
	private bool ClusterChangedAtMyLocation(ClusterMapTravelAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x04003C43 RID: 15427
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04003C44 RID: 15428
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x04003C45 RID: 15429
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State repositioning;

	// Token: 0x04003C46 RID: 15430
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State surfaceTransitioning;

	// Token: 0x04003C47 RID: 15431
	public ClusterMapTravelAnimator.TravelingStates traveling;

	// Token: 0x04003C48 RID: 15432
	public StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x02001D5D RID: 7517
	private class Tuning : TuningData<ClusterMapTravelAnimator.Tuning>
	{
		// Token: 0x04008B19 RID: 35609
		public float visualizerTransitionSpeed = 1f;

		// Token: 0x04008B1A RID: 35610
		public float visualizerRotationSpeed = 1f;
	}

	// Token: 0x02001D5E RID: 7518
	public class TravelingStates : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04008B1B RID: 35611
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State travelIdle;

		// Token: 0x04008B1C RID: 35612
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToPath;

		// Token: 0x04008B1D RID: 35613
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State move;

		// Token: 0x04008B1E RID: 35614
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToIdle;
	}

	// Token: 0x02001D5F RID: 7519
	public class StatesInstance : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600B102 RID: 45314 RVA: 0x003DBFA4 File Offset: 0x003DA1A4
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600B103 RID: 45315 RVA: 0x003DBFC8 File Offset: 0x003DA1C8
		public bool MoveTowards(Vector3 targetPosition, float dt)
		{
			RectTransform component = base.GetComponent<RectTransform>();
			ClusterMapVisualizer component2 = base.GetComponent<ClusterMapVisualizer>();
			Vector3 localPosition = component.GetLocalPosition();
			Vector3 vector = targetPosition - localPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerTransitionSpeed * dt;
			if (num < magnitude)
			{
				Vector3 b = normalized * num;
				component.SetLocalPosition(localPosition + b);
				component2.RefreshPathDrawing();
				return false;
			}
			component.SetLocalPosition(targetPosition);
			component2.RefreshPathDrawing();
			return true;
		}

		// Token: 0x0600B104 RID: 45316 RVA: 0x003DC04C File Offset: 0x003DA24C
		public bool RotateTowards(float targetAngle, float dt)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			float num = targetAngle - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerRotationSpeed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			this.simpleAngle = targetAngle;
			component.SetAnimRotation(this.simpleAngle);
			return true;
		}

		// Token: 0x04008B1F RID: 35615
		public ClusterGridEntity entity;

		// Token: 0x04008B20 RID: 35616
		private float simpleAngle;

		// Token: 0x04008B21 RID: 35617
		public bool keepRotationOnIdle;
	}
}
