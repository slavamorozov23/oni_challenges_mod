using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000D45 RID: 3397
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	// Token: 0x0600694F RID: 26959 RVA: 0x0027E4FC File Offset: 0x0027C6FC
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.ResetState();
		this.equipmentVis.gameObject.SetActive(false);
		this.fallbackVis.gameObject.SetActive(false);
		if (info.UseEquipmentVis)
		{
			this.equipmentVis.gameObject.SetActive(true);
			this.equipmentVis.ConfigureWith(info);
			return;
		}
		this.fallbackVis.gameObject.SetActive(true);
		this.fallbackVis.ConfigureWith(info);
	}

	// Token: 0x06006950 RID: 26960 RVA: 0x0027E574 File Offset: 0x0027C774
	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, this.AnimateInRoutine());
	}

	// Token: 0x06006951 RID: 26961 RVA: 0x0027E582 File Offset: 0x0027C782
	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, this.AnimateOutRoutine());
	}

	// Token: 0x06006952 RID: 26962 RVA: 0x0027E590 File Offset: 0x0027C790
	private IEnumerator AnimateInRoutine()
	{
		this.root.gameObject.SetActive(true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack, -1f);
		yield break;
	}

	// Token: 0x06006953 RID: 26963 RVA: 0x0027E59F File Offset: 0x0027C79F
	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.zero, 0.25f, null, -1f);
		this.root.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06006954 RID: 26964 RVA: 0x0027E5AE File Offset: 0x0027C7AE
	public void ResetState()
	{
		this.root.transform.localScale = Vector3.zero;
	}

	// Token: 0x04004868 RID: 18536
	[SerializeField]
	private RectTransform root;

	// Token: 0x04004869 RID: 18537
	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	// Token: 0x0400486A RID: 18538
	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;
}
