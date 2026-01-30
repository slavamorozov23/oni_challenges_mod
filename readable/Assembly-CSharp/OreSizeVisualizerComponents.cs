using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A84 RID: 2692
public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
	// Token: 0x06004E38 RID: 20024 RVA: 0x001C72C8 File Offset: 0x001C54C8
	public HandleVector<int>.Handle Add(GameObject go)
	{
		HandleVector<int>.Handle handle = base.Add(go, new OreSizeVisualizerData(go));
		this.OnPrefabInit(handle);
		return handle;
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001C72EB File Offset: 0x001C54EB
	public static HashedString GetAnimForMass(float mass)
	{
		return OreSizeVisualizerComponents.GetAnimForMass(OreSizeVisualizerComponents.TiersSetType.Ores, mass);
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001C72F4 File Offset: 0x001C54F4
	public static HashedString GetAnimForMass(OreSizeVisualizerComponents.TiersSetType tierType, float mass)
	{
		for (int i = 0; i < OreSizeVisualizerComponents.TierSets[tierType].Length; i++)
		{
			if (mass <= OreSizeVisualizerComponents.TierSets[tierType][i].massRequired)
			{
				return OreSizeVisualizerComponents.TierSets[tierType][i].animName;
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001C7350 File Offset: 0x001C5550
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		data.absorbHandle = data.primaryElement.Subscribe(-2064133523, OreSizeVisualizerComponents.OnMassChangedDispatcher, handle);
		data.splitFromChunkHandle = data.primaryElement.Subscribe(1335436905, OreSizeVisualizerComponents.OnMassChangedDispatcher, handle);
		base.SetData(handle, data);
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001C73B1 File Offset: 0x001C55B1
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerComponents.OnMassChanged(handle, null);
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x001C73BC File Offset: 0x001C55BC
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		if (data.primaryElement != null)
		{
			data.primaryElement.Unsubscribe(ref data.absorbHandle);
			data.primaryElement.Unsubscribe(ref data.splitFromChunkHandle);
		}
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x001C7404 File Offset: 0x001C5604
	private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
	{
		OreSizeVisualizerData data = GameComps.OreSizeVisualizers.GetData(handle);
		PrimaryElement primaryElement = data.primaryElement;
		float mass = primaryElement.Mass;
		OreSizeVisualizerComponents.MassTier massTier = default(OreSizeVisualizerComponents.MassTier);
		OreSizeVisualizerComponents.MassTier[] array = OreSizeVisualizerComponents.TierSets[data.tierSetType];
		for (int i = 0; i < array.Length; i++)
		{
			if (mass <= array[i].massRequired)
			{
				massTier = array[i];
				break;
			}
		}
		primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName, KAnim.PlayMode.Once, 1f, 0f);
		KCircleCollider2D component = primaryElement.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			component.radius = massTier.colliderRadius;
		}
		primaryElement.Trigger(1807976145, null);
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x001C74C4 File Offset: 0x001C56C4
	// Note: this type is marked as 'beforefieldinit'.
	static OreSizeVisualizerComponents()
	{
		Dictionary<OreSizeVisualizerComponents.TiersSetType, OreSizeVisualizerComponents.MassTier[]> dictionary = new Dictionary<OreSizeVisualizerComponents.TiersSetType, OreSizeVisualizerComponents.MassTier[]>();
		dictionary[OreSizeVisualizerComponents.TiersSetType.Ores] = OreSizeVisualizerComponents.MassTiers;
		dictionary[OreSizeVisualizerComponents.TiersSetType.PokeShells] = OreSizeVisualizerComponents.PokeShellMassTiers;
		dictionary[OreSizeVisualizerComponents.TiersSetType.WoodPokeShells] = OreSizeVisualizerComponents.WoodPokeShellMassTiers;
		dictionary[OreSizeVisualizerComponents.TiersSetType.PlantFiber] = OreSizeVisualizerComponents.PlantMatterMassTiers;
		OreSizeVisualizerComponents.TierSets = dictionary;
		OreSizeVisualizerComponents.OnMassChangedDispatcher = delegate(object context, object data)
		{
			OreSizeVisualizerComponents.OnMassChanged((HandleVector<int>.Handle)context, data);
		};
	}

	// Token: 0x04003420 RID: 13344
	private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 50f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 600f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x04003421 RID: 13345
	private static readonly OreSizeVisualizerComponents.MassTier[] PokeShellMassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 7.5f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 15f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x04003422 RID: 13346
	private static readonly OreSizeVisualizerComponents.MassTier[] WoodPokeShellMassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 75f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 150f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x04003423 RID: 13347
	private static readonly OreSizeVisualizerComponents.MassTier[] PlantMatterMassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 10f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 50f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x04003424 RID: 13348
	private static readonly Dictionary<OreSizeVisualizerComponents.TiersSetType, OreSizeVisualizerComponents.MassTier[]> TierSets;

	// Token: 0x04003425 RID: 13349
	private static Action<object, object> OnMassChangedDispatcher;

	// Token: 0x02001B9D RID: 7069
	private struct MassTier
	{
		// Token: 0x04008558 RID: 34136
		public HashedString animName;

		// Token: 0x04008559 RID: 34137
		public float massRequired;

		// Token: 0x0400855A RID: 34138
		public float colliderRadius;
	}

	// Token: 0x02001B9E RID: 7070
	public enum TiersSetType
	{
		// Token: 0x0400855C RID: 34140
		Ores,
		// Token: 0x0400855D RID: 34141
		PokeShells,
		// Token: 0x0400855E RID: 34142
		WoodPokeShells,
		// Token: 0x0400855F RID: 34143
		PlantFiber
	}
}
