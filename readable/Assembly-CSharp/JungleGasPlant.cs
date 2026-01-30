using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
public class JungleGasPlant : StateMachineComponent<JungleGasPlant.StatesInstance>
{
	// Token: 0x06004EDF RID: 20191 RVA: 0x001CA65A File Offset: 0x001C885A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001CA66D File Offset: 0x001C886D
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x040034B2 RID: 13490
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x040034B3 RID: 13491
	[MyCmpReq]
	private Growing growing;

	// Token: 0x040034B4 RID: 13492
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034B5 RID: 13493
	[MyCmpReq]
	private ElementEmitter elementEmitter;

	// Token: 0x02001BC1 RID: 7105
	public class StatesInstance : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.GameInstance
	{
		// Token: 0x0600AAF4 RID: 43764 RVA: 0x003C687B File Offset: 0x003C4A7B
		public StatesInstance(JungleGasPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001BC2 RID: 7106
	public class States : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant>
	{
		// Token: 0x0600AAF5 RID: 43765 RVA: 0x003C6884 File Offset: 0x003C4A84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive.seed_grow);
			});
			GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).TagTransition(GameTags.Entombed, this.alive.seed_grow, true).EventTransition(GameHashes.TooColdWarning, this.alive.seed_grow, null).EventTransition(GameHashes.TooHotWarning, this.alive.seed_grow, null).TagTransition(GameTags.Uprooted, this.dead, false);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.seed_grow.QueueAnim("seed_grow", false, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting());
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.grown, (JungleGasPlant.StatesInstance smi) => smi.master.growing.IsGrown()).PlayAnim("idle_loop", KAnim.PlayMode.Loop);
			this.alive.grown.DefaultState(this.alive.grown.pre).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(true);
			}).Exit(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(false);
			});
			this.alive.grown.pre.PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.grown.idle);
			this.alive.grown.idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop);
			this.alive.wilting.pre.DefaultState(this.alive.wilting.pre).PlayAnim("wilt_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.wilting.idle).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.idle.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.pst.PlayAnim("wilt_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x040085A2 RID: 34210
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State blocked_from_growing;

		// Token: 0x040085A3 RID: 34211
		public JungleGasPlant.States.AliveStates alive;

		// Token: 0x040085A4 RID: 34212
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State dead;

		// Token: 0x020029FB RID: 10747
		public class AliveStates : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400B99A RID: 47514
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State seed_grow;

			// Token: 0x0400B99B RID: 47515
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x0400B99C RID: 47516
			public JungleGasPlant.States.WiltingState wilting;

			// Token: 0x0400B99D RID: 47517
			public JungleGasPlant.States.GrownState grown;

			// Token: 0x0400B99E RID: 47518
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State destroy;
		}

		// Token: 0x020029FC RID: 10748
		public class GrownState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x0400B99F RID: 47519
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x0400B9A0 RID: 47520
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
		}

		// Token: 0x020029FD RID: 10749
		public class WiltingState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x0400B9A1 RID: 47521
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x0400B9A2 RID: 47522
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x0400B9A3 RID: 47523
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pst;
		}
	}
}
