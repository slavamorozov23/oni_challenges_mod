using System;

// Token: 0x02000E96 RID: 3734
public class HatListable : IListableOption
{
	// Token: 0x06007755 RID: 30549 RVA: 0x002DA8D8 File Offset: 0x002D8AD8
	public HatListable(string name, string hat)
	{
		this.name = name;
		this.hat = hat;
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06007756 RID: 30550 RVA: 0x002DA8EE File Offset: 0x002D8AEE
	// (set) Token: 0x06007757 RID: 30551 RVA: 0x002DA8F6 File Offset: 0x002D8AF6
	public string name { get; private set; }

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x06007758 RID: 30552 RVA: 0x002DA8FF File Offset: 0x002D8AFF
	// (set) Token: 0x06007759 RID: 30553 RVA: 0x002DA907 File Offset: 0x002D8B07
	public string hat { get; private set; }

	// Token: 0x0600775A RID: 30554 RVA: 0x002DA910 File Offset: 0x002D8B10
	public string GetProperName()
	{
		return this.name;
	}
}
