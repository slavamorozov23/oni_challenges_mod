using System;

// Token: 0x02000947 RID: 2375
public class Expectation
{
	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x0600423F RID: 16959 RVA: 0x00175CB5 File Offset: 0x00173EB5
	// (set) Token: 0x06004240 RID: 16960 RVA: 0x00175CBD File Offset: 0x00173EBD
	public string id { get; protected set; }

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06004241 RID: 16961 RVA: 0x00175CC6 File Offset: 0x00173EC6
	// (set) Token: 0x06004242 RID: 16962 RVA: 0x00175CCE File Offset: 0x00173ECE
	public string name { get; protected set; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06004243 RID: 16963 RVA: 0x00175CD7 File Offset: 0x00173ED7
	// (set) Token: 0x06004244 RID: 16964 RVA: 0x00175CDF File Offset: 0x00173EDF
	public string description { get; protected set; }

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06004245 RID: 16965 RVA: 0x00175CE8 File Offset: 0x00173EE8
	// (set) Token: 0x06004246 RID: 16966 RVA: 0x00175CF0 File Offset: 0x00173EF0
	public Action<MinionResume> OnApply { get; protected set; }

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06004247 RID: 16967 RVA: 0x00175CF9 File Offset: 0x00173EF9
	// (set) Token: 0x06004248 RID: 16968 RVA: 0x00175D01 File Offset: 0x00173F01
	public Action<MinionResume> OnRemove { get; protected set; }

	// Token: 0x06004249 RID: 16969 RVA: 0x00175D0A File Offset: 0x00173F0A
	public Expectation(string id, string name, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.OnApply = OnApply;
		this.OnRemove = OnRemove;
	}
}
