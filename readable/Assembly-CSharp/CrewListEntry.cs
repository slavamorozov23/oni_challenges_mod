using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D6D RID: 3437
[AddComponentMenu("KMonoBehaviour/scripts/CrewListEntry")]
public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06006A9D RID: 27293 RVA: 0x00285B16 File Offset: 0x00283D16
	public MinionIdentity Identity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x06006A9E RID: 27294 RVA: 0x00285B1E File Offset: 0x00283D1E
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.BGImage.enabled = true;
		this.BorderHighlight.color = new Color(0.65882355f, 0.2901961f, 0.4745098f);
	}

	// Token: 0x06006A9F RID: 27295 RVA: 0x00285B52 File Offset: 0x00283D52
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.BGImage.enabled = false;
		this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
	}

	// Token: 0x06006AA0 RID: 27296 RVA: 0x00285B88 File Offset: 0x00283D88
	public void OnPointerClick(PointerEventData eventData)
	{
		bool focus = Time.unscaledTime - this.lastClickTime < 0.3f;
		this.SelectCrewMember(focus);
		this.lastClickTime = Time.unscaledTime;
	}

	// Token: 0x06006AA1 RID: 27297 RVA: 0x00285BBC File Offset: 0x00283DBC
	public virtual void Populate(MinionIdentity _identity)
	{
		this.identity = _identity;
		if (this.portrait == null)
		{
			GameObject parent = (this.crewPortraitParent != null) ? this.crewPortraitParent : base.gameObject;
			this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, parent, false);
			if (this.crewPortraitParent == null)
			{
				this.portrait.transform.SetSiblingIndex(2);
			}
		}
		this.portrait.SetIdentityObject(_identity, true);
	}

	// Token: 0x06006AA2 RID: 27298 RVA: 0x00285C3F File Offset: 0x00283E3F
	public virtual void Refresh()
	{
	}

	// Token: 0x06006AA3 RID: 27299 RVA: 0x00285C41 File Offset: 0x00283E41
	public void RefreshCrewPortraitContent()
	{
		if (this.portrait != null)
		{
			this.portrait.ForceRefresh();
		}
	}

	// Token: 0x06006AA4 RID: 27300 RVA: 0x00285C5C File Offset: 0x00283E5C
	private string seniorityString()
	{
		return this.identity.GetAttributes().GetProfessionString(true);
	}

	// Token: 0x06006AA5 RID: 27301 RVA: 0x00285C70 File Offset: 0x00283E70
	public void SelectCrewMember(bool focus)
	{
		if (focus)
		{
			SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
			return;
		}
		SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x04004955 RID: 18773
	protected MinionIdentity identity;

	// Token: 0x04004956 RID: 18774
	protected CrewPortrait portrait;

	// Token: 0x04004957 RID: 18775
	public CrewPortrait PortraitPrefab;

	// Token: 0x04004958 RID: 18776
	public GameObject crewPortraitParent;

	// Token: 0x04004959 RID: 18777
	protected bool mouseOver;

	// Token: 0x0400495A RID: 18778
	public Image BorderHighlight;

	// Token: 0x0400495B RID: 18779
	public Image BGImage;

	// Token: 0x0400495C RID: 18780
	public float lastClickTime;
}
