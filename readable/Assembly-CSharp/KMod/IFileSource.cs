using System;
using System.Collections.Generic;
using Klei;

namespace KMod
{
	// Token: 0x02000FBE RID: 4030
	public interface IFileSource
	{
		// Token: 0x06007E6C RID: 32364
		string GetRoot();

		// Token: 0x06007E6D RID: 32365
		bool Exists();

		// Token: 0x06007E6E RID: 32366
		bool Exists(string relative_path);

		// Token: 0x06007E6F RID: 32367
		void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

		// Token: 0x06007E70 RID: 32368
		IFileDirectory GetFileSystem();

		// Token: 0x06007E71 RID: 32369
		void CopyTo(string path, List<string> extensions = null);

		// Token: 0x06007E72 RID: 32370
		string Read(string relative_path);

		// Token: 0x06007E73 RID: 32371
		void Dispose();
	}
}
