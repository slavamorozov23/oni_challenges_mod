using System;
using UnityEngine;

// Token: 0x02000AFF RID: 2815
public class ResearchType
{
	// Token: 0x060051F0 RID: 20976 RVA: 0x001DBF49 File Offset: 0x001DA149
	public ResearchType(string id, string name, string description, Sprite sprite, Color color, Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription)
	{
		this._id = id;
		this._name = name;
		this._description = description;
		this._sprite = sprite;
		this._color = color;
		this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
	}

	// Token: 0x060051F1 RID: 20977 RVA: 0x001DBF8C File Offset: 0x001DA18C
	public GameObject CreatePrefab(Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription, Color color)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ResearchPointObject>().TypeID = this.id;
		this._recipe = new Recipe(this.id, 1f, (SimHashes)0, this.name, recipeDescription, 0);
		this._recipe.SetFabricators(fabricators, fabricationTime);
		this._recipe.SetIcon(Assets.GetSprite("research_type_icon"), color);
		if (fabricationIngredients != null)
		{
			foreach (Recipe.Ingredient ingredient in fabricationIngredients)
			{
				this._recipe.AddIngredient(ingredient);
			}
		}
		return gameObject;
	}

	// Token: 0x060051F2 RID: 20978 RVA: 0x001DC051 File Offset: 0x001DA251
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060051F3 RID: 20979 RVA: 0x001DC053 File Offset: 0x001DA253
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x060051F4 RID: 20980 RVA: 0x001DC055 File Offset: 0x001DA255
	public string id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x060051F5 RID: 20981 RVA: 0x001DC05D File Offset: 0x001DA25D
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x060051F6 RID: 20982 RVA: 0x001DC065 File Offset: 0x001DA265
	public string description
	{
		get
		{
			return this._description;
		}
	}

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x060051F7 RID: 20983 RVA: 0x001DC06D File Offset: 0x001DA26D
	public string recipe
	{
		get
		{
			return this.recipe;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x060051F8 RID: 20984 RVA: 0x001DC075 File Offset: 0x001DA275
	public Color color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x060051F9 RID: 20985 RVA: 0x001DC07D File Offset: 0x001DA27D
	public Sprite sprite
	{
		get
		{
			return this._sprite;
		}
	}

	// Token: 0x04003771 RID: 14193
	private string _id;

	// Token: 0x04003772 RID: 14194
	private string _name;

	// Token: 0x04003773 RID: 14195
	private string _description;

	// Token: 0x04003774 RID: 14196
	private Recipe _recipe;

	// Token: 0x04003775 RID: 14197
	private Sprite _sprite;

	// Token: 0x04003776 RID: 14198
	private Color _color;
}
