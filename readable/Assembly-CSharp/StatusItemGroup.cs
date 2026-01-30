using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000640 RID: 1600
public class StatusItemGroup
{
	// Token: 0x06002652 RID: 9810 RVA: 0x000DC83C File Offset: 0x000DAA3C
	public IEnumerator<StatusItemGroup.Entry> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06002653 RID: 9811 RVA: 0x000DC84E File Offset: 0x000DAA4E
	// (set) Token: 0x06002654 RID: 9812 RVA: 0x000DC856 File Offset: 0x000DAA56
	public GameObject gameObject { get; private set; }

	// Token: 0x06002655 RID: 9813 RVA: 0x000DC85F File Offset: 0x000DAA5F
	public StatusItemGroup(GameObject go)
	{
		this.gameObject = go;
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x000DC893 File Offset: 0x000DAA93
	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
		Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x000DC8B4 File Offset: 0x000DAAB4
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				return this.items[i];
			}
		}
		return StatusItemGroup.Entry.EmptyEntry;
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x000DC900 File Offset: 0x000DAB00
	public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
	{
		if (item != null && item.allowMultiples)
		{
			throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
		}
		if (category == null)
		{
			throw new ArgumentException("SetStatusItem requires a category.");
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				if (this.items[i].item == item)
				{
					this.Log("Set (exists in category)", item, this.items[i].id, category);
					return this.items[i].id;
				}
				this.Log("Set->Remove existing in category", item, this.items[i].id, category);
				this.RemoveStatusItem(this.items[i].id, false);
			}
		}
		if (item != null)
		{
			Guid guid = this.AddStatusItem(item, data, category);
			this.Log("Set (new)", item, guid, category);
			return guid;
		}
		this.Log("Set (failed)", item, Guid.Empty, category);
		return Guid.Empty;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x000DCA1B File Offset: 0x000DAC1B
	public void SetStatusItem(Guid guid, StatusItemCategory category, StatusItem new_item, object data = null)
	{
		this.RemoveStatusItem(guid, false);
		if (new_item != null)
		{
			this.AddStatusItem(new_item, data, category);
		}
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x000DCA34 File Offset: 0x000DAC34
	public bool HasStatusItem(StatusItem status_item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000DCA80 File Offset: 0x000DAC80
	public bool HasStatusItemID(string status_item_id)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item_id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x000DCAC4 File Offset: 0x000DACC4
	public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
	{
		if (this.gameObject == null || (!item.allowMultiples && this.HasStatusItem(item)))
		{
			return Guid.Empty;
		}
		if (!item.allowMultiples)
		{
			using (List<StatusItemGroup.Entry>.Enumerator enumerator = this.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item.Id == item.Id)
					{
						throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
					}
				}
			}
		}
		StatusItemGroup.Entry entry = new StatusItemGroup.Entry(item, category, data);
		if (item.shouldNotify)
		{
			entry.notification = new Notification(item.notificationText, item.notificationType, StatusItemGroup.OnToolTip, item, false, 0f, item.notificationClickCallback, data, null, true, false, false);
			this.gameObject.AddOrGet<Notifier>().Add(entry.notification, "");
		}
		if (item.ShouldShowIcon())
		{
			Game.Instance.AddStatusItem(this.gameObject.transform, item);
			Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
		}
		this.items.Add(entry);
		if (this.OnAddStatusItem != null)
		{
			this.OnAddStatusItem(entry, category);
		}
		return entry.id;
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x000DCC2C File Offset: 0x000DAE2C
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (status_item.allowMultiples)
		{
			throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
		}
		int i = 0;
		while (i < this.items.Count)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				Guid id = this.items[i].id;
				if (id == Guid.Empty)
				{
					return id;
				}
				this.RemoveStatusItemInternal(id, i, immediate);
				return id;
			}
			else
			{
				i++;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000DCCC4 File Offset: 0x000DAEC4
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (guid == Guid.Empty)
		{
			return guid;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == guid)
			{
				this.RemoveStatusItemInternal(guid, i, immediate);
				return guid;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000DCD20 File Offset: 0x000DAF20
	private void RemoveStatusItemInternal(Guid guid, int itemIdx, bool immediate)
	{
		StatusItemGroup.Entry entry = this.items[itemIdx];
		this.items.RemoveAt(itemIdx);
		if (entry.notification != null)
		{
			this.gameObject.GetComponent<Notifier>().Remove(entry.notification);
		}
		if (entry.item.ShouldShowIcon() && Game.Instance != null)
		{
			Game.Instance.RemoveStatusItem(this.gameObject.transform, entry.item);
		}
		if (this.OnRemoveStatusItem != null)
		{
			this.OnRemoveStatusItem(entry, immediate);
		}
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000DCDAE File Offset: 0x000DAFAE
	public void Destroy()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		while (this.items.Count > 0)
		{
			this.RemoveStatusItem(this.items[0].id, false);
		}
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x000DCDDF File Offset: 0x000DAFDF
	[Conditional("ENABLE_LOGGER")]
	private void Log(string action, StatusItem item, Guid guid)
	{
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x000DCDE1 File Offset: 0x000DAFE1
	private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
	{
	}

	// Token: 0x040016A9 RID: 5801
	private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();

	// Token: 0x040016AA RID: 5802
	public Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;

	// Token: 0x040016AB RID: 5803
	public Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;

	// Token: 0x040016AD RID: 5805
	private Vector3 offset = new Vector3(0f, 0f, 0f);

	// Token: 0x040016AE RID: 5806
	private static Func<List<Notification>, object, string> OnToolTip = (List<Notification> notifications, object data) => ((StatusItem)data).notificationTooltipText + notifications.ReduceMessages(true);

	// Token: 0x0200151B RID: 5403
	public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
	{
		// Token: 0x0600922B RID: 37419 RVA: 0x00373124 File Offset: 0x00371324
		public Entry(StatusItem item, StatusItemCategory category, object data)
		{
			this.id = Guid.NewGuid();
			this.item = item;
			this.data = data;
			this.category = category;
			this.notification = null;
		}

		// Token: 0x0600922C RID: 37420 RVA: 0x0037314D File Offset: 0x0037134D
		public string GetName()
		{
			return this.item.GetName(this.data);
		}

		// Token: 0x0600922D RID: 37421 RVA: 0x00373160 File Offset: 0x00371360
		public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style)
		{
			this.item.ShowToolTip(tooltip_widget, this.data, property_style);
		}

		// Token: 0x0600922E RID: 37422 RVA: 0x00373175 File Offset: 0x00371375
		public void SetIcon(Image image)
		{
			this.item.SetIcon(image, this.data);
		}

		// Token: 0x0600922F RID: 37423 RVA: 0x00373189 File Offset: 0x00371389
		public int CompareTo(StatusItemGroup.Entry other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x06009230 RID: 37424 RVA: 0x0037319C File Offset: 0x0037139C
		public bool Equals(StatusItemGroup.Entry other)
		{
			return this.id == other.id;
		}

		// Token: 0x06009231 RID: 37425 RVA: 0x003731AF File Offset: 0x003713AF
		public void OnClick()
		{
			this.item.OnClick(this.data);
		}

		// Token: 0x040070B1 RID: 28849
		public static StatusItemGroup.Entry EmptyEntry = new StatusItemGroup.Entry
		{
			id = Guid.Empty
		};

		// Token: 0x040070B2 RID: 28850
		public Guid id;

		// Token: 0x040070B3 RID: 28851
		public StatusItem item;

		// Token: 0x040070B4 RID: 28852
		public object data;

		// Token: 0x040070B5 RID: 28853
		public Notification notification;

		// Token: 0x040070B6 RID: 28854
		public StatusItemCategory category;
	}
}
