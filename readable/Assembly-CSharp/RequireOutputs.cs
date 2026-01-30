using System;
using UnityEngine;

// Token: 0x02000AFA RID: 2810
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour
{
	// Token: 0x060051BA RID: 20922 RVA: 0x001DA578 File Offset: 0x001D8778
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ScenePartitionerLayer scenePartitionerLayer = null;
		Building component = base.GetComponent<Building>();
		this.utilityCell = component.GetUtilityOutputCell();
		this.conduitType = component.Def.OutputConduitType;
		switch (component.Def.OutputConduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		this.UpdateConnectionState(true);
		this.UpdatePipeRoomState(true);
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs", base.gameObject, this.utilityCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConnectionState(false);
			});
		}
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
	}

	// Token: 0x060051BB RID: 20923 RVA: 0x001DA650 File Offset: 0x001D8850
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		IConduitFlow conduitFlow = this.GetConduitFlow();
		if (conduitFlow != null)
		{
			conduitFlow.RemoveConduitUpdater(new Action<float>(this.UpdatePipeState));
		}
		base.OnCleanUp();
	}

	// Token: 0x060051BC RID: 20924 RVA: 0x001DA690 File Offset: 0x001D8890
	private void UpdateConnectionState(bool force_update = false)
	{
		this.connected = this.IsConnected(this.utilityCell);
		if (this.connected != this.previouslyConnected || force_update)
		{
			this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
			this.previouslyConnected = this.connected;
			StatusItem status_item = null;
			switch (this.conduitType)
			{
			case ConduitType.Gas:
				status_item = Db.Get().BuildingStatusItems.NeedGasOut;
				break;
			case ConduitType.Liquid:
				status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
				break;
			case ConduitType.Solid:
				status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
				break;
			}
			this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, this);
		}
	}

	// Token: 0x060051BD RID: 20925 RVA: 0x001DA760 File Offset: 0x001D8960
	private bool OutputPipeIsEmpty()
	{
		if (this.ignoreFullPipe)
		{
			return true;
		}
		bool result = true;
		if (this.connected)
		{
			result = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
		}
		return result;
	}

	// Token: 0x060051BE RID: 20926 RVA: 0x001DA794 File Offset: 0x001D8994
	private void UpdatePipeState(float dt)
	{
		this.UpdatePipeRoomState(false);
	}

	// Token: 0x060051BF RID: 20927 RVA: 0x001DA7A0 File Offset: 0x001D89A0
	private void UpdatePipeRoomState(bool force_update = false)
	{
		bool flag = this.OutputPipeIsEmpty();
		if (flag != this.previouslyHadRoom || force_update)
		{
			this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
			this.previouslyHadRoom = flag;
			StatusItem status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
			if (this.conduitType == ConduitType.Solid)
			{
				status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;
			}
			this.pipeBlockedGuid = this.selectable.ToggleStatusItem(status_item, this.pipeBlockedGuid, !flag, null);
		}
	}

	// Token: 0x060051C0 RID: 20928 RVA: 0x001DA824 File Offset: 0x001D8A24
	private IConduitFlow GetConduitFlow()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		case ConduitType.Solid:
			return Game.Instance.solidConduitFlow;
		default:
			global::Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString());
			return null;
		}
	}

	// Token: 0x060051C1 RID: 20929 RVA: 0x001DA890 File Offset: 0x001D8A90
	private bool IsConnected(int cell)
	{
		return RequireOutputs.IsConnected(cell, this.conduitType);
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001DA8A0 File Offset: 0x001D8AA0
	public static bool IsConnected(int cell, ConduitType conduitType)
	{
		ObjectLayer layer = ObjectLayer.NumLayers;
		switch (conduitType)
		{
		case ConduitType.Gas:
			layer = ObjectLayer.GasConduit;
			break;
		case ConduitType.Liquid:
			layer = ObjectLayer.LiquidConduit;
			break;
		case ConduitType.Solid:
			layer = ObjectLayer.SolidConduit;
			break;
		}
		GameObject gameObject = Grid.Objects[cell, (int)layer];
		return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
	}

	// Token: 0x04003753 RID: 14163
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003754 RID: 14164
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003755 RID: 14165
	public bool ignoreFullPipe;

	// Token: 0x04003756 RID: 14166
	private int utilityCell;

	// Token: 0x04003757 RID: 14167
	private ConduitType conduitType;

	// Token: 0x04003758 RID: 14168
	private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

	// Token: 0x04003759 RID: 14169
	private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

	// Token: 0x0400375A RID: 14170
	private bool previouslyConnected = true;

	// Token: 0x0400375B RID: 14171
	private bool previouslyHadRoom = true;

	// Token: 0x0400375C RID: 14172
	private bool connected;

	// Token: 0x0400375D RID: 14173
	private Guid hasPipeGuid;

	// Token: 0x0400375E RID: 14174
	private Guid pipeBlockedGuid;

	// Token: 0x0400375F RID: 14175
	private HandleVector<int>.Handle partitionerEntry;
}
