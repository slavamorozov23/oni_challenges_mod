using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000693 RID: 1683
public class DevToolChoreDebugger : DevTool
{
	// Token: 0x06002976 RID: 10614 RVA: 0x000ED269 File Offset: 0x000EB469
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000ED274 File Offset: 0x000EB474
	public void Update()
	{
		if (!Application.isPlaying || SelectTool.Instance == null || SelectTool.Instance.selected == null || SelectTool.Instance.selected.gameObject == null)
		{
			return;
		}
		GameObject gameObject = SelectTool.Instance.selected.gameObject;
		if (this.Consumer == null || (!this.lockSelection && this.selectedGameObject != gameObject))
		{
			this.Consumer = gameObject.GetComponent<ChoreConsumer>();
			this.selectedGameObject = gameObject;
		}
		if (this.Consumer != null)
		{
			ImGui.InputText("Filter:", ref this.filter, 256U);
			this.DisplayAvailableChores();
			ImGui.Text("");
		}
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x000ED33C File Offset: 0x000EB53C
	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref this.lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref this.showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = this.Consumer.GetLastPreconditionSnapshot();
		if (this.showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = this.Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		this.ShowChores(target_snapshot);
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000ED39C File Offset: 0x000EB59C
	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		this.rowIndex = 0;
		if (ImGui.BeginTable("Available Chores", this.columns.Count, flags))
		{
			foreach (object obj in this.columns.Keys)
			{
				ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
			}
			ImGui.TableHeadersRow();
			for (int i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
			{
				this.ShowContext(target_snapshot.succeededContexts[i]);
			}
			if (target_snapshot.doFailedContextsNeedSorting)
			{
				target_snapshot.failedContexts.Sort();
				target_snapshot.doFailedContextsNeedSorting = false;
			}
			for (int j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
			{
				this.ShowContext(target_snapshot.failedContexts[j]);
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000ED4A0 File Offset: 0x000EB6A0
	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].condition.id;
		}
		string text2 = "";
		if (chore.driver != null)
		{
			text2 = chore.driver.name;
		}
		string text3 = "";
		if (chore.overrideTarget != null)
		{
			text3 = chore.overrideTarget.name;
		}
		string text4 = "";
		if (!chore.isNull)
		{
			text4 = chore.gameObject.name;
		}
		if (Chore.Precondition.Context.ShouldFilter(this.filter, chore.GetType().ToString()) && Chore.Precondition.Context.ShouldFilter(this.filter, chore.choreType.Id) && Chore.Precondition.Context.ShouldFilter(this.filter, text) && Chore.Precondition.Context.ShouldFilter(this.filter, text2) && Chore.Precondition.Context.ShouldFilter(this.filter, text3) && Chore.Precondition.Context.ShouldFilter(this.filter, text4))
		{
			return;
		}
		this.columns["Id"] = chore.id.ToString();
		this.columns["Class"] = chore.GetType().ToString().Replace("`1", "");
		this.columns["Type"] = chore.choreType.Id;
		this.columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
		this.columns["PersonalPriority"] = context.personalPriority.ToString();
		this.columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
		this.columns["Priority"] = context.priority.ToString();
		this.columns["PriorityMod"] = context.priorityMod.ToString();
		this.columns["ConsumerPriority"] = context.consumerPriority.ToString();
		this.columns["Cost"] = context.cost.ToString();
		this.columns["Interrupt"] = context.interruptPriority.ToString();
		this.columns["Precondition"] = text;
		this.columns["Override"] = text3;
		this.columns["Assigned To"] = text2;
		this.columns["Owner"] = text4;
		this.columns["Details"] = "";
		ImGui.TableNextRow();
		string format = "ID_row_{0}";
		int num = this.rowIndex;
		this.rowIndex = num + 1;
		ImGui.PushID(string.Format(format, num));
		for (int i = 0; i < this.columns.Count; i++)
		{
			ImGui.TableSetColumnIndex(i);
			ImGui.Text(this.columns[i].ToString());
		}
		ImGui.PopID();
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x000ED7C3 File Offset: 0x000EB9C3
	public void ConsumerDebugDisplayLog()
	{
	}

	// Token: 0x0400187D RID: 6269
	private string filter = "";

	// Token: 0x0400187E RID: 6270
	private bool showLastSuccessfulPreconditionSnapshot;

	// Token: 0x0400187F RID: 6271
	private bool lockSelection;

	// Token: 0x04001880 RID: 6272
	private ChoreConsumer Consumer;

	// Token: 0x04001881 RID: 6273
	private GameObject selectedGameObject;

	// Token: 0x04001882 RID: 6274
	private OrderedDictionary columns = new OrderedDictionary
	{
		{
			"BP",
			""
		},
		{
			"Id",
			""
		},
		{
			"Class",
			""
		},
		{
			"Type",
			""
		},
		{
			"PriorityClass",
			""
		},
		{
			"PersonalPriority",
			""
		},
		{
			"PriorityValue",
			""
		},
		{
			"Priority",
			""
		},
		{
			"PriorityMod",
			""
		},
		{
			"ConsumerPriority",
			""
		},
		{
			"Cost",
			""
		},
		{
			"Interrupt",
			""
		},
		{
			"Precondition",
			""
		},
		{
			"Override",
			""
		},
		{
			"Assigned To",
			""
		},
		{
			"Owner",
			""
		},
		{
			"Details",
			""
		}
	};

	// Token: 0x04001883 RID: 6275
	private int rowIndex;

	// Token: 0x02001559 RID: 5465
	public class EditorPreconditionSnapshot
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060092F2 RID: 37618 RVA: 0x00374CEF File Offset: 0x00372EEF
		// (set) Token: 0x060092F3 RID: 37619 RVA: 0x00374CF7 File Offset: 0x00372EF7
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> SucceededContexts { get; set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060092F4 RID: 37620 RVA: 0x00374D00 File Offset: 0x00372F00
		// (set) Token: 0x060092F5 RID: 37621 RVA: 0x00374D08 File Offset: 0x00372F08
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> FailedContexts { get; set; }

		// Token: 0x020028B2 RID: 10418
		public struct EditorContext
		{
			// Token: 0x17000D66 RID: 3430
			// (get) Token: 0x0600CCEF RID: 52463 RVA: 0x00430E8E File Offset: 0x0042F08E
			// (set) Token: 0x0600CCF0 RID: 52464 RVA: 0x00430E96 File Offset: 0x0042F096
			public string Chore { readonly get; set; }

			// Token: 0x17000D67 RID: 3431
			// (get) Token: 0x0600CCF1 RID: 52465 RVA: 0x00430E9F File Offset: 0x0042F09F
			// (set) Token: 0x0600CCF2 RID: 52466 RVA: 0x00430EA7 File Offset: 0x0042F0A7
			public string ChoreType { readonly get; set; }

			// Token: 0x17000D68 RID: 3432
			// (get) Token: 0x0600CCF3 RID: 52467 RVA: 0x00430EB0 File Offset: 0x0042F0B0
			// (set) Token: 0x0600CCF4 RID: 52468 RVA: 0x00430EB8 File Offset: 0x0042F0B8
			public string FailedPrecondition { readonly get; set; }

			// Token: 0x17000D69 RID: 3433
			// (get) Token: 0x0600CCF5 RID: 52469 RVA: 0x00430EC1 File Offset: 0x0042F0C1
			// (set) Token: 0x0600CCF6 RID: 52470 RVA: 0x00430EC9 File Offset: 0x0042F0C9
			public int WorldId { readonly get; set; }
		}
	}
}
