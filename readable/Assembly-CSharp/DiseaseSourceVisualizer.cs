using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008FB RID: 2299
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
	// Token: 0x06003FD3 RID: 16339 RVA: 0x001676D8 File Offset: 0x001658D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisibility();
		Components.DiseaseSourceVisualizers.Add(this);
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x001676F4 File Offset: 0x001658F4
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnViewModeChanged));
		base.OnCleanUp();
		Components.DiseaseSourceVisualizers.Remove(this);
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x00167758 File Offset: 0x00165958
	private void CreateVisualizer()
	{
		if (this.visualizer != null)
		{
			return;
		}
		if (GameScreenManager.Instance.worldSpaceCanvas == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x001677A8 File Offset: 0x001659A8
	public void UpdateVisibility()
	{
		this.CreateVisualizer();
		if (string.IsNullOrEmpty(this.alwaysShowDisease))
		{
			this.visible = false;
		}
		else
		{
			Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
			if (disease != null)
			{
				this.SetVisibleDisease(disease);
			}
		}
		if (OverlayScreen.Instance != null)
		{
			this.Show(OverlayScreen.Instance.GetMode());
		}
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x00167810 File Offset: 0x00165A10
	private void SetVisibleDisease(Disease disease)
	{
		Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
		Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
		Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
		component.sprite = overlaySprite;
		component.color = colorByName;
		this.visible = true;
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x00167872 File Offset: 0x00165A72
	private void Update()
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.visualizer.transform.SetPosition(base.transform.GetPosition() + this.offset);
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x001678AA File Offset: 0x00165AAA
	private void OnViewModeChanged(HashedString mode)
	{
		this.Show(mode);
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x001678B3 File Offset: 0x00165AB3
	public void Show(HashedString mode)
	{
		base.enabled = (this.visible && mode == OverlayModes.Disease.ID);
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(base.enabled);
		}
	}

	// Token: 0x04002785 RID: 10117
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04002786 RID: 10118
	private GameObject visualizer;

	// Token: 0x04002787 RID: 10119
	private bool visible;

	// Token: 0x04002788 RID: 10120
	public string alwaysShowDisease;
}
