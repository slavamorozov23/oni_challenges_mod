using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000835 RID: 2101
[AddComponentMenu("KMonoBehaviour/scripts/CO2Manager")]
public class CO2Manager : KMonoBehaviour, ISim33ms
{
	// Token: 0x0600394E RID: 14670 RVA: 0x0013FF65 File Offset: 0x0013E165
	public static void DestroyInstance()
	{
		CO2Manager.instance = null;
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x0013FF70 File Offset: 0x0013E170
	protected override void OnPrefabInit()
	{
		CO2Manager.instance = this;
		this.prefab.gameObject.SetActive(false);
		this.breathPrefab.SetActive(false);
		this.exhaustPrefab.SetActive(false);
		this.co2Pool = new GameObjectPool(new Func<GameObject>(this.InstantiateCO2), new Action<GameObject>(CO2Manager.Deactivate), 16);
		this.breathPool = new GameObjectPool(new Func<GameObject>(this.InstantiateBreath), new Action<GameObject>(CO2Manager.Deactivate), 16);
		this.exhaustPool = new GameObjectPool(new Func<GameObject>(this.InstantiateExhaust), new Action<GameObject>(CO2Manager.Deactivate), 16);
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x0014001B File Offset: 0x0013E21B
	private GameObject InstantiateCO2()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.prefab, Grid.SceneLayer.Front, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x00140033 File Offset: 0x0013E233
	private static void Deactivate(GameObject _)
	{
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x00140035 File Offset: 0x0013E235
	private GameObject InstantiateBreath()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.breathPrefab, Grid.SceneLayer.Front, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x0014004D File Offset: 0x0013E24D
	private GameObject InstantiateExhaust()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.exhaustPrefab, Grid.SceneLayer.Front, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x00140068 File Offset: 0x0013E268
	public void Sim33ms(float dt)
	{
		Vector2I vector2I = default(Vector2I);
		Vector2I vector2I2 = default(Vector2I);
		Vector3 b = this.acceleration * dt;
		int num = this.co2Items.Count;
		for (int i = 0; i < num; i++)
		{
			CO2 co = this.co2Items[i];
			co.velocity += b;
			co.lifetimeRemaining -= dt;
			Grid.PosToXY(co.transform.GetPosition(), out vector2I);
			co.transform.SetPosition(co.transform.GetPosition() + co.velocity * dt);
			Grid.PosToXY(co.transform.GetPosition(), out vector2I2);
			int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
			for (int j = vector2I.y; j >= vector2I2.y; j--)
			{
				int num3 = Grid.XYToCell(vector2I.x, j);
				bool flag = !Grid.IsValidCell(num3) || co.lifetimeRemaining <= 0f;
				if (!flag)
				{
					Element element = Grid.Element[num3];
					flag = (element.IsLiquid || element.IsSolid || (Grid.Properties[num3] & 1) > 0);
				}
				if (flag)
				{
					int gameCell = num3;
					bool flag2 = false;
					if (num2 != num3)
					{
						gameCell = num2;
						flag2 = true;
					}
					else
					{
						bool flag3 = false;
						int num4 = -1;
						int num5 = -1;
						foreach (CellOffset offset in GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS)
						{
							int num6 = Grid.OffsetCell(num3, offset);
							if (Grid.IsValidCell(num6))
							{
								Element element2 = Grid.Element[num6];
								if (element2.id == SimHashes.CarbonDioxide || element2.HasTag(GameTags.Breathable))
								{
									num4 = num6;
									flag3 = true;
									flag2 = true;
									break;
								}
								if (element2.IsGas)
								{
									num5 = num6;
									flag2 = true;
								}
							}
						}
						if (flag2)
						{
							if (flag3)
							{
								gameCell = num4;
							}
							else
							{
								gameCell = num5;
							}
						}
					}
					if (flag2)
					{
						co.TriggerDestroy();
						SimMessages.ModifyMass(gameCell, co.mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, co.temperature, SimHashes.CarbonDioxide);
						num--;
						this.co2Items[i] = this.co2Items[num];
						this.co2Items.RemoveAt(num);
						break;
					}
				}
				num2 = num3;
			}
		}
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x001402E7 File Offset: 0x0013E4E7
	public CO2 SpawnCO2(Vector3 position, float mass, float temperature, bool flip)
	{
		return this.SpawnCO2(position, mass, temperature, flip, 0f);
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x001402FC File Offset: 0x0013E4FC
	public CO2 SpawnCO2(Vector3 position, float mass, float temperature, bool flip, float rotation)
	{
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		GameObject gameObject = this.co2Pool.GetInstance();
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		CO2 component = gameObject.GetComponent<CO2>();
		component.mass = mass;
		component.temperature = temperature;
		component.velocity = Vector3.zero;
		component.lifetimeRemaining = 3f;
		KBatchedAnimController component2 = component.GetComponent<KBatchedAnimController>();
		component2.TintColour = this.tintColour;
		component2.onDestroySelf = new Action<GameObject>(this.OnDestroyCO2);
		component2.FlipX = flip;
		component.StartLoop();
		this.co2Items.Add(component);
		return component;
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x001403A4 File Offset: 0x0013E5A4
	public void SpawnBreath(Vector3 position, float mass, float temperature, bool flip)
	{
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		this.SpawnCO2(position, mass, temperature, flip);
		GameObject gameObject = this.breathPool.GetInstance();
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.TintColour = this.tintColour;
		component.onDestroySelf = new Action<GameObject>(this.OnDestroyBreath);
		component.FlipX = flip;
		component.Play("breath", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x00140434 File Offset: 0x0013E634
	public void SpawnExhaust(Vector3 position, Vector3 velocity, int co2Cell, float mass, float temperature)
	{
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		float rotation = Mathf.Repeat(Vector3.Angle(Vector3.down, velocity) * Mathf.Sign(velocity.x), 360f);
		SimMessages.ModifyMass(co2Cell, mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, temperature, SimHashes.CarbonDioxide);
		GameObject gameObject = this.exhaustPool.GetInstance();
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		CO2 co = gameObject.AddOrGet<CO2>();
		co.mass = mass;
		co.temperature = temperature;
		co.lifetimeRemaining = 3f;
		co.affectedByGravity = false;
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.onDestroySelf = new Action<GameObject>(this.OnDestroyExhaust);
		component.Rotation = rotation;
		component.Play("smoke_particle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x0014050E File Offset: 0x0013E70E
	private void OnDestroyCO2(GameObject co2_go)
	{
		co2_go.SetActive(false);
		this.co2Pool.ReleaseInstance(co2_go);
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x00140523 File Offset: 0x0013E723
	private void OnDestroyBreath(GameObject breath_go)
	{
		breath_go.SetActive(false);
		this.breathPool.ReleaseInstance(breath_go);
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x00140538 File Offset: 0x0013E738
	private void OnDestroyExhaust(GameObject go)
	{
		go.SetActive(false);
		this.exhaustPool.ReleaseInstance(go);
	}

	// Token: 0x040022FD RID: 8957
	private const float CO2Lifetime = 3f;

	// Token: 0x040022FE RID: 8958
	[SerializeField]
	private Vector3 acceleration;

	// Token: 0x040022FF RID: 8959
	[SerializeField]
	private CO2 prefab;

	// Token: 0x04002300 RID: 8960
	[SerializeField]
	private GameObject breathPrefab;

	// Token: 0x04002301 RID: 8961
	[SerializeField]
	private GameObject exhaustPrefab;

	// Token: 0x04002302 RID: 8962
	[SerializeField]
	private Color tintColour;

	// Token: 0x04002303 RID: 8963
	private List<CO2> co2Items = new List<CO2>();

	// Token: 0x04002304 RID: 8964
	private GameObjectPool breathPool;

	// Token: 0x04002305 RID: 8965
	private GameObjectPool exhaustPool;

	// Token: 0x04002306 RID: 8966
	private GameObjectPool co2Pool;

	// Token: 0x04002307 RID: 8967
	public static CO2Manager instance;
}
