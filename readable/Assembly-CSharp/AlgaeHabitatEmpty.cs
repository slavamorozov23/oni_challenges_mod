using System;
using TUNING;
using UnityEngine;

// Token: 0x02000707 RID: 1799
[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
	// Token: 0x06002C93 RID: 11411 RVA: 0x0010370C File Offset: 0x0010190C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x04001A83 RID: 6787
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"sponge_pre",
		"sponge_loop"
	};

	// Token: 0x04001A84 RID: 6788
	private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");
}
