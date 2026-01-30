using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000694 RID: 1684
public class DevToolCommandPalette : DevTool
{
	// Token: 0x0600297D RID: 10621 RVA: 0x000ED901 File Offset: 0x000EBB01
	public DevToolCommandPalette() : this(null)
	{
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x000ED90C File Offset: 0x000EBB0C
	public DevToolCommandPalette(List<DevToolCommandPalette.Command> commands = null)
	{
		this.drawFlags |= ImGuiWindowFlags.NoResize;
		this.drawFlags |= ImGuiWindowFlags.NoScrollbar;
		this.drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
			return;
		}
		this.commands.allValues = commands;
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x000ED99B File Offset: 0x000EBB9B
	public static void Init()
	{
		DevToolCommandPalette.InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000ED9A7 File Offset: 0x000EBBA7
	public static void InitWithCommands(List<DevToolCommandPalette.Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000ED9C0 File Offset: 0x000EBBC0
	protected override void RenderTo(DevPanel panel)
	{
		DevToolCommandPalette.Resize(panel);
		if (this.commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (this.commands.allValues.Count == 0)
		{
			ImGui.Text("Given command list is empty, no results to show.");
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			panel.Close();
			return;
		}
		if (!ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
		{
			panel.Close();
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.m_selected_index--;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_selected_index++;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (this.commands.filteredValues.Count > 0)
		{
			while (this.m_selected_index < 0)
			{
				this.m_selected_index += this.commands.filteredValues.Count;
			}
			this.m_selected_index %= this.commands.filteredValues.Count;
		}
		else
		{
			this.m_selected_index = 0;
		}
		DevToolCommandPalette.Command command = null;
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.commands.filteredValues.Count > 0 && command == null)
		{
			command = this.commands.filteredValues[this.m_selected_index];
		}
		if (this.m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref this.commands.filter, 30U) || this.m_should_focus_search)
		{
			this.commands.Refilter();
		}
		this.m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (this.commands.filteredValues.Count > 0 && this.commands.didUseFilter)
		{
			text += string.Format("Found {0} Results", this.commands.filteredValues.Count);
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (this.commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + this.commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < this.commands.filteredValues.Count; i++)
				{
					DevToolCommandPalette.Command command2 = this.commands.filteredValues[i];
					bool flag = i == this.m_selected_index;
					ImGui.PushID(i);
					bool flag2;
					if (flag)
					{
						flag2 = ImGui.Selectable("> " + command2.display_name, flag);
					}
					else
					{
						flag2 = ImGui.Selectable("  " + command2.display_name, flag);
					}
					ImGui.PopID();
					if (this.shouldScrollToSelectedCommandFlag && flag)
					{
						this.shouldScrollToSelectedCommandFlag = false;
						ImGui.SetScrollHereY(0.5f);
					}
					if (flag2 && command == null)
					{
						command = command2;
					}
				}
			}
		}
		ImGui.EndChild();
		if (command != null)
		{
			command.Internal_Select();
			panel.Close();
		}
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x000EDCB0 File Offset: 0x000EBEB0
	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Rect rect2 = new Rect
		{
			x = rect.x + rect.width / 2f - num / 2f,
			y = rect.y + rect.height / 2f - num2 / 2f,
			width = num,
			height = num2
		};
		devToolPanel.SetPosition(rect2.position, ImGuiCond.None);
		devToolPanel.SetSize(rect2.size, ImGuiCond.None);
	}

	// Token: 0x04001884 RID: 6276
	private int m_selected_index;

	// Token: 0x04001885 RID: 6277
	private StringSearchableList<DevToolCommandPalette.Command> commands = new StringSearchableList<DevToolCommandPalette.Command>(delegate(DevToolCommandPalette.Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, filter);
	});

	// Token: 0x04001886 RID: 6278
	private bool m_should_focus_search = true;

	// Token: 0x04001887 RID: 6279
	private bool shouldScrollToSelectedCommandFlag;

	// Token: 0x0200155A RID: 5466
	public class Command
	{
		// Token: 0x060092F7 RID: 37623 RVA: 0x00374D19 File Offset: 0x00372F19
		public Command(string primary_tag, System.Action on_select) : this(new string[]
		{
			primary_tag
		}, on_select)
		{
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x00374D2C File Offset: 0x00372F2C
		public Command(string primary_tag, string tag_a, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a
		}, on_select)
		{
		}

		// Token: 0x060092F9 RID: 37625 RVA: 0x00374D43 File Offset: 0x00372F43
		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b
		}, on_select)
		{
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x00374D5F File Offset: 0x00372F5F
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c
		}, on_select)
		{
		}

		// Token: 0x060092FB RID: 37627 RVA: 0x00374D80 File Offset: 0x00372F80
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d
		}, on_select)
		{
		}

		// Token: 0x060092FC RID: 37628 RVA: 0x00374DA6 File Offset: 0x00372FA6
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e
		}, on_select)
		{
		}

		// Token: 0x060092FD RID: 37629 RVA: 0x00374DD1 File Offset: 0x00372FD1
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e,
			tag_f
		}, on_select)
		{
		}

		// Token: 0x060092FE RID: 37630 RVA: 0x00374E01 File Offset: 0x00373001
		public Command(string primary_tag, string[] additional_tags, System.Action on_select) : this(new string[]
		{
			primary_tag
		}.Concat(additional_tags).ToArray<string>(), on_select)
		{
		}

		// Token: 0x060092FF RID: 37631 RVA: 0x00374E20 File Offset: 0x00373020
		public Command(string[] tags, System.Action on_select)
		{
			this.display_name = tags[0];
			this.tags = (from t in tags
			select t.ToLowerInvariant()).ToArray<string>();
			this.m_on_select = on_select;
		}

		// Token: 0x06009300 RID: 37632 RVA: 0x00374E73 File Offset: 0x00373073
		public void Internal_Select()
		{
			this.m_on_select();
		}

		// Token: 0x0400718E RID: 29070
		public string display_name;

		// Token: 0x0400718F RID: 29071
		public string[] tags;

		// Token: 0x04007190 RID: 29072
		private System.Action m_on_select;
	}
}
