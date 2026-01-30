using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000920 RID: 2336
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x0600415C RID: 16732 RVA: 0x001714B8 File Offset: 0x0016F6B8
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x0600415D RID: 16733 RVA: 0x001714C0 File Offset: 0x0016F6C0
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x0600415E RID: 16734 RVA: 0x001714C8 File Offset: 0x0016F6C8
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null;
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x0600415F RID: 16735 RVA: 0x001714F0 File Offset: 0x0016F6F0
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

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06004160 RID: 16736 RVA: 0x0017152C File Offset: 0x0016F72C
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

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06004161 RID: 16737 RVA: 0x0017157C File Offset: 0x0016F77C
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

	// Token: 0x06004162 RID: 16738 RVA: 0x001715B8 File Offset: 0x0016F7B8
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06004163 RID: 16739 RVA: 0x001715C1 File Offset: 0x0016F7C1
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06004164 RID: 16740 RVA: 0x001715C9 File Offset: 0x0016F7C9
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06004165 RID: 16741 RVA: 0x001715E9 File Offset: 0x0016F7E9
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06004166 RID: 16742 RVA: 0x00171612 File Offset: 0x0016F812
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06004167 RID: 16743 RVA: 0x0017161A File Offset: 0x0016F81A
	// (set) Token: 0x06004168 RID: 16744 RVA: 0x0017162F File Offset: 0x0016F82F
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

	// Token: 0x06004169 RID: 16745 RVA: 0x00171644 File Offset: 0x0016F844
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

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x0600416A RID: 16746 RVA: 0x0017167C File Offset: 0x0016F87C
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x0600416B RID: 16747 RVA: 0x001716AC File Offset: 0x0016F8AC
	private int GetInputCell(ConduitType inputConduitType)
	{
		return this.occupyArea.GetOffsetCellWithRotation(this.offset);
	}

	// Token: 0x0600416C RID: 16748 RVA: 0x001716C0 File Offset: 0x0016F8C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType, Endpoint.Sink, this.utilityCell, base.gameObject);
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		EntityCellVisualizer.Ports type = EntityCellVisualizer.Ports.LiquidIn;
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			type = EntityCellVisualizer.Ports.SolidIn;
		}
		else if (conduitManager.conduitType == ConduitType.Gas)
		{
			type = EntityCellVisualizer.Ports.GasIn;
		}
		this.cellVisualizer.AddPort(type, this.offset);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x0600416D RID: 16749 RVA: 0x001717E4 File Offset: 0x0016F9E4
	protected override void OnCleanUp()
	{
		if (this.endpoint.ConduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(this.endpoint.ConduitType).RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600416E RID: 16750 RVA: 0x00171876 File Offset: 0x0016FA76
	private void OnConduitConnectionChanged(object _)
	{
		base.Trigger(-2094018600, BoxedBools.Box(this.IsConnected));
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x0017188E File Offset: 0x0016FA8E
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06004170 RID: 16752 RVA: 0x00171898 File Offset: 0x0016FA98
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x06004171 RID: 16753 RVA: 0x001718C4 File Offset: 0x0016FAC4
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
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
		if (flag || this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
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
			if (this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x040028D1 RID: 10449
	private FlowUtilityNetwork.NetworkItem endpoint;

	// Token: 0x040028D2 RID: 10450
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x040028D3 RID: 10451
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x040028D4 RID: 10452
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040028D5 RID: 10453
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040028D6 RID: 10454
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x040028D7 RID: 10455
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040028D8 RID: 10456
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x040028D9 RID: 10457
	[SerializeField]
	public bool isOn = true;

	// Token: 0x040028DA RID: 10458
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x040028DB RID: 10459
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x040028DC RID: 10460
	[MyCmpReq]
	public Operational operational;

	// Token: 0x040028DD RID: 10461
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040028DE RID: 10462
	[MyCmpReq]
	private EntityCellVisualizer cellVisualizer;

	// Token: 0x040028DF RID: 10463
	public Operational.State OperatingRequirement;

	// Token: 0x040028E0 RID: 10464
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040028E1 RID: 10465
	public CellOffset offset;

	// Token: 0x040028E2 RID: 10466
	private int utilityCell = -1;

	// Token: 0x040028E3 RID: 10467
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x040028E4 RID: 10468
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x040028E5 RID: 10469
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040028E6 RID: 10470
	private bool satisfied;

	// Token: 0x040028E7 RID: 10471
	public EntityConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x02001920 RID: 6432
	public enum WrongElementResult
	{
		// Token: 0x04007CFF RID: 31999
		Destroy,
		// Token: 0x04007D00 RID: 32000
		Dump,
		// Token: 0x04007D01 RID: 32001
		Store
	}
}
