using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000FC0 RID: 4032
	internal struct ZipFile : IFileSource
	{
		// Token: 0x06007E7E RID: 32382 RVA: 0x003275D6 File Offset: 0x003257D6
		public ZipFile(string filename)
		{
			this.filename = filename;
			this.zipfile = ZipFile.Read(filename);
			this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath, true);
		}

		// Token: 0x06007E7F RID: 32383 RVA: 0x0032760D File Offset: 0x0032580D
		public string GetRoot()
		{
			return this.filename;
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x00327615 File Offset: 0x00325815
		public bool Exists()
		{
			return File.Exists(this.GetRoot());
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x00327624 File Offset: 0x00325824
		public bool Exists(string relative_path)
		{
			if (!this.Exists())
			{
				return false;
			}
			using (IEnumerator<ZipEntry> enumerator = this.zipfile.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (FileSystem.Normalize(enumerator.Current.FileName).StartsWith(relative_path))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007E82 RID: 32386 RVA: 0x0032768C File Offset: 0x0032588C
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
			string[] array;
			if (!string.IsNullOrEmpty(relative_root))
			{
				relative_root = (relative_root ?? "");
				relative_root = FileSystem.Normalize(relative_root);
				array = relative_root.Split('/', StringSplitOptions.None);
			}
			else
			{
				array = new string[0];
			}
			foreach (ZipEntry zipEntry in this.zipfile)
			{
				List<string> list = (from part in FileSystem.Normalize(zipEntry.FileName).Split('/', StringSplitOptions.None)
				where !string.IsNullOrEmpty(part)
				select part).ToList<string>();
				if (this.IsSharedRoot(array, list))
				{
					list = list.GetRange(array.Length, list.Count - array.Length);
					if (list.Count != 0)
					{
						string text = list[0];
						if (pooledHashSet.Add(text))
						{
							file_system_items.Add(new FileSystemItem
							{
								name = text,
								type = ((1 < list.Count) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
							});
						}
					}
				}
			}
			pooledHashSet.Recycle();
		}

		// Token: 0x06007E83 RID: 32387 RVA: 0x003277B4 File Offset: 0x003259B4
		private bool IsSharedRoot(string[] root_path, List<string> check_path)
		{
			for (int i = 0; i < root_path.Length; i++)
			{
				if (i >= check_path.Count || root_path[i] != check_path[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007E84 RID: 32388 RVA: 0x003277EC File Offset: 0x003259EC
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x06007E85 RID: 32389 RVA: 0x003277F4 File Offset: 0x003259F4
		public void CopyTo(string path, List<string> extensions = null)
		{
			foreach (ZipEntry zipEntry in this.zipfile.Entries)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					foreach (string value in extensions)
					{
						if (zipEntry.FileName.ToLower().EndsWith(value))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					string path2 = FileSystem.Normalize(Path.Combine(path, zipEntry.FileName));
					string directoryName = Path.GetDirectoryName(path2);
					if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
					{
						using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
						{
							zipEntry.Extract(memoryStream);
							using (FileStream fileStream = FileUtil.Create(path2, 0))
							{
								fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007E86 RID: 32390 RVA: 0x00327944 File Offset: 0x00325B44
		public string Read(string relative_path)
		{
			ICollection<ZipEntry> collection = this.zipfile.SelectEntries(relative_path);
			if (collection.Count == 0)
			{
				return string.Empty;
			}
			foreach (ZipEntry zipEntry in collection)
			{
				using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
				{
					zipEntry.Extract(memoryStream);
					return Encoding.UTF8.GetString(memoryStream.GetBuffer());
				}
			}
			return string.Empty;
		}

		// Token: 0x06007E87 RID: 32391 RVA: 0x003279E8 File Offset: 0x00325BE8
		public void Dispose()
		{
			this.zipfile.Dispose();
		}

		// Token: 0x04005D7A RID: 23930
		private string filename;

		// Token: 0x04005D7B RID: 23931
		private ZipFile zipfile;

		// Token: 0x04005D7C RID: 23932
		private ZipFileDirectory file_system;
	}
}
