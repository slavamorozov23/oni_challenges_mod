using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// Token: 0x02000B43 RID: 2883
public static class SerializableOutfitData
{
	// Token: 0x060054D4 RID: 21716 RVA: 0x001EF35C File Offset: 0x001ED55C
	public static int GetVersionFrom(JObject jsonData)
	{
		int result;
		if (jsonData["Version"] == null)
		{
			result = 1;
		}
		else
		{
			result = jsonData.Value<int>("Version");
			jsonData.Remove("Version");
		}
		return result;
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x001EF394 File Offset: 0x001ED594
	public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
	{
		int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
		if (versionFrom == 1)
		{
			return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
		}
		if (versionFrom != 2)
		{
			DebugUtil.DevAssert(false, string.Format("Version {0} of OutfitData is not supported", versionFrom), null);
			return new SerializableOutfitData.Version2();
		}
		return SerializableOutfitData.Version2.FromJson(jsonData);
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x001EF3E1 File Offset: 0x001ED5E1
	public static JObject ToJson(SerializableOutfitData.Version2 data)
	{
		return SerializableOutfitData.Version2.ToJson(data);
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x001EF3EC File Offset: 0x001ED5EC
	public static string ToJsonString(JObject data)
	{
		string result;
		using (StringWriter stringWriter = new StringWriter())
		{
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
				result = stringWriter.ToString();
			}
		}
		return result;
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x001EF44C File Offset: 0x001ED64C
	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
		{
			data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
		}
	}

	// Token: 0x04003951 RID: 14673
	public const string VERSION_KEY = "Version";

	// Token: 0x02001CA3 RID: 7331
	public class Version2
	{
		// Token: 0x0600AE32 RID: 44594 RVA: 0x003D27A0 File Offset: 0x003D09A0
		public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
		{
			Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
			foreach (KeyValuePair<string, string[]> keyValuePair in data.CustomOutfits)
			{
				string text;
				string[] array;
				keyValuePair.Deconstruct(out text, out array);
				string key = text;
				string[] itemIds = array;
				dictionary.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> keyValuePair2 in data.DuplicantOutfits)
			{
				string text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
				keyValuePair2.Deconstruct(out text, out dictionary3);
				string key2 = text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
				Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
				dictionary2[key2] = dictionary5;
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair3 in dictionary4)
				{
					ClothingOutfitUtility.OutfitType outfitType;
					keyValuePair3.Deconstruct(out outfitType, out text);
					ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
					string value = text;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType2), value);
				}
			}
			return new SerializableOutfitData.Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		// Token: 0x0600AE33 RID: 44595 RVA: 0x003D290C File Offset: 0x003D0B0C
		public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
		}

		// Token: 0x0600AE34 RID: 44596 RVA: 0x003D2919 File Offset: 0x003D0B19
		public static JObject ToJson(SerializableOutfitData.Version2 data)
		{
			JObject jobject = JObject.FromObject(data, SerializableOutfitData.Version2.GetSerializer());
			jobject.AddFirst(new JProperty("Version", 2));
			return jobject;
		}

		// Token: 0x0600AE35 RID: 44597 RVA: 0x003D293C File Offset: 0x003D0B3C
		public static JsonSerializer GetSerializer()
		{
			if (SerializableOutfitData.Version2.s_serializer != null)
			{
				return SerializableOutfitData.Version2.s_serializer;
			}
			SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
			SerializableOutfitData.Version2.s_serializer.Converters.Add(new StringEnumConverter());
			return SerializableOutfitData.Version2.s_serializer;
		}

		// Token: 0x040088B0 RID: 34992
		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x040088B1 RID: 34993
		public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();

		// Token: 0x040088B2 RID: 34994
		private static JsonSerializer s_serializer;

		// Token: 0x02002A23 RID: 10787
		public class CustomTemplateOutfitEntry
		{
			// Token: 0x0400BA42 RID: 47682
			public string outfitType;

			// Token: 0x0400BA43 RID: 47683
			public string[] itemIds;
		}
	}

	// Token: 0x02001CA4 RID: 7332
	public class Version1
	{
		// Token: 0x0600AE37 RID: 44599 RVA: 0x003D298C File Offset: 0x003D0B8C
		public static JObject ToJson(SerializableOutfitData.Version1 data)
		{
			return JObject.FromObject(data);
		}

		// Token: 0x0600AE38 RID: 44600 RVA: 0x003D2994 File Offset: 0x003D0B94
		public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
		{
			SerializableOutfitData.Version1 version = new SerializableOutfitData.Version1();
			SerializableOutfitData.Version1 result;
			using (JsonReader jsonReader = jsonData.CreateReader())
			{
				string a = null;
				string b = "DuplicantOutfits";
				string b2 = "CustomOutfits";
				while (jsonReader.Read())
				{
					JsonToken tokenType = jsonReader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						a = jsonReader.Value.ToString();
					}
					if (tokenType == JsonToken.StartObject && a == b)
					{
						ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								string key = jsonReader.Value.ToString();
								while (jsonReader.Read())
								{
									tokenType = jsonReader.TokenType;
									if (tokenType == JsonToken.EndObject)
									{
										break;
									}
									if (tokenType == JsonToken.PropertyName)
									{
										Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out outfitType);
										while (jsonReader.Read())
										{
											tokenType = jsonReader.TokenType;
											if (tokenType == JsonToken.String)
											{
												string value = jsonReader.Value.ToString();
												if (outfitType != ClothingOutfitUtility.OutfitType.LENGTH)
												{
													if (!version.DuplicantOutfits.ContainsKey(key))
													{
														version.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
													}
													version.DuplicantOutfits[key][outfitType] = value;
													break;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					else if (a == b2)
					{
						string text = null;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								text = jsonReader.Value.ToString();
							}
							if (tokenType == JsonToken.StartArray)
							{
								JArray jarray = JArray.Load(jsonReader);
								if (jarray != null)
								{
									string[] array = new string[jarray.Count];
									for (int i = 0; i < jarray.Count; i++)
									{
										array[i] = jarray[i].ToString();
									}
									if (text != null)
									{
										version.CustomOutfits[text] = array;
									}
								}
							}
						}
					}
				}
				result = version;
			}
			return result;
		}

		// Token: 0x040088B3 RID: 34995
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		// Token: 0x040088B4 RID: 34996
		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();
	}
}
