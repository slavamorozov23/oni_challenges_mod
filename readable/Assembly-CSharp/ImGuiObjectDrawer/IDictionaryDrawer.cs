using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F12 RID: 3858
	public sealed class IDictionaryDrawer : CollectionDrawer
	{
		// Token: 0x06007BE5 RID: 31717 RVA: 0x00301012 File Offset: 0x002FF212
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IDictionary>();
		}

		// Token: 0x06007BE6 RID: 31718 RVA: 0x0030101A File Offset: 0x002FF21A
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((IDictionary)member.value).Count == 0;
		}

		// Token: 0x06007BE7 RID: 31719 RVA: 0x00301030 File Offset: 0x002FF230
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IDictionary dictionary = (IDictionary)member.value;
			int num = 0;
			using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry kvp = (DictionaryEntry)enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(string.Format("{0} -> {1}", kvp.Key.GetType(), kvp.Value.GetType()));
					}, () => new
					{
						key = kvp.Key,
						value = kvp.Value
					}));
					num++;
				}
			}
		}
	}
}
