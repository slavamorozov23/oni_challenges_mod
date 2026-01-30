using System;

namespace KMod
{
	// Token: 0x02000FBD RID: 4029
	public struct FileSystemItem
	{
		// Token: 0x04005D76 RID: 23926
		public string name;

		// Token: 0x04005D77 RID: 23927
		public FileSystemItem.ItemType type;

		// Token: 0x020021C2 RID: 8642
		public enum ItemType
		{
			// Token: 0x04009B51 RID: 39761
			Directory,
			// Token: 0x04009B52 RID: 39762
			File
		}
	}
}
