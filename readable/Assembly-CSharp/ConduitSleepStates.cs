using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class ConduitSleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>
{
	// Token: 0x0600041A RID: 1050 RVA: 0x00022390 File Offset: 0x00020590
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.connector.moveToSleepLocation;
		this.root.EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.behaviourcomplete, null).Exit(new StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback(ConduitSleepStates.CleanUp));
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation = this.connector.moveToSleepLocation;
		string name = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		moveToSleepLocation.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo(delegate(ConduitSleepStates.Instance smi)
		{
			ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
			return smi2.sm.targetSleepCell.Get(smi2);
		}, this.drowsy, this.behaviourcomplete, false);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State state = this.drowsy;
		string name2 = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
		}).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (GameClock.Instance.IsNighttime())
			{
				smi.GoTo(this.connector.sleep);
			}
		}).DefaultState(this.drowsy.loop);
		this.drowsy.loop.PlayAnim("drowsy_pre").QueueAnim("drowsy_loop", true, null).EventTransition(GameHashes.Nighttime, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.drowsy.pst, (ConduitSleepStates.Instance smi) => GameClock.Instance.IsNighttime());
		this.drowsy.pst.PlayAnim("drowsy_pst").OnAnimQueueComplete(this.connector.sleep);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State sleep = this.connector.sleep;
		string name3 = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		sleep.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (!smi.staterpillar.IsConnectorBuildingSpawned())
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
			smi.staterpillar.EnableConnector();
			if (smi.staterpillar.IsConnected())
			{
				smi.GoTo(this.connector.sleep.connected);
				return;
			}
			smi.GoTo(this.connector.sleep.noConnection);
		});
		this.connector.sleep.connected.Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(ConduitSleepStates.GetSleepingLayer(smi));
		}).Exit(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures);
		}).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.connectedWake, null).Transition(this.connector.sleep.noConnection, (ConduitSleepStates.Instance smi) => !smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms).PlayAnim("sleep_charging_pre").QueueAnim("sleep_charging_loop", true, null).Update(new Action<ConduitSleepStates.Instance, float>(ConduitSleepStates.UpdateGulpSymbol), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameEvent.Callback(ConduitSleepStates.OnStorageChanged));
		this.connector.sleep.noConnection.PlayAnim("sleep_pre").QueueAnim("sleep_loop", true, null).ToggleStatusItem(new Func<ConduitSleepStates.Instance, StatusItem>(ConduitSleepStates.GetStatusItem), null).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.noConnectionWake, null).Transition(this.connector.sleep.connected, (ConduitSleepStates.Instance smi) => smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms);
		this.connector.connectedWake.QueueAnim("sleep_charging_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.connector.noConnectionWake.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsConduitConnection, false);
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x000227F8 File Offset: 0x000209F8
	private static Grid.SceneLayer GetSleepingLayer(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		Grid.SceneLayer result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
				else
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
			}
			else
			{
				result = Grid.SceneLayer.GasConduitBridges;
			}
		}
		else
		{
			result = Grid.SceneLayer.Gas;
		}
		return result;
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00022834 File Offset: 0x00020A34
	private static StatusItem GetStatusItem(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		StatusItem result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Db.Get().BuildingStatusItems.NoWireConnected;
				}
				else
				{
					result = Db.Get().BuildingStatusItems.Normal;
				}
			}
			else
			{
				result = Db.Get().BuildingStatusItems.NeedLiquidOut;
			}
		}
		else
		{
			result = Db.Get().BuildingStatusItems.NeedGasOut;
		}
		return result;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x000228A4 File Offset: 0x00020AA4
	private static void OnStorageChanged(ConduitSleepStates.Instance smi, object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			smi.amountDeposited += gameObject.GetComponent<PrimaryElement>().Mass;
		}
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x000228D9 File Offset: 0x00020AD9
	private static void UpdateGulpSymbol(ConduitSleepStates.Instance smi, float dt)
	{
		smi.SetGulpSymbolVisibility(smi.amountDeposited > 0f);
		smi.amountDeposited = 0f;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x000228FC File Offset: 0x00020AFC
	private static void CleanUp(ConduitSleepStates.Instance smi)
	{
		ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.sm.targetSleepCell.Set(Grid.InvalidCell, smi2, false);
		}
		smi.staterpillar.DestroyOrphanedConnectorBuilding();
	}

	// Token: 0x04000316 RID: 790
	public ConduitSleepStates.DrowsyStates drowsy;

	// Token: 0x04000317 RID: 791
	public ConduitSleepStates.HasConnectorStates connector;

	// Token: 0x04000318 RID: 792
	public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State behaviourcomplete;

	// Token: 0x020010FE RID: 4350
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040063BA RID: 25530
		public HashedString gulpSymbol = "gulp";
	}

	// Token: 0x020010FF RID: 4351
	public new class Instance : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameInstance
	{
		// Token: 0x06008376 RID: 33654 RVA: 0x003437E4 File Offset: 0x003419E4
		public Instance(Chore<ConduitSleepStates.Instance> chore, ConduitSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsConduitConnection);
		}

		// Token: 0x06008377 RID: 33655 RVA: 0x00343808 File Offset: 0x00341A08
		public void SetGulpSymbolVisibility(bool state)
		{
			string sound = GlobalAssets.GetSound("PlugSlug_Charging_Gulp_LP", false);
			if (this.gulpSymbolVisible != state)
			{
				this.gulpSymbolVisible = state;
				this.animController.SetSymbolVisiblity(base.def.gulpSymbol, state);
				if (state)
				{
					this.loopingSounds.StartSound(sound);
					return;
				}
				this.loopingSounds.StopSound(sound);
			}
		}

		// Token: 0x040063BB RID: 25531
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x040063BC RID: 25532
		[MyCmpReq]
		public Staterpillar staterpillar;

		// Token: 0x040063BD RID: 25533
		[MyCmpAdd]
		private LoopingSounds loopingSounds;

		// Token: 0x040063BE RID: 25534
		public bool gulpSymbolVisible;

		// Token: 0x040063BF RID: 25535
		public float amountDeposited;
	}

	// Token: 0x02001100 RID: 4352
	public class SleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x040063C0 RID: 25536
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connected;

		// Token: 0x040063C1 RID: 25537
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnection;
	}

	// Token: 0x02001101 RID: 4353
	public class DrowsyStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x040063C2 RID: 25538
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State loop;

		// Token: 0x040063C3 RID: 25539
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State pst;
	}

	// Token: 0x02001102 RID: 4354
	public class HasConnectorStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x040063C4 RID: 25540
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation;

		// Token: 0x040063C5 RID: 25541
		public ConduitSleepStates.SleepStates sleep;

		// Token: 0x040063C6 RID: 25542
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnectionWake;

		// Token: 0x040063C7 RID: 25543
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connectedWake;
	}
}
