using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000811 RID: 2065
public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	// Token: 0x060037E4 RID: 14308 RVA: 0x001394E4 File Offset: 0x001376E4
	public void AddNewBaseMinion(GameObject minion, bool extra_power_banks)
	{
		Ref<MinionIdentity> item = new Ref<MinionIdentity>(minion.GetComponent<MinionIdentity>());
		this.aNewHopeEvents.Add(item);
		if (extra_power_banks)
		{
			this.extraPowerBanksEvents.Add(item);
		}
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x00139518 File Offset: 0x00137718
	public void ScheduleNewBaseEvents()
	{
		this.aNewHopeEvents.RemoveAll((Ref<MinionIdentity> entry) => entry == null || entry.Get() == null);
		this.extraPowerBanksEvents.RemoveAll((Ref<MinionIdentity> entry) => entry == null || entry.Get() == null);
		Effect a_new_hope = Db.Get().effects.Get("AnewHope");
		Action<object> <>9__2;
		for (int i = 0; i < this.aNewHopeEvents.Count; i++)
		{
			GameObject gameObject = this.aNewHopeEvents[i].Get().gameObject;
			GameScheduler instance = GameScheduler.Instance;
			string name = "ANewHope";
			float time = 3f + 0.5f * (float)i;
			Action<object> callback;
			if ((callback = <>9__2) == null)
			{
				callback = (<>9__2 = delegate(object m)
				{
					GameObject gameObject3 = m as GameObject;
					if (gameObject3 == null)
					{
						return;
					}
					this.RemoveFromEvents(this.aNewHopeEvents, gameObject3);
					gameObject3.GetComponent<Effects>().Add(a_new_hope, true);
				});
			}
			instance.Schedule(name, time, callback, gameObject, null);
		}
		Action<object> <>9__3;
		for (int j = 0; j < this.extraPowerBanksEvents.Count; j++)
		{
			GameObject gameObject2 = this.extraPowerBanksEvents[j].Get().gameObject;
			GameScheduler instance2 = GameScheduler.Instance;
			string name2 = "ExtraPowerBanks";
			float time2 = 3f + 4.5f * (float)j;
			Action<object> callback2;
			if ((callback2 = <>9__3) == null)
			{
				callback2 = (<>9__3 = delegate(object m)
				{
					GameObject gameObject3 = m as GameObject;
					if (gameObject3 == null)
					{
						return;
					}
					this.RemoveFromEvents(this.extraPowerBanksEvents, gameObject3);
					GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id).Trigger(1982288670, null);
				});
			}
			instance2.Schedule(name2, time2, callback2, gameObject2, null);
		}
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x00139684 File Offset: 0x00137884
	private void RemoveFromEvents(List<Ref<MinionIdentity>> listToRemove, GameObject go)
	{
		for (int i = listToRemove.Count - 1; i >= 0; i--)
		{
			if (listToRemove[i].Get() != null && listToRemove[i].Get() == go.GetComponent<MinionIdentity>())
			{
				listToRemove.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x001396DC File Offset: 0x001378DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x00139750 File Offset: 0x00137950
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
		this.ScheduleNewBaseEvents();
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x001397D8 File Offset: 0x001379D8
	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x001397EC File Offset: 0x001379EC
	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x001398D5 File Offset: 0x00137AD5
	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00139900 File Offset: 0x00137B00
	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
			if (component.HasTag(GameTags.Minions.Models.Bionic))
			{
				GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, delegate(object data)
				{
					base.Trigger(1982288670, null);
				}, null, null);
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x00139A44 File Offset: 0x00137C44
	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	// Token: 0x040021F8 RID: 8696
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040021F9 RID: 8697
	private MeterController meter;

	// Token: 0x040021FA RID: 8698
	private const float MAX_IMMIGRATION_TIME = 120f;

	// Token: 0x040021FB RID: 8699
	private const int NUM_METER_NOTCHES = 8;

	// Token: 0x040021FC RID: 8700
	private List<MinionStartingStats> minionStats;

	// Token: 0x040021FD RID: 8701
	public float startingSkillPoints;

	// Token: 0x040021FE RID: 8702
	[Serialize]
	private List<Ref<MinionIdentity>> aNewHopeEvents = new List<Ref<MinionIdentity>>();

	// Token: 0x040021FF RID: 8703
	[Serialize]
	private List<Ref<MinionIdentity>> extraPowerBanksEvents = new List<Ref<MinionIdentity>>();

	// Token: 0x04002200 RID: 8704
	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	// Token: 0x020017A4 RID: 6052
	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		// Token: 0x06009BE0 RID: 39904 RVA: 0x00397719 File Offset: 0x00395919
		public StatesInstance(Telepad master) : base(master)
		{
		}

		// Token: 0x06009BE1 RID: 39905 RVA: 0x00397722 File Offset: 0x00395922
		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		// Token: 0x06009BE2 RID: 39906 RVA: 0x00397740 File Offset: 0x00395940
		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06009BE3 RID: 39907 RVA: 0x00397783 File Offset: 0x00395983
		public IEnumerator SpawnExtraPowerBanks()
		{
			int cellTarget = Grid.OffsetCell(Grid.PosToCell(base.gameObject), 1, 2);
			int count = 5;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, MISC.POPFX.EXTRA_POWERBANKS_BIONIC, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
				gameObject.SetActive(true);
				Vector2 initial_velocity = new Vector2((-2.5f + 5f * ((float)i / 5f)) / 2f, 2f);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, initial_velocity);
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield return new WaitForSeconds(0.35f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, ITEMS.LUBRICATIONSTICK.NAME, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
			GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("LubricationStick"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
			gameObject2.SetActive(true);
			Vector2 initial_velocity2 = new Vector2(3.75f, 2.5f);
			if (GameComps.Fallers.Has(gameObject2))
			{
				GameComps.Fallers.Remove(gameObject2);
			}
			GameComps.Fallers.Add(gameObject2, initial_velocity2);
			yield return 0;
			yield break;
		}
	}

	// Token: 0x020017A5 RID: 6053
	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		// Token: 0x06009BE4 RID: 39908 RVA: 0x00397794 File Offset: 0x00395994
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, this.bonusDelivery.pre, null);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.bonusDelivery.pre.PlayAnim("bionic_working_pre").OnAnimQueueComplete(this.bonusDelivery.loop);
			this.bonusDelivery.loop.PlayAnim("bionic_working_loop", KAnim.PlayMode.Loop).ScheduleAction("SpawnBonusDelivery", 1f, delegate(Telepad.StatesInstance smi)
			{
				smi.master.StartCoroutine(smi.SpawnExtraPowerBanks());
			}).ScheduleGoTo(3f, this.bonusDelivery.pst);
			this.bonusDelivery.pst.PlayAnim("bionic_working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04007841 RID: 30785
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		// Token: 0x04007842 RID: 30786
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		// Token: 0x04007843 RID: 30787
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		// Token: 0x04007844 RID: 30788
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		// Token: 0x04007845 RID: 30789
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		// Token: 0x04007846 RID: 30790
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		// Token: 0x04007847 RID: 30791
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		// Token: 0x04007848 RID: 30792
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		// Token: 0x04007849 RID: 30793
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		// Token: 0x0400784A RID: 30794
		public Telepad.States.BonusDeliveryStates bonusDelivery;

		// Token: 0x0400784B RID: 30795
		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};

		// Token: 0x02002953 RID: 10579
		public class BonusDeliveryStates : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State
		{
			// Token: 0x0400B6DD RID: 46813
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pre;

			// Token: 0x0400B6DE RID: 46814
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State loop;

			// Token: 0x0400B6DF RID: 46815
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pst;
		}
	}
}
