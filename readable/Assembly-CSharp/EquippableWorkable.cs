using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000935 RID: 2357
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
	// Token: 0x060041E3 RID: 16867 RVA: 0x00173F08 File Offset: 0x00172108
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_equip_clothing_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x00173F55 File Offset: 0x00172155
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x00173F5D File Offset: 0x0017215D
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x00173F66 File Offset: 0x00172166
	protected override void OnSpawn()
	{
		base.SetWorkTime(1.5f);
		this.equippable.OnAssign += this.RefreshChore;
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x00173F8C File Offset: 0x0017218C
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = new EquipChore(this);
		Chore chore = this.chore;
		chore.onExit = (Action<Chore>)Delegate.Combine(chore.onExit, new Action<Chore>(this.OnChoreExit));
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x00173FDF File Offset: 0x001721DF
	private void OnChoreExit(Chore chore)
	{
		if (!chore.isComplete)
		{
			this.RefreshChore(this.currentTarget);
		}
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x00173FF5 File Offset: 0x001721F5
	public void CancelChore(string reason = "")
	{
		if (this.chore != null)
		{
			this.chore.Cancel(reason);
			Prioritizable.RemoveRef(this.equippable.gameObject);
			this.chore = null;
		}
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x00174022 File Offset: 0x00172222
	private void RefreshChore(IAssignableIdentity target)
	{
		if (this.chore != null)
		{
			this.CancelChore("Equipment Reassigned");
		}
		this.currentTarget = target;
		if (target != null && !target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
		{
			this.CreateChore();
		}
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x00174060 File Offset: 0x00172260
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.equippable.assignee != null)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner)
			{
				soleOwner.GetComponent<Equipment>().Equip(this.equippable);
				Prioritizable.RemoveRef(this.equippable.gameObject);
				this.chore = null;
			}
		}
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x001740BB File Offset: 0x001722BB
	protected override void OnStopWork(WorkerBase worker)
	{
		this.workTimeRemaining = this.GetWorkTime();
		base.OnStopWork(worker);
	}

	// Token: 0x04002925 RID: 10533
	[MyCmpReq]
	private Equippable equippable;

	// Token: 0x04002926 RID: 10534
	private Chore chore;

	// Token: 0x04002927 RID: 10535
	private IAssignableIdentity currentTarget;

	// Token: 0x04002928 RID: 10536
	private global::QualityLevel quality;
}
