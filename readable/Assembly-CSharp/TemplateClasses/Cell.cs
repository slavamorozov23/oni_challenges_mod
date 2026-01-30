using System;

namespace TemplateClasses
{
	// Token: 0x02000EFF RID: 3839
	[Serializable]
	public class Cell
	{
		// Token: 0x06007B71 RID: 31601 RVA: 0x003001DD File Offset: 0x002FE3DD
		public Cell()
		{
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x003001E8 File Offset: 0x002FE3E8
		public Cell(int loc_x, int loc_y, SimHashes _element, float _temperature, float _mass, string _diseaseName, int _diseaseCount, bool _preventFoWReveal = false)
		{
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.element = _element;
			this.temperature = _temperature;
			this.mass = _mass;
			this.diseaseName = _diseaseName;
			this.diseaseCount = _diseaseCount;
			this.preventFoWReveal = _preventFoWReveal;
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06007B73 RID: 31603 RVA: 0x00300238 File Offset: 0x002FE438
		// (set) Token: 0x06007B74 RID: 31604 RVA: 0x00300240 File Offset: 0x002FE440
		public SimHashes element { get; set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06007B75 RID: 31605 RVA: 0x00300249 File Offset: 0x002FE449
		// (set) Token: 0x06007B76 RID: 31606 RVA: 0x00300251 File Offset: 0x002FE451
		public float mass { get; set; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06007B77 RID: 31607 RVA: 0x0030025A File Offset: 0x002FE45A
		// (set) Token: 0x06007B78 RID: 31608 RVA: 0x00300262 File Offset: 0x002FE462
		public float temperature { get; set; }

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06007B79 RID: 31609 RVA: 0x0030026B File Offset: 0x002FE46B
		// (set) Token: 0x06007B7A RID: 31610 RVA: 0x00300273 File Offset: 0x002FE473
		public string diseaseName { get; set; }

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06007B7B RID: 31611 RVA: 0x0030027C File Offset: 0x002FE47C
		// (set) Token: 0x06007B7C RID: 31612 RVA: 0x00300284 File Offset: 0x002FE484
		public int diseaseCount { get; set; }

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06007B7D RID: 31613 RVA: 0x0030028D File Offset: 0x002FE48D
		// (set) Token: 0x06007B7E RID: 31614 RVA: 0x00300295 File Offset: 0x002FE495
		public int location_x { get; set; }

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06007B7F RID: 31615 RVA: 0x0030029E File Offset: 0x002FE49E
		// (set) Token: 0x06007B80 RID: 31616 RVA: 0x003002A6 File Offset: 0x002FE4A6
		public int location_y { get; set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06007B81 RID: 31617 RVA: 0x003002AF File Offset: 0x002FE4AF
		// (set) Token: 0x06007B82 RID: 31618 RVA: 0x003002B7 File Offset: 0x002FE4B7
		public bool preventFoWReveal { get; set; }
	}
}
