using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200073D RID: 1853
[AddComponentMenu("KMonoBehaviour/Workable/DesalinatorWorkableEmpty")]
public class DesalinatorWorkableEmpty : Workable
{
	// Token: 0x06002EAF RID: 11951 RVA: 0x0010D9F0 File Offset: 0x0010BBF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x0010DAAD File Offset: 0x0010BCAD
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001BAC RID: 7084
	[Serialize]
	public int timesCleaned;

	// Token: 0x04001BAD RID: 7085
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04001BAE RID: 7086
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
