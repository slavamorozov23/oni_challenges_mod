using System;
using TUNING;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class DataRainerChore : Chore<DataRainerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001910 RID: 6416 RVA: 0x0008BD24 File Offset: 0x00089F24
	public DataRainerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DataRainerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x0008BDBE File Offset: 0x00089FBE
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000EAA RID: 3754
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x020012CA RID: 4810
	public class States : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore>
	{
		// Token: 0x060089A4 RID: 35236 RVA: 0x003539AC File Offset: 0x00351BAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.dataRainer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (DataRainerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((DataRainerChore.StatesInstance smi) => smi.GetTargetCell(), this.raining, this.idle, false);
			this.raining.ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.raining.loop).Update(delegate(DataRainerChore.StatesInstance smi, float dt)
			{
				this.nextBankTimer.Delta(dt, smi);
				if (this.nextBankTimer.Get(smi) >= DataRainer.databankSpawnInterval)
				{
					this.nextBankTimer.Delta(-DataRainer.databankSpawnInterval, smi);
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("PowerStationTools"), smi.master.transform.position + Vector3.up);
					gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Iron, true);
					gameObject.SetActive(true);
					KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
					float num = (float)component.currentFrame / (float)component.GetCurrentNumFrames();
					Vector2 initial_velocity = new Vector2((num < 0.5f) ? -2.5f : 2.5f, 4f);
					if (GameComps.Fallers.Has(gameObject))
					{
						GameComps.Fallers.Remove(gameObject);
					}
					GameComps.Fallers.Add(gameObject, initial_velocity);
					DataRainer.Instance smi2 = this.dataRainer.Get(smi).GetSMI<DataRainer.Instance>();
					DataRainer sm = smi2.sm;
					sm.databanksCreated.Set(sm.databanksCreated.Get(smi2) + 1, smi2, false);
				}
			}, UpdateRate.SIM_33ms, false);
			this.raining.loop.PlayAnim("makeitrain2", KAnim.PlayMode.Loop);
		}

		// Token: 0x04006922 RID: 26914
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.TargetParameter dataRainer;

		// Token: 0x04006923 RID: 26915
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter nextBankTimer = new StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter(DataRainer.databankSpawnInterval / 2f);

		// Token: 0x04006924 RID: 26916
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State idle;

		// Token: 0x04006925 RID: 26917
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State goToStand;

		// Token: 0x04006926 RID: 26918
		public DataRainerChore.States.RainingStates raining;

		// Token: 0x020027A5 RID: 10149
		public class RainingStates : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State
		{
			// Token: 0x0400AFCD RID: 45005
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pre;

			// Token: 0x0400AFCE RID: 45006
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State loop;

			// Token: 0x0400AFCF RID: 45007
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pst;
		}
	}

	// Token: 0x020012CB RID: 4811
	public class StatesInstance : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.GameInstance
	{
		// Token: 0x060089A7 RID: 35239 RVA: 0x00353BD4 File Offset: 0x00351DD4
		public StatesInstance(DataRainerChore master, GameObject dataRainer) : base(master)
		{
			this.dataRainer = dataRainer;
			base.sm.dataRainer.Set(dataRainer, base.smi, false);
		}

		// Token: 0x060089A8 RID: 35240 RVA: 0x00353BFD File Offset: 0x00351DFD
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060089A9 RID: 35241 RVA: 0x00353C20 File Offset: 0x00351E20
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x04006927 RID: 26919
		private GameObject dataRainer;
	}
}
