using System;
using TUNING;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class ReanimateBionicWorkable : Workable
{
	// Token: 0x0600144F RID: 5199 RVA: 0x00073598 File Offset: 0x00071798
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = new HashedString[]
		{
			"offline_battery_change_pre",
			"offline_battery_change_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"offline_battery_change_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"offline_battery_change_failed"
		};
		base.SetWorkTime(30f);
		this.readyForSkillWorkStatusItem = Db.Get().DuplicantStatusItems.BionicRequiresSkillPerk;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.InstallingElectrobank);
		this.workingStatusItem = Db.Get().DuplicantStatusItems.BionicBeingRebooted;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bionic_kanim")
		};
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.resetProgressOnStop = false;
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x000736A8 File Offset: 0x000718A8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Vector3 position = worker.transform.GetPosition();
		position.x = base.transform.GetPosition().x;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		worker.transform.SetPosition(position);
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000736FC File Offset: 0x000718FC
	protected override void OnStopWork(WorkerBase worker)
	{
		Vector3 position = worker.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		worker.transform.SetPosition(position);
		base.OnStopWork(worker);
	}
}
