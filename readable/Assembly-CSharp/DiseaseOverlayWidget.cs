using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D03 RID: 3331
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseOverlayWidget")]
public class DiseaseOverlayWidget : KMonoBehaviour
{
	// Token: 0x06006714 RID: 26388 RVA: 0x0026D64C File Offset: 0x0026B84C
	public void Refresh(AmountInstance value_src)
	{
		GameObject gameObject = value_src.gameObject;
		if (gameObject == null)
		{
			return;
		}
		KAnimControllerBase component = gameObject.GetComponent<KAnimControllerBase>();
		Vector3 a = (component != null) ? component.GetWorldPivot() : (gameObject.transform.GetPosition() + Vector3.down);
		base.transform.SetPosition(a + this.offset);
		if (value_src != null)
		{
			this.progressFill.transform.parent.gameObject.SetActive(true);
			float num = value_src.value / value_src.GetMax();
			Vector3 localScale = this.progressFill.rectTransform.localScale;
			localScale.y = num;
			this.progressFill.rectTransform.localScale = localScale;
			this.progressToolTip.toolTip = DUPLICANTS.ATTRIBUTES.IMMUNITY.NAME + " " + GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
		else
		{
			this.progressFill.transform.parent.gameObject.SetActive(false);
		}
		int num2 = 0;
		Amounts amounts = gameObject.GetComponent<Modifiers>().GetAmounts();
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			float value = amounts.Get(disease.amount).value;
			if (value > 0f)
			{
				Image image;
				if (num2 < this.displayedDiseases.Count)
				{
					image = this.displayedDiseases[num2];
				}
				else
				{
					image = Util.KInstantiateUI(this.germsImage.gameObject, this.germsImage.transform.parent.gameObject, true).GetComponent<Image>();
					this.displayedDiseases.Add(image);
				}
				image.color = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				image.GetComponent<ToolTip>().toolTip = disease.Name + " " + GameUtil.GetFormattedDiseaseAmount((int)value, GameUtil.TimeSlice.None);
				num2++;
			}
		}
		for (int i = this.displayedDiseases.Count - 1; i >= num2; i--)
		{
			Util.KDestroyGameObject(this.displayedDiseases[i].gameObject);
			this.displayedDiseases.RemoveAt(i);
		}
		this.diseasedImage.enabled = false;
		this.progressFill.transform.parent.gameObject.SetActive(this.displayedDiseases.Count > 0);
		this.germsImage.transform.parent.gameObject.SetActive(this.displayedDiseases.Count > 0);
	}

	// Token: 0x04004682 RID: 18050
	[SerializeField]
	private Image progressFill;

	// Token: 0x04004683 RID: 18051
	[SerializeField]
	private ToolTip progressToolTip;

	// Token: 0x04004684 RID: 18052
	[SerializeField]
	private Image germsImage;

	// Token: 0x04004685 RID: 18053
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04004686 RID: 18054
	[SerializeField]
	private Image diseasedImage;

	// Token: 0x04004687 RID: 18055
	private List<Image> displayedDiseases = new List<Image>();
}
