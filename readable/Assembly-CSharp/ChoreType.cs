using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200067B RID: 1659
[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060028AD RID: 10413 RVA: 0x000E97A8 File Offset: 0x000E79A8
	// (set) Token: 0x060028AE RID: 10414 RVA: 0x000E97B0 File Offset: 0x000E79B0
	public Urge urge { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060028AF RID: 10415 RVA: 0x000E97B9 File Offset: 0x000E79B9
	// (set) Token: 0x060028B0 RID: 10416 RVA: 0x000E97C1 File Offset: 0x000E79C1
	public ChoreGroup[] groups { get; private set; }

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060028B1 RID: 10417 RVA: 0x000E97CA File Offset: 0x000E79CA
	// (set) Token: 0x060028B2 RID: 10418 RVA: 0x000E97D2 File Offset: 0x000E79D2
	public int priority { get; private set; }

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060028B3 RID: 10419 RVA: 0x000E97DB File Offset: 0x000E79DB
	// (set) Token: 0x060028B4 RID: 10420 RVA: 0x000E97E3 File Offset: 0x000E79E3
	public int interruptPriority { get; set; }

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000E97EC File Offset: 0x000E79EC
	// (set) Token: 0x060028B6 RID: 10422 RVA: 0x000E97F4 File Offset: 0x000E79F4
	public int explicitPriority { get; private set; }

	// Token: 0x060028B7 RID: 10423 RVA: 0x000E97FD File Offset: 0x000E79FD
	private string ResolveStringCallback(string str, object data)
	{
		return ((Chore)data).ResolveString(str);
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x000E980C File Offset: 0x000E7A0C
	public ChoreType(string id, ResourceSet parent, string[] chore_groups, string urge, string name, string status_message, string tooltip, IEnumerable<Tag> interrupt_exclusion, int implicit_priority, int explicit_priority) : base(id, parent, name)
	{
		this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
		this.tags.Add(TagManager.Create(id));
		this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
		Db.Get().DuplicantStatusItems.Add(this.statusItem);
		List<ChoreGroup> list = new List<ChoreGroup>();
		for (int i = 0; i < chore_groups.Length; i++)
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.TryGet(chore_groups[i]);
			if (choreGroup != null)
			{
				if (!choreGroup.choreTypes.Contains(this))
				{
					choreGroup.choreTypes.Add(this);
				}
				list.Add(choreGroup);
			}
		}
		this.groups = list.ToArray();
		if (!string.IsNullOrEmpty(urge))
		{
			this.urge = Db.Get().Urges.Get(urge);
		}
		this.priority = implicit_priority;
		this.explicitPriority = explicit_priority;
	}

	// Token: 0x04001807 RID: 6151
	public StatusItem statusItem;

	// Token: 0x0400180C RID: 6156
	public HashSet<Tag> tags = new HashSet<Tag>();

	// Token: 0x0400180D RID: 6157
	public HashSet<Tag> interruptExclusion;

	// Token: 0x0400180F RID: 6159
	public string reportName;
}
