using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x02000FBA RID: 4026
	public class KModUtil
	{
		// Token: 0x06007E60 RID: 32352 RVA: 0x00326D1C File Offset: 0x00324F1C
		public static KModHeader GetHeader(IFileSource file_source, string defaultStaticID, string defaultTitle, string defaultDescription, bool devMod)
		{
			string text = "mod.yaml";
			string text2 = file_source.Read(text);
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !devMod);
			};
			KModHeader kmodHeader = (!string.IsNullOrEmpty(text2)) ? YamlIO.Parse<KModHeader>(text2, new FileHandle
			{
				full_path = Path.Combine(file_source.GetRoot(), text)
			}, handle_error, null) : null;
			if (kmodHeader == null)
			{
				kmodHeader = new KModHeader
				{
					title = defaultTitle,
					description = defaultDescription,
					staticID = defaultStaticID
				};
			}
			if (string.IsNullOrEmpty(kmodHeader.staticID))
			{
				kmodHeader.staticID = defaultStaticID;
			}
			if (kmodHeader.title == null)
			{
				kmodHeader.title = defaultTitle;
			}
			if (kmodHeader.description == null)
			{
				kmodHeader.description = UI.FRONTEND.MODS.NO_DESCRIPTION;
			}
			return kmodHeader;
		}
	}
}
