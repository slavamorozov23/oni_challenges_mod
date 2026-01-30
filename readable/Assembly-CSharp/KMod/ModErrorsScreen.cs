using System;
using System.Collections.Generic;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000FC6 RID: 4038
	public class ModErrorsScreen : KScreen
	{
		// Token: 0x06007EF7 RID: 32503 RVA: 0x0032B400 File Offset: 0x00329600
		public static bool ShowErrors(List<Event> events)
		{
			if (Global.Instance.modManager.events.Count == 0)
			{
				return false;
			}
			GameObject parent = GameObject.Find("Canvas");
			ModErrorsScreen modErrorsScreen = Util.KInstantiateUI<ModErrorsScreen>(Global.Instance.modErrorsPrefab, parent, false);
			modErrorsScreen.Initialize(events);
			modErrorsScreen.gameObject.SetActive(true);
			return true;
		}

		// Token: 0x06007EF8 RID: 32504 RVA: 0x0032B454 File Offset: 0x00329654
		private void Initialize(List<Event> events)
		{
			foreach (Event @event in events)
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, true);
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				LocText reference2 = hierarchyReferences.GetReference<LocText>("Description");
				KButton reference3 = hierarchyReferences.GetReference<KButton>("Details");
				string text;
				string toolTip;
				Event.GetUIStrings(@event.event_type, out text, out toolTip);
				reference.text = text;
				reference.GetComponent<ToolTip>().toolTip = toolTip;
				reference2.text = @event.mod.title;
				ToolTip component = reference2.GetComponent<ToolTip>();
				if (component != null)
				{
					ToolTip toolTip2 = component;
					Label mod = @event.mod;
					toolTip2.toolTip = mod.ToString();
				}
				reference3.isInteractable = false;
				Mod mod2 = Global.Instance.modManager.FindMod(@event.mod);
				if (mod2 != null)
				{
					if (component != null && !string.IsNullOrEmpty(mod2.description))
					{
						StringEntry entry;
						if (Strings.TryGet(mod2.description, out entry))
						{
							component.toolTip = entry;
						}
						else
						{
							component.toolTip = mod2.description;
						}
					}
					if (mod2.on_managed != null)
					{
						reference3.onClick += mod2.on_managed;
						reference3.isInteractable = true;
					}
				}
			}
		}

		// Token: 0x06007EF9 RID: 32505 RVA: 0x0032B5D4 File Offset: 0x003297D4
		protected override void OnActivate()
		{
			base.OnActivate();
			this.closeButtonTitle.onClick += this.Deactivate;
			this.closeButton.onClick += this.Deactivate;
		}

		// Token: 0x04005DB9 RID: 23993
		[SerializeField]
		private KButton closeButtonTitle;

		// Token: 0x04005DBA RID: 23994
		[SerializeField]
		private KButton closeButton;

		// Token: 0x04005DBB RID: 23995
		[SerializeField]
		private GameObject entryPrefab;

		// Token: 0x04005DBC RID: 23996
		[SerializeField]
		private Transform entryParent;
	}
}
