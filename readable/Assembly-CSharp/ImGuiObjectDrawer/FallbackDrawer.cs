using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F08 RID: 3848
	public sealed class FallbackDrawer : SimpleDrawer
	{
		// Token: 0x06007BC1 RID: 31681 RVA: 0x00300BBB File Offset: 0x002FEDBB
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x00300BBE File Offset: 0x002FEDBE
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}
	}
}
