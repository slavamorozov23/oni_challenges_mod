using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200081A RID: 2074
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x0600384C RID: 14412 RVA: 0x0013B2E7 File Offset: 0x001394E7
	// (set) Token: 0x0600384B RID: 14411 RVA: 0x0013B2DE File Offset: 0x001394DE
	public bool IsCloggedByGunk { get; private set; }

	// Token: 0x0600384D RID: 14413 RVA: 0x0013B2F0 File Offset: 0x001394F0
	public void SetIsCloggedByGunk(bool isIt)
	{
		this.IsCloggedByGunk = isIt;
		this.workAnims = (this.IsCloggedByGunk ? ToiletWorkableClean.CLEAN_GUNK_ANIMS : ToiletWorkableClean.CLEAN_ANIMS);
		this.workingPstComplete = (this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM);
		this.workingPstFailed = (this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x0013B354 File Offset: 0x00139554
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x0013B3D8 File Offset: 0x001395D8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		ToiletWorkableUse component = base.gameObject.GetComponent<ToiletWorkableUse>();
		if (component != null && this.IsCloggedByGunk && base.gameObject.GetComponent<FlushToilet>() == null)
		{
			LiquidSourceManager.Instance.CreateChunk(SimHashes.LiquidGunk, component.lastAmountOfWasteMassRemovedFromDupe, DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL, byte.MaxValue, 0, Grid.CellToPos(Grid.PosToCell(base.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore));
		}
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04002233 RID: 8755
	[Serialize]
	public int timesCleaned;

	// Token: 0x04002234 RID: 8756
	private static readonly HashedString[] CLEAN_GUNK_ANIMS = new HashedString[]
	{
		"degunk_pre",
		"degunk_loop"
	};

	// Token: 0x04002235 RID: 8757
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"unclog_pre",
		"unclog_loop"
	};

	// Token: 0x04002236 RID: 8758
	private static readonly HashedString[] PST_ANIM = new HashedString[]
	{
		new HashedString("unclog_pst")
	};

	// Token: 0x04002237 RID: 8759
	private static readonly HashedString[] PST_GUNK_ANIM = new HashedString[]
	{
		new HashedString("degunk_pst")
	};
}
