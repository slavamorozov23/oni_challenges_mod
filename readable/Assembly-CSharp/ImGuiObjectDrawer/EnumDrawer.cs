using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0A RID: 3850
	public sealed class EnumDrawer : InlineDrawer
	{
		// Token: 0x06007BC7 RID: 31687 RVA: 0x00300C06 File Offset: 0x002FEE06
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsEnum;
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x00300C13 File Offset: 0x002FEE13
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
