using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class CritterCondoStates : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>
{
	// Token: 0x06000430 RID: 1072 RVA: 0x00022E70 File Offset: 0x00021070
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToCondo;
		this.root.Enter(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ReserveCondo)).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.UnreserveCondo));
		this.goingToCondo.MoveTo(new Func<CritterCondoStates.Instance, int>(CritterCondoStates.GetCondoInteractCell), this.interact, null, false).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.moveToStatusItem, null).OnTargetLost(this.targetCondo, null);
		this.interact.DefaultState(this.interact.pre).OnTargetLost(this.targetCondo, null).Enter(delegate(CritterCondoStates.Instance smi)
		{
			this.SetFacing(smi);
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).Exit(delegate(CritterCondoStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.interactStatusItem, null);
		this.interact.pre.PlayAnim("cc_working_pre").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pre");
		}).OnAnimQueueComplete(this.interact.loop);
		this.interact.loop.PlayAnim("cc_working").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, smi.def.working_anim);
		}).OnAnimQueueComplete(this.interact.pst);
		this.interact.pst.PlayAnim("cc_working_pst").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pst");
		}).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviour_InteractWithCritterCondo, false).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ApplyEffects));
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00023078 File Offset: 0x00021278
	private void SetFacing(CritterCondoStates.Instance smi)
	{
		bool isRotated = CritterCondoStates.GetTargetCondo(smi).Get<Rotatable>().IsRotated;
		smi.Get<Facing>().SetFacing(isRotated);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000230A4 File Offset: 0x000212A4
	private static CritterCondo.Instance GetTargetCondo(CritterCondoStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCondo.Get(smi);
		CritterCondo.Instance instance = (gameObject != null) ? gameObject.GetSMI<CritterCondo.Instance>() : null;
		if (instance.IsNullOrStopped())
		{
			return null;
		}
		return instance;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000230E4 File Offset: 0x000212E4
	private static void ReserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = smi.GetSMI<CritterCondoInteractMontior.Instance>().targetCondo;
		if (instance == null)
		{
			return;
		}
		smi.sm.targetCondo.Set(instance.gameObject, smi, false);
		instance.SetReserved(true);
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00023124 File Offset: 0x00021324
	private static void UnreserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return;
		}
		instance.GetComponent<KBatchedAnimController>().Play("on", KAnim.PlayMode.Loop, 1f, 0f);
		smi.sm.targetCondo.Set(null, smi);
		instance.SetReserved(false);
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00023178 File Offset: 0x00021378
	private static int GetCondoInteractCell(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		int num = instance.GetInteractStartCell();
		if (smi.isLargeCritter)
		{
			bool isRotated = instance.Get<Rotatable>().IsRotated;
			Vector2I vector2I = Grid.PosToXY(smi.gameObject.transform.position);
			Vector2I vector2I2 = Grid.CellToXY(num);
			if (vector2I.x > vector2I2.x && !isRotated)
			{
				num = Grid.CellLeft(num);
			}
			else if (vector2I.x < vector2I2.x && isRotated)
			{
				num = Grid.CellRight(num);
			}
		}
		return num;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00023204 File Offset: 0x00021404
	private static void ApplyEffects(CritterCondoStates.Instance smi)
	{
		smi.Get<Effects>().Add(CritterCondoStates.GetTargetCondo(smi).def.effectId, true);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00023224 File Offset: 0x00021424
	private static void PlayCondoBuildingAnim(CritterCondoStates.Instance smi, string anim_name)
	{
		CritterCondo.Instance smi2 = smi.sm.targetCondo.GetSMI<CritterCondo.Instance>(smi);
		if (smi2 != null)
		{
			smi2.UpdateCritterAnims(anim_name, smi.def.entersBuilding, smi.isLargeCritter);
		}
	}

	// Token: 0x04000320 RID: 800
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State goingToCondo;

	// Token: 0x04000321 RID: 801
	public CritterCondoStates.InteractState interact;

	// Token: 0x04000322 RID: 802
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State behaviourComplete;

	// Token: 0x04000323 RID: 803
	public StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.TargetParameter targetCondo;

	// Token: 0x0200110A RID: 4362
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040063DC RID: 25564
		public bool entersBuilding = true;

		// Token: 0x040063DD RID: 25565
		public string working_anim = "cc_working";
	}

	// Token: 0x0200110B RID: 4363
	public new class Instance : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.GameInstance
	{
		// Token: 0x06008394 RID: 33684 RVA: 0x003439CC File Offset: 0x00341BCC
		public Instance(Chore<CritterCondoStates.Instance> chore, CritterCondoStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_InteractWithCritterCondo);
			this.isLargeCritter = base.GetComponent<KPrefabID>().HasTag(GameTags.LargeCreature);
		}

		// Token: 0x040063DE RID: 25566
		public bool isLargeCritter;
	}

	// Token: 0x0200110C RID: 4364
	public class InteractState : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State
	{
		// Token: 0x040063DF RID: 25567
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pre;

		// Token: 0x040063E0 RID: 25568
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State loop;

		// Token: 0x040063E1 RID: 25569
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pst;
	}
}
