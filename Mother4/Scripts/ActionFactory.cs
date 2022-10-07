using System;
using System.Collections.Generic;
using System.Text;
using Carbine.Utility;
using fNbt;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Mother4.Scripts.Actions.Types;
using Rufini.Actions.Types;
using Rufini.Strings;
using SFML.Graphics;

namespace Mother4.Scripts
{
	internal class ActionFactory
	{
		public static List<RufiniAction> GetDefaultActions()
		{
			List<RufiniAction> list = new List<RufiniAction>();
			foreach (KeyValuePair<string, Type> keyValuePair in ActionFactory.actions)
			{
				if (typeof(RufiniAction).IsAssignableFrom(keyValuePair.Value))
				{
					RufiniAction item = (RufiniAction)Activator.CreateInstance(keyValuePair.Value);
					list.Add(item);
				}
			}
			return list;
		}

		public static RufiniAction FromCode(string code)
		{
			Type type = null;
			ActionFactory.actions.TryGetValue(code, out type);
			if (type != null)
			{
				return (RufiniAction)Activator.CreateInstance(type);
			}
			throw new ArgumentException(string.Format("\"{0}\" does not correspond to an action type.", code));
		}

		private static string GetType(NbtCompound tag)
		{
			string result = null;
			NbtTag nbtTag = tag.Get("_typ");
			if (nbtTag != null)
			{
				if (nbtTag is NbtInt)
				{
					uint intValue = (uint)nbtTag.IntValue;
					ActionFactory.tagNameBuf[3] = (char)((byte)intValue);
					ActionFactory.tagNameBuf[2] = (char)((byte)(intValue >> 8));
					ActionFactory.tagNameBuf[1] = (char)((byte)(intValue >> 16));
					ActionFactory.tagNameBuf[0] = (char)((byte)(intValue >> 24));
					result = ActionFactory.tagNameBuf.ToString();
				}
				else if (nbtTag is NbtString)
				{
					result = nbtTag.StringValue.Substring(0, 4);
				}
			}
			return result;
		}

		public static RufiniAction FromNbt(NbtCompound tag)
		{
			string type = ActionFactory.GetType(tag);
			RufiniAction rufiniAction = null;
			if (type != null)
			{
				Type type2 = null;
				ActionFactory.actions.TryGetValue(type, out type2);
				if (type2 != null)
				{
					rufiniAction = (RufiniAction)Activator.CreateInstance(type2);
					NbtCompound nbtCompound = tag.Get<NbtCompound>("params");
					if (nbtCompound != null)
					{
						using (IEnumerator<NbtTag> enumerator = nbtCompound.Tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NbtTag paramTag = enumerator.Current;
								ActionParam actionParam = rufiniAction.Params.Find((ActionParam x) => x.Name == paramTag.Name);
								if (actionParam != null)
								{
									if (paramTag is NbtString)
									{
										if (actionParam.Type == typeof(RufiniString))
										{
											RufiniString value = StringFile.Instance.Get(paramTag.StringValue);
											rufiniAction.SetValue<RufiniString>(paramTag.Name, value);
										}
										else if (actionParam.Type == typeof(string))
										{
											rufiniAction.SetValue<string>(paramTag.Name, paramTag.StringValue);
										}
									}
									else if (paramTag is NbtInt)
									{
										if (actionParam.Type == typeof(RufiniOption))
										{
											rufiniAction.SetValue<RufiniOption>(paramTag.Name, new RufiniOption
											{
												Option = paramTag.IntValue
											});
										}
										else if (actionParam.Type == typeof(int))
										{
											rufiniAction.SetValue<int>(paramTag.Name, paramTag.IntValue);
										}
										else if (actionParam.Type == typeof(Color))
										{
											Color value2 = ColorHelper.FromInt(paramTag.IntValue);
											rufiniAction.SetValue<Color>(paramTag.Name, value2);
										}
									}
									else if (paramTag is NbtIntArray)
									{
										rufiniAction.SetValue<int[]>(paramTag.Name, ((NbtIntArray)paramTag).IntArrayValue);
									}
									else if (paramTag is NbtFloat)
									{
										rufiniAction.SetValue<float>(paramTag.Name, paramTag.FloatValue);
									}
									else if (paramTag is NbtByte)
									{
										if (actionParam.Type == typeof(byte))
										{
											rufiniAction.SetValue<byte>(paramTag.Name, paramTag.ByteValue);
										}
										else if (actionParam.Type == typeof(bool))
										{
											rufiniAction.SetValue<bool>(paramTag.Name, paramTag.ByteValue != 0);
										}
									}
								}
							}
						}
					}
				}
			}
			return rufiniAction;
		}

		private const string TYPE_TAG = "_typ";

		private const string PARAMS_TAG = "params";

		private static StringBuilder tagNameBuf = new StringBuilder("\0\0\0\0", 4);

		private static Dictionary<string, Type> actions = new Dictionary<string, Type>
		{
			{
				"PRLN",
				typeof(PrintLnAction)
			},
			{
				"TXBX",
				typeof(TextboxAction)
			},
			{
				"SNPC",
				typeof(SetNpcAction)
			},
			{
				"SNTG",
				typeof(SetNametagAction)
			},
			{
				"QSTN",
				typeof(QuestionAction)
			},
			{
				"CMOV",
				typeof(CameraMoveAction)
			},
			{
				"CFLP",
				typeof(CameraPlayerAction)
			},
			{
				"CFLN",
				typeof(CameraNPCAction)
			},
			{
				"WAIT",
				typeof(WaitAction)
			},
			{
				"CSPP",
				typeof(ChangeSpritePlayerAction)
			},
			{
				"CSPN",
				typeof(ChangeSpriteNPCAction)
			},
			{
				"CSSP",
				typeof(ChangeSubspritePlayerAction)
			},
			{
				"CSSN",
				typeof(ChangeSubspriteNPCAction)
			},
			{
				"EADD",
				typeof(EntityAddAction)
			},
			{
				"EDEL",
				typeof(EntityDeleteAction)
			},
			{
				"EDIR",
				typeof(EntityDirectionAction)
			},
			{
				"EMOV",
				typeof(EntityMoveAction)
			},
			{
				"IADD",
				typeof(ItemAddAction)
			},
			{
				"IREM",
				typeof(ItemRemoveAction)
			},
			{
				"MPMK",
				typeof(MapMarkSetAction)
			},
			{
				"MPCL",
				typeof(MapMarkClearAction)
			},
			{
				"SFLG",
				typeof(SetFlagAction)
			},
			{
				"ANIM",
				typeof(AnimationAction)
			},
			{
				"SSHK",
				typeof(ScreenShakeAction)
			},
			{
				"SMOD",
				typeof(StatModifyAction)
			},
			{
				"SSET",
				typeof(StatSetAction)
			},
			{
				"AMNY",
				typeof(AddMoneyAction)
			},
			{
				"SMNY",
				typeof(SetMoneyAction)
			},
			{
				"SGVL",
				typeof(ValueSetAction)
			},
			{
				"AGVL",
				typeof(ValueAddAction)
			},
			{
				"IFFL",
				typeof(IfFlagAction)
			},
			{
				"IFVL",
				typeof(IfValueAction)
			},
			{
				"IFEN",
				typeof(EndIfAction)
			},
			{
				"ELSE",
				typeof(ElseAction)
			},
			{
				"CALL",
				typeof(CallAction)
			},
			{
				"WTHR",
				typeof(WeatherAction)
			},
			{
				"SCEF",
				typeof(ScreenEffectAction)
			},
			{
				"SCFD",
				typeof(ScreenFadeAction)
			},
			{
				"TIME",
				typeof(TimeAction)
			},
			{
				"AEXP",
				typeof(AddExpAction)
			},
			{
				"SCFL",
				typeof(ScreenFlashAction)
			},
			{
				"EMNP",
				typeof(EmoticonNPCAction)
			},
			{
				"EMPL",
				typeof(EmoticonPlayerAction)
			},
			{
				"SBGM",
				typeof(SetBGMAction)
			},
			{
				"PSFX",
				typeof(PlaySFXAction)
			},
			{
				"ENMM",
				typeof(EntityMoveModeAction)
			},
			{
				"HNPC",
				typeof(HopNPCAction)
			},
			{
				"HPLR",
				typeof(HopPlayerAction)
			},
			{
				"APRT",
				typeof(AddPartyMemberAction)
			},
			{
				"RPRT",
				typeof(RemovePartyMemberAction)
			},
			{
				"SBTL",
				typeof(StartBattleAction)
			},
			{
				"IRIS",
				typeof(IrisOverlayAction)
			},
			{
				"GOMP",
				typeof(GoToMapAction)
			},
			{
				"PPMV",
				typeof(PlayerPathMoveAction)
			},
			{
				"FOTX",
				typeof(FlyoverTextAction)
			},
			{
				"MVPL",
				typeof(PlayerPositionAction)
			},
			{
				"EDPT",
				typeof(EntityDepthAction)
			},
			{
				"PMOV",
				typeof(PlayerMoveAction)
			},
			{
				"TSPL",
				typeof(PlayerShadowAction)
			},
			{
				"STSP",
				typeof(SetTilesetPaletteAction)
			}
		};
	}
}
