using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0E RID: 3854
	public sealed class Vector3Drawer : InlineDrawer
	{
		// Token: 0x06007BD3 RID: 31699 RVA: 0x00300D88 File Offset: 0x002FEF88
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector3;
		}

		// Token: 0x06007BD4 RID: 31700 RVA: 0x00300D98 File Offset: 0x002FEF98
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector3 vector = (Vector3)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2} )", vector.x, vector.y, vector.z));
		}
	}
}
