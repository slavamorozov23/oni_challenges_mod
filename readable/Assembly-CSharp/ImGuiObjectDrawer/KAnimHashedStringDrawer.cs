using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F0C RID: 3852
	public sealed class KAnimHashedStringDrawer : InlineDrawer
	{
		// Token: 0x06007BCD RID: 31693 RVA: 0x00300CAE File Offset: 0x002FEEAE
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is KAnimHashedString;
		}

		// Token: 0x06007BCE RID: 31694 RVA: 0x00300CC0 File Offset: 0x002FEEC0
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			KAnimHashedString kanimHashedString = (KAnimHashedString)member.value;
			string str = kanimHashedString.ToString();
			string str2 = "0x" + kanimHashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
