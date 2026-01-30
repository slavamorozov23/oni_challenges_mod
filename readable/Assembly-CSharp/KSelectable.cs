using System;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600234E RID: 9038 RVA: 0x000CC893 File Offset: 0x000CAA93
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x0600234F RID: 9039 RVA: 0x000CC89B File Offset: 0x000CAA9B
	// (set) Token: 0x06002350 RID: 9040 RVA: 0x000CC8AD File Offset: 0x000CAAAD
	public bool IsSelectable
	{
		get
		{
			return this.selectable && base.isActiveAndEnabled;
		}
		set
		{
			this.selectable = value;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06002351 RID: 9041 RVA: 0x000CC8B6 File Offset: 0x000CAAB6
	public bool DisableSelectMarker
	{
		get
		{
			return this.disableSelectMarker;
		}
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000CC8C0 File Offset: 0x000CAAC0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.statusItemGroup = new StatusItemGroup(base.gameObject);
		base.GetComponent<KPrefabID>() != null;
		if (this.entityName == null || this.entityName.Length <= 0)
		{
			this.SetName(base.name);
		}
		if (this.entityGender == null)
		{
			this.entityGender = "NB";
		}
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000CC928 File Offset: 0x000CAB28
	public virtual string GetName()
	{
		if (this.entityName == null || this.entityName == "" || this.entityName.Length <= 0)
		{
			global::Debug.Log("Warning Item has blank name!", base.gameObject);
			return base.name;
		}
		return this.entityName;
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000CC97A File Offset: 0x000CAB7A
	public void SetStatusIndicatorOffset(Vector3 offset)
	{
		if (this.statusItemGroup == null)
		{
			return;
		}
		this.statusItemGroup.SetOffset(offset);
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000CC991 File Offset: 0x000CAB91
	public void SetName(string name)
	{
		this.entityName = name;
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000CC99A File Offset: 0x000CAB9A
	public void SetGender(string Gender)
	{
		this.entityGender = Gender;
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000CC9A4 File Offset: 0x000CABA4
	public float GetZoom()
	{
		Bounds bounds = Util.GetBounds(base.gameObject);
		return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000CC9E0 File Offset: 0x000CABE0
	public Vector3 GetPortraitLocation()
	{
		return Util.GetBounds(base.gameObject).center;
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000CCA00 File Offset: 0x000CAC00
	private void ClearHighlight()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(0f, 0f, 0f, 0f);
		}
		base.Trigger(-1201923725, BoxedBools.False);
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000CCA54 File Offset: 0x000CAC54
	private void ApplyHighlight(float highlight)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(highlight, highlight, highlight, highlight);
		}
		base.Trigger(-1201923725, BoxedBools.True);
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000CCA98 File Offset: 0x000CAC98
	public void Select()
	{
		this.selected = true;
		this.ClearHighlight();
		this.ApplyHighlight(0.2f);
		base.Trigger(-1503271301, BoxedBools.True);
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			int childCount2 = base.transform.GetChild(i).childCount;
			for (int j = 0; j < childCount2; j++)
			{
				if (base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>() != null)
				{
					base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
				}
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000CCBB0 File Offset: 0x000CADB0
	public void Unselect()
	{
		if (this.selected)
		{
			this.selected = false;
			this.ClearHighlight();
			base.Trigger(-1503271301, BoxedBools.False);
		}
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<LoopingSounds>() != null)
			{
				transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000CCCA8 File Offset: 0x000CAEA8
	public void Hover(bool playAudio)
	{
		this.ClearHighlight();
		if (!DebugHandler.HideUI)
		{
			this.ApplyHighlight(0.25f);
		}
		if (playAudio)
		{
			this.PlayHoverSound();
		}
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000CCCCB File Offset: 0x000CAECB
	private void PlayHoverSound()
	{
		if (CellSelectionObject.IsSelectionObject(base.gameObject))
		{
			return;
		}
		UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000CCCE1 File Offset: 0x000CAEE1
	public void Unhover()
	{
		if (!this.selected)
		{
			this.ClearHighlight();
		}
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000CCCF1 File Offset: 0x000CAEF1
	public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
	{
		if (on)
		{
			return this.AddStatusItem(status_item, data);
		}
		return this.RemoveStatusItem(status_item, false);
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000CCD07 File Offset: 0x000CAF07
	public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
	{
		if (show)
		{
			if (guid != Guid.Empty)
			{
				return guid;
			}
			return this.AddStatusItem(status_item, data);
		}
		else
		{
			if (guid != Guid.Empty)
			{
				return this.RemoveStatusItem(guid, false);
			}
			return guid;
		}
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000CCD3C File Offset: 0x000CAF3C
	public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.SetStatusItem(category, status_item, data);
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000CCD5A File Offset: 0x000CAF5A
	public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		if (guid != Guid.Empty)
		{
			this.statusItemGroup.RemoveStatusItem(guid, false);
		}
		return this.AddStatusItem(status_item, data);
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000CCD8D File Offset: 0x000CAF8D
	public Guid AddStatusItem(StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.AddStatusItem(status_item, data, null);
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000CCDAB File Offset: 0x000CAFAB
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(status_item, immediate);
		return Guid.Empty;
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000CCDCE File Offset: 0x000CAFCE
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(guid, immediate);
		return Guid.Empty;
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000CCDF1 File Offset: 0x000CAFF1
	public bool HasStatusItem(StatusItem status_item)
	{
		return this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000CCE09 File Offset: 0x000CB009
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		return this.statusItemGroup.GetStatusItem(category);
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000CCE17 File Offset: 0x000CB017
	public StatusItemGroup GetStatusItemGroup()
	{
		return this.statusItemGroup;
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000CCE20 File Offset: 0x000CB020
	public void UpdateWorkerSelection(bool selected)
	{
		Workable[] components = base.GetComponents<Workable>();
		if (components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].worker != null && components[i].GetComponent<LoopingSounds>() != null)
				{
					components[i].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
				}
			}
		}
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000CCE74 File Offset: 0x000CB074
	public void UpdateWorkableSelection(bool selected)
	{
		WorkerBase component = base.GetComponent<WorkerBase>();
		if (component != null && component.GetWorkable() != null)
		{
			Workable workable = base.GetComponent<WorkerBase>().GetWorkable();
			if (workable.GetComponent<LoopingSounds>() != null)
			{
				workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
			}
		}
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000CCEC5 File Offset: 0x000CB0C5
	protected override void OnLoadLevel()
	{
		this.OnCleanUp();
		base.OnLoadLevel();
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000CCED4 File Offset: 0x000CB0D4
	protected override void OnCleanUp()
	{
		if (this.statusItemGroup != null)
		{
			this.statusItemGroup.Destroy();
			this.statusItemGroup = null;
		}
		if (this.selected && SelectTool.Instance != null)
		{
			if (SelectTool.Instance.selected == this)
			{
				SelectTool.Instance.Select(null, true);
			}
			else
			{
				this.Unselect();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x0400149F RID: 5279
	private const float hoverHighlight = 0.25f;

	// Token: 0x040014A0 RID: 5280
	private const float selectHighlight = 0.2f;

	// Token: 0x040014A1 RID: 5281
	public string entityName;

	// Token: 0x040014A2 RID: 5282
	public string entityGender;

	// Token: 0x040014A3 RID: 5283
	private bool selected;

	// Token: 0x040014A4 RID: 5284
	[SerializeField]
	private bool selectable = true;

	// Token: 0x040014A5 RID: 5285
	[SerializeField]
	private bool disableSelectMarker;

	// Token: 0x040014A6 RID: 5286
	private StatusItemGroup statusItemGroup;
}
