using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F06 RID: 3846
	public class NullDrawer : InlineDrawer
	{
		// Token: 0x06007BB9 RID: 31673 RVA: 0x00300B59 File Offset: 0x002FED59
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x06007BBA RID: 31674 RVA: 0x00300B5C File Offset: 0x002FED5C
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value == null;
		}

		// Token: 0x06007BBB RID: 31675 RVA: 0x00300B67 File Offset: 0x002FED67
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, "null");
		}
	}
}
