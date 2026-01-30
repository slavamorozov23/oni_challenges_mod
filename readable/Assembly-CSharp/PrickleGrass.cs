using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA8 RID: 2728
public class PrickleGrass : StateMachineComponent<PrickleGrass.StatesInstance>
{
	// Token: 0x06004F13 RID: 20243 RVA: 0x001CB2B1 File Offset: 0x001C94B1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PrickleGrass>(1309017699, PrickleGrass.SetReplantedTrueDelegate);
	}

	// Token: 0x06004F14 RID: 20244 RVA: 0x001CB2CA File Offset: 0x001C94CA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004F15 RID: 20245 RVA: 0x001CB2DD File Offset: 0x001C94DD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x040034D3 RID: 13523
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034D4 RID: 13524
	[MyCmpReq]
	private EntombVulnerable entombVulnerable;

	// Token: 0x040034D5 RID: 13525
	public bool replanted;

	// Token: 0x040034D6 RID: 13526
	public EffectorValues positive_decor_effect = new EffectorValues
	{
		amount = 1,
		radius = 5
	};

	// Token: 0x040034D7 RID: 13527
	public EffectorValues negative_decor_effect = new EffectorValues
	{
		amount = -1,
		radius = 5
	};

	// Token: 0x040034D8 RID: 13528
	private static readonly EventSystem.IntraObjectHandler<PrickleGrass> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<PrickleGrass>(delegate(PrickleGrass component, object data)
	{
		component.replanted = true;
	});

	// Token: 0x02001BD6 RID: 7126
	public class StatesInstance : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.GameInstance
	{
		// Token: 0x0600AB5A RID: 43866 RVA: 0x003C8417 File Offset: 0x003C6617
		public StatesInstance(PrickleGrass smi) : base(smi)
		{
		}
	}

	// Token: 0x02001BD7 RID: 7127
	public class States : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass>
	{
		// Token: 0x0600AB5B RID: 43867 RVA: 0x003C8420 File Offset: 0x003C6620
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.AreaElementSafeChanged, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				if (smi.master.replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State state2 = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
			string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
			string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (PrickleGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
			});
			this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
			});
		}

		// Token: 0x040085D2 RID: 34258
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State grow;

		// Token: 0x040085D3 RID: 34259
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State blocked_from_growing;

		// Token: 0x040085D4 RID: 34260
		public PrickleGrass.States.AliveStates alive;

		// Token: 0x040085D5 RID: 34261
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State dead;

		// Token: 0x02002A04 RID: 10756
		public class AliveStates : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.PlantAliveSubState
		{
			// Token: 0x0400B9BF RID: 47551
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State idle;

			// Token: 0x0400B9C0 RID: 47552
			public PrickleGrass.States.WiltingState wilting;
		}

		// Token: 0x02002A05 RID: 10757
		public class WiltingState : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State
		{
			// Token: 0x0400B9C1 RID: 47553
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pre;

			// Token: 0x0400B9C2 RID: 47554
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting;

			// Token: 0x0400B9C3 RID: 47555
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pst;
		}
	}
}
