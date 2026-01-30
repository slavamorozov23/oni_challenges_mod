using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class TreeClimbStates : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>
{
	// Token: 0x06000528 RID: 1320 RVA: 0x00029A1C File Offset: 0x00027C1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state = this.root.Enter(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.SetTarget)).Enter(delegate(TreeClimbStates.Instance smi)
		{
			if (!TreeClimbStates.ReserveClimbable(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.UnreserveClimbable));
		string name = CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME;
		string tooltip = CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moving.MoveTo(new Func<TreeClimbStates.Instance, int>(TreeClimbStates.GetClimbableCell), this.climbing, this.behaviourcomplete, false);
		this.climbing.DefaultState(this.climbing.pre);
		this.climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(this.climbing.loop);
		this.climbing.loop.QueueAnim("rummage_loop", true, null).ScheduleGoTo(3.5f, this.climbing.pst).Update(new Action<TreeClimbStates.Instance, float>(TreeClimbStates.Rummage), UpdateRate.SIM_1000ms, false);
		this.climbing.pst.QueueAnim("rummage_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree, false);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00029B84 File Offset: 0x00027D84
	private static void SetTarget(TreeClimbStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi, false);
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00029BA4 File Offset: 0x00027DA4
	private static bool ReserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00029BE8 File Offset: 0x00027DE8
	private static void UnreserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00029C1C File Offset: 0x00027E1C
	private static void Rummage(TreeClimbStates.Instance smi, float dt)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			ForestTreeSeedMonitor component = gameObject.GetComponent<ForestTreeSeedMonitor>();
			if (component != null)
			{
				component.ExtractExtraSeed();
				return;
			}
			Storage component2 = gameObject.GetComponent<Storage>();
			if (component2 && component2.items.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, component2.items.Count - 1);
				GameObject gameObject2 = component2.items[index];
				Pickupable pickupable = gameObject2 ? gameObject2.GetComponent<Pickupable>() : null;
				if (pickupable && pickupable.UnreservedAmount > 0.01f)
				{
					smi.Toss(pickupable);
				}
			}
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00029CD1 File Offset: 0x00027ED1
	private static int GetClimbableCell(TreeClimbStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x040003B7 RID: 951
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving;

	// Token: 0x040003B8 RID: 952
	public TreeClimbStates.ClimbState climbing;

	// Token: 0x040003B9 RID: 953
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State behaviourcomplete;

	// Token: 0x040003BA RID: 954
	public StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.TargetParameter target;

	// Token: 0x020011A5 RID: 4517
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011A6 RID: 4518
	public new class Instance : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.GameInstance
	{
		// Token: 0x0600851C RID: 34076 RVA: 0x00346C24 File Offset: 0x00344E24
		public Instance(Chore<TreeClimbStates.Instance> chore, TreeClimbStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToClimbTree);
			this.storage = base.GetComponent<Storage>();
		}

		// Token: 0x0600851D RID: 34077 RVA: 0x00346C54 File Offset: 0x00344E54
		public void Toss(Pickupable pu)
		{
			Pickupable pickupable;
			if (pu.PrimaryElement.MassPerUnit > 1f)
			{
				pickupable = pu.TakeUnit(1f);
			}
			else
			{
				pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedFetchAmount));
			}
			if (pickupable != null)
			{
				this.storage.Store(pickupable.gameObject, true, false, true, false);
				this.storage.Drop(pickupable.gameObject, true);
				this.Throw(pickupable.gameObject);
			}
		}

		// Token: 0x0600851E RID: 34078 RVA: 0x00346CD8 File Offset: 0x00344ED8
		private void Throw(GameObject ore_go)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(position);
			int num2 = Grid.CellAbove(num);
			Vector2 zero;
			if ((Grid.IsValidCell(num) && Grid.Solid[num]) || (Grid.IsValidCell(num2) && Grid.Solid[num2]))
			{
				zero = Vector2.zero;
			}
			else
			{
				position.y += 0.5f;
				zero = new Vector2(UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.x, TreeClimbStates.Instance.VEL_MAX.x), UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.y, TreeClimbStates.Instance.VEL_MAX.y));
			}
			ore_go.transform.SetPosition(position);
			if (GameComps.Fallers.Has(ore_go))
			{
				GameComps.Fallers.Remove(ore_go);
			}
			GameComps.Fallers.Add(ore_go, zero);
		}

		// Token: 0x0400654A RID: 25930
		private Storage storage;

		// Token: 0x0400654B RID: 25931
		private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);

		// Token: 0x0400654C RID: 25932
		private static readonly Vector2 VEL_MAX = new Vector2(1f, 4f);
	}

	// Token: 0x020011A7 RID: 4519
	public class ClimbState : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State
	{
		// Token: 0x0400654D RID: 25933
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pre;

		// Token: 0x0400654E RID: 25934
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State loop;

		// Token: 0x0400654F RID: 25935
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pst;
	}
}
