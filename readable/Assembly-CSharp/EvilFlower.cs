using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9D RID: 2717
public class EvilFlower : StateMachineComponent<EvilFlower.StatesInstance>
{
	// Token: 0x06004ED7 RID: 20183 RVA: 0x001CA3B3 File Offset: 0x001C85B3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<EvilFlower>(1309017699, EvilFlower.SetReplantedTrueDelegate);
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001CA3CC File Offset: 0x001C85CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001CA3DF File Offset: 0x001C85DF
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x040034A4 RID: 13476
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034A5 RID: 13477
	[MyCmpReq]
	private EntombVulnerable entombVulnerable;

	// Token: 0x040034A6 RID: 13478
	public bool replanted;

	// Token: 0x040034A7 RID: 13479
	public EffectorValues positive_decor_effect = new EffectorValues
	{
		amount = 1,
		radius = 5
	};

	// Token: 0x040034A8 RID: 13480
	public EffectorValues negative_decor_effect = new EffectorValues
	{
		amount = -1,
		radius = 5
	};

	// Token: 0x040034A9 RID: 13481
	private static readonly EventSystem.IntraObjectHandler<EvilFlower> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<EvilFlower>(delegate(EvilFlower component, object data)
	{
		component.replanted = true;
	});

	// Token: 0x02001BBE RID: 7102
	public class StatesInstance : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.GameInstance
	{
		// Token: 0x0600AAEA RID: 43754 RVA: 0x003C654B File Offset: 0x003C474B
		public StatesInstance(EvilFlower smi) : base(smi)
		{
		}
	}

	// Token: 0x02001BBF RID: 7103
	public class States : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower>
	{
		// Token: 0x0600AAEB RID: 43755 RVA: 0x003C6554 File Offset: 0x003C4754
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TriggerOnEnter(GameHashes.BurstEmitDisease, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(EvilFlower.StatesInstance smi)
			{
				if (smi.master.replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state2 = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
			string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
			string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (EvilFlower.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
			});
			this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
			});
		}

		// Token: 0x0400859D RID: 34205
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State grow;

		// Token: 0x0400859E RID: 34206
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State blocked_from_growing;

		// Token: 0x0400859F RID: 34207
		public EvilFlower.States.AliveStates alive;

		// Token: 0x040085A0 RID: 34208
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead;

		// Token: 0x020029F8 RID: 10744
		public class AliveStates : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.PlantAliveSubState
		{
			// Token: 0x0400B990 RID: 47504
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State idle;

			// Token: 0x0400B991 RID: 47505
			public EvilFlower.States.WiltingState wilting;
		}

		// Token: 0x020029F9 RID: 10745
		public class WiltingState : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State
		{
			// Token: 0x0400B992 RID: 47506
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pre;

			// Token: 0x0400B993 RID: 47507
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting;

			// Token: 0x0400B994 RID: 47508
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pst;
		}
	}
}
