using System;
using TUNING;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationLaunchWorkable")]
public class RocketControlStationLaunchWorkable : Workable
{
	// Token: 0x06003683 RID: 13955 RVA: 0x00133968 File Offset: 0x00131B68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_rocket_control_station_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.attributeConverter = Db.Get().AttributeConverters.PilotingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Rocketry.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		base.SetWorkTime(30f);
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x00133A00 File Offset: 0x00131C00
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
			smi.LaunchRocket();
		}
	}

	// Token: 0x04002119 RID: 8473
	[MyCmpReq]
	private Operational operational;
}
