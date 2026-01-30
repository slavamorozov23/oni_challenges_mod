using System;
using System.Collections.Generic;

namespace ImGuiObjectDrawer
{
	// Token: 0x02000F04 RID: 3844
	public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06007BB3 RID: 31667 RVA: 0x00300AA0 File Offset: 0x002FECA0
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06007BB4 RID: 31668 RVA: 0x00300AA4 File Offset: 0x002FECA4
		public void AppendDrawersTo(List<MemberDrawer> drawers)
		{
			drawers.AddRange(new MemberDrawer[]
			{
				new NullDrawer(),
				new SimpleDrawer(),
				new LocStringDrawer(),
				new EnumDrawer(),
				new HashedStringDrawer(),
				new KAnimHashedStringDrawer(),
				new Vector2Drawer(),
				new Vector3Drawer(),
				new Vector4Drawer(),
				new UnityObjectDrawer(),
				new ArrayDrawer(),
				new IDictionaryDrawer(),
				new IEnumerableDrawer(),
				new PlainCSharpObjectDrawer(),
				new FallbackDrawer()
			});
		}
	}
}
