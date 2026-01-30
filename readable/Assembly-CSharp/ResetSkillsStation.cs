using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007EF RID: 2031
[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
	// Token: 0x0600365A RID: 13914 RVA: 0x00132DAC File Offset: 0x00130FAC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00132DBB File Offset: 0x00130FBB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnAssign(this.assignable.assignee);
		this.assignable.OnAssign += this.OnAssign;
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x00132DEB File Offset: 0x00130FEB
	private void OnAssign(IAssignableIdentity obj)
	{
		if (obj != null)
		{
			this.CreateChore();
			return;
		}
		if (this.chore != null)
		{
			this.chore.Cancel("Unassigned");
			this.chore = null;
		}
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x00132E18 File Offset: 0x00131018
	private void CreateChore()
	{
		this.chore = new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x00132E51 File Offset: 0x00131051
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x00132E84 File Offset: 0x00131084
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.assignable.Unassign();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null)
		{
			component.ResetSkillLevels(true);
			component.SetHats(component.CurrentHat, null);
			component.ApplyTargetHat();
			this.notification = new Notification(MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
			worker.GetComponent<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00132F25 File Offset: 0x00131125
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, this);
		this.chore = null;
	}

	// Token: 0x04002101 RID: 8449
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04002102 RID: 8450
	private Notification notification;

	// Token: 0x04002103 RID: 8451
	private Chore chore;
}
