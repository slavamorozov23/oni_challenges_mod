using System;
using UnityEngine;

// Token: 0x02000B5A RID: 2906
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x060055D6 RID: 21974 RVA: 0x001F4893 File Offset: 0x001F2A93
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x060055D7 RID: 21975 RVA: 0x001F489B File Offset: 0x001F2A9B
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x060055D8 RID: 21976 RVA: 0x001F489E File Offset: 0x001F2A9E
	public bool IsConsuming
	{
		get
		{
			return this.consuming;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060055D9 RID: 21977 RVA: 0x001F48A8 File Offset: 0x001F2AA8
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x001F48DF File Offset: 0x001F2ADF
	private SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x001F48EC File Offset: 0x001F2AEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x001F4966 File Offset: 0x001F2B66
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x001F4995 File Offset: 0x001F2B95
	private void OnConduitConnectionChanged(object data)
	{
		this.consuming = (this.consuming && this.IsConnected);
		base.Trigger(-2094018600, BoxedBools.Box(this.IsConnected));
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x001F49C4 File Offset: 0x001F2BC4
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		SolidConduitFlow conduitFlow = this.GetConduitFlow();
		if (this.IsConnected)
		{
			SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
			if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
			{
				float num = (this.capacityTag != GameTags.Any) ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
				float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
				float num3 = Mathf.Max(0f, num2 - num);
				if (num3 > 0f)
				{
					Pickupable pickupable = conduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable.PrimaryElement.Mass <= num3 || pickupable.PrimaryElement.Mass > num2)
					{
						Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
						if (pickupable2)
						{
							this.storage.Store(pickupable2.gameObject, true, false, true, false);
							flag = true;
						}
					}
				}
			}
		}
		if (this.storage != null)
		{
			this.storage.storageNetworkID = this.GetConnectedNetworkID();
		}
		this.consuming = flag;
	}

	// Token: 0x060055DF RID: 21983 RVA: 0x001F4B04 File Offset: 0x001F2D04
	private int GetConnectedNetworkID()
	{
		GameObject gameObject = Grid.Objects[this.utilityCell, 20];
		SolidConduit solidConduit = (gameObject != null) ? gameObject.GetComponent<SolidConduit>() : null;
		UtilityNetwork utilityNetwork = (solidConduit != null) ? solidConduit.GetNetwork() : null;
		if (utilityNetwork == null)
		{
			return -1;
		}
		return utilityNetwork.id;
	}

	// Token: 0x060055E0 RID: 21984 RVA: 0x001F4B58 File Offset: 0x001F2D58
	private int GetInputCell()
	{
		if (this.useSecondaryInput)
		{
			foreach (ISecondaryInput secondaryInput in base.GetComponents<ISecondaryInput>())
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x040039F2 RID: 14834
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040039F3 RID: 14835
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040039F4 RID: 14836
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040039F5 RID: 14837
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x040039F6 RID: 14838
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040039F7 RID: 14839
	[MyCmpReq]
	private Building building;

	// Token: 0x040039F8 RID: 14840
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040039F9 RID: 14841
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040039FA RID: 14842
	private int utilityCell = -1;

	// Token: 0x040039FB RID: 14843
	private bool consuming;
}
