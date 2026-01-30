using System;
using System.Diagnostics;
using ImGuiNET;
using UnityEngine;

// Token: 0x020006B1 RID: 1713
public class DevToolStatusItems : DevTool
{
	// Token: 0x06002A26 RID: 10790 RVA: 0x000F6C13 File Offset: 0x000F4E13
	public DevToolStatusItems() : this(Option.None)
	{
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000F6C28 File Offset: 0x000F4E28
	public DevToolStatusItems(Option<DevToolEntityTarget.ForWorldGameObject> target)
	{
		this.targetOpt = target;
		this.tableDrawer = ImGuiObjectTableDrawer<StatusItemGroup.Entry>.New().RemoveFlags(ImGuiTableFlags.SizingFixedFit).AddFlags(ImGuiTableFlags.Resizable).Column("Text", (StatusItemGroup.Entry entry) => entry.GetName()).Column("Id Name", (StatusItemGroup.Entry entry) => entry.item.Id).Column("Notification Type", (StatusItemGroup.Entry entry) => entry.item.notificationType).Column("Category", delegate(StatusItemGroup.Entry entry)
		{
			StatusItemCategory category = entry.category;
			return ((category != null) ? category.Name : null) ?? "<no category>";
		}).Column("OnAdded Callstack", delegate(StatusItemGroup.Entry entry)
		{
			StackTrace stackTrace;
			if (this.statusItemStackTraceWatcher.GetStackTraceForEntry(entry, out stackTrace))
			{
				if (ImGui.Selectable("copy callstack"))
				{
					ImGui.SetClipboardText(stackTrace.ToString());
				}
				ImGuiEx.TooltipForPrevious(stackTrace.ToString());
				return;
			}
			ImGui.Text("<None>");
		}).Build();
		base.OnUninit += delegate()
		{
			this.statusItemStackTraceWatcher.Dispose();
		};
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000F6D40 File Offset: 0x000F4F40
	protected override void RenderTo(DevPanel panel)
	{
		this.statusItemStackTraceWatcher.SetTarget(this.targetOpt.AndThen<GameObject>((DevToolEntityTarget.ForWorldGameObject t) => t.gameObject).AndThen<KSelectable>((GameObject go) => go.GetComponent<KSelectable>()).AndThen<StatusItemGroup>((KSelectable s) => s.GetStatusItemGroup()));
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					this.targetOpt = (DevToolEntityTarget.ForWorldGameObject)target;
				}, new Func<DevToolEntityTarget, Option<string>>(DevToolStatusItems.GetErrorForCandidateTarget)));
			}
			string error = null;
			if (this.targetOpt.IsNone())
			{
				error = "No target selected.";
			}
			else
			{
				Option<string> errorForCandidateTarget = DevToolStatusItems.GetErrorForCandidateTarget(this.targetOpt.Unwrap());
				if (errorForCandidateTarget.IsSome())
				{
					error = errorForCandidateTarget.Unwrap();
				}
			}
			if (ImGuiEx.MenuItem("Debug Target", error))
			{
				panel.PushValue<DevToolEntityTarget.ForWorldGameObject>(this.targetOpt.Unwrap());
			}
			ImGui.EndMenuBar();
		}
		this.Name = "Status Items";
		if (this.targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = this.targetOpt.Unwrap();
		Option<string> errorForCandidateTarget2 = DevToolStatusItems.GetErrorForCandidateTarget(forWorldGameObject);
		if (errorForCandidateTarget2.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget2.Unwrap());
			return;
		}
		this.Name = "Status Items for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		bool shouldWatch = this.statusItemStackTraceWatcher.GetShouldWatch();
		if (ImGui.Checkbox("Should Track OnAdded Callstacks", ref shouldWatch))
		{
			this.statusItemStackTraceWatcher.SetShouldWatch(shouldWatch);
		}
		ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
		this.tableDrawer.Draw(forWorldGameObject.gameObject.GetComponent<KSelectable>().GetStatusItemGroup().GetEnumerator());
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = forWorldGameObject.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), forWorldGameObject.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000F6F5C File Offset: 0x000F515C
	public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
	{
		if (!(uncastTarget is DevToolEntityTarget.ForWorldGameObject))
		{
			return "Target must be a world GameObject";
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = (DevToolEntityTarget.ForWorldGameObject)uncastTarget;
		if (forWorldGameObject.gameObject.IsNullOrDestroyed())
		{
			return "Target GameObject is null or destroyed";
		}
		KSelectable component = forWorldGameObject.gameObject.GetComponent<KSelectable>();
		if (component.IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a KSelectable";
		}
		if (component.GetStatusItemGroup().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a StatusItemGroup";
		}
		return Option.None;
	}

	// Token: 0x04001910 RID: 6416
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	// Token: 0x04001911 RID: 6417
	private ImGuiObjectTableDrawer<StatusItemGroup.Entry> tableDrawer;

	// Token: 0x04001912 RID: 6418
	private StatusItemStackTraceWatcher statusItemStackTraceWatcher = new StatusItemStackTraceWatcher();

	// Token: 0x04001913 RID: 6419
	private bool shouldDrawBoundingBox = true;
}
