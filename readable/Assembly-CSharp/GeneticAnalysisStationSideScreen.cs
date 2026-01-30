using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E3F RID: 3647
public class GeneticAnalysisStationSideScreen : SideScreenContent
{
	// Token: 0x060073A5 RID: 29605 RVA: 0x002C2840 File Offset: 0x002C0A40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Refresh();
	}

	// Token: 0x060073A6 RID: 29606 RVA: 0x002C284E File Offset: 0x002C0A4E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeneticAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x060073A7 RID: 29607 RVA: 0x002C2859 File Offset: 0x002C0A59
	public override void SetTarget(GameObject target)
	{
		this.target = target.GetSMI<GeneticAnalysisStation.StatesInstance>();
		target.GetComponent<GeneticAnalysisStationWorkable>();
		this.Refresh();
	}

	// Token: 0x060073A8 RID: 29608 RVA: 0x002C2874 File Offset: 0x002C0A74
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.DrawPickerMenu();
	}

	// Token: 0x060073A9 RID: 29609 RVA: 0x002C2888 File Offset: 0x002C0A88
	private void DrawPickerMenu()
	{
		Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> dictionary = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();
		foreach (Tag tag in PlantSubSpeciesCatalog.Instance.GetAllDiscoveredSpecies())
		{
			dictionary[tag] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>(PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(tag));
		}
		int num = 0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in dictionary)
		{
			if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(keyValuePair.Key).Count > 1)
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
				if (!(prefab == null))
				{
					SeedProducer component = prefab.GetComponent<SeedProducer>();
					if (!(component == null))
					{
						Tag tag2 = component.seedInfo.seedId.ToTag();
						if (DiscoveredResources.Instance.IsDiscovered(tag2))
						{
							HierarchyReferences hierarchyReferences;
							if (num < this.rows.Count)
							{
								hierarchyReferences = this.rows[num];
							}
							else
							{
								hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.rowPrefab.gameObject, this.rowContainer, false);
								this.rows.Add(hierarchyReferences);
							}
							this.ConfigureButton(hierarchyReferences, keyValuePair.Key);
							this.rows[num].gameObject.SetActive(true);
							num++;
						}
					}
				}
			}
		}
		for (int i = num; i < this.rows.Count; i++)
		{
			this.rows[i].gameObject.SetActive(false);
		}
		if (num == 0)
		{
			this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.NONE_DISCOVERED;
			this.contents.gameObject.SetActive(false);
			return;
		}
		this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SELECT_SEEDS;
		this.contents.gameObject.SetActive(true);
	}

	// Token: 0x060073AA RID: 29610 RVA: 0x002C2A94 File Offset: 0x002C0C94
	private void ConfigureButton(HierarchyReferences button, Tag speciesID)
	{
		TMP_Text reference = button.GetReference<LocText>("Label");
		Image reference2 = button.GetReference<Image>("Icon");
		LocText reference3 = button.GetReference<LocText>("ProgressLabel");
		button.GetReference<ToolTip>("ToolTip");
		Tag seedID = this.GetSeedIDFromPlantID(speciesID);
		bool isForbidden = this.target.GetSeedForbidden(seedID);
		reference.text = seedID.ProperName();
		reference2.sprite = Def.GetUISprite(seedID, "ui", false).first;
		if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesID).Count > 0)
		{
			reference3.text = (isForbidden ? UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_FORBIDDEN : UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_ALLOWED);
		}
		else
		{
			reference3.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_NO_MUTANTS;
		}
		KToggle component = button.GetComponent<KToggle>();
		component.isOn = !isForbidden;
		component.ClearOnClick();
		component.onClick += delegate()
		{
			this.target.SetSeedForbidden(seedID, !isForbidden);
			this.Refresh();
		};
	}

	// Token: 0x060073AB RID: 29611 RVA: 0x002C2BA2 File Offset: 0x002C0DA2
	private Tag GetSeedIDFromPlantID(Tag speciesID)
	{
		return Assets.GetPrefab(speciesID).GetComponent<SeedProducer>().seedInfo.seedId;
	}

	// Token: 0x04004FF9 RID: 20473
	[SerializeField]
	private LocText message;

	// Token: 0x04004FFA RID: 20474
	[SerializeField]
	private GameObject contents;

	// Token: 0x04004FFB RID: 20475
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x04004FFC RID: 20476
	[SerializeField]
	private HierarchyReferences rowPrefab;

	// Token: 0x04004FFD RID: 20477
	private List<HierarchyReferences> rows = new List<HierarchyReferences>();

	// Token: 0x04004FFE RID: 20478
	private GeneticAnalysisStation.StatesInstance target;

	// Token: 0x04004FFF RID: 20479
	private Dictionary<Tag, bool> expandedSeeds = new Dictionary<Tag, bool>();
}
