using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F13 RID: 3859
	public sealed class IEnumerableDrawer : CollectionDrawer
	{
		// Token: 0x06007BE9 RID: 31721 RVA: 0x003010C8 File Offset: 0x002FF2C8
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IEnumerable>();
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x003010D0 File Offset: 0x002FF2D0
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return !((IEnumerable)member.value).GetEnumerator().MoveNext();
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x003010EC File Offset: 0x002FF2EC
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IEnumerable enumerable = (IEnumerable)member.value;
			int num = 0;
			using (IEnumerator enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object el = enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(el.GetType());
					}, () => new
					{
						value = el
					}));
					num++;
				}
			}
		}
	}
}
