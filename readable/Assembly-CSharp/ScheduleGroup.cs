using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02000680 RID: 1664
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060028E4 RID: 10468 RVA: 0x000E9DC8 File Offset: 0x000E7FC8
	// (set) Token: 0x060028E5 RID: 10469 RVA: 0x000E9DD0 File Offset: 0x000E7FD0
	public int defaultSegments { get; private set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000E9DD9 File Offset: 0x000E7FD9
	// (set) Token: 0x060028E7 RID: 10471 RVA: 0x000E9DE1 File Offset: 0x000E7FE1
	public string description { get; private set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060028E8 RID: 10472 RVA: 0x000E9DEA File Offset: 0x000E7FEA
	// (set) Token: 0x060028E9 RID: 10473 RVA: 0x000E9DF2 File Offset: 0x000E7FF2
	public string notificationTooltip { get; private set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060028EA RID: 10474 RVA: 0x000E9DFB File Offset: 0x000E7FFB
	// (set) Token: 0x060028EB RID: 10475 RVA: 0x000E9E03 File Offset: 0x000E8003
	public List<ScheduleBlockType> allowedTypes { get; private set; }

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060028EC RID: 10476 RVA: 0x000E9E0C File Offset: 0x000E800C
	// (set) Token: 0x060028ED RID: 10477 RVA: 0x000E9E14 File Offset: 0x000E8014
	public bool alarm { get; private set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060028EE RID: 10478 RVA: 0x000E9E1D File Offset: 0x000E801D
	// (set) Token: 0x060028EF RID: 10479 RVA: 0x000E9E25 File Offset: 0x000E8025
	public Color uiColor { get; private set; }

	// Token: 0x060028F0 RID: 10480 RVA: 0x000E9E2E File Offset: 0x000E802E
	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
		this.uiColor = uiColor;
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000E9E69 File Offset: 0x000E8069
	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000E9E77 File Offset: 0x000E8077
	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
