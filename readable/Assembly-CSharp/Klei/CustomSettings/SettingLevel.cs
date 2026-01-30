using System;

namespace Klei.CustomSettings
{
	// Token: 0x02001012 RID: 4114
	public class SettingLevel
	{
		// Token: 0x06007F9F RID: 32671 RVA: 0x00336424 File Offset: 0x00334624
		public SettingLevel(string id, string label, string tooltip, long coordinate_value = 0L, object userdata = null)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.userdata = userdata;
			this.coordinate_value = coordinate_value;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06007FA0 RID: 32672 RVA: 0x00336451 File Offset: 0x00334651
		// (set) Token: 0x06007FA1 RID: 32673 RVA: 0x00336459 File Offset: 0x00334659
		public string id { get; private set; }

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06007FA2 RID: 32674 RVA: 0x00336462 File Offset: 0x00334662
		// (set) Token: 0x06007FA3 RID: 32675 RVA: 0x0033646A File Offset: 0x0033466A
		public string tooltip { get; private set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x00336473 File Offset: 0x00334673
		// (set) Token: 0x06007FA5 RID: 32677 RVA: 0x0033647B File Offset: 0x0033467B
		public string label { get; private set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06007FA6 RID: 32678 RVA: 0x00336484 File Offset: 0x00334684
		// (set) Token: 0x06007FA7 RID: 32679 RVA: 0x0033648C File Offset: 0x0033468C
		public object userdata { get; private set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06007FA8 RID: 32680 RVA: 0x00336495 File Offset: 0x00334695
		// (set) Token: 0x06007FA9 RID: 32681 RVA: 0x0033649D File Offset: 0x0033469D
		public long coordinate_value { get; private set; }
	}
}
