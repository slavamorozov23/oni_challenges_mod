using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
public class DevToolStateMachineDebug : DevTool
{
	// Token: 0x06002A15 RID: 10773 RVA: 0x000F61EC File Offset: 0x000F43EC
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.selectedGameObject == null)
		{
			this.lockSelection = false;
		}
		SelectTool instance = SelectTool.Instance;
		GameObject gameObject;
		if (instance == null)
		{
			gameObject = null;
		}
		else
		{
			KSelectable selected = instance.selected;
			gameObject = ((selected != null) ? selected.gameObject : null);
		}
		GameObject gameObject2 = gameObject;
		if (!this.lockSelection && this.selectedGameObject != gameObject2 && gameObject2 != null && gameObject2.GetComponentsInChildren<StateMachineController>().Length != 0)
		{
			this.selectedGameObject = gameObject2;
			this.selectedStateMachine = 0;
		}
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000F626C File Offset: 0x000F446C
	public void ShowEditor(StateMachineDebuggerSettings.Entry entry)
	{
		ImGui.Text(entry.typeName);
		ImGui.SameLine();
		ImGui.PushID(entry.typeName);
		ImGui.PushID(1);
		ImGui.Checkbox("", ref entry.enableConsoleLogging);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(2);
		ImGui.Checkbox("", ref entry.breakOnGoTo);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(3);
		ImGui.Checkbox("", ref entry.saveHistory);
		ImGui.PopID();
		ImGui.PopID();
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000F62F8 File Offset: 0x000F44F8
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
		ImGui.InputText("Filter:", ref this.stateMachineFilter, 256U);
		if (this.showSettings = ImGui.CollapsingHeader("Debug Settings:"))
		{
			if (ImGui.Button("Reset"))
			{
				StateMachineDebuggerSettings.Get().Clear();
			}
			ImGui.Text("EnableConsoleLogging / BreakOnGoTo / SaveHistory");
			int num = 0;
			foreach (StateMachineDebuggerSettings.Entry entry in StateMachineDebuggerSettings.Get())
			{
				if (string.IsNullOrEmpty(this.stateMachineFilter) || entry.typeName.ToLower().IndexOf(this.stateMachineFilter) >= 0)
				{
					this.ShowEditor(entry);
					num++;
				}
			}
		}
		if (Application.isPlaying && this.selectedGameObject != null)
		{
			StateMachineController[] componentsInChildren = this.selectedGameObject.GetComponentsInChildren<StateMachineController>();
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			List<StateMachine.Instance> list2 = new List<StateMachine.Instance>();
			List<StateMachine.BaseDef> list3 = new List<StateMachine.BaseDef>();
			foreach (StateMachineController stateMachineController in componentsInChildren)
			{
				foreach (StateMachine.Instance instance in stateMachineController)
				{
					string text = stateMachineController.name + "." + instance.ToString();
					if (instance.isCrashed)
					{
						text = "(ERROR)" + text;
					}
					list.Add(text);
				}
			}
			List<string> list4;
			if (this.stateMachineFilter == null || this.stateMachineFilter == "")
			{
				list4 = (from name in list
				select name.ToLower()).ToList<string>();
			}
			else
			{
				list4 = (from name in list
				where name.ToLower().Contains(this.stateMachineFilter)
				select name.ToLower()).ToList<string>();
			}
			foreach (StateMachineController stateMachineController2 in componentsInChildren)
			{
				foreach (StateMachine.Instance instance2 in stateMachineController2)
				{
					string text2 = stateMachineController2.name + "." + instance2.ToString();
					if (instance2.isCrashed)
					{
						text2 = "(ERROR)" + text2;
					}
					if (list4.Contains(text2.ToLower()))
					{
						list2.Add(instance2);
					}
				}
				foreach (StateMachine.BaseDef item in stateMachineController2.GetDefs<StateMachine.BaseDef>())
				{
					list3.Add(item);
				}
			}
			if (list4.Count == 0)
			{
				string label = "Defs";
				string fmt;
				if (list3.Count != 0)
				{
					fmt = string.Join(", ", from d in list3
					select d.GetType().ToString());
				}
				else
				{
					fmt = "(none)";
				}
				ImGui.LabelText(label, fmt);
				foreach (StateMachineController controller in componentsInChildren)
				{
					this.ShowControllerLog(controller);
				}
				return;
			}
			this.selectedStateMachine = Math.Min(this.selectedStateMachine, list4.Count - 1);
			string label2 = "Defs";
			string fmt2;
			if (list3.Count != 0)
			{
				fmt2 = string.Join(", ", from d in list3
				select d.GetType().ToString());
			}
			else
			{
				fmt2 = "(none)";
			}
			ImGui.LabelText(label2, fmt2);
			ImGui.Checkbox("Lock selection", ref this.lockSelection);
			ImGui.Indent();
			ImGui.Combo("Select state machine", ref this.selectedStateMachine, list4.ToArray(), list4.Count);
			ImGui.Unindent();
			StateMachine.Instance instance3 = list2[this.selectedStateMachine];
			this.ShowStates(instance3);
			this.ShowTags(instance3);
			this.ShowDetails(instance3);
			this.ShowLog(instance3);
			this.ShowControllerLog(instance3);
			this.ShowHistory(instance3.GetMaster().GetComponent<StateMachineController>());
			this.ShowKAnimControllerLog();
		}
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x000F6770 File Offset: 0x000F4970
	private void ShowStates(StateMachine.Instance state_machine_instance)
	{
		StateMachine stateMachine = state_machine_instance.GetStateMachine();
		ImGui.Text(stateMachine.ToString() + ": ");
		ImGui.Checkbox("Break On GoTo: ", ref state_machine_instance.breakOnGoTo);
		ImGui.Checkbox("Console Logging: ", ref state_machine_instance.enableConsoleLogging);
		string value = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			value = currentState.name;
		}
		string[] array = stateMachine.GetStateNames().Append("None");
		array[0] = array[0];
		int num = Array.IndexOf<string>(array, value);
		int num2 = num;
		for (int i = 0; i < array.Length; i++)
		{
			ImGui.RadioButton(array[i], ref num2, i);
		}
		if (num2 != num)
		{
			if (array[num2] == "None")
			{
				state_machine_instance.StopSM("StateMachineEditor.StopSM");
				return;
			}
			state_machine_instance.GoTo(array[num2]);
		}
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x000F6840 File Offset: 0x000F4A40
	public void ShowTags(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Tags:");
		ImGui.Indent();
		KPrefabID component = state_machine_instance.GetComponent<KPrefabID>();
		if (component != null)
		{
			foreach (Tag tag in component.Tags)
			{
				ImGui.Text(tag.Name);
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x000F68BC File Offset: 0x000F4ABC
	private void ShowDetails(StateMachine.Instance state_machine_instance)
	{
		state_machine_instance.GetStateMachine();
		string str = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			str = currentState.name;
		}
		ImGui.Text(str + ": ");
		ImGui.Indent();
		this.ShowParameters(state_machine_instance);
		this.ShowEvents(state_machine_instance);
		this.ShowTransitions(state_machine_instance);
		this.ShowEnterActions(state_machine_instance);
		this.ShowExitActions(state_machine_instance);
		ImGui.Unindent();
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000F6924 File Offset: 0x000F4B24
	private void ShowParameters(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Parameters:");
		ImGui.Indent();
		StateMachine.Parameter.Context[] parameterContexts = state_machine_instance.GetParameterContexts();
		for (int i = 0; i < parameterContexts.Length; i++)
		{
			parameterContexts[i].ShowDevTool(state_machine_instance);
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000F6964 File Offset: 0x000F4B64
	private void ShowEvents(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Events: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.events != null)
			{
				foreach (StateEvent stateEvent in state.events)
				{
					ImGui.Text(stateEvent.GetName());
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x000F69FC File Offset: 0x000F4BFC
	private void ShowTransitions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Transitions:");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.transitions != null)
			{
				for (int j = 0; j < state.transitions.Count; j++)
				{
					ImGui.Text(state.transitions[j].ToString());
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x000F6A78 File Offset: 0x000F4C78
	private void ShowExitActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Exit Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.exitActions != null)
			{
				foreach (StateMachine.Action action in state.exitActions)
				{
					ImGui.Text(action.name);
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x000F6B10 File Offset: 0x000F4D10
	private void ShowEnterActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Enter Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.enterActions != null)
			{
				foreach (StateMachine.Action action in state.enterActions)
				{
					ImGui.Text(action.name);
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000F6BA8 File Offset: 0x000F4DA8
	private void ShowLog(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Machine Log:");
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000F6BB4 File Offset: 0x000F4DB4
	private void ShowKAnimControllerLog()
	{
		this.selectedGameObject.GetComponentInChildren<KAnimControllerBase>() == null;
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000F6BC8 File Offset: 0x000F4DC8
	private void ShowHistory(StateMachineController controller)
	{
		ImGui.Text("Logger disabled");
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x000F6BD4 File Offset: 0x000F4DD4
	private void ShowControllerLog(StateMachineController controller)
	{
		ImGui.Text("Object Log:");
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x000F6BE0 File Offset: 0x000F4DE0
	private void ShowControllerLog(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetMaster().isNull)
		{
			this.ShowControllerLog(state_machine.GetMaster().GetComponent<StateMachineController>());
		}
	}

	// Token: 0x04001909 RID: 6409
	private int selectedStateMachine;

	// Token: 0x0400190A RID: 6410
	private int selectedLog;

	// Token: 0x0400190B RID: 6411
	private GameObject selectedGameObject;

	// Token: 0x0400190C RID: 6412
	private Vector2 scrollPos;

	// Token: 0x0400190D RID: 6413
	private bool lockSelection;

	// Token: 0x0400190E RID: 6414
	private bool showSettings;

	// Token: 0x0400190F RID: 6415
	private string stateMachineFilter = "";
}
