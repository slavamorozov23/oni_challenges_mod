using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F07 RID: 3847
	public class SimpleDrawer : InlineDrawer
	{
		// Token: 0x06007BBD RID: 31677 RVA: 0x00300B81 File Offset: 0x002FED81
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x00300B84 File Offset: 0x002FED84
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsPrimitive || member.CanAssignToType<string>();
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x00300B9B File Offset: 0x002FED9B
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
