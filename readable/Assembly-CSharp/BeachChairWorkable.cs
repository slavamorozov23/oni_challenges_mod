using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020006E7 RID: 1767
[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06002B99 RID: 11161 RVA: 0x000FE5CC File Offset: 0x000FC7CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_beach_chair_kanim")
		};
		this.workAnims = null;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.lightEfficiencyBonus = false;
		base.SetWorkTime(150f);
		this.beachChair = base.GetComponent<BeachChair>();
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000FE64D File Offset: 0x000FC84D
	protected override void OnStartWork(WorkerBase worker)
	{
		this.timeLit = 0f;
		this.beachChair.SetWorker(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000FE688 File Offset: 0x000FC888
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		int i = Grid.PosToCell(base.gameObject);
		bool flag = (float)Grid.LightIntensity[i] >= (float)BeachChairConfig.TAN_LUX - 1f;
		this.beachChair.SetLit(flag);
		if (flag)
		{
			this.loopingSound.SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
			this.timeLit += dt;
		}
		else
		{
			this.loopingSound.SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0f);
		}
		return false;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000FE718 File Offset: 0x000FC918
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.timeLit / this.workTime >= 0.75f)
		{
			component.Add(this.beachChair.specificEffectLit, true);
			component.Remove(this.beachChair.specificEffectUnlit);
		}
		else
		{
			component.Add(this.beachChair.specificEffectUnlit, true);
			component.Remove(this.beachChair.specificEffectLit);
		}
		component.Add(this.beachChair.trackingEffect, true);
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000FE79D File Offset: 0x000FC99D
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000FE7BC File Offset: 0x000FC9BC
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.beachChair.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040019F9 RID: 6649
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040019FA RID: 6650
	[MyCmpReq]
	private LoopingSounds loopingSound;

	// Token: 0x040019FB RID: 6651
	private float timeLit;

	// Token: 0x040019FC RID: 6652
	public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp", false);

	// Token: 0x040019FD RID: 6653
	public HashedString BEACH_CHAIR_LIT_PARAMETER = "beachChair_lit";

	// Token: 0x040019FE RID: 6654
	public int basePriority;

	// Token: 0x040019FF RID: 6655
	private BeachChair beachChair;
}
