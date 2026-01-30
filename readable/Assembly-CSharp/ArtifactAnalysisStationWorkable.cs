using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200070A RID: 1802
public class ArtifactAnalysisStationWorkable : Workable
{
	// Token: 0x06002C9F RID: 11423 RVA: 0x00103A50 File Offset: 0x00101C50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyArtifact.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingArtifact;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_artifact_analysis_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
		Components.ArtifactAnalysisStations.Add(this);
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x00103B19 File Offset: 0x00101D19
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.animController.SetSymbolVisiblity("snapTo_artifact", false);
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x00103B43 File Offset: 0x00101D43
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ArtifactAnalysisStations.Remove(this);
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x00103B56 File Offset: 0x00101D56
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.InitialDisplayStoredArtifact();
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x00103B65 File Offset: 0x00101D65
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		this.PositionArtifact();
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x00103B78 File Offset: 0x00101D78
	private void InitialDisplayStoredArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		gameObject.transform.SetPosition(new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(true);
		component.enabled = false;
		component.enabled = true;
		this.PositionArtifact();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x00103C24 File Offset: 0x00101E24
	private void ReleaseStoredArtifact()
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = component.GetItems()[0];
		KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Ore)));
		component2.enabled = false;
		component2.enabled = true;
		component.Drop(gameObject, true);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x00103CB8 File Offset: 0x00101EB8
	private void PositionArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform("snapTo_artifact", out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
		gameObject.transform.SetPosition(position);
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x00103D16 File Offset: 0x00101F16
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.ConsumeCharm();
		this.ReleaseStoredArtifact();
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x00103D2C File Offset: 0x00101F2C
	private void ConsumeCharm()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.CharmedArtifact);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"ArtifactAnalysisStation finished studying a charmed artifact but there is not one in its storage"
		});
		if (gameObject != null)
		{
			this.YieldPayload(gameObject.GetComponent<SpaceArtifact>());
			gameObject.GetComponent<SpaceArtifact>().RemoveCharm();
		}
		if (ArtifactSelector.Instance.RecordArtifactAnalyzed(gameObject.GetComponent<KPrefabID>().PrefabID().ToString()))
		{
			if (gameObject.HasTag(GameTags.TerrestrialArtifact))
			{
				ArtifactSelector.Instance.IncrementAnalyzedTerrestrialArtifacts();
				return;
			}
			ArtifactSelector.Instance.IncrementAnalyzedSpaceArtifacts();
		}
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x00103DCC File Offset: 0x00101FCC
	private void YieldPayload(SpaceArtifact artifact)
	{
		if (this.nextYeildRoll == -1f)
		{
			this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
		}
		if (this.nextYeildRoll <= artifact.GetArtifactTier().payloadDropChance)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("GeneShufflerRecharge"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		int num = Mathf.FloorToInt(artifact.GetArtifactTier().payloadDropChance * 20f);
		for (int i = 0; i < num; i++)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x04001A88 RID: 6792
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001A89 RID: 6793
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001A8A RID: 6794
	[SerializeField]
	public Vector3 finishedArtifactDropOffset;

	// Token: 0x04001A8B RID: 6795
	private Notification notification;

	// Token: 0x04001A8C RID: 6796
	public ArtifactAnalysisStation.StatesInstance statesInstance;

	// Token: 0x04001A8D RID: 6797
	private KBatchedAnimController animController;

	// Token: 0x04001A8E RID: 6798
	[Serialize]
	private float nextYeildRoll = -1f;
}
