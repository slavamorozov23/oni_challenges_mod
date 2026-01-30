using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000FBF RID: 4031
	internal struct Directory : IFileSource
	{
		// Token: 0x06007E74 RID: 32372 RVA: 0x003272D8 File Offset: 0x003254D8
		public Directory(string root)
		{
			this.root = root;
			this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
		}

		// Token: 0x06007E75 RID: 32373 RVA: 0x003272F4 File Offset: 0x003254F4
		public string GetRoot()
		{
			return this.root;
		}

		// Token: 0x06007E76 RID: 32374 RVA: 0x003272FC File Offset: 0x003254FC
		public bool Exists()
		{
			return Directory.Exists(this.GetRoot());
		}

		// Token: 0x06007E77 RID: 32375 RVA: 0x00327309 File Offset: 0x00325509
		public bool Exists(string relative_path)
		{
			return this.Exists() && new DirectoryInfo(FileSystem.Normalize(Path.Combine(this.root, relative_path))).Exists;
		}

		// Token: 0x06007E78 RID: 32376 RVA: 0x00327330 File Offset: 0x00325530
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			relative_root = (relative_root ?? "");
			string text = FileSystem.Normalize(Path.Combine(this.root, relative_root));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				global::Debug.LogError("Cannot iterate over $" + text + ", this directory does not exist");
				return;
			}
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
			{
				file_system_items.Add(new FileSystemItem
				{
					name = fileSystemInfo.Name,
					type = ((fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
				});
			}
		}

		// Token: 0x06007E79 RID: 32377 RVA: 0x003273CC File Offset: 0x003255CC
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06007E7A RID: 32378 RVA: 0x003273D4 File Offset: 0x003255D4
		public void CopyTo(string path, List<string> extensions = null)
		{
			try
			{
				Directory.CopyDirectory(this.root, path, extensions);
			}
			catch (UnauthorizedAccessException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, null, null);
			}
			catch (IOException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, null, null);
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06007E7B RID: 32379 RVA: 0x00327434 File Offset: 0x00325634
		public string Read(string relative_path)
		{
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x003274B0 File Offset: 0x003256B0
		private static int CopyDirectory(string sourceDirName, string destDirName, List<string> extensions)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return 0;
			}
			if (!FileUtil.CreateDirectory(destDirName, 0))
			{
				return 0;
			}
			FileInfo[] files = directoryInfo.GetFiles();
			int num = 0;
			foreach (FileInfo fileInfo in files)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					using (List<string>.Enumerator enumerator = extensions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == Path.GetExtension(fileInfo.Name).ToLower())
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					string destFileName = Path.Combine(destDirName, fileInfo.Name);
					fileInfo.CopyTo(destFileName, false);
					num++;
				}
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				num += Directory.CopyDirectory(directoryInfo2.FullName, destDirName2, extensions);
			}
			if (num == 0)
			{
				FileUtil.DeleteDirectory(destDirName, 0);
			}
			return num;
		}

		// Token: 0x06007E7D RID: 32381 RVA: 0x003275D4 File Offset: 0x003257D4
		public void Dispose()
		{
		}

		// Token: 0x04005D78 RID: 23928
		private AliasDirectory file_system;

		// Token: 0x04005D79 RID: 23929
		private string root;
	}
}
