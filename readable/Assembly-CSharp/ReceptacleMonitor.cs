using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008B6 RID: 2230
[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause
{
	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0015736F File Offset: 0x0015556F
	public bool Replanted
	{
		get
		{
			return this.replanted;
		}
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x00157377 File Offset: 0x00155577
	private static bool HasReceptacleOperationalComponent(ReceptacleMonitor.StatesInstance smi)
	{
		return smi.ReceptacleObject != null && smi.ReceptacleObject.GetComponent<Operational>() != null;
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x0015739A File Offset: 0x0015559A
	private static bool IsReceptacleOperational(ReceptacleMonitor.StatesInstance smi)
	{
		return ReceptacleMonitor.HasReceptacleOperationalComponent(smi) && smi.ReceptacleObject.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x001573B6 File Offset: 0x001555B6
	private static bool IsReceptacleOperational(ReceptacleMonitor.StatesInstance smi, object obj)
	{
		return ReceptacleMonitor.IsReceptacleOperational(smi);
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x001573BE File Offset: 0x001555BE
	private static bool IsReceptacle_NOT_Operational(ReceptacleMonitor.StatesInstance smi, object obj)
	{
		return !ReceptacleMonitor.IsReceptacleOperational(smi);
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x001573C9 File Offset: 0x001555C9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x001573DC File Offset: 0x001555DC
	public PlantablePlot GetReceptacle()
	{
		return (PlantablePlot)base.smi.sm.receptacle.Get(base.smi);
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x00157400 File Offset: 0x00155600
	public void SetReceptacle(PlantablePlot plot = null)
	{
		if (plot == null)
		{
			base.smi.sm.receptacle.Set(null, base.smi, false);
			this.replanted = false;
		}
		else
		{
			base.smi.sm.receptacle.Set(plot, base.smi, false);
			this.replanted = true;
		}
		base.Trigger(-1636776682, null);
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06003D7F RID: 15743 RVA: 0x0015746E File Offset: 0x0015566E
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Receptacle
			};
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06003D80 RID: 15744 RVA: 0x0015747C File Offset: 0x0015567C
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.domestic.operationalExist.inoperational))
			{
				text += CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
			}
			return text;
		}
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x001574C8 File Offset: 0x001556C8
	public bool HasReceptacle()
	{
		return !base.smi.IsInsideState(base.smi.sm.wild);
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x001574E8 File Offset: 0x001556E8
	public bool HasOperationalReceptacle()
	{
		return base.smi.IsInsideState(base.smi.sm.domestic.operationalExist.operational);
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x0015750F File Offset: 0x0015570F
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040025F9 RID: 9721
	private bool replanted;

	// Token: 0x020018BE RID: 6334
	public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x0600A034 RID: 41012 RVA: 0x003A939E File Offset: 0x003A759E
		public SingleEntityReceptacle ReceptacleObject
		{
			get
			{
				return base.sm.receptacle.Get(this);
			}
		}

		// Token: 0x0600A035 RID: 41013 RVA: 0x003A93B1 File Offset: 0x003A75B1
		public StatesInstance(ReceptacleMonitor master) : base(master)
		{
		}
	}

	// Token: 0x020018BF RID: 6335
	public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
	{
		// Token: 0x0600A036 RID: 41014 RVA: 0x003A93BC File Offset: 0x003A75BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.wild;
			base.serializable = StateMachine.SerializeType.Never;
			this.wild.ParamTransition<SingleEntityReceptacle>(this.receptacle, this.domestic, (ReceptacleMonitor.StatesInstance smi, SingleEntityReceptacle p) => p != null);
			this.domestic.ParamTransition<SingleEntityReceptacle>(this.receptacle, this.wild, (ReceptacleMonitor.StatesInstance smi, SingleEntityReceptacle p) => p == null).EnterTransition(this.domestic.operationalExist, new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.HasReceptacleOperationalComponent)).EnterTransition(this.domestic.simple, GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Not(new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.HasReceptacleOperationalComponent)));
			this.domestic.simple.DoNothing();
			this.domestic.operationalExist.EnterTransition(this.domestic.operationalExist.operational, new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.IsReceptacleOperational)).EnterGoTo(this.domestic.operationalExist.inoperational);
			this.domestic.operationalExist.inoperational.EventHandlerTransition(GameHashes.ReceptacleOperational, this.domestic.operationalExist.operational, new Func<ReceptacleMonitor.StatesInstance, object, bool>(ReceptacleMonitor.IsReceptacleOperational));
			this.domestic.operationalExist.operational.EventHandlerTransition(GameHashes.ReceptacleInoperational, this.domestic.operationalExist.inoperational, new Func<ReceptacleMonitor.StatesInstance, object, bool>(ReceptacleMonitor.IsReceptacle_NOT_Operational));
		}

		// Token: 0x04007BE7 RID: 31719
		public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;

		// Token: 0x04007BE8 RID: 31720
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;

		// Token: 0x04007BE9 RID: 31721
		public ReceptacleMonitor.States.DomesticState domestic;

		// Token: 0x02002994 RID: 10644
		public class DomesticState : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State
		{
			// Token: 0x0400B7DA RID: 47066
			public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State simple;

			// Token: 0x0400B7DB RID: 47067
			public ReceptacleMonitor.States.OperationalState operationalExist;
		}

		// Token: 0x02002995 RID: 10645
		public class OperationalState : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State
		{
			// Token: 0x0400B7DC RID: 47068
			public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;

			// Token: 0x0400B7DD RID: 47069
			public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
		}
	}
}
