using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class Dinofern : StateMachineComponent<Dinofern.StatesInstance>
{
	// Token: 0x06004ED3 RID: 20179 RVA: 0x001CA350 File Offset: 0x001C8550
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001CA368 File Offset: 0x001C8568
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001CA37B File Offset: 0x001C857B
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.09f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.0225f;
	}

	// Token: 0x040034A0 RID: 13472
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034A1 RID: 13473
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x040034A2 RID: 13474
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x040034A3 RID: 13475
	private Growing growing;

	// Token: 0x02001BBC RID: 7100
	public class StatesInstance : GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.GameInstance
	{
		// Token: 0x0600AAE3 RID: 43747 RVA: 0x003C61EA File Offset: 0x003C43EA
		public StatesInstance(Dinofern master) : base(master)
		{
			master.growing = base.GetComponent<Growing>();
		}
	}

	// Token: 0x02001BBD RID: 7101
	public class States : GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern>
	{
		// Token: 0x0600AAE4 RID: 43748 RVA: 0x003C6200 File Offset: 0x003C4400
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(Dinofern.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (Dinofern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (Dinofern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (Dinofern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(Dinofern.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing);
			this.alive.growing.Transition(this.alive.mature, (Dinofern.StatesInstance smi) => smi.master.growing.IsGrown(), UpdateRate.SIM_200ms).EventTransition(GameHashes.Wilt, this.alive.wilting, (Dinofern.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(Dinofern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(Dinofern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			});
			this.alive.mature.Transition(this.alive.growing, (Dinofern.StatesInstance smi) => !smi.master.growing.IsGrown(), UpdateRate.SIM_200ms).EventTransition(GameHashes.Wilt, this.alive.wilting, (Dinofern.StatesInstance smi) => smi.master.wiltCondition.IsWilting());
			this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (Dinofern.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04008599 RID: 34201
		public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State grow;

		// Token: 0x0400859A RID: 34202
		public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State blocked_from_growing;

		// Token: 0x0400859B RID: 34203
		public Dinofern.States.AliveStates alive;

		// Token: 0x0400859C RID: 34204
		public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State dead;

		// Token: 0x020029F6 RID: 10742
		public class AliveStates : GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.PlantAliveSubState
		{
			// Token: 0x0400B984 RID: 47492
			public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State growing;

			// Token: 0x0400B985 RID: 47493
			public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State mature;

			// Token: 0x0400B986 RID: 47494
			public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State wilting;
		}
	}
}
