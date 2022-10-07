using System;
using System.Collections.Generic;

namespace Mother4.Actors.Animation
{
	internal static class AnimationNames
	{
		public static string GetString(AnimationType type)
		{
			string result;
			if (!AnimationNames.ANIMTYPE_TO_STRING.TryGetValue(type, out result))
			{
				result = "stand south";
			}
			return result;
		}

		public static AnimationType GetAnimationType(string name)
		{
			AnimationType result;
			if (!AnimationNames.STRING_TO_ANIMTYPE.TryGetValue(name, out result))
			{
				result = AnimationType.INVALID;
			}
			return result;
		}

		public const string EAST = "east";

		public const string NORTHEAST = "northeast";

		public const string NORTH = "north";

		public const string NORTHWEST = "northwest";

		public const string WEST = "west";

		public const string SOUTHWEST = "southwest";

		public const string SOUTH = "south";

		public const string SOUTHEAST = "southeast";

		public const string STAND = "stand";

		public const string WALK = "walk";

		public const string RUN = "run";

		public const string CROUCH = "crouch";

		public const string DEAD = "dead";

		public const string IDLE = "idle";

		public const string TALK = "talk";

		public const string BLINK = "blink";

		public const string CLIMB = "climb";

		public const string SWIM = "swim";

		public const string FLOAT = "float";

		public const string STAND_EAST = "stand east";

		public const string STAND_NORTHEAST = "stand northeast";

		public const string STAND_NORTH = "stand north";

		public const string STAND_NORTHWEST = "stand northwest";

		public const string STAND_WEST = "stand west";

		public const string STAND_SOUTHWEST = "stand southwest";

		public const string STAND_SOUTH = "stand south";

		public const string STAND_SOUTHEAST = "stand southeast";

		public const string WALK_EAST = "walk east";

		public const string WALK_NORTHEAST = "walk northeast";

		public const string WALK_NORTH = "walk north";

		public const string WALK_NORTHWEST = "walk northwest";

		public const string WALK_WEST = "walk west";

		public const string WALK_SOUTHWEST = "walk southwest";

		public const string WALK_SOUTH = "walk south";

		public const string WALK_SOUTHEAST = "walk southeast";

		public const string RUN_EAST = "run east";

		public const string RUN_NORTHEAST = "run northeast";

		public const string RUN_NORTH = "run north";

		public const string RUN_NORTHWEST = "run northwest";

		public const string RUN_WEST = "run west";

		public const string RUN_SOUTHWEST = "run southwest";

		public const string RUN_SOUTH = "run south";

		public const string RUN_SOUTHEAST = "run southeast";

		public const string CROUCH_EAST = "crouch east";

		public const string CROUCH_NORTHEAST = "crouch northeast";

		public const string CROUCH_NORTH = "crouch north";

		public const string CROUCH_NORTHWEST = "crouch northwest";

		public const string CROUCH_WEST = "crouch west";

		public const string CROUCH_SOUTHWEST = "crouch southwest";

		public const string CROUCH_SOUTH = "crouch south";

		public const string CROUCH_SOUTHEAST = "crouch southeast";

		public const string DEAD_EAST = "dead east";

		public const string DEAD_NORTHEAST = "dead northeast";

		public const string DEAD_NORTH = "dead north";

		public const string DEAD_NORTHWEST = "dead northwest";

		public const string DEAD_WEST = "dead west";

		public const string DEAD_SOUTHWEST = "dead southwest";

		public const string DEAD_SOUTH = "dead south";

		public const string DEAD_SOUTHEAST = "dead southeast";

		public const string IDLE_EAST = "idle east";

		public const string IDLE_NORTHEAST = "idle northeast";

		public const string IDLE_NORTH = "idle north";

		public const string IDLE_NORTHWEST = "idle northwest";

		public const string IDLE_WEST = "idle west";

		public const string IDLE_SOUTHWEST = "idle southwest";

		public const string IDLE_SOUTH = "idle south";

		public const string IDLE_SOUTHEAST = "idle southeast";

		public const string TALK_EAST = "talk east";

		public const string TALK_NORTHEAST = "talk northeast";

		public const string TALK_NORTH = "talk north";

		public const string TALK_NORTHWEST = "talk northwest";

		public const string TALK_WEST = "talk west";

		public const string TALK_SOUTHWEST = "talk southwest";

		public const string TALK_SOUTH = "talk south";

		public const string TALK_SOUTHEAST = "talk southeast";

		public const string BLINK_EAST = "blink east";

		public const string BLINK_NORTHEAST = "blink northeast";

		public const string BLINK_NORTH = "blink north";

		public const string BLINK_NORTHWEST = "blink northwest";

		public const string BLINK_WEST = "blink west";

		public const string BLINK_SOUTHWEST = "blink southwest";

		public const string BLINK_SOUTH = "blink south";

		public const string BLINK_SOUTHEAST = "blink southeast";

		public const string CLIMB_EAST = "climb east";

		public const string CLIMB_NORTHEAST = "climb northeast";

		public const string CLIMB_NORTH = "climb north";

		public const string CLIMB_NORTHWEST = "climb northwest";

		public const string CLIMB_WEST = "climb west";

		public const string CLIMB_SOUTHWEST = "climb southwest";

		public const string CLIMB_SOUTH = "climb south";

		public const string CLIMB_SOUTHEAST = "climb southeast";

		public const string SWIM_EAST = "swim east";

		public const string SWIM_NORTHEAST = "swim northeast";

		public const string SWIM_NORTH = "swim north";

		public const string SWIM_NORTHWEST = "swim northwest";

		public const string SWIM_WEST = "swim west";

		public const string SWIM_SOUTHWEST = "swim southwest";

		public const string SWIM_SOUTH = "swim south";

		public const string SWIM_SOUTHEAST = "swim southeast";

		public const string FLOAT_EAST = "float east";

		public const string FLOAT_NORTHEAST = "float northeast";

		public const string FLOAT_NORTH = "float north";

		public const string FLOAT_NORTHWEST = "float northwest";

		public const string FLOAT_WEST = "float west";

		public const string FLOAT_SOUTHWEST = "float southwest";

		public const string FLOAT_SOUTH = "float south";

		public const string FLOAT_SOUTHEAST = "float southeast";

		private static readonly Dictionary<AnimationType, string> ANIMTYPE_TO_STRING = new Dictionary<AnimationType, string>
		{
			{
				(AnimationType)257,
				"stand east"
			},
			{
				(AnimationType)258,
				"stand northeast"
			},
			{
				(AnimationType)259,
				"stand north"
			},
			{
				(AnimationType)260,
				"stand northwest"
			},
			{
				(AnimationType)261,
				"stand west"
			},
			{
				(AnimationType)262,
				"stand southwest"
			},
			{
				(AnimationType)263,
				"stand south"
			},
			{
				(AnimationType)264,
				"stand southeast"
			},
			{
				(AnimationType)513,
				"walk east"
			},
			{
				(AnimationType)514,
				"walk northeast"
			},
			{
				(AnimationType)515,
				"walk north"
			},
			{
				(AnimationType)516,
				"walk northwest"
			},
			{
				(AnimationType)517,
				"walk west"
			},
			{
				(AnimationType)518,
				"walk southwest"
			},
			{
				(AnimationType)519,
				"walk south"
			},
			{
				(AnimationType)520,
				"walk southeast"
			},
			{
				(AnimationType)769,
				"run east"
			},
			{
				(AnimationType)770,
				"run northeast"
			},
			{
				(AnimationType)771,
				"run north"
			},
			{
				(AnimationType)772,
				"run northwest"
			},
			{
				(AnimationType)773,
				"run west"
			},
			{
				(AnimationType)774,
				"run southwest"
			},
			{
				(AnimationType)775,
				"run south"
			},
			{
				(AnimationType)776,
				"run southeast"
			},
			{
				(AnimationType)1025,
				"crouch east"
			},
			{
				(AnimationType)1026,
				"crouch northeast"
			},
			{
				(AnimationType)1027,
				"crouch north"
			},
			{
				(AnimationType)1028,
				"crouch northwest"
			},
			{
				(AnimationType)1029,
				"crouch west"
			},
			{
				(AnimationType)1030,
				"crouch southwest"
			},
			{
				(AnimationType)1031,
				"crouch south"
			},
			{
				(AnimationType)1032,
				"crouch southeast"
			},
			{
				(AnimationType)1281,
				"dead east"
			},
			{
				(AnimationType)1282,
				"dead northeast"
			},
			{
				(AnimationType)1283,
				"dead north"
			},
			{
				(AnimationType)1284,
				"dead northwest"
			},
			{
				(AnimationType)1285,
				"dead west"
			},
			{
				(AnimationType)1286,
				"dead southwest"
			},
			{
				(AnimationType)1287,
				"dead south"
			},
			{
				(AnimationType)1288,
				"dead southeast"
			},
			{
				(AnimationType)1537,
				"idle east"
			},
			{
				(AnimationType)1538,
				"idle northeast"
			},
			{
				(AnimationType)1539,
				"idle north"
			},
			{
				(AnimationType)1540,
				"idle northwest"
			},
			{
				(AnimationType)1541,
				"idle west"
			},
			{
				(AnimationType)1542,
				"idle southwest"
			},
			{
				(AnimationType)1543,
				"idle south"
			},
			{
				(AnimationType)1544,
				"idle southeast"
			},
			{
				(AnimationType)1793,
				"talk east"
			},
			{
				(AnimationType)1794,
				"talk northeast"
			},
			{
				(AnimationType)1795,
				"talk north"
			},
			{
				(AnimationType)1796,
				"talk northwest"
			},
			{
				(AnimationType)1797,
				"talk west"
			},
			{
				(AnimationType)1798,
				"talk southwest"
			},
			{
				(AnimationType)1799,
				"talk south"
			},
			{
				(AnimationType)1800,
				"talk southeast"
			},
			{
				(AnimationType)2049,
				"blink east"
			},
			{
				(AnimationType)2050,
				"blink northeast"
			},
			{
				(AnimationType)2051,
				"blink north"
			},
			{
				(AnimationType)2052,
				"blink northwest"
			},
			{
				(AnimationType)2053,
				"blink west"
			},
			{
				(AnimationType)2054,
				"blink southwest"
			},
			{
				(AnimationType)2055,
				"blink south"
			},
			{
				(AnimationType)2056,
				"blink southeast"
			},
			{
				(AnimationType)2305,
				"climb east"
			},
			{
				(AnimationType)2306,
				"climb northeast"
			},
			{
				(AnimationType)2307,
				"climb north"
			},
			{
				(AnimationType)2308,
				"climb northwest"
			},
			{
				(AnimationType)2309,
				"climb west"
			},
			{
				(AnimationType)2310,
				"climb southwest"
			},
			{
				(AnimationType)2311,
				"climb south"
			},
			{
				(AnimationType)2312,
				"climb southeast"
			},
			{
				(AnimationType)2561,
				"swim east"
			},
			{
				(AnimationType)2562,
				"swim northeast"
			},
			{
				(AnimationType)2563,
				"swim north"
			},
			{
				(AnimationType)2564,
				"swim northwest"
			},
			{
				(AnimationType)2565,
				"swim west"
			},
			{
				(AnimationType)2566,
				"swim southwest"
			},
			{
				(AnimationType)2567,
				"swim south"
			},
			{
				(AnimationType)2568,
				"swim southeast"
			},
			{
				(AnimationType)2817,
				"float east"
			},
			{
				(AnimationType)2818,
				"float northeast"
			},
			{
				(AnimationType)2819,
				"float north"
			},
			{
				(AnimationType)2820,
				"float northwest"
			},
			{
				(AnimationType)2821,
				"float west"
			},
			{
				(AnimationType)2822,
				"float southwest"
			},
			{
				(AnimationType)2823,
				"float south"
			},
			{
				(AnimationType)2824,
				"float southeast"
			}
		};

		private static readonly Dictionary<string, AnimationType> STRING_TO_ANIMTYPE = new Dictionary<string, AnimationType>
		{
			{
				"stand east",
				(AnimationType)257
			},
			{
				"stand northeast",
				(AnimationType)258
			},
			{
				"stand north",
				(AnimationType)259
			},
			{
				"stand northwest",
				(AnimationType)260
			},
			{
				"stand west",
				(AnimationType)261
			},
			{
				"stand southwest",
				(AnimationType)262
			},
			{
				"stand south",
				(AnimationType)263
			},
			{
				"stand southeast",
				(AnimationType)264
			},
			{
				"walk east",
				(AnimationType)513
			},
			{
				"walk northeast",
				(AnimationType)514
			},
			{
				"walk north",
				(AnimationType)515
			},
			{
				"walk northwest",
				(AnimationType)516
			},
			{
				"walk west",
				(AnimationType)517
			},
			{
				"walk southwest",
				(AnimationType)518
			},
			{
				"walk south",
				(AnimationType)519
			},
			{
				"walk southeast",
				(AnimationType)520
			},
			{
				"run east",
				(AnimationType)769
			},
			{
				"run northeast",
				(AnimationType)770
			},
			{
				"run north",
				(AnimationType)771
			},
			{
				"run northwest",
				(AnimationType)772
			},
			{
				"run west",
				(AnimationType)773
			},
			{
				"run southwest",
				(AnimationType)774
			},
			{
				"run south",
				(AnimationType)775
			},
			{
				"run southeast",
				(AnimationType)776
			},
			{
				"crouch east",
				(AnimationType)1025
			},
			{
				"crouch northeast",
				(AnimationType)1026
			},
			{
				"crouch north",
				(AnimationType)1027
			},
			{
				"crouch northwest",
				(AnimationType)1028
			},
			{
				"crouch west",
				(AnimationType)1029
			},
			{
				"crouch southwest",
				(AnimationType)1030
			},
			{
				"crouch south",
				(AnimationType)1031
			},
			{
				"crouch southeast",
				(AnimationType)1032
			},
			{
				"dead east",
				(AnimationType)1281
			},
			{
				"dead northeast",
				(AnimationType)1282
			},
			{
				"dead north",
				(AnimationType)1283
			},
			{
				"dead northwest",
				(AnimationType)1284
			},
			{
				"dead west",
				(AnimationType)1285
			},
			{
				"dead southwest",
				(AnimationType)1286
			},
			{
				"dead south",
				(AnimationType)1287
			},
			{
				"dead southeast",
				(AnimationType)1288
			},
			{
				"idle east",
				(AnimationType)1537
			},
			{
				"idle northeast",
				(AnimationType)1538
			},
			{
				"idle north",
				(AnimationType)1539
			},
			{
				"idle northwest",
				(AnimationType)1540
			},
			{
				"idle west",
				(AnimationType)1541
			},
			{
				"idle southwest",
				(AnimationType)1542
			},
			{
				"idle south",
				(AnimationType)1543
			},
			{
				"idle southeast",
				(AnimationType)1544
			},
			{
				"talk east",
				(AnimationType)1793
			},
			{
				"talk northeast",
				(AnimationType)1794
			},
			{
				"talk north",
				(AnimationType)1795
			},
			{
				"talk northwest",
				(AnimationType)1796
			},
			{
				"talk west",
				(AnimationType)1797
			},
			{
				"talk southwest",
				(AnimationType)1798
			},
			{
				"talk south",
				(AnimationType)1799
			},
			{
				"talk southeast",
				(AnimationType)1800
			},
			{
				"blink east",
				(AnimationType)2049
			},
			{
				"blink northeast",
				(AnimationType)2050
			},
			{
				"blink north",
				(AnimationType)2051
			},
			{
				"blink northwest",
				(AnimationType)2052
			},
			{
				"blink west",
				(AnimationType)2053
			},
			{
				"blink southwest",
				(AnimationType)2054
			},
			{
				"blink south",
				(AnimationType)2055
			},
			{
				"blink southeast",
				(AnimationType)2056
			},
			{
				"climb east",
				(AnimationType)2305
			},
			{
				"climb northeast",
				(AnimationType)2306
			},
			{
				"climb north",
				(AnimationType)2307
			},
			{
				"climb northwest",
				(AnimationType)2308
			},
			{
				"climb west",
				(AnimationType)2309
			},
			{
				"climb southwest",
				(AnimationType)2310
			},
			{
				"climb south",
				(AnimationType)2311
			},
			{
				"climb southeast",
				(AnimationType)2312
			},
			{
				"swim east",
				(AnimationType)2561
			},
			{
				"swim northeast",
				(AnimationType)2562
			},
			{
				"swim north",
				(AnimationType)2563
			},
			{
				"swim northwest",
				(AnimationType)2564
			},
			{
				"swim west",
				(AnimationType)2565
			},
			{
				"swim southwest",
				(AnimationType)2566
			},
			{
				"swim south",
				(AnimationType)2567
			},
			{
				"swim southeast",
				(AnimationType)2568
			},
			{
				"float east",
				(AnimationType)2817
			},
			{
				"float northeast",
				(AnimationType)2818
			},
			{
				"float north",
				(AnimationType)2819
			},
			{
				"float northwest",
				(AnimationType)2820
			},
			{
				"float west",
				(AnimationType)2821
			},
			{
				"float southwest",
				(AnimationType)2822
			},
			{
				"float south",
				(AnimationType)2823
			},
			{
				"float southeast",
				(AnimationType)2824
			}
		};
	}
}
