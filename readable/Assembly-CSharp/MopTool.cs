using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009B1 RID: 2481
public class MopTool : DragTool
{
	// Token: 0x060047D7 RID: 18391 RVA: 0x0019F727 File Offset: 0x0019D927
	public static void DestroyInstance()
	{
		MopTool.Instance = null;
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x0019F72F File Offset: 0x0019D92F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
		this.interceptNumberKeysForPriority = true;
		MopTool.Instance = this;
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x0019F759 File Offset: 0x0019D959
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x0019F768 File Offset: 0x0019D968
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			if (DebugHandler.InstantBuildMode)
			{
				if (Grid.IsValidCell(cell))
				{
					Moppable.MopCell(cell, 1000000f, null);
					return;
				}
			}
			else
			{
				GameObject gameObject = Grid.Objects[cell, 8];
				if (!Grid.Solid[cell] && gameObject == null && Grid.Element[cell].IsLiquid)
				{
					bool flag = Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)];
					bool flag2 = Grid.Mass[cell] <= MopTool.maxMopAmt;
					if (flag && flag2)
					{
						gameObject = Util.KInstantiate(this.Placer, null, null);
						Grid.Objects[cell, 8] = gameObject;
						Vector3 vector = Grid.CellToPosCBC(cell, this.visualizerLayer);
						float num = -0.15f;
						vector.z += num;
						gameObject.transform.SetPosition(vector);
						gameObject.SetActive(true);
					}
					else
					{
						string text = UI.TOOLS.MOP.TOO_MUCH_LIQUID;
						if (!flag)
						{
							text = UI.TOOLS.MOP.NOT_ON_FLOOR;
						}
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, text, null, Grid.CellToPosCBC(cell, this.visualizerLayer), 1.5f, false, false);
					}
				}
				if (gameObject != null)
				{
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x0019F8E1 File Offset: 0x0019DAE1
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x0019F8F9 File Offset: 0x0019DAF9
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04003010 RID: 12304
	private GameObject Placer;

	// Token: 0x04003011 RID: 12305
	public static MopTool Instance;

	// Token: 0x04003012 RID: 12306
	private SimHashes Element;

	// Token: 0x04003013 RID: 12307
	public static float maxMopAmt = 150f;
}
