using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200096D RID: 2413
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x0600448D RID: 17549 RVA: 0x0018B9C7 File Offset: 0x00189BC7
	public int PowerDistributionOrder
	{
		get
		{
			return this.powerDistributionOrder;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x0600448E RID: 17550 RVA: 0x0018B9CF File Offset: 0x00189BCF
	public virtual float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x0600448F RID: 17551 RVA: 0x0018B9D7 File Offset: 0x00189BD7
	public virtual bool IsEmpty
	{
		get
		{
			return this.joulesAvailable <= 0f;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06004490 RID: 17552 RVA: 0x0018B9E9 File Offset: 0x00189BE9
	public virtual float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06004491 RID: 17553 RVA: 0x0018B9F1 File Offset: 0x00189BF1
	public float WattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating * this.Efficiency;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06004492 RID: 17554 RVA: 0x0018BA0A File Offset: 0x00189C0A
	public float BaseWattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06004493 RID: 17555 RVA: 0x0018BA1C File Offset: 0x00189C1C
	public float PercentFull
	{
		get
		{
			if (this.Capacity == 0f)
			{
				return 1f;
			}
			return this.joulesAvailable / this.Capacity;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06004494 RID: 17556 RVA: 0x0018BA3E File Offset: 0x00189C3E
	// (set) Token: 0x06004495 RID: 17557 RVA: 0x0018BA46 File Offset: 0x00189C46
	public int PowerCell { get; private set; }

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06004496 RID: 17558 RVA: 0x0018BA4F File Offset: 0x00189C4F
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06004497 RID: 17559 RVA: 0x0018BA61 File Offset: 0x00189C61
	private float Efficiency
	{
		get
		{
			return Mathf.Max(1f + this.generatorOutputAttribute.GetTotalValue() / 100f, 0f);
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06004498 RID: 17560 RVA: 0x0018BA84 File Offset: 0x00189C84
	// (set) Token: 0x06004499 RID: 17561 RVA: 0x0018BA8C File Offset: 0x00189C8C
	public bool IsVirtual { get; protected set; }

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x0600449A RID: 17562 RVA: 0x0018BA95 File Offset: 0x00189C95
	// (set) Token: 0x0600449B RID: 17563 RVA: 0x0018BA9D File Offset: 0x00189C9D
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x0600449C RID: 17564 RVA: 0x0018BAA8 File Offset: 0x00189CA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.generatorOutputAttribute = attributes.Add(Db.Get().Attributes.GeneratorOutput);
	}

	// Token: 0x0600449D RID: 17565 RVA: 0x0018BAE4 File Offset: 0x00189CE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Generators.Add(this);
		this.cachedPrefabId = base.gameObject.PrefabID();
		base.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.capacity = Generator.CalculateCapacity(this.building.Def, null);
		this.PowerCell = this.building.GetPowerOutputCell();
		this.CheckConnectionStatus();
		Game.Instance.energySim.AddGenerator(this);
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x0018BB69 File Offset: 0x00189D69
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x0018BB9A File Offset: 0x00189D9A
	public virtual bool IsProducingPower()
	{
		return this.operational.IsActive;
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x0018BBA7 File Offset: 0x00189DA7
	public virtual void EnergySim200ms(float dt)
	{
		this.CheckConnectionStatus();
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x0018BBB0 File Offset: 0x00189DB0
	private void SetStatusItem(StatusItem status_item)
	{
		if (status_item != this.currentStatusItem && this.currentStatusItem != null)
		{
			this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
		}
		if (status_item != null && this.statusItemID == Guid.Empty)
		{
			this.statusItemID = this.selectable.AddStatusItem(status_item, this);
		}
		this.currentStatusItem = status_item;
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x0018BC18 File Offset: 0x00189E18
	private void CheckConnectionStatus()
	{
		if (this.CircuitID == 65535)
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, false);
			return;
		}
		if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, true);
			return;
		}
		this.SetStatusItem(null);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x0018BCD6 File Offset: 0x00189ED6
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveGenerator(this);
		Game.Instance.circuitManager.Disconnect(this);
		Components.Generators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060044A4 RID: 17572 RVA: 0x0018BD09 File Offset: 0x00189F09
	public static float CalculateCapacity(BuildingDef def, Element element)
	{
		if (element == null)
		{
			return def.GeneratorBaseCapacity;
		}
		return def.GeneratorBaseCapacity * (1f + (element.HasTag(GameTags.RefinedMetal) ? 1f : 0f));
	}

	// Token: 0x060044A5 RID: 17573 RVA: 0x0018BD3B File Offset: 0x00189F3B
	public void ResetJoules()
	{
		this.joulesAvailable = 0f;
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x0018BD48 File Offset: 0x00189F48
	public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
	{
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0f, canOverPower ? float.MaxValue : this.Capacity);
	}

	// Token: 0x060044A7 RID: 17575 RVA: 0x0018BD74 File Offset: 0x00189F74
	public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
	{
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, joulesAvailable, this.selectable.GetProperName(), null);
		float num = this.joulesAvailable + joulesAvailable;
		this.joulesAvailable = Mathf.Clamp(num, 0f, canOverPower ? float.MaxValue : this.Capacity);
		if (num > joulesAvailable)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, this.joulesAvailable - num, StringFormatter.Replace(BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", base.gameObject.GetProperName()), null);
		}
		if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.cachedPrefabId))
		{
			Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.cachedPrefabId, 0f);
		}
		Dictionary<Tag, float> powerCreatedbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType;
		Tag key = this.cachedPrefabId;
		powerCreatedbyGeneratorType[key] += this.joulesAvailable;
	}

	// Token: 0x060044A8 RID: 17576 RVA: 0x0018BE63 File Offset: 0x0018A063
	public void AssignJoulesAvailable(float joulesAvailable)
	{
		this.joulesAvailable = joulesAvailable;
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x0018BE6C File Offset: 0x0018A06C
	public virtual void ConsumeEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x04002E07 RID: 11783
	protected const int SimUpdateSortKey = 1001;

	// Token: 0x04002E08 RID: 11784
	[MyCmpReq]
	protected Building building;

	// Token: 0x04002E09 RID: 11785
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002E0A RID: 11786
	[MyCmpReq]
	protected KSelectable selectable;

	// Token: 0x04002E0B RID: 11787
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04002E0C RID: 11788
	[SerializeField]
	public int powerDistributionOrder;

	// Token: 0x04002E0D RID: 11789
	private Tag cachedPrefabId;

	// Token: 0x04002E0E RID: 11790
	public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04002E0F RID: 11791
	protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04002E10 RID: 11792
	private float capacity;

	// Token: 0x04002E14 RID: 11796
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04002E15 RID: 11797
	[SerializeField]
	public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04002E16 RID: 11798
	public bool showConnectedConsumerStatusItems = true;

	// Token: 0x04002E17 RID: 11799
	private StatusItem currentStatusItem;

	// Token: 0x04002E18 RID: 11800
	private Guid statusItemID;

	// Token: 0x04002E19 RID: 11801
	private AttributeInstance generatorOutputAttribute;

	// Token: 0x04002E1A RID: 11802
	private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>(delegate(Generator component, object data)
	{
		component.OnTagsChanged(data);
	});
}
