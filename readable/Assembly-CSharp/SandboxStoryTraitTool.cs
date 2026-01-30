using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TemplateClasses;
using UnityEngine;

// Token: 0x020009C0 RID: 2496
public class SandboxStoryTraitTool : InterfaceTool
{
	// Token: 0x06004881 RID: 18561 RVA: 0x001A262C File Offset: 0x001A082C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.preview = new StampToolPreview(this, new IStampToolPreviewPlugin[]
		{
			new StampToolPreview_Area(),
			new StampToolPreview_SolidLiquidGas(),
			new StampToolPreview_Prefabs()
		});
		this.setupPreviewFn = delegate()
		{
			this.preview.Cleanup();
			Story story;
			TemplateContainer stampTemplate;
			if (SandboxStoryTraitTool.TryGetStoryAndTemplate(out story, out stampTemplate))
			{
				base.StartCoroutine(this.preview.Setup(stampTemplate));
				this.preview.OnErrorChange(this.prevError);
			}
		};
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x001A267C File Offset: 0x001A087C
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.storySelector.row.SetActive(true);
		this.setupPreviewFn();
		SandboxSettings settings = SandboxToolParameterMenu.instance.settings;
		settings.OnChangeStory = (System.Action)Delegate.Remove(settings.OnChangeStory, this.setupPreviewFn);
		SandboxSettings settings2 = SandboxToolParameterMenu.instance.settings;
		settings2.OnChangeStory = (System.Action)Delegate.Combine(settings2.OnChangeStory, this.setupPreviewFn);
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x001A2714 File Offset: 0x001A0914
	public void Update()
	{
		Vector3 cursorPos = PlayerController.GetCursorPos(KInputManager.GetMousePos());
		int originCell = Grid.PosToCell(cursorPos);
		this.preview.Refresh(originCell);
		this.timeUntilNextErrorUpdate -= Time.unscaledDeltaTime;
		if (this.timeUntilNextErrorUpdate <= 0f)
		{
			this.timeUntilNextErrorUpdate = 0.1f;
			Story story;
			TemplateContainer templateContainer;
			string error = this.GetError(cursorPos, out story, out templateContainer);
			if (this.prevError != error)
			{
				this.preview.OnErrorChange(error);
				this.prevError = error;
			}
		}
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x001A2798 File Offset: 0x001A0998
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		SandboxSettings settings = SandboxToolParameterMenu.instance.settings;
		settings.OnChangeStory = (System.Action)Delegate.Remove(settings.OnChangeStory, this.setupPreviewFn);
		this.preview.Cleanup();
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x001A27EC File Offset: 0x001A09EC
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.isPlacingTemplate)
		{
			return;
		}
		Story story;
		TemplateContainer stampTemplate;
		if (this.GetError(cursor_pos, out story, out stampTemplate) != null)
		{
			return;
		}
		this.isPlacingTemplate = true;
		SandboxStoryTraitTool.Stamp(cursor_pos, stampTemplate, delegate
		{
			this.isPlacingTemplate = false;
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(story);
			StoryInstance.State currentState = storyInstance.CurrentState;
			storyInstance.CurrentState = StoryInstance.State.RETROFITTED;
			storyInstance.CurrentState = currentState;
		});
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x001A2848 File Offset: 0x001A0A48
	public static void Stamp(Vector2 pos, TemplateContainer stampTemplate, System.Action onCompleteFn)
	{
		bool shouldPauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (stampTemplate.cells != null)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					int cell = Grid.XYToCell((int)(pos.x + (float)stampTemplate.cells[i].location_x), (int)(pos.y + (float)stampTemplate.cells[i].location_y));
					GameObject gameObject = Grid.Objects[cell, j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject gameObject2 in list)
			{
				if (gameObject2 != null)
				{
					Util.KDestroyGameObject(gameObject2);
				}
			}
		}
		TemplateLoader.Stamp(stampTemplate, pos, delegate
		{
			if (shouldPauseOnComplete)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			onCompleteFn();
		});
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Stamp", false));
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x001A2998 File Offset: 0x001A0B98
	public static bool TryGetStoryAndTemplate(out Story story, out TemplateContainer stampTemplate)
	{
		stampTemplate = null;
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
		story = Db.Get().Stories.TryGet(stringSetting);
		if (story == null)
		{
			return false;
		}
		if (story.sandboxStampTemplateId == null)
		{
			return false;
		}
		stampTemplate = TemplateCache.GetTemplate(story.sandboxStampTemplateId);
		return stampTemplate != null;
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x001A29F8 File Offset: 0x001A0BF8
	public string GetError(Vector3 stampPos, out Story story, out TemplateContainer stampTemplate)
	{
		SandboxStoryTraitTool.<>c__DisplayClass13_0 CS$<>8__locals1;
		CS$<>8__locals1.stampPos = stampPos;
		if (!SandboxStoryTraitTool.TryGetStoryAndTemplate(out story, out stampTemplate))
		{
			return "-";
		}
		CS$<>8__locals1._stampTemplate = stampTemplate;
		if (StoryManager.Instance.GetStoryInstance(story) != null)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ALREADY_EXISTS.Replace("{StoryName}", Strings.Get(story.StoryTrait.name));
		}
		int num = Grid.OffsetCell(Grid.PosToCell(CS$<>8__locals1.stampPos), Mathf.FloorToInt(-stampTemplate.info.size.X / 2f), Mathf.FloorToInt(-stampTemplate.info.size.Y / 2f) + 1);
		int num2 = Grid.OffsetCell(Grid.PosToCell(CS$<>8__locals1.stampPos), Mathf.FloorToInt(stampTemplate.info.size.X / 2f), Mathf.FloorToInt(stampTemplate.info.size.Y / 2f) + 1);
		bool flag;
		if (Grid.IsValidBuildingCell(num) && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[num] && Grid.IsValidBuildingCell(num2) && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[num2])
		{
			flag = !SandboxStoryTraitTool.<GetError>g__IsTrueForAnyStampCell|13_0((Cell cellInfo, int cellIndex) => Grid.Element[cellIndex].id == SimHashes.Unobtanium, ref CS$<>8__locals1);
		}
		else
		{
			flag = false;
		}
		if (!flag)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId);
		if (world == null || world.IsModuleInterior)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_INVALID_LOCATION;
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		using (IEnumerator enumerator = Components.Brains.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Brain brain = (Brain)enumerator.Current;
				if (SandboxStoryTraitTool.<GetError>g__IsTrueForAnyStampCell|13_0(delegate(Cell cellInfo, int cellIndex)
				{
					int num3 = Grid.PosToCell(brain.gameObject);
					if (num3 == cellIndex)
					{
						return true;
					}
					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 2; j++)
						{
							if (Grid.OffsetCell(num3, i, j) == cellIndex)
							{
								return true;
							}
						}
					}
					return false;
				}, ref CS$<>8__locals1))
				{
					if (brain.HasTag(GameTags.BaseMinion))
					{
						flag2 = true;
						break;
					}
					if (brain.HasTag(GameTags.Robot))
					{
						flag4 = true;
						break;
					}
					if (brain.HasTag(GameTags.Creature))
					{
						flag3 = true;
						break;
					}
					break;
				}
			}
		}
		if (flag2)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_DUPE_HAZARD;
		}
		if (flag4)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_ROBOT_HAZARD;
		}
		if (flag3)
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_CREATURE_HAZARD;
		}
		if (SandboxStoryTraitTool.<GetError>g__IsTrueForAnyStampCell|13_0(delegate(Cell cellInfo, int cellIndex)
		{
			GameObject gameObject;
			return Grid.ObjectLayers[1].TryGetValue(cellIndex, out gameObject) && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Plant);
		}, ref CS$<>8__locals1))
		{
			return UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.ERROR_BUILDING_HAZARD;
		}
		return null;
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x001A2D20 File Offset: 0x001A0F20
	[CompilerGenerated]
	internal static bool <GetError>g__IsTrueForAnyStampCell|13_0(Func<Cell, int, bool> isTrueFn, ref SandboxStoryTraitTool.<>c__DisplayClass13_0 A_1)
	{
		foreach (Cell cell in A_1._stampTemplate.cells)
		{
			int arg = Grid.OffsetCell(Grid.PosToCell(A_1.stampPos), cell.location_x, cell.location_y);
			if (isTrueFn(cell, arg))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003041 RID: 12353
	private System.Action setupPreviewFn;

	// Token: 0x04003042 RID: 12354
	private StampToolPreview preview;

	// Token: 0x04003043 RID: 12355
	private bool isPlacingTemplate;

	// Token: 0x04003044 RID: 12356
	private string prevError;

	// Token: 0x04003045 RID: 12357
	private const float ERROR_UPDATE_FREQUENCY = 0.1f;

	// Token: 0x04003046 RID: 12358
	private float timeUntilNextErrorUpdate = -1f;
}
