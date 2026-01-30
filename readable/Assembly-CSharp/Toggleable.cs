using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064D RID: 1613
[AddComponentMenu("KMonoBehaviour/Workable/Toggleable")]
public class Toggleable : Workable
{
	// Token: 0x06002742 RID: 10050 RVA: 0x000E19A7 File Offset: 0x000DFBA7
	protected Toggleable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x000E19BC File Offset: 0x000DFBBC
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.targets = new List<KeyValuePair<IToggleHandler, Chore>>();
		base.SetWorkTime(3f);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Toggling;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000E1A26 File Offset: 0x000DFC26
	public int SetTarget(IToggleHandler handler)
	{
		this.targets.Add(new KeyValuePair<IToggleHandler, Chore>(handler, null));
		return this.targets.Count - 1;
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000E1A48 File Offset: 0x000DFC48
	public IToggleHandler GetToggleHandlerForWorker(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1)
		{
			return this.targets[targetForWorker].Key;
		}
		return null;
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x000E1A78 File Offset: 0x000DFC78
	private int GetTargetForWorker(WorkerBase worker)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Value != null && this.targets[i].Value.driver != null && this.targets[i].Value.driver.gameObject == worker.gameObject)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x000E1B00 File Offset: 0x000DFD00
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1 && this.targets[targetForWorker].Key != null)
		{
			this.targets[targetForWorker] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetForWorker].Key, null);
			this.targets[targetForWorker].Key.HandleToggle();
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x000E1B8C File Offset: 0x000DFD8C
	private void QueueToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value == null)
		{
			if (DebugHandler.InstantBuildMode)
			{
				this.targets[targetIdx].Key.HandleToggle();
				return;
			}
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, new WorkChore<Toggleable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true));
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x000E1C3C File Offset: 0x000DFE3C
	public void Toggle(int targetIdx)
	{
		if (targetIdx >= this.targets.Count)
		{
			return;
		}
		if (this.targets[targetIdx].Value == null)
		{
			this.QueueToggle(targetIdx);
			return;
		}
		this.CancelToggle(targetIdx);
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x000E1C80 File Offset: 0x000DFE80
	private void CancelToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value != null)
		{
			this.targets[targetIdx].Value.Cancel("Toggle cancelled");
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x000E1D04 File Offset: 0x000DFF04
	public bool IsToggleQueued(int targetIdx)
	{
		return this.targets[targetIdx].Value != null;
	}

	// Token: 0x04001736 RID: 5942
	private List<KeyValuePair<IToggleHandler, Chore>> targets;
}
