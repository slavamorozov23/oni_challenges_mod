using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x02000FBB RID: 4027
	public class Local : IDistributionPlatform
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06007E62 RID: 32354 RVA: 0x00326DE3 File Offset: 0x00324FE3
		// (set) Token: 0x06007E63 RID: 32355 RVA: 0x00326DEB File Offset: 0x00324FEB
		public string folder { get; private set; }

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06007E64 RID: 32356 RVA: 0x00326DF4 File Offset: 0x00324FF4
		// (set) Token: 0x06007E65 RID: 32357 RVA: 0x00326DFC File Offset: 0x00324FFC
		public Label.DistributionPlatform distribution_platform { get; private set; }

		// Token: 0x06007E66 RID: 32358 RVA: 0x00326E05 File Offset: 0x00325005
		public string GetDirectory()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.folder));
		}

		// Token: 0x06007E67 RID: 32359 RVA: 0x00326E1C File Offset: 0x0032501C
		private void Subscribe(string directoryName, long timestamp, IFileSource file_source, bool isDevMod)
		{
			Label label = new Label
			{
				id = directoryName,
				distribution_platform = this.distribution_platform,
				version = (long)directoryName.GetHashCode(),
				title = directoryName
			};
			KModHeader header = KModUtil.GetHeader(file_source, label.defaultStaticID, directoryName, directoryName, isDevMod);
			label.title = header.title;
			Mod mod = new Mod(label, header.staticID, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, delegate()
			{
				App.OpenWebURL("file://" + file_source.GetRoot());
			});
			if (file_source.GetType() == typeof(Directory))
			{
				mod.status = Mod.Status.Installed;
			}
			Global.Instance.modManager.Subscribe(mod, this);
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x00326EF0 File Offset: 0x003250F0
		public Local(string folder, Label.DistributionPlatform distribution_platform, bool isDevFolder)
		{
			this.folder = folder;
			this.distribution_platform = distribution_platform;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				this.Subscribe(name, directoryInfo2.LastWriteTime.ToFileTime(), new Directory(directoryInfo2.FullName), isDevFolder);
			}
		}
	}
}
