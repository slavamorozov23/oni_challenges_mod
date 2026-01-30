using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA7 RID: 3751
public class SubSpeciesInfoScreen : KModalScreen
{
	// Token: 0x0600782B RID: 30763 RVA: 0x002E3A5C File Offset: 0x002E1C5C
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600782C RID: 30764 RVA: 0x002E3A5F File Offset: 0x002E1C5F
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600782D RID: 30765 RVA: 0x002E3A68 File Offset: 0x002E1C68
	private void ClearMutations()
	{
		for (int i = this.mutationLineItems.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.mutationLineItems[i]);
		}
		this.mutationLineItems.Clear();
	}

	// Token: 0x0600782E RID: 30766 RVA: 0x002E3AA9 File Offset: 0x002E1CA9
	public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
	{
		this.SetSubspecies(speciesID, subSpeciesID);
		this.targetStation = station;
	}

	// Token: 0x0600782F RID: 30767 RVA: 0x002E3ABC File Offset: 0x002E1CBC
	private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
	{
		this.ClearMutations();
		ref PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
		this.plantIcon.sprite = Def.GetUISprite(Assets.GetPrefab(speciesID), "ui", false).first;
		foreach (string id in subSpecies.mutationIDs)
		{
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
			component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
			this.mutationLineItems.Add(gameObject);
		}
	}

	// Token: 0x040053BF RID: 21439
	[SerializeField]
	private KButton renameButton;

	// Token: 0x040053C0 RID: 21440
	[SerializeField]
	private KButton saveButton;

	// Token: 0x040053C1 RID: 21441
	[SerializeField]
	private KButton discardButton;

	// Token: 0x040053C2 RID: 21442
	[SerializeField]
	private RectTransform mutationsList;

	// Token: 0x040053C3 RID: 21443
	[SerializeField]
	private Image plantIcon;

	// Token: 0x040053C4 RID: 21444
	[SerializeField]
	private GameObject mutationsItemPrefab;

	// Token: 0x040053C5 RID: 21445
	private List<GameObject> mutationLineItems = new List<GameObject>();

	// Token: 0x040053C6 RID: 21446
	private GeneticAnalysisStation targetStation;
}
