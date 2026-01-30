using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B5B RID: 2907
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060055E2 RID: 21986 RVA: 0x001F4BED File Offset: 0x001F2DED
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x060055E3 RID: 21987 RVA: 0x001F4BF5 File Offset: 0x001F2DF5
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x060055E4 RID: 21988 RVA: 0x001F4BF8 File Offset: 0x001F2DF8
	public SolidConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitFlow().GetContents(this.utilityCell);
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060055E5 RID: 21989 RVA: 0x001F4C0B File Offset: 0x001F2E0B
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x060055E6 RID: 21990 RVA: 0x001F4C13 File Offset: 0x001F2E13
	public SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060055E7 RID: 21991 RVA: 0x001F4C20 File Offset: 0x001F2E20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetOutputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x001F4C9B File Offset: 0x001F2E9B
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060055E9 RID: 21993 RVA: 0x001F4CCA File Offset: 0x001F2ECA
	private void OnConduitConnectionChanged(object data)
	{
		this.dispensing = (this.dispensing && this.IsConnected);
		base.Trigger(-2094018600, BoxedBools.Box(this.IsConnected));
	}

	// Token: 0x060055EA RID: 21994 RVA: 0x001F4CFC File Offset: 0x001F2EFC
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			SolidConduitFlow conduitFlow = this.GetConduitFlow();
			if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
			{
				Pickupable pickupable = this.FindSuitableItem();
				if (pickupable)
				{
					if (pickupable.PrimaryElement.Mass > 20f)
					{
						pickupable = pickupable.Take(Mathf.Max(20f, pickupable.PrimaryElement.MassPerUnit));
					}
					conduitFlow.AddPickupable(this.utilityCell, pickupable);
					flag = true;
				}
			}
		}
		this.storage.storageNetworkID = this.GetConnectedNetworkID();
		this.dispensing = flag;
	}

	// Token: 0x060055EB RID: 21995 RVA: 0x001F4DC0 File Offset: 0x001F2FC0
	private bool isSolid(GameObject o)
	{
		PrimaryElement component = o.GetComponent<PrimaryElement>();
		return !(component == null) && (component.Element.IsSolid || (double)component.MassPerUnit != 1.0);
	}

	// Token: 0x060055EC RID: 21996 RVA: 0x001F4E04 File Offset: 0x001F3004
	private Pickupable FindSuitableItem()
	{
		List<GameObject> items = this.storage.items;
		if (items.Count < 1)
		{
			return null;
		}
		this.round_robin_index %= items.Count;
		GameObject gameObject = items[this.round_robin_index];
		this.round_robin_index++;
		if (this.solidOnly && !this.isSolid(gameObject))
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < items.Count)
			{
				gameObject = items[(this.round_robin_index + num) % items.Count];
				if (this.isSolid(gameObject))
				{
					flag = true;
				}
				num++;
			}
			if (!flag)
			{
				return null;
			}
		}
		if (!gameObject)
		{
			return null;
		}
		return gameObject.GetComponent<Pickupable>();
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060055ED RID: 21997 RVA: 0x001F4EB4 File Offset: 0x001F30B4
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060055EE RID: 21998 RVA: 0x001F4EEC File Offset: 0x001F30EC
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

	// Token: 0x060055EF RID: 21999 RVA: 0x001F4F40 File Offset: 0x001F3140
	private int GetOutputCell()
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			foreach (ISecondaryOutput secondaryOutput in base.GetComponents<ISecondaryOutput>())
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x040039FC RID: 14844
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x040039FD RID: 14845
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x040039FE RID: 14846
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x040039FF RID: 14847
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04003A00 RID: 14848
	[SerializeField]
	public bool solidOnly;

	// Token: 0x04003A01 RID: 14849
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04003A02 RID: 14850
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003A03 RID: 14851
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04003A04 RID: 14852
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003A05 RID: 14853
	private int utilityCell = -1;

	// Token: 0x04003A06 RID: 14854
	private bool dispensing;

	// Token: 0x04003A07 RID: 14855
	private int round_robin_index;
}
