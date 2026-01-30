using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E0D RID: 3597
public class SideDetailsScreen : KScreen
{
	// Token: 0x060071E4 RID: 29156 RVA: 0x002B8419 File Offset: 0x002B6619
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SideDetailsScreen.Instance = this;
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060071E5 RID: 29157 RVA: 0x002B8439 File Offset: 0x002B6639
	protected override void OnForcedCleanUp()
	{
		SideDetailsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060071E6 RID: 29158 RVA: 0x002B8448 File Offset: 0x002B6648
	private void Initialize()
	{
		if (this.screens == null)
		{
			return;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.screenMap = new Dictionary<string, SideTargetScreen>();
		List<SideTargetScreen> list = new List<SideTargetScreen>();
		foreach (SideTargetScreen sideTargetScreen in this.screens)
		{
			SideTargetScreen sideTargetScreen2 = Util.KInstantiateUI<SideTargetScreen>(sideTargetScreen.gameObject, this.body.gameObject, false);
			sideTargetScreen2.gameObject.SetActive(false);
			list.Add(sideTargetScreen2);
		}
		list.ForEach(delegate(SideTargetScreen s)
		{
			this.screenMap.Add(s.name, s);
		});
		this.backButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x060071E7 RID: 29159 RVA: 0x002B850C File Offset: 0x002B670C
	public void SetTitle(string newTitle)
	{
		this.title.text = newTitle;
	}

	// Token: 0x060071E8 RID: 29160 RVA: 0x002B851C File Offset: 0x002B671C
	public void SetScreen(string screenName, object content, float x)
	{
		if (!this.screenMap.ContainsKey(screenName))
		{
			global::Debug.LogError("Tried to open a screen that does exist on the manager!");
			return;
		}
		if (content == null)
		{
			global::Debug.LogError("Tried to set " + screenName + " with null content!");
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		Rect rect = this.rectTransform.rect;
		this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
		this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
		if (this.activeScreen != null)
		{
			this.activeScreen.gameObject.SetActive(false);
		}
		this.activeScreen = this.screenMap[screenName];
		this.activeScreen.gameObject.SetActive(true);
		this.SetTitle(this.activeScreen.displayName);
		this.activeScreen.SetTarget(content);
	}

	// Token: 0x04004EA7 RID: 20135
	[SerializeField]
	private List<SideTargetScreen> screens;

	// Token: 0x04004EA8 RID: 20136
	[SerializeField]
	private LocText title;

	// Token: 0x04004EA9 RID: 20137
	[SerializeField]
	private KButton backButton;

	// Token: 0x04004EAA RID: 20138
	[SerializeField]
	private RectTransform body;

	// Token: 0x04004EAB RID: 20139
	private RectTransform rectTransform;

	// Token: 0x04004EAC RID: 20140
	private Dictionary<string, SideTargetScreen> screenMap;

	// Token: 0x04004EAD RID: 20141
	private SideTargetScreen activeScreen;

	// Token: 0x04004EAE RID: 20142
	public static SideDetailsScreen Instance;
}
