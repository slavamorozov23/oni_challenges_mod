using System;
using UnityEngine;

// Token: 0x020009A8 RID: 2472
public class DigTool : DragTool
{
	// Token: 0x0600474E RID: 18254 RVA: 0x0019C9C3 File Offset: 0x0019ABC3
	public static void DestroyInstance()
	{
		DigTool.Instance = null;
	}

	// Token: 0x0600474F RID: 18255 RVA: 0x0019C9CB File Offset: 0x0019ABCB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DigTool.Instance = this;
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x0019C9D9 File Offset: 0x0019ABD9
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		InterfaceTool.ActiveConfig.DigAction.Uproot(cell);
		InterfaceTool.ActiveConfig.DigAction.Dig(cell, distFromOrigin);
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x0019C9FC File Offset: 0x0019ABFC
	public static GameObject PlaceDig(int cell, int animationDelay = 0)
	{
		if (Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Objects[cell, 7] == null)
		{
			for (int i = 0; i < 45; i++)
			{
				if (Grid.Objects[cell, i] != null && Grid.Objects[cell, i].GetComponent<Constructable>() != null)
				{
					return null;
				}
			}
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), null, null);
			gameObject.SetActive(true);
			Grid.Objects[cell, 7] = gameObject;
			Vector3 vector = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
			float num = -0.15f;
			vector.z += num;
			gameObject.transform.SetPosition(vector);
			gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0f, (float)animationDelay * 0.02f));
			return gameObject;
		}
		if (Grid.Objects[cell, 7] != null)
		{
			return Grid.Objects[cell, 7];
		}
		return null;
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x0019CB20 File Offset: 0x0019AD20
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x0019CB38 File Offset: 0x0019AD38
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002FCD RID: 12237
	public static DigTool Instance;
}
