using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F15 RID: 3861
	public class PlainCSharpObjectDrawer : MemberDrawer
	{
		// Token: 0x06007BF0 RID: 31728 RVA: 0x003011E7 File Offset: 0x002FF3E7
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x003011EA File Offset: 0x002FF3EA
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Custom;
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x003011ED File Offset: 0x002FF3ED
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x003011F4 File Offset: 0x002FF3F4
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x0030123B File Offset: 0x002FF43B
		protected virtual void DrawContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			DrawerUtil.DrawObjectContents(member.value, context, depth + 1);
		}
	}
}
