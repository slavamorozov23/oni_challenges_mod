using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0F RID: 3855
	public sealed class Vector4Drawer : InlineDrawer
	{
		// Token: 0x06007BD6 RID: 31702 RVA: 0x00300DEF File Offset: 0x002FEFEF
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector4;
		}

		// Token: 0x06007BD7 RID: 31703 RVA: 0x00300E00 File Offset: 0x002FF000
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector4 vector = (Vector4)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2}, {3} )", new object[]
			{
				vector.x,
				vector.y,
				vector.z,
				vector.w
			}));
		}
	}
}
