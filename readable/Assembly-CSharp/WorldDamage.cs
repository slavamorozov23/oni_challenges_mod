using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;

// Token: 0x02000C26 RID: 3110
[AddComponentMenu("KMonoBehaviour/scripts/WorldDamage")]
public class WorldDamage : KMonoBehaviour
{
	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06005DE2 RID: 24034 RVA: 0x0021F648 File Offset: 0x0021D848
	// (set) Token: 0x06005DE3 RID: 24035 RVA: 0x0021F64F File Offset: 0x0021D84F
	public static WorldDamage Instance { get; private set; }

	// Token: 0x06005DE4 RID: 24036 RVA: 0x0021F657 File Offset: 0x0021D857
	public static void DestroyInstance()
	{
		WorldDamage.Instance = null;
	}

	// Token: 0x06005DE5 RID: 24037 RVA: 0x0021F65F File Offset: 0x0021D85F
	protected override void OnPrefabInit()
	{
		WorldDamage.Instance = this;
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x0021F667 File Offset: 0x0021D867
	public void RestoreDamageToValue(int cell, float amount)
	{
		if (Grid.Damage[cell] > amount)
		{
			Grid.Damage[cell] = amount;
		}
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x0021F67B File Offset: 0x0021D87B
	public float ApplyDamage(Sim.WorldDamageInfo damage_info)
	{
		return this.ApplyDamage(damage_info.gameCell, this.damageAmount, damage_info.damageSourceOffset, BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x0021F6AC File Offset: 0x0021D8AC
	public float ApplyDamage(int cell, float amount, int src_cell, WorldDamage.DamageType damageType, string source_name = null, string pop_text = null)
	{
		float result = 0f;
		if (Grid.Solid[cell])
		{
			float num = Grid.Damage[cell];
			result = Mathf.Min(amount, 1f - num);
			num += amount;
			bool flag = num > 0.15f;
			if (flag && damageType != WorldDamage.DamageType.NoBuildingDamage)
			{
				GameObject gameObject = Grid.Objects[cell, 9];
				if (gameObject != null)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						if (!component.invincible)
						{
							int damage = Mathf.RoundToInt(Mathf.Max((float)component.HitPoints - (1f - num) * (float)component.MaxHitPoints, 0f));
							gameObject.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
							{
								damage = damage,
								source = source_name,
								popString = pop_text
							});
						}
						else
						{
							num = 0f;
						}
					}
				}
			}
			Grid.Damage[cell] = Mathf.Min(1f, num);
			if (Grid.Damage[cell] >= 1f)
			{
				this.DestroyCell(cell);
			}
			else if (Grid.IsValidCell(src_cell) && flag)
			{
				Element element = Grid.Element[src_cell];
				if (element.IsLiquid && Grid.Mass[src_cell] > 1f)
				{
					int num2 = cell - src_cell;
					if (num2 == 1 || num2 == -1 || num2 == Grid.WidthInCells || num2 == -Grid.WidthInCells)
					{
						int num3 = cell + num2;
						if (Grid.IsValidCell(num3))
						{
							Element element2 = Grid.Element[num3];
							if (!element2.IsSolid && (!element2.IsLiquid || (element2.id == element.id && Grid.Mass[num3] <= 100f)) && (Grid.Properties[num3] & 2) == 0 && !this.spawnTimes.ContainsKey(num3))
							{
								this.spawnTimes[num3] = Time.realtimeSinceStartup;
								ushort idx = element.idx;
								float temperature = Grid.Temperature[src_cell];
								base.StartCoroutine(this.DelayedSpawnFX(src_cell, num3, num2, element, idx, temperature));
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x0021F8D2 File Offset: 0x0021DAD2
	public float ApplyDamage(int cell, float amount, int src_cell, string source_name = null, string pop_text = null)
	{
		return this.ApplyDamage(cell, amount, src_cell, WorldDamage.DamageType.Absolute, source_name, pop_text);
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x0021F8E2 File Offset: 0x0021DAE2
	private void ReleaseGO(GameObject go)
	{
		go.DeleteObject();
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x0021F8EA File Offset: 0x0021DAEA
	private IEnumerator DelayedSpawnFX(int src_cell, int dest_cell, int offset, Element elem, ushort idx, float temperature)
	{
		float seconds = UnityEngine.Random.value * 0.25f;
		yield return new WaitForSeconds(seconds);
		Vector3 position = Grid.CellToPosCCC(dest_cell, Grid.SceneLayer.Front);
		GameObject gameObject = GameUtil.KInstantiate(this.leakEffect.gameObject, position, Grid.SceneLayer.Front, null, 0);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.TintColour = elem.substance.colour;
		component.onDestroySelf = new Action<GameObject>(this.ReleaseGO);
		SimMessages.AddRemoveSubstance(src_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, -1f, temperature, byte.MaxValue, 0, true, -1);
		if (offset == -1)
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.FlipX = true;
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else if (offset == Grid.WidthInCells)
		{
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.up * 0.5f);
			component.Play("floor", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			SimMessages.AddRemoveSubstance(dest_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, 1f, temperature, byte.MaxValue, 0, true, -1);
		}
		else if (offset == -Grid.WidthInCells)
		{
			component.Play("ceiling", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.up * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		if (CameraController.Instance.IsAudibleSound(gameObject.transform.GetPosition(), this.leakSoundMigrated))
		{
			SoundEvent.PlayOneShot(this.leakSoundMigrated, gameObject.transform.GetPosition(), 1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x0021F928 File Offset: 0x0021DB28
	private void Update()
	{
		this.expiredCells.Clear();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (KeyValuePair<int, float> keyValuePair in this.spawnTimes)
		{
			if (realtimeSinceStartup - keyValuePair.Value > 1f)
			{
				this.expiredCells.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.expiredCells)
		{
			this.spawnTimes.Remove(key);
		}
		this.expiredCells.Clear();
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x0021F9FC File Offset: 0x0021DBFC
	public void DestroyCell(int cell)
	{
		if (Grid.Solid[cell])
		{
			SimMessages.Dig(cell, -1, false);
		}
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x0021FA13 File Offset: 0x0021DC13
	public void OnSolidStateChanged(int cell)
	{
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x0021FA24 File Offset: 0x0021DC24
	public void OnDigComplete(int cell, float mass, float temperature, ushort element_idx, byte disease_idx, int disease_count)
	{
		Vector3 vector = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
		Element element = ElementLoader.elements[(int)element_idx];
		Grid.Damage[cell] = 0f;
		WorldDamage.Instance.PlaySoundForSubstance(element, vector);
		float num = mass * 0.5f;
		if (num <= 0f)
		{
			return;
		}
		GameObject gameObject = element.substance.SpawnResource(vector, num, temperature, disease_idx, disease_count, false, false, false);
		Pickupable component = gameObject.GetComponent<Pickupable>();
		if (component != null && component.GetMyWorld() != null && component.GetMyWorld().worldInventory.IsReachable(component))
		{
			PopFXManager.Instance.SpawnFX(Def.GetUISprite(element, "ui", false).first, PopFXManager.Instance.sprite_Plus, Mathf.RoundToInt(num).ToString() + " " + element.name, gameObject.transform, Vector3.zero, 1.5f, true, false, false);
		}
	}

	// Token: 0x06005DF0 RID: 24048 RVA: 0x0021FB18 File Offset: 0x0021DD18
	private void PlaySoundForSubstance(Element element, Vector3 pos)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "Break_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), 1f);
		}
	}

	// Token: 0x04003E77 RID: 15991
	public KBatchedAnimController leakEffect;

	// Token: 0x04003E78 RID: 15992
	[SerializeField]
	private FMODAsset leakSound;

	// Token: 0x04003E79 RID: 15993
	[SerializeField]
	private EventReference leakSoundMigrated;

	// Token: 0x04003E7A RID: 15994
	private float damageAmount = 0.00083333335f;

	// Token: 0x04003E7C RID: 15996
	private const float SPAWN_DELAY = 1f;

	// Token: 0x04003E7D RID: 15997
	private Dictionary<int, float> spawnTimes = new Dictionary<int, float>();

	// Token: 0x04003E7E RID: 15998
	private List<int> expiredCells = new List<int>();

	// Token: 0x02001DBC RID: 7612
	public enum DamageType
	{
		// Token: 0x04008C18 RID: 35864
		Absolute,
		// Token: 0x04008C19 RID: 35865
		NoBuildingDamage
	}
}
