using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public class PeeChore : Chore<PeeChore.StatesInstance>
{
	// Token: 0x0600197F RID: 6527 RVA: 0x0008E808 File Offset: 0x0008CA08
	public PeeChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Pee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PeeChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020012FF RID: 4863
	public class StatesInstance : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.GameInstance
	{
		// Token: 0x06008A59 RID: 35417 RVA: 0x00358944 File Offset: 0x00356B44
		public StatesInstance(PeeChore master, GameObject worker) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(worker);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(worker);
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x003589E9 File Offset: 0x00356BE9
		public bool IsDonePeeing()
		{
			return this.bladder.value <= 0f;
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x00358A00 File Offset: 0x00356C00
		public void SpawnDirtyWater(float dt)
		{
			int gameCell = Grid.PosToCell(base.sm.worker.Get<KMonoBehaviour>(base.smi));
			byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE);
			float num = dt * -this.bladder.GetDelta() / this.bladder.GetMax();
			if (num > 0f)
			{
				float mass = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_FLOOR_PEE * num;
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(SimHashes.DirtyWater, mass, this.bodyTemperature.value, index, Mathf.CeilToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE * num), false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(gameCell, SimHashes.DirtyWater, CellEventLogger.Instance.Vomit, mass, this.bodyTemperature.value, index, Mathf.CeilToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE * num), true, -1);
			}
		}

		// Token: 0x040069DF RID: 27103
		public Notification stressfullyEmptyingBladder = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x040069E0 RID: 27104
		public AmountInstance bladder;

		// Token: 0x040069E1 RID: 27105
		private AmountInstance bodyTemperature;
	}

	// Token: 0x02001300 RID: 4864
	public class States : GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore>
	{
		// Token: 0x06008A5C RID: 35420 RVA: 0x00358B10 File Offset: 0x00356D10
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.running;
			base.Target(this.worker);
			this.running.ToggleAnims("anim_expel_kanim", 0f).ToggleEffect("StressfulyEmptyingBladder").DoNotification((PeeChore.StatesInstance smi) => smi.stressfullyEmptyingBladder).DoReport(ReportManager.ReportType.ToiletIncident, (PeeChore.StatesInstance smi) => 1f, (PeeChore.StatesInstance smi) => this.masterTarget.Get(smi).GetProperName()).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Transition(null, (PeeChore.StatesInstance smi) => smi.IsDonePeeing(), UpdateRate.SIM_200ms).Update("SpawnDirtyWater", delegate(PeeChore.StatesInstance smi, float dt)
			{
				smi.SpawnDirtyWater(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop).ToggleTag(GameTags.MakingMess).Enter(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(PeeChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled())
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
					AmountInstance amountInstance = smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id);
					RadiationMonitor.Instance smi2 = smi.master.gameObject.GetSMI<RadiationMonitor.Instance>();
					if (smi2 != null)
					{
						float num = Math.Min(amountInstance.value, 100f * smi2.difficultySettingMod);
						smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
						if (num >= 1f)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
						}
					}
				}
			});
		}

		// Token: 0x040069E2 RID: 27106
		public StateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.TargetParameter worker;

		// Token: 0x040069E3 RID: 27107
		public GameStateMachine<PeeChore.States, PeeChore.StatesInstance, PeeChore, object>.State running;
	}
}
