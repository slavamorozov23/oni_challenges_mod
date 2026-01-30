using System;
using UnityEngine;

// Token: 0x020009A3 RID: 2467
public class ClearTool : DragTool
{
	// Token: 0x0600471F RID: 18207 RVA: 0x0019BE75 File Offset: 0x0019A075
	public static void DestroyInstance()
	{
		ClearTool.Instance = null;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x0019BE7D File Offset: 0x0019A07D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClearTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x0019BE92 File Offset: 0x0019A092
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x0019BEA0 File Offset: 0x0019A0A0
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject == null)
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			Pickupable pickupable = objectLayerListItem.pickupable;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && !pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.Clearable.isClearable)
			{
				pickupable.Clearable.MarkForClear(false, false);
				Prioritizable component = gameObject2.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x0019BF47 File Offset: 0x0019A147
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x0019BF5F File Offset: 0x0019A15F
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002FC1 RID: 12225
	public static ClearTool Instance;
}
