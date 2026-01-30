using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
[AddComponentMenu("KMonoBehaviour/Workable/Disinfectable")]
public class Disinfectable : Workable
{
	// Token: 0x06002268 RID: 8808 RVA: 0x000C81FC File Offset: 0x000C63FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		base.Subscribe<Disinfectable>(2127324410, Disinfectable.OnCancelDelegate);
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000C82B3 File Offset: 0x000C64B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDisinfect)
		{
			this.MarkForDisinfect(true);
		}
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x000C82DC File Offset: 0x000C64DC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x000C82FD File Offset: 0x000C64FD
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000C8330 File Offset: 0x000C6530
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.isMarkedForDisinfect = false;
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000C83B2 File Offset: 0x000C65B2
	private void ToggleMarkForDisinfect()
	{
		if (this.isMarkedForDisinfect)
		{
			this.CancelDisinfection();
			return;
		}
		base.SetWorkTime(10f);
		this.MarkForDisinfect(false);
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000C83D8 File Offset: 0x000C65D8
	private void CancelDisinfection()
	{
		if (this.isMarkedForDisinfect)
		{
			Prioritizable.RemoveRef(base.gameObject);
			base.ShowProgressBar(false);
			this.isMarkedForDisinfect = false;
			this.chore.Cancel("disinfection cancelled");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x000C8440 File Offset: 0x000C6640
	public void MarkForDisinfect(bool force = false)
	{
		if (!this.isMarkedForDisinfect || force)
		{
			this.isMarkedForDisinfect = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Disinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000C84B4 File Offset: 0x000C66B4
	private void OnCancel(object _)
	{
		this.CancelDisinfection();
	}

	// Token: 0x04001413 RID: 5139
	private Chore chore;

	// Token: 0x04001414 RID: 5140
	[Serialize]
	private bool isMarkedForDisinfect;

	// Token: 0x04001415 RID: 5141
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x04001416 RID: 5142
	private float diseasePerSecond;

	// Token: 0x04001417 RID: 5143
	private static readonly EventSystem.IntraObjectHandler<Disinfectable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Disinfectable>(delegate(Disinfectable component, object data)
	{
		component.OnCancel(data);
	});
}
