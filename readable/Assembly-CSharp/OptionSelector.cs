using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ED2 RID: 3794
public class OptionSelector : MonoBehaviour
{
	// Token: 0x06007971 RID: 31089 RVA: 0x002EB069 File Offset: 0x002E9269
	private void Start()
	{
		this.selectedItem.GetComponent<KButton>().onBtnClick += this.OnClick;
	}

	// Token: 0x06007972 RID: 31090 RVA: 0x002EB087 File Offset: 0x002E9287
	public void Initialize(object id)
	{
		this.id = id;
	}

	// Token: 0x06007973 RID: 31091 RVA: 0x002EB090 File Offset: 0x002E9290
	private void OnClick(KKeyCode button)
	{
		if (button == KKeyCode.Mouse0)
		{
			this.OnChangePriority(this.id, 1);
			return;
		}
		if (button != KKeyCode.Mouse1)
		{
			return;
		}
		this.OnChangePriority(this.id, -1);
	}

	// Token: 0x06007974 RID: 31092 RVA: 0x002EB0C8 File Offset: 0x002E92C8
	public void ConfigureItem(bool disabled, OptionSelector.DisplayOptionInfo display_info)
	{
		HierarchyReferences component = this.selectedItem.GetComponent<HierarchyReferences>();
		KImage kimage = component.GetReference("BG") as KImage;
		if (display_info.bgOptions == null)
		{
			kimage.gameObject.SetActive(false);
		}
		else
		{
			kimage.sprite = display_info.bgOptions[display_info.bgIndex];
		}
		KImage kimage2 = component.GetReference("FG") as KImage;
		if (display_info.fgOptions == null)
		{
			kimage2.gameObject.SetActive(false);
		}
		else
		{
			kimage2.sprite = display_info.fgOptions[display_info.fgIndex];
		}
		KImage kimage3 = component.GetReference("Fill") as KImage;
		if (kimage3 != null)
		{
			kimage3.enabled = !disabled;
			kimage3.color = display_info.fillColour;
		}
		KImage kimage4 = component.GetReference("Outline") as KImage;
		if (kimage4 != null)
		{
			kimage4.enabled = !disabled;
		}
	}

	// Token: 0x040054F3 RID: 21747
	private object id;

	// Token: 0x040054F4 RID: 21748
	public Action<object, int> OnChangePriority;

	// Token: 0x040054F5 RID: 21749
	[SerializeField]
	private KImage selectedItem;

	// Token: 0x040054F6 RID: 21750
	[SerializeField]
	private KImage itemTemplate;

	// Token: 0x02002133 RID: 8499
	public class DisplayOptionInfo
	{
		// Token: 0x04009894 RID: 39060
		public IList<Sprite> bgOptions;

		// Token: 0x04009895 RID: 39061
		public IList<Sprite> fgOptions;

		// Token: 0x04009896 RID: 39062
		public int bgIndex;

		// Token: 0x04009897 RID: 39063
		public int fgIndex;

		// Token: 0x04009898 RID: 39064
		public Color32 fillColour;
	}
}
