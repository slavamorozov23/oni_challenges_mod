using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000837 RID: 2103
public class CargoDropperMinion : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>
{
	// Token: 0x0600395E RID: 14686 RVA: 0x00140594 File Offset: 0x0013E794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notLanded;
		this.root.ParamTransition<bool>(this.hasLanded, this.complete, GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.IsTrue);
		this.notLanded.EventHandlerTransition(GameHashes.JettisonCargo, this.landed, (CargoDropperMinion.StatesInstance smi, object obj) => true);
		this.landed.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			smi.JettisonCargo(null);
			smi.GoTo(this.exiting);
		});
		this.exiting.Update(delegate(CargoDropperMinion.StatesInstance smi, float dt)
		{
			if (!smi.SyncMinionExitAnimation())
			{
				smi.GoTo(this.complete);
			}
		}, UpdateRate.SIM_200ms, false);
		this.complete.Enter(delegate(CargoDropperMinion.StatesInstance smi)
		{
			this.hasLanded.Set(true, smi, false);
		});
	}

	// Token: 0x0400230D RID: 8973
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State notLanded;

	// Token: 0x0400230E RID: 8974
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State landed;

	// Token: 0x0400230F RID: 8975
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State exiting;

	// Token: 0x04002310 RID: 8976
	private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State complete;

	// Token: 0x04002311 RID: 8977
	public StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter hasLanded = new StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter(false);

	// Token: 0x020017DC RID: 6108
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040078F8 RID: 30968
		public Vector3 dropOffset;

		// Token: 0x040078F9 RID: 30969
		public string kAnimName;

		// Token: 0x040078FA RID: 30970
		public string animName;

		// Token: 0x040078FB RID: 30971
		public Grid.SceneLayer animLayer = Grid.SceneLayer.Move;

		// Token: 0x040078FC RID: 30972
		public bool notifyOnJettison;
	}

	// Token: 0x020017DD RID: 6109
	public class StatesInstance : GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.GameInstance
	{
		// Token: 0x06009CCB RID: 40139 RVA: 0x0039B1D0 File Offset: 0x003993D0
		public StatesInstance(IStateMachineTarget master, CargoDropperMinion.Def def) : base(master, def)
		{
		}

		// Token: 0x06009CCC RID: 40140 RVA: 0x0039B1DC File Offset: 0x003993DC
		public void JettisonCargo(object data = null)
		{
			Vector3 pos = base.master.transform.GetPosition() + base.def.dropOffset;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
				{
					MinionStorage.Info info = storedMinionInfo[i];
					GameObject gameObject = component.DeserializeMinion(info.id, pos);
					this.escapingMinion = gameObject.GetComponent<MinionIdentity>();
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						this.exitAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, base.def.kAnimName, new HashedString[]
						{
							base.def.animName
						}, KAnim.PlayMode.Once, false);
						Vector3 position = gameObject.transform.GetPosition();
						position.z = Grid.GetLayerZ(base.def.animLayer);
						gameObject.transform.SetPosition(position);
						gameObject.GetMyWorld().SetDupeVisited();
					}
					if (base.def.notifyOnJettison)
					{
						gameObject.GetComponent<Notifier>().Add(this.CreateCrashLandedNotification(), "");
					}
				}
			}
		}

		// Token: 0x06009CCD RID: 40141 RVA: 0x0039B338 File Offset: 0x00399538
		public bool SyncMinionExitAnimation()
		{
			if (this.escapingMinion != null && this.exitAnimChore != null && !this.exitAnimChore.isComplete)
			{
				KBatchedAnimController component = this.escapingMinion.GetComponent<KBatchedAnimController>();
				KBatchedAnimController component2 = base.master.GetComponent<KBatchedAnimController>();
				if (component2.CurrentAnim.name == base.def.animName)
				{
					component.SetElapsedTime(component2.GetElapsedTime());
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009CCE RID: 40142 RVA: 0x0039B3AC File Offset: 0x003995AC
		public Notification CreateCrashLandedNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x040078FD RID: 30973
		public MinionIdentity escapingMinion;

		// Token: 0x040078FE RID: 30974
		public Chore exitAnimChore;
	}
}
