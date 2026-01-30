using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using ImGuiObjectDrawer;
using UnityEngine;

// Token: 0x0200069B RID: 1691
public class DevToolEntity_RanchStation : DevTool
{
	// Token: 0x060029A3 RID: 10659 RVA: 0x000EF3F8 File Offset: 0x000ED5F8
	public DevToolEntity_RanchStation() : this(Option.None)
	{
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000EF40A File Offset: 0x000ED60A
	public DevToolEntity_RanchStation(Option<DevToolEntityTarget.ForWorldGameObject> target)
	{
		this.targetOpt = target;
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000EF420 File Offset: 0x000ED620
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					this.targetOpt = (DevToolEntityTarget.ForWorldGameObject)target;
				}, new Func<DevToolEntityTarget, Option<string>>(DevToolEntity_RanchStation.GetErrorForCandidateTarget)));
			}
			ImGui.EndMenuBar();
		}
		this.Name = "RanchStation debug";
		if (this.targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = this.targetOpt.Unwrap();
		Option<string> errorForCandidateTarget = DevToolEntity_RanchStation.GetErrorForCandidateTarget(forWorldGameObject);
		if (errorForCandidateTarget.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget.Unwrap());
			return;
		}
		this.Name = "RanchStation debug for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		RanchStation.Instance smi = forWorldGameObject.gameObject.GetSMI<RanchStation.Instance>();
		RanchStation.Def def = forWorldGameObject.gameObject.GetDef<RanchStation.Def>();
		StateMachine stateMachine = smi.GetStateMachine();
		DevToolEntity_RanchStation.DrawRanchableCollection("Target Ranchables", smi.DEBUG_GetTargetRanchables());
		if (ImGui.CollapsingHeader("Full Debug Info"))
		{
			ImGuiEx.DrawObject("State Machine Instance", smi, new MemberDrawContext?(new MemberDrawContext(false, false)));
			ImGuiEx.DrawObject("State Machine Def", def, new MemberDrawContext?(new MemberDrawContext(false, false)));
			ImGuiEx.DrawObject("State Machine", stateMachine, new MemberDrawContext?(new MemberDrawContext(false, false)));
		}
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = forWorldGameObject.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), "[Ranching Station]", ImGui.IsWindowFocused());
			}
			List<RanchableMonitor.Instance> list = smi.DEBUG_GetTargetRanchables();
			for (int i = 0; i < list.Count; i++)
			{
				RanchableMonitor.Instance instance = list[i];
				if (!instance.gameObject.IsNullOrDestroyed())
				{
					Option<ValueTuple<Vector2, Vector2>> screenRect2 = new DevToolEntityTarget.ForWorldGameObject(instance.gameObject).GetScreenRect();
					if (screenRect2.IsSome())
					{
						DevToolEntity.DrawBoundingBox(screenRect2.Unwrap(), string.Format("[Target Ranchable @ Index {0}]", i), ImGui.IsWindowFocused());
					}
				}
			}
		}
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000EF5F8 File Offset: 0x000ED7F8
	public static void DrawRanchableCollection(string name, IEnumerable<RanchableMonitor.Instance> ranchables)
	{
		if (ImGui.CollapsingHeader(name))
		{
			if (ranchables.IsNullOrDestroyed())
			{
				ImGui.Text("List is null");
				return;
			}
			if (ranchables.Count<RanchableMonitor.Instance>() == 0)
			{
				ImGui.Text("List is empty");
				return;
			}
			int num = 0;
			foreach (RanchableMonitor.Instance instance in ranchables)
			{
				ImGui.Text(instance.IsNullOrDestroyed() ? "<null RanchableMonitor>" : DevToolEntity.GetNameFor(instance.gameObject));
				ImGui.SameLine();
				if (ImGui.Button(string.Format("DevTool Inspect###ID_Inspect_{0}", num)))
				{
					DevToolSceneInspector.Inspect(instance);
				}
				num++;
			}
		}
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000EF6B4 File Offset: 0x000ED8B4
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
		if (forWorldGameObject.gameObject.GetDef<RanchStation.Def>().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a RanchStation.Def";
		}
		return Option.None;
	}

	// Token: 0x04001894 RID: 6292
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	// Token: 0x04001895 RID: 6293
	private bool shouldDrawBoundingBox = true;
}
