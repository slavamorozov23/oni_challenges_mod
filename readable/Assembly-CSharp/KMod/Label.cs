using System;
using System.Diagnostics;
using System.IO;
using Klei;
using Newtonsoft.Json;

namespace KMod
{
	// Token: 0x02000FC1 RID: 4033
	[JsonObject(MemberSerialization.Fields)]
	[DebuggerDisplay("{title}")]
	public struct Label
	{
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06007E88 RID: 32392 RVA: 0x003279F5 File Offset: 0x00325BF5
		[JsonIgnore]
		private string distribution_platform_name
		{
			get
			{
				return this.distribution_platform.ToString();
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06007E89 RID: 32393 RVA: 0x00327A08 File Offset: 0x00325C08
		[JsonIgnore]
		public string install_path
		{
			get
			{
				return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name, this.id));
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06007E8A RID: 32394 RVA: 0x00327A25 File Offset: 0x00325C25
		[JsonIgnore]
		public string defaultStaticID
		{
			get
			{
				return this.id + "." + this.distribution_platform.ToString();
			}
		}

		// Token: 0x06007E8B RID: 32395 RVA: 0x00327A48 File Offset: 0x00325C48
		public override string ToString()
		{
			return this.title;
		}

		// Token: 0x06007E8C RID: 32396 RVA: 0x00327A50 File Offset: 0x00325C50
		public bool Match(Label rhs)
		{
			return this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;
		}

		// Token: 0x04005D7D RID: 23933
		public Label.DistributionPlatform distribution_platform;

		// Token: 0x04005D7E RID: 23934
		public string id;

		// Token: 0x04005D7F RID: 23935
		public string title;

		// Token: 0x04005D80 RID: 23936
		public long version;

		// Token: 0x020021C4 RID: 8644
		public enum DistributionPlatform
		{
			// Token: 0x04009B56 RID: 39766
			Local,
			// Token: 0x04009B57 RID: 39767
			Steam,
			// Token: 0x04009B58 RID: 39768
			Epic,
			// Token: 0x04009B59 RID: 39769
			Rail,
			// Token: 0x04009B5A RID: 39770
			Dev
		}
	}
}
