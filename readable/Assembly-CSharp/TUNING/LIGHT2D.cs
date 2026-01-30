using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000FDF RID: 4063
	public class LIGHT2D
	{
		// Token: 0x04005EE3 RID: 24291
		public const int SUNLIGHT_MAX_DEFAULT = 80000;

		// Token: 0x04005EE4 RID: 24292
		public static readonly Color LIGHT_BLUE = new Color(0.38f, 0.61f, 1f, 1f);

		// Token: 0x04005EE5 RID: 24293
		public static readonly Color LIGHT_PURPLE = new Color(0.9f, 0.4f, 0.74f, 1f);

		// Token: 0x04005EE6 RID: 24294
		public static readonly Color LIGHT_PINK = new Color(0.9f, 0.4f, 0.6f, 1f);

		// Token: 0x04005EE7 RID: 24295
		public static readonly Color LIGHT_YELLOW = new Color(0.57f, 0.55f, 0.44f, 1f);

		// Token: 0x04005EE8 RID: 24296
		public static readonly Color LIGHT_OVERLAY = new Color(0.56f, 0.56f, 0.56f, 1f);

		// Token: 0x04005EE9 RID: 24297
		public static readonly Vector2 DEFAULT_DIRECTION = new Vector2(0f, -1f);

		// Token: 0x04005EEA RID: 24298
		public const int FLOORLAMP_LUX = 1000;

		// Token: 0x04005EEB RID: 24299
		public const float FLOORLAMP_RANGE = 4f;

		// Token: 0x04005EEC RID: 24300
		public const float FLOORLAMP_ANGLE = 0f;

		// Token: 0x04005EED RID: 24301
		public const global::LightShape FLOORLAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04005EEE RID: 24302
		public static readonly Color FLOORLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005EEF RID: 24303
		public static readonly Color FLOORLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005EF0 RID: 24304
		public static readonly Vector2 FLOORLAMP_OFFSET = new Vector2(0.05f, 1.5f);

		// Token: 0x04005EF1 RID: 24305
		public static readonly Vector2 FLOORLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005EF2 RID: 24306
		public const float CEILINGLIGHT_RANGE = 8f;

		// Token: 0x04005EF3 RID: 24307
		public const float CEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04005EF4 RID: 24308
		public const global::LightShape CEILINGLIGHT_SHAPE = global::LightShape.Cone;

		// Token: 0x04005EF5 RID: 24309
		public static readonly Color CEILINGLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005EF6 RID: 24310
		public static readonly Color CEILINGLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005EF7 RID: 24311
		public static readonly Vector2 CEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04005EF8 RID: 24312
		public static readonly Vector2 CEILINGLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005EF9 RID: 24313
		public const int CEILINGLIGHT_LUX = 1800;

		// Token: 0x04005EFA RID: 24314
		public const float FOSSILSCULPTURE_RANGE = 8f;

		// Token: 0x04005EFB RID: 24315
		public const float FOSSILSCULPTURE_CEILING_RANGE = 8f;

		// Token: 0x04005EFC RID: 24316
		public const float FOSSILSCULPTURE_ANGLE = 0f;

		// Token: 0x04005EFD RID: 24317
		public const float FOSSILSCULPTURE_CEILING_ANGLE = 2.6f;

		// Token: 0x04005EFE RID: 24318
		public const int FOSSILSCULPTURE_LIGHT_WIDTH = 3;

		// Token: 0x04005EFF RID: 24319
		public const DiscreteShadowCaster.Direction FOSSILSCULPTURE_LIGHT_DIRECTION = DiscreteShadowCaster.Direction.North;

		// Token: 0x04005F00 RID: 24320
		public const DiscreteShadowCaster.Direction FOSSILSCULPTURE_CEILING_LIGHT_DIRECTION = DiscreteShadowCaster.Direction.South;

		// Token: 0x04005F01 RID: 24321
		public const global::LightShape FOSSILSCULPTURE_SHAPE = global::LightShape.Quad;

		// Token: 0x04005F02 RID: 24322
		public const global::LightShape FOSSILSCULPTURE_CEILING_SHAPE = global::LightShape.Quad;

		// Token: 0x04005F03 RID: 24323
		public static readonly Color FOSSILSCULPTURE_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F04 RID: 24324
		public static readonly Color FOSSILSCULPTURE_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F05 RID: 24325
		public static readonly Vector2 FOSSILSCULPTURE_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04005F06 RID: 24326
		public static readonly Vector2 FOSSILSCULPTURE_CEILING_OFFSET = new Vector2(0.05f, 1.65f);

		// Token: 0x04005F07 RID: 24327
		public static readonly Vector2 FOSSILSCULPTURE_DIRECTION = Vector2.up;

		// Token: 0x04005F08 RID: 24328
		public static readonly Vector2 FOSSILSCULPTURE_CEILING_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F09 RID: 24329
		public const int FOSSILSCULPTURE_LUX = 3000;

		// Token: 0x04005F0A RID: 24330
		public static readonly int SUNLAMP_LUX = (int)((float)BeachChairConfig.TAN_LUX * 4f);

		// Token: 0x04005F0B RID: 24331
		public const float SUNLAMP_RANGE = 16f;

		// Token: 0x04005F0C RID: 24332
		public const float SUNLAMP_ANGLE = 5.2f;

		// Token: 0x04005F0D RID: 24333
		public const global::LightShape SUNLAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04005F0E RID: 24334
		public static readonly Color SUNLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F0F RID: 24335
		public static readonly Color SUNLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F10 RID: 24336
		public static readonly Vector2 SUNLAMP_OFFSET = new Vector2(0f, 3.5f);

		// Token: 0x04005F11 RID: 24337
		public static readonly Vector2 SUNLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F12 RID: 24338
		public const int MERCURYCEILINGLIGHT_LUX = 60000;

		// Token: 0x04005F13 RID: 24339
		public const float MERCURYCEILINGLIGHT_RANGE = 8f;

		// Token: 0x04005F14 RID: 24340
		public const float MERCURYCEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04005F15 RID: 24341
		public const float MERCURYCEILINGLIGHT_FALLOFFRATE = 0.4f;

		// Token: 0x04005F16 RID: 24342
		public const int MERCURYCEILINGLIGHT_WIDTH = 3;

		// Token: 0x04005F17 RID: 24343
		public const global::LightShape MERCURYCEILINGLIGHT_SHAPE = global::LightShape.Quad;

		// Token: 0x04005F18 RID: 24344
		public static readonly Color MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F19 RID: 24345
		public static readonly Color MERCURYCEILINGLIGHT_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04005F1A RID: 24346
		public static readonly Vector2 MERCURYCEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04005F1B RID: 24347
		public static readonly Vector2 MERCURYCEILINGLIGHT_DIRECTIONVECTOR = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F1C RID: 24348
		public const DiscreteShadowCaster.Direction MERCURYCEILINGLIGHT_DIRECTION = DiscreteShadowCaster.Direction.South;

		// Token: 0x04005F1D RID: 24349
		public static readonly Color LIGHT_PREVIEW_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F1E RID: 24350
		public const float HEADQUARTERS_RANGE = 5f;

		// Token: 0x04005F1F RID: 24351
		public const global::LightShape HEADQUARTERS_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F20 RID: 24352
		public static readonly Color HEADQUARTERS_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F21 RID: 24353
		public static readonly Color HEADQUARTERS_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F22 RID: 24354
		public static readonly Vector2 HEADQUARTERS_OFFSET = new Vector2(0.5f, 3f);

		// Token: 0x04005F23 RID: 24355
		public static readonly Vector2 EXOBASE_HEADQUARTERS_OFFSET = new Vector2(0f, 2.5f);

		// Token: 0x04005F24 RID: 24356
		public const float POI_TECH_UNLOCK_RANGE = 5f;

		// Token: 0x04005F25 RID: 24357
		public const float POI_TECH_UNLOCK_ANGLE = 2.6f;

		// Token: 0x04005F26 RID: 24358
		public const global::LightShape POI_TECH_UNLOCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04005F27 RID: 24359
		public static readonly Color POI_TECH_UNLOCK_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F28 RID: 24360
		public static readonly Color POI_TECH_UNLOCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F29 RID: 24361
		public static readonly Vector2 POI_TECH_UNLOCK_OFFSET = new Vector2(0f, 3.4f);

		// Token: 0x04005F2A RID: 24362
		public const int POI_TECH_UNLOCK_LUX = 1800;

		// Token: 0x04005F2B RID: 24363
		public static readonly Vector2 POI_TECH_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F2C RID: 24364
		public const float ENGINE_RANGE = 10f;

		// Token: 0x04005F2D RID: 24365
		public const global::LightShape ENGINE_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F2E RID: 24366
		public const int ENGINE_LUX = 80000;

		// Token: 0x04005F2F RID: 24367
		public const float WALLLIGHT_RANGE = 4f;

		// Token: 0x04005F30 RID: 24368
		public const float WALLLIGHT_ANGLE = 0f;

		// Token: 0x04005F31 RID: 24369
		public const global::LightShape WALLLIGHT_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F32 RID: 24370
		public static readonly Color WALLLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F33 RID: 24371
		public static readonly Color WALLLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F34 RID: 24372
		public static readonly Vector2 WALLLIGHT_OFFSET = new Vector2(0f, 0.5f);

		// Token: 0x04005F35 RID: 24373
		public static readonly Vector2 WALLLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F36 RID: 24374
		public const float LIGHTBUG_RANGE = 5f;

		// Token: 0x04005F37 RID: 24375
		public const float LIGHTBUG_ANGLE = 0f;

		// Token: 0x04005F38 RID: 24376
		public const global::LightShape LIGHTBUG_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F39 RID: 24377
		public const int LIGHTBUG_LUX = 1800;

		// Token: 0x04005F3A RID: 24378
		public static readonly Color LIGHTBUG_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F3B RID: 24379
		public static readonly Color LIGHTBUG_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F3C RID: 24380
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(0.5686275f, 0.48235294f, 0.4392157f, 1f);

		// Token: 0x04005F3D RID: 24381
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(0.49019608f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04005F3E RID: 24382
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(0.5686275f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04005F3F RID: 24383
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(0.4392157f, 0.4862745f, 0.5686275f, 1f);

		// Token: 0x04005F40 RID: 24384
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(0.5137255f, 0.6666667f, 0.6666667f, 1f);

		// Token: 0x04005F41 RID: 24385
		public static readonly Color LIGHTBUG_COLOR_GREEN = new Color(0.43137255f, 1f, 0.53333336f, 1f);

		// Token: 0x04005F42 RID: 24386
		public const int MAJORFOSSILDIGSITE_LAMP_LUX = 1000;

		// Token: 0x04005F43 RID: 24387
		public const float MAJORFOSSILDIGSITE_LAMP_RANGE = 3f;

		// Token: 0x04005F44 RID: 24388
		public static readonly Vector2 MAJORFOSSILDIGSITE_LAMP_OFFSET = new Vector2(-0.15f, 2.35f);

		// Token: 0x04005F45 RID: 24389
		public static readonly Vector2 LIGHTBUG_OFFSET = new Vector2(0.05f, 0.25f);

		// Token: 0x04005F46 RID: 24390
		public static readonly Vector2 LIGHTBUG_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F47 RID: 24391
		public const int PLASMALAMP_LUX = 666;

		// Token: 0x04005F48 RID: 24392
		public const float PLASMALAMP_RANGE = 2f;

		// Token: 0x04005F49 RID: 24393
		public const float PLASMALAMP_ANGLE = 0f;

		// Token: 0x04005F4A RID: 24394
		public const global::LightShape PLASMALAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F4B RID: 24395
		public static readonly Color PLASMALAMP_COLOR = LIGHT2D.LIGHT_PURPLE;

		// Token: 0x04005F4C RID: 24396
		public static readonly Color PLASMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F4D RID: 24397
		public static readonly Vector2 PLASMALAMP_OFFSET = new Vector2(0.05f, 0.5f);

		// Token: 0x04005F4E RID: 24398
		public static readonly Vector2 PLASMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F4F RID: 24399
		public const int MAGMALAMP_LUX = 666;

		// Token: 0x04005F50 RID: 24400
		public const float MAGMALAMP_RANGE = 2f;

		// Token: 0x04005F51 RID: 24401
		public const float MAGMALAMP_ANGLE = 0f;

		// Token: 0x04005F52 RID: 24402
		public const global::LightShape MAGMALAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04005F53 RID: 24403
		public static readonly Color MAGMALAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005F54 RID: 24404
		public static readonly Color MAGMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F55 RID: 24405
		public static readonly Vector2 MAGMALAMP_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005F56 RID: 24406
		public static readonly Vector2 MAGMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F57 RID: 24407
		public const int BIOLUMROCK_LUX = 666;

		// Token: 0x04005F58 RID: 24408
		public const float BIOLUMROCK_RANGE = 2f;

		// Token: 0x04005F59 RID: 24409
		public const float BIOLUMROCK_ANGLE = 0f;

		// Token: 0x04005F5A RID: 24410
		public const global::LightShape BIOLUMROCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04005F5B RID: 24411
		public static readonly Color BIOLUMROCK_COLOR = LIGHT2D.LIGHT_BLUE;

		// Token: 0x04005F5C RID: 24412
		public static readonly Color BIOLUMROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F5D RID: 24413
		public static readonly Vector2 BIOLUMROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005F5E RID: 24414
		public static readonly Vector2 BIOLUMROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005F5F RID: 24415
		public const float PINKROCK_RANGE = 2f;

		// Token: 0x04005F60 RID: 24416
		public const float PINKROCK_ANGLE = 0f;

		// Token: 0x04005F61 RID: 24417
		public const global::LightShape PINKROCK_SHAPE = global::LightShape.Circle;

		// Token: 0x04005F62 RID: 24418
		public static readonly Color PINKROCK_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04005F63 RID: 24419
		public static readonly Color PINKROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005F64 RID: 24420
		public static readonly Vector2 PINKROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005F65 RID: 24421
		public static readonly Vector2 PINKROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;
	}
}
