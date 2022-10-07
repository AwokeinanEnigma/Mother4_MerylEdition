using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Psi;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Data
{
	internal class PsiAnimations
	{
		private static PsiElementList GenerateFreezeAlpha()
		{
			List<PsiElement> elements = new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "freeze_a.sdat", default(Vector2f), 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.SFXBATTLEPSI + "pkFreezeA.wav", AudioType.Sound),
					LockToTargetPosition = true,
					PositionIndex = 0,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128))
				},
				new PsiElement
				{
					Timestamp = 20,
					TargetFlashColor = new Color?(Color.Cyan),
					TargetFlashBlendMode = ColorBlendMode.Screen,
					TargetFlashFrames = 10,
					TargetFlashCount = 1
				},
				new PsiElement
				{
					Timestamp = 50,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
				}
			};
			return new PsiElementList(elements);
		}

		private static PsiElementList GenerateBeamAlpha()
		{
			MultipartAnimation animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam1.sdat", default(Vector2f), 0.5f, 32767);
			MultipartAnimation multipartAnimation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam1.sdat", default(Vector2f), 0.5f, 32767);
			multipartAnimation.Scale = new Vector2f(-1f, 1f);
			List<PsiElement> list = new List<PsiElement>();
			list.Add(new PsiElement
			{
				Timestamp = 0,
				Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam2.sdat", new Vector2f(160f, 90f), 0.3f, 32767),
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128)),
				Sound = AudioManager.Instance.Use(Paths.SFXBATTLEPSI + "pkBeamA.wav", AudioType.Sound)
			});
			list.Add(new PsiElement
			{
				Timestamp = 50,
				Animation = animation,
				Offset = new Vector2f(-52f, -48f),
				LockToTargetPosition = true,
				PositionIndex = 0
			});
			list.Add(new PsiElement
			{
				Timestamp = 50,
				Animation = multipartAnimation,
				Offset = new Vector2f(52f, -48f),
				LockToTargetPosition = true,
				PositionIndex = 0
			});
			list.Add(new PsiElement
			{
				Timestamp = 80,
				TargetFlashColor = new Color?(Color.Yellow),
				TargetFlashBlendMode = ColorBlendMode.Screen,
				TargetFlashFrames = 20,
				TargetFlashCount = 1
			});
			for (int i = 0; i < 6; i++)
			{
				list.Add(new PsiElement
				{
					Timestamp = 80 + i * 5,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam3.sdat", default(Vector2f), 0.5f, 32767),
					Offset = new Vector2f((float)(i * -8), 0f),
					LockToTargetPosition = true,
					PositionIndex = 0
				});
				list.Add(new PsiElement
				{
					Timestamp = 80 + i * 5,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam3.sdat", default(Vector2f), 0.5f, 32767),
					Offset = new Vector2f((float)(i * 8), 0f),
					LockToTargetPosition = true,
					PositionIndex = 0
				});
			}
			list.Add(new PsiElement
			{
				Timestamp = list[list.Count - 1].Timestamp + 30,
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
			});
			return new PsiElementList(list);
		}

		private static PsiElementList GenerateHitback()
		{
			Vector2f position = new Vector2f(160f, 90f);
			return new PsiElementList(new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet_reflect.sdat", position, 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.SFXBATTLE + "rocketReflect.wav", AudioType.Sound),
					CardSpringMode = BattleCard.SpringMode.BounceUp,
					CardSpringAmplitude = new Vector2f(0f, 4f),
					CardSpringSpeed = new Vector2f(0f, 0.2f),
					CardSpringDecay = new Vector2f(0f, 0.5f)
				}
			});
		}

		private static List<PsiElement> GenerateThrow()
		{
			Vector2f position = new Vector2f(160f, 90f);
			return new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet.sdat", position, 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.SFXBATTLE + "rocket.wav", AudioType.Sound)
				}
			};
		}

		private static List<PsiElement> GenerateExplosion(int startTimestamp)
		{
			Vector2f v = new Vector2f(160f, 90f);
			List<PsiElement> list = new List<PsiElement>();
			list.Add(new PsiElement
			{
				Timestamp = startTimestamp,
				ScreenDarkenColor = new Color?(Color.Cyan),
				ScreenDarkenDepth = new int?(0),
				Sound = AudioManager.Instance.Use(Paths.SFXBATTLE + "explosion.wav", AudioType.Sound)
			});
			int num = 98;
			int[] array = new int[]
			{
				1,
				2,
				3,
				2,
				1
			};
			int num2 = 180 / (array.Length + 1);
			for (int i = 0; i < array.Length; i++)
			{
				int num3 = array[i];
				int num4 = (num3 - 1) * (num + 20);
				int num5 = num4 / 2;
				for (int j = 0; j < num3; j++)
				{
					Vector2f vector2f = v + new Vector2f((float)(-(float)num5 + (num + 20) * j), -v.Y + (float)(num2 * (i + 1)));
					int num6 = (int)(VectorMath.Magnitude(v - vector2f) / 10f);
					list.Add(new PsiElement
					{
						Timestamp = startTimestamp + num6,
						Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet_boom.sdat", vector2f, 0.4f, 32767)
					});
				}
			}
			list.Add(new PsiElement
			{
				Timestamp = list[list.Count - 1].Timestamp + 10,
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0)),
				ScreenDarkenDepth = new int?(0)
			});
			return list;
		}

		private static PsiElementList GenerateComet()
		{
			List<PsiElement> list = new List<PsiElement>();
			list.AddRange(PsiAnimations.GenerateThrow());
			list.AddRange(PsiAnimations.GenerateExplosion(40));
			return new PsiElementList(list);
		}

		private static PsiElementList GenerateFireAlpha()
		{
			List<PsiElement> elements = new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "fire_a.sdat", new Vector2f(160f, 90f), 0.4f, 32767),
					Sound = AudioManager.Instance.Use(Paths.SFXBATTLEPSI + "pkFireA.wav", AudioType.Sound),
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128))
				},
				new PsiElement
				{
					Timestamp = 40,
					TargetFlashColor = new Color?(Color.Red),
					TargetFlashBlendMode = ColorBlendMode.Screen,
					TargetFlashFrames = 10,
					TargetFlashCount = 1
				},
				new PsiElement
				{
					Timestamp = 80,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
				}
			};
			return new PsiElementList(elements);
		}

		public static PsiElementList Get(int psiId)
		{
			switch (psiId)
			{
			case 1:
				return PsiAnimations.GenerateFreezeAlpha();
			case 2:
				return PsiAnimations.GenerateBeamAlpha();
			case 3:
				return PsiAnimations.GenerateComet();
			case 4:
				return new PsiElementList(PsiAnimations.GenerateThrow());
			case 5:
				return PsiAnimations.GenerateHitback();
			case 6:
				return new PsiElementList(PsiAnimations.GenerateExplosion(0));
			case 7:
				return PsiAnimations.GenerateFireAlpha();
			default:
				return PsiAnimations.GenerateHitback();
			}
		}

		public static PsiElementList Get(IPsi psi)
		{
			return Get(1);
			//return PSI;
		}
	}
}
