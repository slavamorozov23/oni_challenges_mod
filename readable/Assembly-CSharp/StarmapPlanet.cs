using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA3 RID: 3747
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanet")]
public class StarmapPlanet : KMonoBehaviour
{
	// Token: 0x060077D5 RID: 30677 RVA: 0x002DF284 File Offset: 0x002DD484
	public void SetSprite(Sprite sprite, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.sprite = sprite;
			starmapPlanetVisualizer.image.color = color;
		}
	}

	// Token: 0x060077D6 RID: 30678 RVA: 0x002DF2E8 File Offset: 0x002DD4E8
	public void SetFillAmount(float amount)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.fillAmount = amount;
		}
	}

	// Token: 0x060077D7 RID: 30679 RVA: 0x002DF340 File Offset: 0x002DD540
	public void SetUnknownBGActive(bool active, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.unknownBG.gameObject.SetActive(active);
			starmapPlanetVisualizer.unknownBG.color = color;
		}
	}

	// Token: 0x060077D8 RID: 30680 RVA: 0x002DF3A8 File Offset: 0x002DD5A8
	public void SetSelectionActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.gameObject.SetActive(active);
		}
	}

	// Token: 0x060077D9 RID: 30681 RVA: 0x002DF404 File Offset: 0x002DD604
	public void SetAnalysisActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.analysisSelection.SetActive(active);
		}
	}

	// Token: 0x060077DA RID: 30682 RVA: 0x002DF45C File Offset: 0x002DD65C
	public void SetLabel(string text)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.text = text;
			this.ShowLabel(false);
		}
	}

	// Token: 0x060077DB RID: 30683 RVA: 0x002DF4BC File Offset: 0x002DD6BC
	public void ShowLabel(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.gameObject.SetActive(show);
		}
	}

	// Token: 0x060077DC RID: 30684 RVA: 0x002DF518 File Offset: 0x002DD718
	public void SetOnClick(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onClick = del;
		}
	}

	// Token: 0x060077DD RID: 30685 RVA: 0x002DF570 File Offset: 0x002DD770
	public void SetOnEnter(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onEnter = del;
		}
	}

	// Token: 0x060077DE RID: 30686 RVA: 0x002DF5C8 File Offset: 0x002DD7C8
	public void SetOnExit(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onExit = del;
		}
	}

	// Token: 0x060077DF RID: 30687 RVA: 0x002DF620 File Offset: 0x002DD820
	public void AnimateSelector(float time)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.anchoredPosition = new Vector2(0f, 25f + Mathf.Sin(time * 4f) * 5f);
		}
	}

	// Token: 0x060077E0 RID: 30688 RVA: 0x002DF698 File Offset: 0x002DD898
	public void ShowAsCurrentRocketDestination(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			if (rectTransform.childCount > 0)
			{
				rectTransform.GetChild(rectTransform.childCount - 1).GetComponent<HierarchyReferences>().GetReference<Image>("fg").color = (show ? new Color(0.11764706f, 0.8627451f, 0.3137255f) : Color.white);
			}
		}
	}

	// Token: 0x060077E1 RID: 30689 RVA: 0x002DF738 File Offset: 0x002DD938
	public void SetRocketIcons(int numRockets, GameObject iconPrefab)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			for (int i = rectTransform.childCount; i < numRockets; i++)
			{
				Util.KInstantiateUI(iconPrefab, starmapPlanetVisualizer.rocketIconContainer, true);
			}
			for (int j = rectTransform.childCount; j > numRockets; j--)
			{
				UnityEngine.Object.Destroy(rectTransform.GetChild(j - 1).gameObject);
			}
			int num = 0;
			foreach (object obj in rectTransform)
			{
				((RectTransform)obj).anchoredPosition = new Vector2((float)num * -10f, 0f);
				num++;
			}
		}
	}

	// Token: 0x0400534F RID: 21327
	public List<StarmapPlanetVisualizer> visualizers;
}
