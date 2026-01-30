using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F09 RID: 3849
	public sealed class LocStringDrawer : InlineDrawer
	{
		// Token: 0x06007BC4 RID: 31684 RVA: 0x00300BC9 File Offset: 0x002FEDC9
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<LocString>();
		}

		// Token: 0x06007BC5 RID: 31685 RVA: 0x00300BD1 File Offset: 0x002FEDD1
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, string.Format("{0}({1})", member.value, ((LocString)member.value).text));
		}
	}
}
