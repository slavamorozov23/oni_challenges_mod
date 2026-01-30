using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000A81 RID: 2689
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalMechanics")]
public class OrbitalMechanics : KMonoBehaviour
{
	// Token: 0x06004E27 RID: 20007 RVA: 0x001C68BB File Offset: 0x001C4ABB
	protected override void OnPrefabInit()
	{
		base.Subscribe<OrbitalMechanics>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001C68D0 File Offset: 0x001C4AD0
	private void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		this.UpdateLocation(clusterLocationChangedEvent.newLocation);
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001C68F0 File Offset: 0x001C4AF0
	protected override void OnCleanUp()
	{
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
		}
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001C695C File Offset: 0x001C4B5C
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		List<string> list = new List<string>();
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					list.Add(@ref.Get().orbitalDBId);
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateOrbitalObject(list[i]);
			}
		}
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001C6A14 File Offset: 0x001C4C14
	private void UpdateLocation(AxialI location)
	{
		if (this.orbitingObjects.Count > 0)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);
		if (visibleEntityOfLayerAtCell != null)
		{
			ArtifactPOIClusterGridEntity component = visibleEntityOfLayerAtCell.GetComponent<ArtifactPOIClusterGridEntity>();
			if (component != null)
			{
				ArtifactPOIStates.Instance smi = component.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.configuration.poiType.orbitalObject != null)
				{
					foreach (string text in smi.configuration.poiType.orbitalObject)
					{
					}
				}
			}
			HarvestablePOIClusterGridEntity component2 = visibleEntityOfLayerAtCell.GetComponent<HarvestablePOIClusterGridEntity>();
			if (component2 != null)
			{
				HarvestablePOIStates.Instance smi2 = component2.GetSMI<HarvestablePOIStates.Instance>();
				if (smi2 != null && smi2.configuration.poiType.orbitalObject != null)
				{
					List<string> orbitalObject = smi2.configuration.poiType.orbitalObject;
					KRandom krandom = new KRandom();
					float num = smi2.poiCapacity / smi2.configuration.GetMaxCapacity() * (float)smi2.configuration.poiType.maxNumOrbitingObjects;
					int num2 = 0;
					while ((float)num2 < num)
					{
						krandom.Next(orbitalObject.Count);
						num2++;
					}
					return;
				}
			}
		}
		else
		{
			Clustercraft component3 = base.GetComponent<Clustercraft>();
			if (component3 != null && !(component3.GetOrbitAsteroid() != null) && component3.Status != Clustercraft.CraftStatus.Launching && component3.Status == Clustercraft.CraftStatus.Landing)
			{
				this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
			}
		}
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001C6C10 File Offset: 0x001C4E10
	public void CreateOrbitalObject(string orbit_db_name)
	{
		WorldContainer component = base.GetComponent<WorldContainer>();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OrbitalBGConfig.ID), base.gameObject, null);
		OrbitalObject component2 = gameObject.GetComponent<OrbitalObject>();
		component2.Init(orbit_db_name, component, this.orbitingObjects);
		gameObject.SetActive(true);
		this.orbitingObjects.Add(new Ref<OrbitalObject>(component2));
	}

	// Token: 0x04003410 RID: 13328
	[Serialize]
	private List<Ref<OrbitalObject>> orbitingObjects = new List<Ref<OrbitalObject>>();

	// Token: 0x04003411 RID: 13329
	private EventSystem.IntraObjectHandler<OrbitalMechanics> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<OrbitalMechanics>(delegate(OrbitalMechanics cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
