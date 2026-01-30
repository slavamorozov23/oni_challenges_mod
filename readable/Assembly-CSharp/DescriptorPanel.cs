using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CF6 RID: 3318
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
	// Token: 0x06006680 RID: 26240 RVA: 0x0026962F File Offset: 0x0026782F
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06006681 RID: 26241 RVA: 0x00269640 File Offset: 0x00267840
	public void SetDescriptors(IList<Descriptor> descriptors)
	{
		int i;
		for (i = 0; i < descriptors.Count; i++)
		{
			GameObject gameObject;
			if (i >= this.labels.Count)
			{
				gameObject = Util.KInstantiate((this.customLabelPrefab != null) ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, base.gameObject, null);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.labels.Add(gameObject);
			}
			else
			{
				gameObject = this.labels[i];
			}
			gameObject.GetComponent<LocText>().text = descriptors[i].IndentedText();
			gameObject.GetComponent<ToolTip>().toolTip = descriptors[i].tooltipText;
			gameObject.SetActive(true);
		}
		while (i < this.labels.Count)
		{
			this.labels[i].SetActive(false);
			i++;
		}
	}

	// Token: 0x04004603 RID: 17923
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x04004604 RID: 17924
	private List<GameObject> labels = new List<GameObject>();
}
