using System;
using UnityEngine;

// Token: 0x020009A7 RID: 2471
public class DeconstructTool : FilteredDragTool
{
	// Token: 0x06004744 RID: 18244 RVA: 0x0019C8DD File Offset: 0x0019AADD
	public static void DestroyInstance()
	{
		DeconstructTool.Instance = null;
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x0019C8E5 File Offset: 0x0019AAE5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DeconstructTool.Instance = this;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x0019C8F3 File Offset: 0x0019AAF3
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x0019C900 File Offset: 0x0019AB00
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x0019C907 File Offset: 0x0019AB07
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x0019C90E File Offset: 0x0019AB0E
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.DeconstructCell(cell);
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x0019C918 File Offset: 0x0019AB18
	public void DeconstructCell(int cell)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(-790448070, null);
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x0019C98A File Offset: 0x0019AB8A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x0019C9A2 File Offset: 0x0019ABA2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002FCC RID: 12236
	public static DeconstructTool Instance;
}
