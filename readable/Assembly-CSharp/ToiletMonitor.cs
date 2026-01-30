using System;

// Token: 0x02000A54 RID: 2644
public class ToiletMonitor : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance>
{
	// Token: 0x06004CF1 RID: 19697 RVA: 0x001BFC20 File Offset: 0x001BDE20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventHandler(GameHashes.ToiletSensorChanged, delegate(ToiletMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).Exit("ClearStatusItem", delegate(ToiletMonitor.Instance smi)
		{
			smi.ClearStatusItem();
		});
	}

	// Token: 0x04003344 RID: 13124
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003345 RID: 13125
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State unsatisfied;

	// Token: 0x02001B60 RID: 7008
	public new class Instance : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9AC RID: 43436 RVA: 0x003C2222 File Offset: 0x003C0422
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.toiletSensor = base.GetComponent<Sensors>().GetSensor<ToiletSensor>();
		}

		// Token: 0x0600A9AD RID: 43437 RVA: 0x003C223C File Offset: 0x003C043C
		public void RefreshStatusItem()
		{
			StatusItem status_item = null;
			if (!this.toiletSensor.AreThereAnyToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoToilets;
			}
			else if (!this.toiletSensor.AreThereAnyUsableToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoUsableToilets;
			}
			else if (this.toiletSensor.GetNearestUsableToilet() == null)
			{
				status_item = Db.Get().DuplicantStatusItems.ToiletUnreachable;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, status_item, null);
		}

		// Token: 0x0600A9AE RID: 43438 RVA: 0x003C22C3 File Offset: 0x003C04C3
		public void ClearStatusItem()
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, null, null);
		}

		// Token: 0x040084BC RID: 33980
		private ToiletSensor toiletSensor;
	}
}
