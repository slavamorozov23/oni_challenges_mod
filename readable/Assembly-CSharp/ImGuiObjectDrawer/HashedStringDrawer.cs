using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0B RID: 3851
	public sealed class HashedStringDrawer : InlineDrawer
	{
		// Token: 0x06007BCA RID: 31690 RVA: 0x00300C33 File Offset: 0x002FEE33
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is HashedString;
		}

		// Token: 0x06007BCB RID: 31691 RVA: 0x00300C44 File Offset: 0x002FEE44
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			HashedString hashedString = (HashedString)member.value;
			string str = hashedString.ToString();
			string str2 = "0x" + hashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
