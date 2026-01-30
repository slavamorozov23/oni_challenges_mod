using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A38 RID: 2616
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06004C4F RID: 19535 RVA: 0x001BB7BE File Offset: 0x001B99BE
	public List<string> MutationIDs
	{
		get
		{
			return this.mutationIDs;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06004C50 RID: 19536 RVA: 0x001BB7C6 File Offset: 0x001B99C6
	public bool IsOriginal
	{
		get
		{
			return this.mutationIDs == null || this.mutationIDs.Count == 0;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06004C51 RID: 19537 RVA: 0x001BB7E0 File Offset: 0x001B99E0
	public bool IsIdentified
	{
		get
		{
			return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06004C52 RID: 19538 RVA: 0x001BB7FC File Offset: 0x001B99FC
	// (set) Token: 0x06004C53 RID: 19539 RVA: 0x001BB81F File Offset: 0x001B9A1F
	public Tag SpeciesID
	{
		get
		{
			global::Debug.Assert(this.speciesID != null, "Ack, forgot to configure the species ID for this mutantPlant!");
			return this.speciesID;
		}
		set
		{
			this.speciesID = value;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06004C54 RID: 19540 RVA: 0x001BB828 File Offset: 0x001B9A28
	public Tag SubSpeciesID
	{
		get
		{
			if (this.cachedSubspeciesID == null)
			{
				this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
			}
			return this.cachedSubspeciesID;
		}
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x001BB854 File Offset: 0x001B9A54
	protected override void OnPrefabInit()
	{
		base.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
		base.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x001BB878 File Offset: 0x001B9A78
	protected override void OnSpawn()
	{
		if (this.IsOriginal || this.HasTag(GameTags.Plant))
		{
			this.analyzed = true;
		}
		if (!this.IsOriginal)
		{
			this.AddTag(GameTags.MutatedSeed);
		}
		this.AddTag(this.SubSpeciesID);
		Components.MutantPlants.Add(this);
		base.OnSpawn();
		this.ApplyMutations();
		this.UpdateNameAndTags();
		PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001BB8EE File Offset: 0x001B9AEE
	protected override void OnCleanUp()
	{
		Components.MutantPlants.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001BB904 File Offset: 0x001B9B04
	private void OnAbsorb(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		global::Debug.Assert(component != null && this.SubSpeciesID == component.SubSpeciesID, "Two seeds of different subspecies just absorbed!");
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001BB944 File Offset: 0x001B9B44
	private void OnSplitFromChunk(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		if (component != null)
		{
			component.CopyMutationsTo(this);
		}
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x001BB970 File Offset: 0x001B9B70
	public void Mutate()
	{
		List<string> list = (this.mutationIDs != null) ? new List<string>(this.mutationIDs) : new List<string>();
		while (list.Count >= 1 && list.Count > 0)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		list.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
		this.SetSubSpecies(list);
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001BB9ED File Offset: 0x001B9BED
	public void Analyze()
	{
		this.analyzed = true;
		this.UpdateNameAndTags();
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001BB9FC File Offset: 0x001B9BFC
	public void ApplyMutations()
	{
		if (this.IsOriginal)
		{
			return;
		}
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).ApplyTo(this);
		}
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x001BBA68 File Offset: 0x001B9C68
	public void DummySetSubspecies(List<string> mutations)
	{
		this.mutationIDs = mutations;
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x001BBA71 File Offset: 0x001B9C71
	public void SetSubSpecies(List<string> mutations)
	{
		if (base.gameObject.HasTag(this.SubSpeciesID))
		{
			base.gameObject.RemoveTag(this.SubSpeciesID);
		}
		this.cachedSubspeciesID = Tag.Invalid;
		this.mutationIDs = mutations;
		this.UpdateNameAndTags();
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x001BBAAF File Offset: 0x001B9CAF
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
	{
		return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x001BBAC2 File Offset: 0x001B9CC2
	public void CopyMutationsTo(MutantPlant target)
	{
		target.SetSubSpecies(this.mutationIDs);
		target.analyzed = this.analyzed;
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x001BBADC File Offset: 0x001B9CDC
	public void UpdateNameAndTags()
	{
		bool flag = !base.IsInitialized() || this.IsIdentified;
		bool flag2 = PlantSubSpeciesCatalog.Instance == null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
		KPrefabID component = base.GetComponent<KPrefabID>();
		component.AddTag(this.SubSpeciesID, false);
		component.SetTag(GameTags.UnidentifiedSeed, !flag);
		base.gameObject.name = component.PrefabTag.ToString() + " (" + this.SubSpeciesID.ToString() + ")";
		base.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component.PrefabTag.ProperName(), flag, flag2));
		KSelectable component2 = base.GetComponent<KSelectable>();
		foreach (Guid guid in this.statusItemHandles)
		{
			component2.RemoveStatusItem(guid, false);
		}
		this.statusItemHandles.Clear();
		if (!flag2)
		{
			if (this.IsOriginal)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation, null));
				return;
			}
			if (!flag)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation, null));
				return;
			}
			foreach (string id in this.mutationIDs)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, Db.Get().PlantMutations.Get(id)));
			}
		}
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x001BBCD0 File Offset: 0x001B9ED0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.IsOriginal)
		{
			return null;
		}
		List<Descriptor> result = new List<Descriptor>();
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).GetDescriptors(ref result, go);
		}
		return result;
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x001BBD48 File Offset: 0x001B9F48
	public List<string> GetSoundEvents()
	{
		List<string> list = new List<string>();
		if (this.mutationIDs != null)
		{
			foreach (string id in this.mutationIDs)
			{
				PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
				list.AddRange(plantMutation.AdditionalSoundEvents);
			}
		}
		return list;
	}

	// Token: 0x040032B0 RID: 12976
	[Serialize]
	private bool analyzed;

	// Token: 0x040032B1 RID: 12977
	[Serialize]
	private List<string> mutationIDs;

	// Token: 0x040032B2 RID: 12978
	private List<Guid> statusItemHandles = new List<Guid>();

	// Token: 0x040032B3 RID: 12979
	private const int MAX_MUTATIONS = 1;

	// Token: 0x040032B4 RID: 12980
	[SerializeField]
	private Tag speciesID;

	// Token: 0x040032B5 RID: 12981
	private Tag cachedSubspeciesID;

	// Token: 0x040032B6 RID: 12982
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x040032B7 RID: 12983
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnSplitFromChunk(data);
	});
}
