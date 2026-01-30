using System;
using FMODUnity;
using Klei;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000BE9 RID: 3049
[Serializable]
public class Substance
{
	// Token: 0x06005B7A RID: 23418 RVA: 0x00211ED4 File Offset: 0x002100D4
	public GameObject SpawnResource(Vector3 position, float mass, float temperature, byte disease_idx, int disease_count, bool prevent_merge = false, bool forceTemperature = false, bool manual_activation = false)
	{
		GameObject gameObject = null;
		PrimaryElement primaryElement = null;
		if (!prevent_merge)
		{
			int cell = Grid.PosToCell(position);
			GameObject gameObject2 = Grid.Objects[cell, 3];
			if (gameObject2 != null)
			{
				Pickupable component = gameObject2.GetComponent<Pickupable>();
				if (component != null)
				{
					Tag b = GameTagExtensions.Create(this.elementID);
					for (ObjectLayerListItem objectLayerListItem = component.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
					{
						KPrefabID component2 = objectLayerListItem.gameObject.GetComponent<KPrefabID>();
						if (component2.PrefabTag == b)
						{
							PrimaryElement component3 = component2.GetComponent<PrimaryElement>();
							if (component3.Mass + mass <= PrimaryElement.MAX_MASS)
							{
								gameObject = component2.gameObject;
								primaryElement = component3;
								temperature = SimUtil.CalculateFinalTemperature(primaryElement.Mass, primaryElement.Temperature, mass, temperature);
								position = gameObject.transform.GetPosition();
								break;
							}
						}
					}
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.nameTag), Grid.SceneLayer.Ore, null, 0);
			primaryElement = gameObject.GetComponent<PrimaryElement>();
			primaryElement.Mass = mass;
		}
		else
		{
			global::Debug.Assert(primaryElement != null);
			Pickupable component4 = primaryElement.GetComponent<Pickupable>();
			if (component4 != null)
			{
				component4.TotalAmount += mass / primaryElement.MassPerUnit;
			}
			else
			{
				primaryElement.Mass += mass;
			}
		}
		primaryElement.Temperature = temperature;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		gameObject.transform.SetPosition(position);
		if (!manual_activation)
		{
			this.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
		}
		return gameObject;
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x00212050 File Offset: 0x00210250
	public void ActivateSubstanceGameObject(GameObject obj, byte disease_idx, int disease_count)
	{
		obj.SetActive(true);
		obj.GetComponent<PrimaryElement>().AddDisease(disease_idx, disease_count, "Substances.SpawnResource");
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x0021206C File Offset: 0x0021026C
	private void SetTexture(MaterialPropertyBlock block, string texture_name)
	{
		Texture texture = this.material.GetTexture(texture_name);
		if (texture != null)
		{
			this.propertyBlock.SetTexture(texture_name, texture);
		}
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x0021209C File Offset: 0x0021029C
	public void RefreshPropertyBlock()
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		if (this.material != null)
		{
			this.SetTexture(this.propertyBlock, "_MainTex");
			float @float = this.material.GetFloat("_WorldUVScale");
			this.propertyBlock.SetFloat("_WorldUVScale", @float);
			if (ElementLoader.FindElementByHash(this.elementID).IsSolid)
			{
				this.SetTexture(this.propertyBlock, "_MainTex2");
				this.SetTexture(this.propertyBlock, "_HeightTex2");
				this.propertyBlock.SetFloat("_Frequency", this.material.GetFloat("_Frequency"));
				this.propertyBlock.SetColor("_ShineColour", this.material.GetColor("_ShineColour"));
				this.propertyBlock.SetColor("_ColourTint", this.material.GetColor("_ColourTint"));
			}
		}
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x00212197 File Offset: 0x00210397
	internal AmbienceType GetAmbience()
	{
		if (this.audioConfig == null)
		{
			return AmbienceType.None;
		}
		return this.audioConfig.ambienceType;
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x002121AE File Offset: 0x002103AE
	internal SolidAmbienceType GetSolidAmbience()
	{
		if (this.audioConfig == null)
		{
			return SolidAmbienceType.None;
		}
		return this.audioConfig.solidAmbienceType;
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x002121C5 File Offset: 0x002103C5
	internal string GetMiningSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningSound;
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x002121E0 File Offset: 0x002103E0
	internal string GetMiningBreakSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningBreakSound;
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x002121FB File Offset: 0x002103FB
	internal string GetOreBumpSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.oreBumpSound;
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x00212216 File Offset: 0x00210416
	internal string GetFloorEventAudioCategory()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.floorEventAudioCategory;
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x00212231 File Offset: 0x00210431
	internal string GetCreatureChewSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.creatureChewSound;
	}

	// Token: 0x04003CEC RID: 15596
	public string name;

	// Token: 0x04003CED RID: 15597
	public SimHashes elementID;

	// Token: 0x04003CEE RID: 15598
	internal Tag nameTag;

	// Token: 0x04003CEF RID: 15599
	public Color32 colour;

	// Token: 0x04003CF0 RID: 15600
	[FormerlySerializedAs("debugColour")]
	public Color32 uiColour;

	// Token: 0x04003CF1 RID: 15601
	[FormerlySerializedAs("overlayColour")]
	public Color32 conduitColour = Color.white;

	// Token: 0x04003CF2 RID: 15602
	[NonSerialized]
	internal bool renderedByWorld;

	// Token: 0x04003CF3 RID: 15603
	[NonSerialized]
	internal int idx;

	// Token: 0x04003CF4 RID: 15604
	public Material material;

	// Token: 0x04003CF5 RID: 15605
	public KAnimFile anim;

	// Token: 0x04003CF6 RID: 15606
	[SerializeField]
	internal bool showInEditor = true;

	// Token: 0x04003CF7 RID: 15607
	[NonSerialized]
	internal KAnimFile[] anims;

	// Token: 0x04003CF8 RID: 15608
	[NonSerialized]
	internal ElementsAudio.ElementAudioConfig audioConfig;

	// Token: 0x04003CF9 RID: 15609
	[NonSerialized]
	internal MaterialPropertyBlock propertyBlock;

	// Token: 0x04003CFA RID: 15610
	public EventReference fallingStartSound;

	// Token: 0x04003CFB RID: 15611
	public EventReference fallingStopSound;
}
