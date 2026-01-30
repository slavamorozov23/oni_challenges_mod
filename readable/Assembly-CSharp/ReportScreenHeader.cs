using System;
using UnityEngine;

// Token: 0x02000DED RID: 3565
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
	// Token: 0x06007041 RID: 28737 RVA: 0x002AA42E File Offset: 0x002A862E
	public void SetMainEntry(ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenHeaderRow>();
		}
		this.mainRow.SetLine(reportGroup);
	}

	// Token: 0x04004D10 RID: 19728
	[SerializeField]
	private ReportScreenHeaderRow rowTemplate;

	// Token: 0x04004D11 RID: 19729
	private ReportScreenHeaderRow mainRow;
}
