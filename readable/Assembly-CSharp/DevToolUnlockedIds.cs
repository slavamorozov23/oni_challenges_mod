using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
public class DevToolUnlockedIds : DevTool
{
	// Token: 0x06002A46 RID: 10822 RVA: 0x000F789F File Offset: 0x000F5A9F
	public DevToolUnlockedIds()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000F78C4 File Offset: 0x000F5AC4
	protected override void RenderTo(DevPanel panel)
	{
		bool flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper;
		this.GetUnlocks().Deconstruct(out flag, out unlocksWrapper);
		bool flag2 = flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper2 = unlocksWrapper;
		if (!flag2)
		{
			ImGui.Text("Couldn't access global unlocks");
			return;
		}
		if (ImGui.TreeNode("Help"))
		{
			ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
			ImGui.Spacing();
			ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
			ImGui.TreePop();
		}
		ImGui.Spacing();
		ImGuiEx.InputFilter("Filter", ref this.filterForUnlockIds, 50U);
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.ScrollY;
		if (ImGui.BeginTable("ID_unlockIds", 2, flags))
		{
			ImGui.TableSetupScrollFreeze(2, 2);
			ImGui.TableSetupColumn("Unlock ID");
			ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableHeadersRow();
			ImGui.PushID("ID_row_add_new");
			ImGui.TableNextRow();
			ImGui.TableSetColumnIndex(0);
			ImGui.InputText("", ref this.unlockIdToAdd, 50U);
			ImGui.TableSetColumnIndex(1);
			if (ImGui.Button("Add"))
			{
				unlocksWrapper2.AddId(this.unlockIdToAdd);
				global::Debug.Log("[Added unlock id] " + this.unlockIdToAdd);
				this.unlockIdToAdd = "";
			}
			ImGui.PopID();
			int num = 0;
			foreach (string text in unlocksWrapper2.GetAllIds())
			{
				string text2 = (text == null) ? "<<null>>" : ("\"" + text + "\"");
				if (text2.ToLower().Contains(this.filterForUnlockIds.ToLower()))
				{
					ImGui.TableNextRow();
					ImGui.PushID(string.Format("ID_row_{0}", num++));
					ImGui.TableSetColumnIndex(0);
					ImGui.Text(text2);
					ImGui.TableSetColumnIndex(1);
					if (ImGui.Button("Copy"))
					{
						GUIUtility.systemCopyBuffer = text;
						global::Debug.Log("[Copied to clipboard] " + text);
					}
					ImGui.SameLine();
					if (ImGui.Button("Remove"))
					{
						unlocksWrapper2.RemoveId(text);
						global::Debug.Log("[Removed unlock id] " + text);
					}
					ImGui.PopID();
				}
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000F7AF0 File Offset: 0x000F5CF0
	private Option<DevToolUnlockedIds.UnlocksWrapper> GetUnlocks()
	{
		if (App.IsExiting)
		{
			return Option.None;
		}
		if (Game.Instance == null || !Game.Instance)
		{
			return Option.None;
		}
		if (Game.Instance.unlocks == null)
		{
			return Option.None;
		}
		return Option.Some<DevToolUnlockedIds.UnlocksWrapper>(new DevToolUnlockedIds.UnlocksWrapper(Game.Instance.unlocks));
	}

	// Token: 0x0400191D RID: 6429
	private string filterForUnlockIds = "";

	// Token: 0x0400191E RID: 6430
	private string unlockIdToAdd = "";

	// Token: 0x02001573 RID: 5491
	public readonly struct UnlocksWrapper
	{
		// Token: 0x06009352 RID: 37714 RVA: 0x00375707 File Offset: 0x00373907
		public UnlocksWrapper(Unlocks unlocks)
		{
			this.unlocks = unlocks;
		}

		// Token: 0x06009353 RID: 37715 RVA: 0x00375710 File Offset: 0x00373910
		public void AddId(string unlockId)
		{
			this.unlocks.Unlock(unlockId, true);
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x0037571F File Offset: 0x0037391F
		public void RemoveId(string unlockId)
		{
			this.unlocks.Lock(unlockId);
		}

		// Token: 0x06009355 RID: 37717 RVA: 0x0037572D File Offset: 0x0037392D
		public IEnumerable<string> GetAllIds()
		{
			return from s in this.unlocks.GetAllUnlockedIds()
			orderby s
			select s;
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06009356 RID: 37718 RVA: 0x0037575E File Offset: 0x0037395E
		public int Count
		{
			get
			{
				return this.unlocks.GetAllUnlockedIds().Count;
			}
		}

		// Token: 0x040071CF RID: 29135
		public readonly Unlocks unlocks;
	}
}
