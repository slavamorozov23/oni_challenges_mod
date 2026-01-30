using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class GravitasBathroomStall : GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>
{
	// Token: 0x06003082 RID: 12418 RVA: 0x00117FAC File Offset: 0x001161AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.start;
		this.root.DefaultState(this.start);
		this.start.PlayAnim("idle").Update(delegate(GravitasBathroomStall.Instance smi, float dt)
		{
			if (HijackedHeadquarters.Instance.PrinterceptorInstance != null && HijackedHeadquarters.IsOperational(HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>()))
			{
				smi.sm.hasBeenActivated.Set(smi.master.GetComponent<Activatable>().IsActivated, smi, false);
				smi.GoTo(this.branch);
			}
		}, UpdateRate.SIM_200ms, false);
		this.branch.ParamTransition<bool>(this.hasBeenActivated, this.blinking, GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.IsFalse).ParamTransition<bool>(this.hasBeenActivated, this.activated, GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.IsTrue);
		this.blinking.PlayAnim("code_ready", KAnim.PlayMode.Loop).EventHandlerTransition(GameHashes.BuildingActivated, this.activated, (GravitasBathroomStall.Instance smi, object data) => ((Boxed<bool>)data).value).Enter(delegate(GravitasBathroomStall.Instance smi)
		{
			smi.SubscribeToPrinterceptorOperational();
		}).Exit(delegate(GravitasBathroomStall.Instance smi)
		{
			smi.UnsubscribeFromPrinterceptorOperational();
		});
		this.activated.Enter(delegate(GravitasBathroomStall.Instance smi)
		{
			if (!smi.sm.hasShownPopup.Get(smi))
			{
				smi.ShowLoreUnlockedPopup();
			}
			else
			{
				smi.GoTo(this.complete);
			}
			smi.sm.hasBeenActivated.Set(true, smi, false);
		}).PlayAnim("activated");
		this.complete.PlayAnim("idle");
	}

	// Token: 0x04001CE1 RID: 7393
	public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State start;

	// Token: 0x04001CE2 RID: 7394
	public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State branch;

	// Token: 0x04001CE3 RID: 7395
	public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State blinking;

	// Token: 0x04001CE4 RID: 7396
	public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State activated;

	// Token: 0x04001CE5 RID: 7397
	public GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.State complete;

	// Token: 0x04001CE6 RID: 7398
	public StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.BoolParameter hasBeenActivated;

	// Token: 0x04001CE7 RID: 7399
	public StateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.BoolParameter hasShownPopup;

	// Token: 0x02001676 RID: 5750
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001677 RID: 5751
	public new class Instance : GameStateMachine<GravitasBathroomStall, GravitasBathroomStall.Instance, IStateMachineTarget, GravitasBathroomStall.Def>.GameInstance
	{
		// Token: 0x06009740 RID: 38720 RVA: 0x00383B1F File Offset: 0x00381D1F
		public Instance(IStateMachineTarget master, GravitasBathroomStall.Def def) : base(master, def)
		{
		}

		// Token: 0x06009741 RID: 38721 RVA: 0x00383B38 File Offset: 0x00381D38
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<Activatable>().activationCondition = (() => HijackedHeadquarters.Instance.PrinterceptorInstance != null && HijackedHeadquarters.IsOperational(HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>()));
			this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.HijackedHeadquarters.HashId);
			this.onBuildingSelectHandle = base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
		}

		// Token: 0x06009742 RID: 38722 RVA: 0x00383BB6 File Offset: 0x00381DB6
		public override void StopSM(string reason)
		{
			if (this.onBuildingSelectHandle != -1)
			{
				base.Unsubscribe(ref this.onBuildingSelectHandle);
			}
			base.StopSM(reason);
		}

		// Token: 0x06009743 RID: 38723 RVA: 0x00383BD4 File Offset: 0x00381DD4
		private void OnBuildingSelect(object obj)
		{
			if (!((Boxed<bool>)obj).value)
			{
				return;
			}
			if (this.completeNotification != null)
			{
				this.completeNotification.customClickCallback(this.completeNotification.customClickData);
			}
		}

		// Token: 0x06009744 RID: 38724 RVA: 0x00383C08 File Offset: 0x00381E08
		public void ShowLoreUnlockedPopup()
		{
			EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.NAME, CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.UNLOCK_POPUP.BUTTON, "printerceptorcoderevealed_kanim", EventInfoDataHelper.PopupType.NORMAL, null, null, delegate
			{
				base.smi.sm.hasShownPopup.Set(true, base.smi, false);
				base.smi.master.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(GravitasBathroomStall.Instance.Sequence(base.smi));
				base.smi.GoTo(base.smi.sm.complete);
			});
			this.completeNotification = EventInfoScreen.CreateNotification(eventInfoData, null);
			base.gameObject.AddOrGet<Notifier>().Add(this.completeNotification, "");
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
		}

		// Token: 0x06009745 RID: 38725 RVA: 0x00383C9B File Offset: 0x00381E9B
		private static IEnumerator Sequence(GravitasBathroomStall.Instance smi)
		{
			StoryManager.Instance.GetStoryInstance(Db.Get().Stories.HijackedHeadquarters.HashId);
			smi.ClearEndNotification();
			if (!HijackedHeadquarters.Instance.PrinterceptorInstance.IsNullOrDestroyed())
			{
				smi.RevealPrinterceptor();
				CameraController.Instance.FadeOut(1f, 1f, null);
				yield return SequenceUtil.WaitForSecondsRealtime(1f);
				Vector3 b = new Vector3(2f, 3f, 0f);
				GameUtil.FocusCamera(HijackedHeadquarters.Instance.PrinterceptorInstance.transform.position + b, 10f, false, true);
				yield return SequenceUtil.WaitForSecondsRealtime(1f);
				if (SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Unpause(false);
				}
				CameraController.Instance.FadeIn(0f, 1f, null);
				yield return SequenceUtil.WaitForSecondsRealtime(1f);
				HijackedHeadquarters.Instance.PrinterceptorInstance.GetSMI<HijackedHeadquarters.Instance>().UnlockPrinterceptor();
				yield break;
			}
			yield break;
		}

		// Token: 0x06009746 RID: 38726 RVA: 0x00383CAC File Offset: 0x00381EAC
		private void RevealPrinterceptor()
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("HijackedHeadquarters", worldContainer.id, false));
			}
			foreach (WorldGenSpawner.Spawnable spawnable in list)
			{
				int baseX;
				int baseY;
				Grid.CellToXY(spawnable.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, 10, 10f);
			}
		}

		// Token: 0x06009747 RID: 38727 RVA: 0x00383D7C File Offset: 0x00381F7C
		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			if (this.completeNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.completeNotification);
			}
			this.completeNotification = null;
		}

		// Token: 0x06009748 RID: 38728 RVA: 0x00383DCF File Offset: 0x00381FCF
		public void SubscribeToPrinterceptorOperational()
		{
			this.UnsubscribeFromPrinterceptorOperational();
			if (HijackedHeadquarters.Instance.PrinterceptorInstance != null)
			{
				this.printerceptorOperationalEventHandle = HijackedHeadquarters.Instance.PrinterceptorInstance.Subscribe(-592767678, delegate(object data)
				{
					base.smi.master.GetComponent<Activatable>().CancelChore();
					base.smi.GoTo(base.smi.sm.start);
				});
			}
		}

		// Token: 0x06009749 RID: 38729 RVA: 0x00383E05 File Offset: 0x00382005
		public void UnsubscribeFromPrinterceptorOperational()
		{
			if (this.printerceptorOperationalEventHandle != -1 && HijackedHeadquarters.Instance.PrinterceptorInstance != null)
			{
				HijackedHeadquarters.Instance.PrinterceptorInstance.Unsubscribe(this.printerceptorOperationalEventHandle);
			}
			this.printerceptorOperationalEventHandle = -1;
		}

		// Token: 0x040074EA RID: 29930
		private StoryInstance storyInstance;

		// Token: 0x040074EB RID: 29931
		private Notification completeNotification;

		// Token: 0x040074EC RID: 29932
		private int onBuildingSelectHandle = -1;

		// Token: 0x040074ED RID: 29933
		private int printerceptorOperationalEventHandle = -1;
	}
}
