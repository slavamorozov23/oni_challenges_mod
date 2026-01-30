using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F05 RID: 3845
	public abstract class InlineDrawer : MemberDrawer
	{
		// Token: 0x06007BB6 RID: 31670 RVA: 0x00300B44 File Offset: 0x002FED44
		public sealed override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Inline;
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x00300B47 File Offset: 0x002FED47
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			this.DrawInline(context, member);
		}
	}
}
