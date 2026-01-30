using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000960 RID: 2400
[SerializationConfig(MemberSerialization.OptIn)]
public class GameFlowManager : StateMachineComponent<GameFlowManager.StatesInstance>, ISaveLoadable
{
	// Token: 0x06004365 RID: 17253 RVA: 0x0017DE9F File Offset: 0x0017C09F
	public static void DestroyInstance()
	{
		GameFlowManager.Instance = null;
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x0017DEA7 File Offset: 0x0017C0A7
	protected override void OnPrefabInit()
	{
		GameFlowManager.Instance = this;
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x0017DEAF File Offset: 0x0017C0AF
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x0017DEBC File Offset: 0x0017C0BC
	public bool IsGameOver()
	{
		return base.smi.IsInsideState(base.smi.sm.gameover);
	}

	// Token: 0x04002AAB RID: 10923
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04002AAC RID: 10924
	public static GameFlowManager Instance;

	// Token: 0x02001968 RID: 6504
	public class StatesInstance : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.GameInstance
	{
		// Token: 0x0600A22D RID: 41517 RVA: 0x003ADBF4 File Offset: 0x003ABDF4
		public bool IsIncapacitated(GameObject go)
		{
			return false;
		}

		// Token: 0x0600A22E RID: 41518 RVA: 0x003ADBF8 File Offset: 0x003ABDF8
		public void CheckForGameOver()
		{
			if (!Game.Instance.GameStarted())
			{
				return;
			}
			if (GenericGameSettings.instance.disableGameOver)
			{
				return;
			}
			bool flag = false;
			if (Components.LiveMinionIdentities.Count == 0)
			{
				flag = true;
			}
			else
			{
				flag = true;
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!this.IsIncapacitated(minionIdentity.gameObject))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.GoTo(base.sm.gameover.pending);
			}
		}

		// Token: 0x0600A22F RID: 41519 RVA: 0x003ADCA4 File Offset: 0x003ABEA4
		public StatesInstance(GameFlowManager smi) : base(smi)
		{
		}

		// Token: 0x04007DD3 RID: 32211
		public Notification colonyLostNotification = new Notification(MISC.NOTIFICATIONS.COLONYLOST.NAME, NotificationType.Bad, null, null, false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x02001969 RID: 6505
	public class States : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager>
	{
		// Token: 0x0600A230 RID: 41520 RVA: 0x003ADCDC File Offset: 0x003ABEDC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.loading;
			this.loading.ScheduleGoTo(4f, this.running);
			this.running.Update("CheckForGameOver", delegate(GameFlowManager.StatesInstance smi, float dt)
			{
				smi.CheckForGameOver();
			}, UpdateRate.SIM_200ms, false);
			this.gameover.TriggerOnEnter(GameHashes.GameOver, null).ToggleNotification((GameFlowManager.StatesInstance smi) => smi.colonyLostNotification);
			this.gameover.pending.Enter("Goto(gameover.active)", delegate(GameFlowManager.StatesInstance smi)
			{
				UIScheduler.Instance.Schedule("Goto(gameover.active)", 4f, delegate(object d)
				{
					smi.GoTo(this.gameover.active);
				}, null, null);
			});
			this.gameover.active.Enter(delegate(GameFlowManager.StatesInstance smi)
			{
				if (GenericGameSettings.instance.demoMode)
				{
					DemoTimer.Instance.EndDemo();
					return;
				}
				GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.GameOverScreen, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<KScreen>().Show(true);
			});
		}

		// Token: 0x04007DD4 RID: 32212
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State loading;

		// Token: 0x04007DD5 RID: 32213
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State running;

		// Token: 0x04007DD6 RID: 32214
		public GameFlowManager.States.GameOverState gameover;

		// Token: 0x020029AA RID: 10666
		public class GameOverState : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State
		{
			// Token: 0x0400B82E RID: 47150
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State pending;

			// Token: 0x0400B82F RID: 47151
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State active;
		}
	}
}
