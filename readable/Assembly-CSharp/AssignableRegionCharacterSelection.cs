using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CA6 RID: 3238
[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x0600630E RID: 25358 RVA: 0x0024B868 File Offset: 0x00249A68
	// (remove) Token: 0x0600630F RID: 25359 RVA: 0x0024B8A0 File Offset: 0x00249AA0
	public event Action<MinionIdentity> OnDuplicantSelected;

	// Token: 0x06006310 RID: 25360 RVA: 0x0024B8D5 File Offset: 0x00249AD5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006311 RID: 25361 RVA: 0x0024B8FC File Offset: 0x00249AFC
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.buttonPool.ClearAll();
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
			CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
			componentInChildren.SetIdentityObject(minionIdentity, true);
			this.portraitList.Add(componentInChildren);
			btn.ClearOnClick();
			btn.onClick += delegate()
			{
				this.SelectDuplicant(btn);
			};
			this.buttonIdentityMap.Add(btn, minionIdentity);
		}
	}

	// Token: 0x06006312 RID: 25362 RVA: 0x0024B9E4 File Offset: 0x00249BE4
	public void Close()
	{
		this.buttonPool.DestroyAllActive();
		this.buttonIdentityMap.Clear();
		this.portraitList.Clear();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006313 RID: 25363 RVA: 0x0024BA13 File Offset: 0x00249C13
	private void SelectDuplicant(KButton btn)
	{
		if (this.OnDuplicantSelected != null)
		{
			this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
		}
		this.Close();
	}

	// Token: 0x04004321 RID: 17185
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04004322 RID: 17186
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04004323 RID: 17187
	private UIPool<KButton> buttonPool;

	// Token: 0x04004324 RID: 17188
	private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();

	// Token: 0x04004325 RID: 17189
	private List<CrewPortrait> portraitList = new List<CrewPortrait>();
}
