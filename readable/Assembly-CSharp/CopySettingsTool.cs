using System;
using UnityEngine;

// Token: 0x020009A5 RID: 2469
public class CopySettingsTool : DragTool
{
	// Token: 0x06004732 RID: 18226 RVA: 0x0019C200 File Offset: 0x0019A400
	public static void DestroyInstance()
	{
		CopySettingsTool.Instance = null;
	}

	// Token: 0x06004733 RID: 18227 RVA: 0x0019C208 File Offset: 0x0019A408
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CopySettingsTool.Instance = this;
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x0019C216 File Offset: 0x0019A416
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x0019C223 File Offset: 0x0019A423
	public void SetSourceObject(GameObject sourceGameObject)
	{
		this.sourceGameObject = sourceGameObject;
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x0019C22C File Offset: 0x0019A42C
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.sourceGameObject == null)
		{
			return;
		}
		if (Grid.IsValidCell(cell))
		{
			CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
		}
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x0019C252 File Offset: 0x0019A452
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x0019C25A File Offset: 0x0019A45A
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.sourceGameObject = null;
	}

	// Token: 0x04002FC7 RID: 12231
	public static CopySettingsTool Instance;

	// Token: 0x04002FC8 RID: 12232
	public GameObject Placer;

	// Token: 0x04002FC9 RID: 12233
	private GameObject sourceGameObject;
}
