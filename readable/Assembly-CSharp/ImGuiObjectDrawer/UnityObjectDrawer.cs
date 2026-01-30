using System;
using ImGuiNET;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F14 RID: 3860
	public class UnityObjectDrawer : PlainCSharpObjectDrawer
	{
		// Token: 0x06007BED RID: 31725 RVA: 0x0030117C File Offset: 0x002FF37C
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is UnityEngine.Object;
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x0030118C File Offset: 0x002FF38C
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			UnityEngine.Object @object = (UnityEngine.Object)member.value;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				base.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}
	}
}
