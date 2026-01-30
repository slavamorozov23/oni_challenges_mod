using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE5 RID: 3557
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x002A85C6 File Offset: 0x002A67C6
	// (set) Token: 0x06006FE7 RID: 28647 RVA: 0x002A85D3 File Offset: 0x002A67D3
	public Color barColor
	{
		get
		{
			return this.bar.color;
		}
		set
		{
			this.bar.color = value;
		}
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06006FE8 RID: 28648 RVA: 0x002A85E1 File Offset: 0x002A67E1
	// (set) Token: 0x06006FE9 RID: 28649 RVA: 0x002A85EE File Offset: 0x002A67EE
	public float PercentFull
	{
		get
		{
			return this.bar.fillAmount;
		}
		set
		{
			this.bar.fillAmount = value;
		}
	}

	// Token: 0x06006FEA RID: 28650 RVA: 0x002A85FC File Offset: 0x002A67FC
	public void SetVisibility(bool visible)
	{
		this.lastVisibilityValue = visible;
		this.RefreshVisibility();
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002A860C File Offset: 0x002A680C
	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = this.lastVisibilityValue;
		flag &= (!this.hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId);
		flag &= (!this.autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID);
		base.gameObject.SetActive(flag);
		if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006FEC RID: 28652 RVA: 0x002A86A8 File Offset: 0x002A68A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hasBeenInitialize = true;
		if (this.autoHide)
		{
			this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(false);
			}
		}
		this.activeWorldChangedHandlerID = Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = (this.updatePercentFull != null);
		this.RefreshVisibility();
	}

	// Token: 0x06006FED RID: 28653 RVA: 0x002A8764 File Offset: 0x002A6964
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		this.SetWorldActive(tuple.first);
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x002A8784 File Offset: 0x002A6984
	private void SetWorldActive(int worldId)
	{
		this.RefreshVisibility();
	}

	// Token: 0x06006FEF RID: 28655 RVA: 0x002A878C File Offset: 0x002A698C
	public void SetUpdateFunc(Func<float> func)
	{
		this.updatePercentFull = func;
		base.enabled = (this.updatePercentFull != null);
	}

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002A87A4 File Offset: 0x002A69A4
	public virtual void Update()
	{
		if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			this.PercentFull = this.updatePercentFull();
		}
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002A87D1 File Offset: 0x002A69D1
	public virtual void OnOverlayChanged(object _ = null)
	{
		this.RefreshVisibility();
	}

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002A87DC File Offset: 0x002A69DC
	public void Retarget(GameObject entity)
	{
		Vector3 vector = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			vector -= Vector3.right * 0.5f * (float)(component.Def.WidthInCells % 2);
		}
		else
		{
			vector -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002A8867 File Offset: 0x002A6A67
	protected override void OnCleanUp()
	{
		if (this.overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(ref this.activeWorldChangedHandlerID);
		base.OnCleanUp();
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002A8898 File Offset: 0x002A6A98
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x002A88A1 File Offset: 0x002A6AA1
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002A88AC File Offset: 0x002A6AAC
	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}

	// Token: 0x04004CC0 RID: 19648
	public Image bar;

	// Token: 0x04004CC1 RID: 19649
	private Func<float> updatePercentFull;

	// Token: 0x04004CC2 RID: 19650
	private int overlayUpdateHandle = -1;

	// Token: 0x04004CC3 RID: 19651
	public bool autoHide = true;

	// Token: 0x04004CC4 RID: 19652
	private bool lastVisibilityValue = true;

	// Token: 0x04004CC5 RID: 19653
	private bool hasBeenInitialize;

	// Token: 0x04004CC6 RID: 19654
	private int activeWorldChangedHandlerID = -1;
}
