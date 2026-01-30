using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200086F RID: 2159
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06003B3C RID: 15164 RVA: 0x0014B396 File Offset: 0x00149596
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06003B3D RID: 15165 RVA: 0x0014B39E File Offset: 0x0014959E
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06003B3E RID: 15166 RVA: 0x0014B3A6 File Offset: 0x001495A6
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null && this.m_buildingComplete != null;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06003B3F RID: 15167 RVA: 0x0014B3E0 File Offset: 0x001495E0
	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				result = (this.GetConduitManager().GetContents(this.utilityCell).mass > 0f);
			}
			return result;
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06003B40 RID: 15168 RVA: 0x0014B41C File Offset: 0x0014961C
	public float stored_mass
	{
		get
		{
			if (this.storage == null)
			{
				return 0f;
			}
			if (!(this.capacityTag != GameTags.Any))
			{
				return this.storage.MassStored();
			}
			return this.storage.GetMassAvailable(this.capacityTag);
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06003B41 RID: 15169 RVA: 0x0014B46C File Offset: 0x0014966C
	public float space_remaining_kg
	{
		get
		{
			float num = this.capacityKG - this.stored_mass;
			if (!(this.storage == null))
			{
				return Mathf.Min(this.storage.RemainingCapacity(), num);
			}
			return num;
		}
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x0014B4A8 File Offset: 0x001496A8
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06003B43 RID: 15171 RVA: 0x0014B4B1 File Offset: 0x001496B1
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06003B44 RID: 15172 RVA: 0x0014B4B9 File Offset: 0x001496B9
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06003B45 RID: 15173 RVA: 0x0014B4D9 File Offset: 0x001496D9
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06003B46 RID: 15174 RVA: 0x0014B502 File Offset: 0x00149702
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06003B47 RID: 15175 RVA: 0x0014B50A File Offset: 0x0014970A
	// (set) Token: 0x06003B48 RID: 15176 RVA: 0x0014B51F File Offset: 0x0014971F
	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x0014B534 File Offset: 0x00149734
	private ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06003B4A RID: 15178 RVA: 0x0014B56C File Offset: 0x0014976C
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0014B59C File Offset: 0x0014979C
	protected virtual int GetInputCell(ConduitType inputConduitType)
	{
		if (this.useSecondaryInput)
		{
			ISecondaryInput[] components = base.GetComponents<ISecondaryInput>();
			foreach (ISecondaryInput secondaryInput in components)
			{
				if (secondaryInput.HasSecondaryConduitType(inputConduitType))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(inputConduitType));
				}
			}
			global::Debug.LogWarning("No secondaryInput of type was found");
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(inputConduitType));
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x0014B61C File Offset: 0x0014981C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x0014B6E6 File Offset: 0x001498E6
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x0014B715 File Offset: 0x00149915
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, BoxedBools.Box(this.IsConnected));
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x0014B72D File Offset: 0x0014992D
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0014B738 File Offset: 0x00149938
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x0014B764 File Offset: 0x00149964
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		if (this.building.Def.CanMove)
		{
			this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		}
		if (!this.IsConnected)
		{
			return;
		}
		ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		this.IsSatisfied = true;
		if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
		{
			return;
		}
		float num = this.ConsumptionRate * dt;
		num = Mathf.Min(num, this.space_remaining_kg);
		Element element = ElementLoader.FindElementByHash(contents.element);
		if (contents.element != this.lastConsumedElement)
		{
			DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);
		}
		float num2 = 0f;
		if (num > 0f)
		{
			ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, num);
			num2 = conduitContents.mass;
			this.lastConsumedElement = conduitContents.element;
		}
		bool flag = element.HasTag(this.capacityTag);
		if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
		{
			base.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
			});
		}
		if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
		{
			if (num2 > 0f)
			{
				this.consumedLastTick = true;
				int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				Element element2 = ElementLoader.FindElementByHash(contents.element);
				ConduitType conduitType = this.conduitType;
				if (conduitType != ConduitType.Gas)
				{
					if (conduitType == ConduitType.Liquid)
					{
						if (element2.IsLiquid)
						{
							this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
							return;
						}
						global::Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString());
						return;
					}
				}
				else
				{
					if (element2.IsGas)
					{
						this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
						return;
					}
					global::Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString());
					return;
				}
			}
		}
		else if (num2 > 0f)
		{
			this.consumedLastTick = true;
			if (this.wrongElementResult == ConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x0400249E RID: 9374
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x0400249F RID: 9375
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x040024A0 RID: 9376
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040024A1 RID: 9377
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040024A2 RID: 9378
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x040024A3 RID: 9379
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040024A4 RID: 9380
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x040024A5 RID: 9381
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x040024A6 RID: 9382
	[SerializeField]
	public bool isOn = true;

	// Token: 0x040024A7 RID: 9383
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x040024A8 RID: 9384
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x040024A9 RID: 9385
	[MyCmpReq]
	public Operational operational;

	// Token: 0x040024AA RID: 9386
	[MyCmpReq]
	protected Building building;

	// Token: 0x040024AB RID: 9387
	public Operational.State OperatingRequirement;

	// Token: 0x040024AC RID: 9388
	public ISecondaryInput targetSecondaryInput;

	// Token: 0x040024AD RID: 9389
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040024AE RID: 9390
	[MyCmpGet]
	private BuildingComplete m_buildingComplete;

	// Token: 0x040024AF RID: 9391
	private int utilityCell = -1;

	// Token: 0x040024B0 RID: 9392
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x040024B1 RID: 9393
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x040024B2 RID: 9394
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040024B3 RID: 9395
	private bool satisfied;

	// Token: 0x040024B4 RID: 9396
	public ConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x02001828 RID: 6184
	public enum WrongElementResult
	{
		// Token: 0x040079F5 RID: 31221
		Destroy,
		// Token: 0x040079F6 RID: 31222
		Dump,
		// Token: 0x040079F7 RID: 31223
		Store
	}
}
