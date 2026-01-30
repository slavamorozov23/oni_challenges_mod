using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0D RID: 3853
	public sealed class Vector2Drawer : InlineDrawer
	{
		// Token: 0x06007BD0 RID: 31696 RVA: 0x00300D2A File Offset: 0x002FEF2A
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector2;
		}

		// Token: 0x06007BD1 RID: 31697 RVA: 0x00300D3C File Offset: 0x002FEF3C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector2 vector = (Vector2)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1} )", vector.x, vector.y));
		}
	}
}
