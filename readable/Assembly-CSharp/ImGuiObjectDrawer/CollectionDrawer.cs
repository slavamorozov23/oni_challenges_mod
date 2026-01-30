using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F10 RID: 3856
	public abstract class CollectionDrawer : MemberDrawer
	{
		// Token: 0x06007BD9 RID: 31705
		public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

		// Token: 0x06007BDA RID: 31706 RVA: 0x00300E74 File Offset: 0x002FF074
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			if (this.IsEmpty(context, member))
			{
				return MemberDrawType.Inline;
			}
			return MemberDrawType.Custom;
		}

		// Token: 0x06007BDB RID: 31707 RVA: 0x00300E83 File Offset: 0x002FF083
		protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Debug.Assert(this.IsEmpty(context, member));
			this.DrawEmpty(context, member);
		}

		// Token: 0x06007BDC RID: 31708 RVA: 0x00300E9A File Offset: 0x002FF09A
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			Debug.Assert(!this.IsEmpty(context, member));
			this.DrawWithContents(context, member, depth);
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x00300EB5 File Offset: 0x002FF0B5
		private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			ImGui.Text(member.name + "(empty)");
		}

		// Token: 0x06007BDE RID: 31710 RVA: 0x00300ECC File Offset: 0x002FF0CC
		private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			CollectionDrawer.<>c__DisplayClass5_0 CS$<>8__locals1 = new CollectionDrawer.<>c__DisplayClass5_0();
			CS$<>8__locals1.depth = depth;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && CS$<>8__locals1.depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.VisitElements(new CollectionDrawer.ElementVisitor(CS$<>8__locals1.<DrawWithContents>g__Visitor|0), context, member);
				ImGui.TreePop();
			}
		}

		// Token: 0x06007BDF RID: 31711
		protected abstract void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);

		// Token: 0x02002185 RID: 8581
		// (Invoke) Token: 0x0600BC65 RID: 48229
		protected delegate void ElementVisitor(in MemberDrawContext context, CollectionDrawer.Element element);

		// Token: 0x02002186 RID: 8582
		protected struct Element
		{
			// Token: 0x0600BC68 RID: 48232 RVA: 0x00400010 File Offset: 0x003FE210
			public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this.node_name = node_name;
				this.draw_tooltip = draw_tooltip;
				this.get_object_to_inspect = get_object_to_inspect;
			}

			// Token: 0x0600BC69 RID: 48233 RVA: 0x00400027 File Offset: 0x003FE227
			public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this = new CollectionDrawer.Element(string.Format("[{0}]", index), draw_tooltip, get_object_to_inspect);
			}

			// Token: 0x04009988 RID: 39304
			public readonly string node_name;

			// Token: 0x04009989 RID: 39305
			public readonly System.Action draw_tooltip;

			// Token: 0x0400998A RID: 39306
			public readonly Func<object> get_object_to_inspect;
		}
	}
}
