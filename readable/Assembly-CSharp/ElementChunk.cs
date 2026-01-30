using System;
using Klei;
using UnityEngine;

// Token: 0x02000913 RID: 2323
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ElementChunk")]
public class ElementChunk : KMonoBehaviour
{
	// Token: 0x0600409F RID: 16543 RVA: 0x0016DBDF File Offset: 0x0016BDDF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.OreSizeVisualizers.Add(base.gameObject);
		GameComps.ElementSplitters.Add(base.gameObject);
		base.Subscribe<ElementChunk>(-2064133523, ElementChunk.OnAbsorbDelegate);
	}

	// Token: 0x060040A0 RID: 16544 RVA: 0x0016DC1C File Offset: 0x0016BE1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		base.transform.SetPosition(position);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = component.Element;
		KSelectable component2 = base.GetComponent<KSelectable>();
		Func<Element> data = () => element;
		component2.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreTemp, base.gameObject);
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x0016DCD0 File Offset: 0x0016BED0
	protected override void OnCleanUp()
	{
		GameComps.ElementSplitters.Remove(base.gameObject);
		GameComps.OreSizeVisualizers.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x0016DCF8 File Offset: 0x0016BEF8
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			PrimaryElement primaryElement = pickupable.PrimaryElement;
			if (primaryElement != null)
			{
				float mass = primaryElement.Mass;
				if (mass > 0f)
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					float mass2 = component.Mass;
					float temperature = (mass2 > 0f) ? SimUtil.CalculateFinalTemperature(mass2, component.Temperature, mass, primaryElement.Temperature) : primaryElement.Temperature;
					component.SetMassTemperature(mass2 + mass, temperature);
				}
				if (CameraController.Instance != null)
				{
					string sound = GlobalAssets.GetSound("Ore_absorb", false);
					Vector3 position = pickupable.transform.GetPosition();
					position.z = 0f;
					if (sound != null && CameraController.Instance.IsAudibleSound(position, sound))
					{
						KFMOD.PlayOneShot(sound, position, 1f);
					}
				}
			}
		}
	}

	// Token: 0x04002865 RID: 10341
	private static readonly EventSystem.IntraObjectHandler<ElementChunk> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<ElementChunk>(delegate(ElementChunk component, object data)
	{
		component.OnAbsorb(data);
	});
}
