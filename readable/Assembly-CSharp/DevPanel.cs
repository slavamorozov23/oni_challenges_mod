using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000682 RID: 1666
public class DevPanel
{
	// Token: 0x170001FA RID: 506
	// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000E9ED4 File Offset: 0x000E80D4
	// (set) Token: 0x060028F7 RID: 10487 RVA: 0x000E9EDC File Offset: 0x000E80DC
	public bool isRequestingToClose { get; private set; }

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000E9EE5 File Offset: 0x000E80E5
	// (set) Token: 0x060028F9 RID: 10489 RVA: 0x000E9EED File Offset: 0x000E80ED
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x060028FA RID: 10490 RVA: 0x000E9EF6 File Offset: 0x000E80F6
	// (set) Token: 0x060028FB RID: 10491 RVA: 0x000E9EFE File Offset: 0x000E80FE
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	// Token: 0x060028FC RID: 10492 RVA: 0x000E9F08 File Offset: 0x000E8108
	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x000E9F64 File Offset: 0x000E8164
	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x000E9F95 File Offset: 0x000E8195
	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000E9FA3 File Offset: 0x000E81A3
	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000E9FB8 File Offset: 0x000E81B8
	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.manager.AddPanelFor(devTool);
			return;
		}
		for (int i = this.devTools.Count - 1; i > this.currentDevToolIndex; i--)
		{
			this.devTools[i].Internal_Uninit();
			this.devTools.RemoveAt(i);
		}
		this.devTools.Add(devTool);
		this.currentDevToolIndex = this.devTools.Count - 1;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000EA038 File Offset: 0x000E8238
	public bool NavGoBack()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000EA068 File Offset: 0x000E8268
	public bool NavGoForward()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x000EA096 File Offset: 0x000E8296
	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x000EA0AC File Offset: 0x000E82AC
	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = this.currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= this.devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x000EA0F0 File Offset: 0x000E82F0
	public void RenderPanel()
	{
		DevTool currentDevTool = this.GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
			return;
		}
		ImGuiWindowFlags flags;
		this.ConfigureImGuiWindowFor(currentDevTool, out flags);
		currentDevTool.Internal_Update();
		bool flag = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + this.uniquePanelId, ref flag, flags))
		{
			if (!flag)
			{
				this.isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				this.DrawNavigation();
				ImGui.SameLine(0f, 20f);
				this.DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (this.GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (this.GetCurrentDevTool().isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
		}
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x000EA1B8 File Offset: 0x000E83B8
	private void DrawNavigation()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			this.currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + this.devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = this.TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			this.currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + this.devTools[option2.Unwrap()].Name);
			return;
		}
		ImGuiEx.TooltipForPrevious("Go forward");
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000EA299 File Offset: 0x000E8499
	private void DrawMenuBarContents()
	{
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x000EA29C File Offset: 0x000E849C
	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = (ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags);
		if (this.nextImGuiWindowPosition.HasValue)
		{
			ValueTuple<Vector2, ImGuiCond> value = this.nextImGuiWindowPosition.Value;
			Vector2 item = value.Item1;
			ImGuiCond item2 = value.Item2;
			ImGui.SetNextWindowPos(item, item2);
			this.nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
		if (this.nextImGuiWindowSize.HasValue)
		{
			Vector2 item3 = this.nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item3);
			this.nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x000EA333 File Offset: 0x000E8533
	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000EA347 File Offset: 0x000E8547
	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000EA35B File Offset: 0x000E855B
	public void Close()
	{
		this.isRequestingToClose = true;
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000EA364 File Offset: 0x000E8564
	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	// Token: 0x0400183A RID: 6202
	public readonly string uniquePanelId;

	// Token: 0x0400183B RID: 6203
	public readonly DevPanelList manager;

	// Token: 0x0400183C RID: 6204
	public readonly Type initialDevToolType;

	// Token: 0x0400183D RID: 6205
	public readonly uint idPostfixNumber;

	// Token: 0x0400183E RID: 6206
	private List<DevTool> devTools;

	// Token: 0x0400183F RID: 6207
	private int currentDevToolIndex;
}
