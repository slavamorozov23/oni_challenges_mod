using System;
using TUNING;
using UnityEngine;

// Token: 0x020008FD RID: 2301
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
public class DoctorStationDoctorWorkable : Workable
{
	// Token: 0x06003FEB RID: 16363 RVA: 0x00167D63 File Offset: 0x00165F63
	private DoctorStationDoctorWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x00167D74 File Offset: 0x00165F74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x00167DCC File Offset: 0x00165FCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x00167DD4 File Offset: 0x00165FD4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.station.SetHasDoctor(true);
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x00167DE9 File Offset: 0x00165FE9
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.station.SetHasDoctor(false);
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x00167DFE File Offset: 0x00165FFE
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.station.CompleteDoctoring();
	}

	// Token: 0x04002791 RID: 10129
	[MyCmpReq]
	private DoctorStation station;
}
