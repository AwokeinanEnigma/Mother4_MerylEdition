using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.Battle.Background;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.GUI;
using Mother4.Items;
using Mother4.Overworld;
using Mother4.Scenes;
using Mother4.Scenes.Transitions;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.Types;
using Mother4.Utility;
using Carbine;
using Carbine.Audio;
using Carbine.Collision;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Maps;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Tiles;
using Carbine.Utility;

namespace Mother4.Scenes
{
    internal class OverworldScene : StandardScene
	{
		public ScreenDimmer Dimmer
		{
			get
			{
				return this.screenDimmer;
			}
		}

		public PartyTrain PartyTrain
		{
			get
			{
				return this.partyTrain;
			}
		}

		public IrisOverlay IrisOverlay
		{
			get
			{
				return this.GetIrisOverlay();
			}
		}
		private ICollidable[] collisionResults;

        public static OverworldScene instance;

		public OverworldScene(string mapName, Vector2f initialPosition, int initialDirection, bool initialRunning, bool extendParty, bool enableLoadScripts)
		{
			this.mapName = mapName;
			this.initialPosition = initialPosition;
			this.initialDirection = initialDirection;
			this.initialRunning = initialRunning;
			this.enableLoadScripts = enableLoadScripts;
			this.extendParty = extendParty;
			this.collisionResults = new ICollidable[8];
			this.initialized = false;
		}

		public OverworldScene(string mapName, bool enableLoadScripts)
		{
			this.mapName = mapName;
			this.initialPosition = VectorMath.ZERO_VECTOR;
			this.initialDirection = 6;
			this.initialRunning = false;
			this.enableLoadScripts = enableLoadScripts;
			this.extendParty = false;
			this.collisionResults = new ICollidable[8];
			this.initialized = false;
		}

		private IrisOverlay GetIrisOverlay()
		{
			if (this.iris == null)
			{
				Vector2f origin = new Vector2f(160f, 90f);
				this.iris = new IrisOverlay(ViewManager.Instance.FinalCenter, origin, 0f);
			}
			return this.iris;
		}

		public void SetTilesetPalette(int tilesetPalette)
		{
			if (this.initialized && this.mapGroups.Count > 0)
			{
				this.mapGroups[0].Tileset.CurrentPalette = (uint)tilesetPalette;
				
			}
		}

		private void SetExecutorScript(string scriptName, bool isTelepathy)
		{
			Script? script = ScriptLoader.Load(scriptName);
			if (script != null)
			{
				Script value = script.Value;
				if (isTelepathy)
				{
					RufiniAction[] array = new RufiniAction[value.Actions.Length + 2];
					Array.Copy(value.Actions, 0, array, 1, value.Actions.Length);
					array[0] = new TelepathyStartAction();
					array[array.Length - 1] = new TelepathyEndAction();
					value.Actions = array;
				}
				this.executor.PushScript(value);
			}
		}
		private bool HandleNpcCheck(NPC npc, bool isTelepathy)
		{
			bool result = false;
			Map.NPCtext npctext = new Map.NPCtext
			{
				ID = "",
				Flag = -1
			};
			int num = int.MinValue;
			List<Map.NPCtext> list = (!isTelepathy) ? npc.Text : npc.TeleText;
			foreach (Map.NPCtext npctext2 in list)
			{
				if (FlagManager.Instance[npctext2.Flag] && npctext2.Flag > num)
				{
					num = npctext2.Flag;
					npctext = npctext2;
				}
			}
			if (npctext.Flag > -1)
			{
				double num2 = Math.Atan2((double)(npc.Position.Y - this.player.Position.Y), (double)(-(double)(npc.Position.X - this.player.Position.X)));
				int num3 = (int)Math.Round(num2 / 0.7853981633974483);
				if (num3 < 0)
				{
					num3 += 8;
				}
				npc.Direction = num3;
				Script? script = ScriptLoader.Load(npctext.ID);
				if (script != null)
				{
					result = true;
					Script value = script.Value;
					if (isTelepathy)
					{
						RufiniAction[] array = new RufiniAction[value.Actions.Length + 2];
						Array.Copy(value.Actions, 0, array, 1, value.Actions.Length);
						array[0] = new TelepathyStartAction();
						array[array.Length - 1] = new TelepathyEndAction();
						value.Actions = array;
					}
					this.executor.SetCheckedNPC(npc);
					this.executor.PushScript(value);
				}
			}
			return result;
		}
		private void HandleCheckAction(bool isTelepathy)
		{
			bool flag = false;
			bool flag2 = false;
			Vector2f position = this.player.Position + this.player.CheckVector;
			Console.WriteLine("Checking at {0},{1}", position.X, position.Y);
			if (!this.collisionManager.PlaceFree(this.player, position, this.collisionResults))
			{
				do
				{
					Console.WriteLine("results loop start");
					for (int i = 0; i < this.collisionResults.Length; i++)
					{
						Console.Write("{0}: ", i);
						ICollidable collidable = this.collisionResults[i];
						if (collidable is NPC)
						{
							Console.WriteLine("Found NPC");
							flag2 = this.HandleNpcCheck((NPC)collidable, isTelepathy);
							flag = false;
							break;
						}
						if (!flag && collidable is SolidStatic)
						{
							Console.WriteLine("Found SolidStatic");
							flag = true;
						}
						else
						{
							Console.WriteLine("Not an NPC or SolidStatic");
							flag = false;
							if (collidable == null)
							{
								break;
							}
						}
					}
					Console.WriteLine("results loop end");
					if (flag)
					{
						Vector2f position2 = this.player.Position + this.player.CheckVector * 2f;
						Console.WriteLine("Checking again at {0},{1}", position2.X, position2.Y);
						bool flag3 = this.collisionManager.PlaceFree(this.player, position2, this.collisionResults);
					}
				}
				while (flag);
			}
			if (!flag2)
			{
				Console.WriteLine("Tried checking, but there was no script");
				this.SetExecutorScript("Default", isTelepathy);
			}
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.F1)
			{
				Console.WriteLine("View position: ({0},{1})", ViewManager.Instance.FinalCenter.X, ViewManager.Instance.FinalCenter.Y);
			}

            if (!executor.Running)
            {
				if (b == Button.F6) {
					rainOverlay = new RainOverlay();
				}

                if (b == Button.A)
                {
                    this.HandleCheckAction(false);
                    return;
                }
                if (b == Button.Start)
                {
                    CharacterType[] array2 = PartyManager.Instance.ToArray();
                    foreach (CharacterType key in array2)
                    {
                        Inventory inventory = InventoryManager.Instance.Get(key);
                        int num = Engine.Random.Next(14);
                        for (int k = 0; k < num; k++)
                        {
                            Item item = new Item(false);
                            item.Set("name", "Test item " + (k + 1));
                            inventory.Add(item);
                        }
                    }
                    this.dontPauseMusic = true;
                    this.openingMenu = true;
                    MenuScene newScene = new MenuScene();
                    SceneManager.Instance.Transition = new InstantTransition();
                    SceneManager.Instance.Push(newScene);
                    return;
                }
			}

			/*
            if (!this.executor.Running)
            {	
                if (b == Button.One)
                {
                    
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    CharacterStats.SetStats(CharacterType.Travis, new StatSet
                    {
                        HP = 89,
                        PP = 35,
                        Meter = 0f,
                        Offense = 6,
                        Speed = 150,
                        Guts = 0,
                        Level = 40
                    });
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.NotSoDeer
                    }, true));
                }
                else if (b == Button.Two)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    CharacterStats.SetStats(CharacterType.Travis, new StatSet
                    {
                        HP = 110,
                        PP = 80,
                        Meter = 0.4f,
                        Offense = 16,
                        Level = 40,
                        Speed = 5
                    });
                    CharacterStats.SetStats(CharacterType.Floyd, new StatSet
                    {
                        HP = 59,
                        PP = 0,
                        Meter = 0.58666664f,
                        Offense = 14,
                        Guts = int.MaxValue,
                        Speed = 20
                    });
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.Mouse
                    }, true));
                }
                else if (b == Button.Three)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    CharacterStats.SetStats(CharacterType.Travis, new StatSet
                    {
                        HP = 178,
                        PP = 80,
                        Meter = 0.06666667f,
                        Offense = 20,
                        Level = 40,
                        Speed = 5
                    });
                    CharacterStats.SetStats(CharacterType.Floyd, new StatSet
                    {
                        HP = 160,
                        PP = 0,
                        Meter = 0.7866667f,
                        Offense = 20,
                        Speed = 20
                    });
                    CharacterStats.SetStats(CharacterType.Meryl, new StatSet
                    {
                        HP = 93,
                        MaxHP = 120,
                        PP = 116,
                        MaxPP = 116,
                        Meter = 0.44f,
                        Offense = 20,
                        Level = 40,
                        Speed = 30
                    });
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.HermitCan,
                        EnemyType.Flamingo
                    }, true));
                }
                else if (b == Button.Four)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    CharacterStats.SetStats(CharacterType.Leo, new StatSet
                    {
                        HP = 177,
                        PP = 209,
                        Meter = 0.82666665f,
                        Offense = 20,
                        Level = 24,
                        Speed = 40
                    });
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.AtomicPowerRobo
                    }, true));
                }
                else if (b == Button.Five)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    CharacterStats.SetStats(CharacterType.Travis, new StatSet
                    {
                        HP = 290,
                        PP = 11,
                        Meter = 0.33333334f,
                        Offense = 20,
                        Level = 40,
                        Speed = 10
                    });
                    CharacterStats.SetStats(CharacterType.Floyd, new StatSet
                    {
                        HP = 213,
                        PP = 0,
                        Meter = 0.06666667f,
                        Offense = 20,
                        Speed = 20
                    });
                    CharacterStats.SetStats(CharacterType.Meryl, new StatSet
                    {
                        HP = 177,
                        PP = 209,
                        Meter = 0.97333336f,
                        Offense = 20,
                        Level = 40,
                        Speed = 30
                    });
                    
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.MeltyRobot,
                        EnemyType.MeltyRobot
                    }, true));
                }
                else if (b == Button.Six)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Boss);
                    CharacterStats.SetStats(CharacterType.Travis, new StatSet
                    {
                        HP = 490,
                        MaxHP = 492,
                        PP = 7,
                        MaxPP = 380,
                        Meter = 0.14666666f,
                        Offense = 20,
                        Level = 40,
                        Speed = 5
                    });
                    CharacterStats.SetStats(CharacterType.Floyd, new StatSet
                    {
                        HP = 1,
                        MaxHP = 460,
                        PP = 0,
                        MaxPP = 0,
                        Meter = 0.93333334f,
                        Offense = 20,
                        Speed = 15
                    });
                    CharacterStats.SetStats(CharacterType.Meryl, new StatSet
                    {
                        HP = 14,
                        MaxHP = 380,
                        PP = 155,
                        MaxPP = 220,
                        Meter = 0.04f,
                        Offense = 20,
                        Level = 40,
                        Speed = 40
                    });
                    CharacterStats.SetStats(CharacterType.Leo, new StatSet
                    {
                        HP = 199,
                        MaxHP = 512,
                        PP = 6,
                        MaxPP = 180,
                        Meter = 0.6666667f,
                        Offense = 20,
                        Level = 40,
                        Speed = 30
                    });
                    
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.ModernMind
                    }, false));
                }
                else if (b == Button.Seven)
                {
                    
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    EnemyType[] array = new EnemyType[Engine.Random.Next(12) + 1];
                    string[] names = Enum.GetNames(typeof(EnemyType));
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = (EnemyType)Enum.Parse(typeof(EnemyType), names[Engine.Random.Next(names.Length - 1) + 1]);
                    }
                    SceneManager.Instance.Push(new BattleScene(array, true));
                }
                else if (b == Button.Eight)
                {
                    SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                    SceneManager.Instance.Push(new BattleScene(new EnemyType[]
                    {
                        EnemyType.Flamingo,
                        EnemyType.Flamingo
                    }, true));
                }
                if (b == Button.Start)
                {
                    CharacterType[] array2 = PartyManager.Instance.ToArray();
                    foreach (CharacterType key in array2)
                    {
                        Inventory inventory = InventoryManager.Instance.Get(key);
                        int num = Engine.Random.Next(14);
                        for (int k = 0; k < num; k++)
                        {
                            Item item = new Item(false);
                            item.Set("name", "Test item " + (k + 1));
                            inventory.Add(item);
                        }
                    }
                    this.dontPauseMusic = true;
                    this.openingMenu = true;
                    MenuScene newScene = new MenuScene();
                    SceneManager.Instance.Transition = new InstantTransition();
                    SceneManager.Instance.Push(newScene);
                    return;
                }
                if (b == Button.A)
                {
                    this.HandleCheckAction(false);
                    return;
                }
                if (b == Button.F6)
                {
                    SaveProfile currentProfile = SaveFileManager.Instance.CurrentProfile;
                    currentProfile.IsValid = true;
                    currentProfile.Party = PartyManager.Instance.ToArray();
                    currentProfile.MapName = this.mapName;
                    currentProfile.Position = this.player.Position;
                    currentProfile.Time += Engine.SessionTime;
                    currentProfile.Flavor = (int)Settings.WindowFlavor;
                //	SceneManager.Instance.Transition = new InstantTransition();
                //	SceneManager.Instance.Push(new SaveScene(SaveScene.Location.Belring, currentProfile));
                
                    SaveFileManager.Instance.CurrentProfile = currentProfile;
                    SaveFileManager.Instance.SaveFile();
                    return;
                }
                if (b == Button.F7)
                {
                    BattleSwirlOverlay renderable = new BattleSwirlOverlay(BattleSwirlOverlay.Style.Blue, 2147483547, 0.015f);
                    this.pipeline.Add(renderable);
                }
            }
        */
		}

		private void Initialize()
		{
			int colorIndex = 1;
            instance = this;
			ViewManager.Instance.Reset();
            AllEnemyNpcs = new List<EnemyNPC>();
			this.screenDimmer = new ScreenDimmer(this.pipeline, Color.Transparent, 0, 2147450870);
			this.footstepPlayer = new FootstepPlayer();
			this.textbox = new TextBox(this.pipeline, colorIndex);
			this.actorManager.Add(this.textbox);
			this.questionbox = new QuestionBox(this.pipeline, colorIndex);
			this.actorManager.Add(this.questionbox);
			Map map = MapLoader.Load(Paths.MAPS + this.mapName, Paths.GRAPHICS);
			if (this.initialPosition == VectorMath.ZERO_VECTOR)
			{
				this.initialPosition = new Vector2f((float)(map.Head.Width / 2), (float)(map.Head.Height / 2));
			}
			this.collisionManager = new CollisionManager(map.Head.Width, map.Head.Height);
			CharacterType[] array = PartyManager.Instance.ToArray();
			this.partyTrain = new PartyTrain(this.initialPosition, this.initialDirection, map.Head.Ocean ? TerrainType.Ocean : TerrainType.None, this.extendParty);
			this.player = new Player(this.pipeline, this.collisionManager, this.partyTrain, this.initialPosition, this.initialDirection, array[0], map.Head.Shadows, map.Head.Ocean, this.initialRunning);
			this.player.OnCollision += this.OnPlayerCollision;
			this.player.OnRunningChange += this.OnPlayerRunningChange;
			this.actorManager.Add(this.player);
			this.collisionManager.Add(this.player);
			for (int i = 1; i < array.Length; i++)
			{
				PartyFollower partyFollower = new PartyFollower(this.pipeline, this.collisionManager, this.partyTrain, array[i], this.player.Position, this.player.Direction, map.Head.Shadows);
				this.partyTrain.Add(partyFollower);
				this.collisionManager.Add(partyFollower);
			}
			List<NPC> addActors = MapPopulator.GenerateNPCs(this.pipeline, this.collisionManager, map);
			this.actorManager.AddAll<NPC>(addActors.ToArray());
			IList<ICollidable> collidables = MapPopulator.GeneratePortals(map);
			this.collisionManager.AddAll<ICollidable>(collidables);
			IList<ICollidable> collidables2 = MapPopulator.GenerateTriggerAreas(map);
			this.collisionManager.AddAll<ICollidable>(collidables2);
			this.spawners = MapPopulator.GenerateSpawners(map);
			this.parallaxes = MapPopulator.GenerateParallax(map);
			this.pipeline.AddAll<ParallaxBackground>(this.parallaxes);
			this.testBack = MapPopulator.GenerateBBGOverlay(map);
			if (this.testBack != null)
			{
				this.pipeline.Add(this.testBack);
			}
			foreach (Mesh mesh in map.Mesh)
			{
				this.collisionManager.Add(new SolidStatic(mesh));
			}
			bool flag = FlagManager.Instance[1];
			this.mapGroups = map.MakeTileGroups(Paths.GRAPHICS, flag ? 1U : 0U);
			this.pipeline.AddAll<TileGroup>(this.mapGroups);
			this.backColor = (flag ? map.Head.SecondaryColor : map.Head.PrimaryColor);
			ExecutionContext context = new ExecutionContext
			{
				Pipeline = this.pipeline,
				ActorManager = this.actorManager,
				CollisionManager = this.collisionManager,
				TextBox = this.textbox,
				QuestionBox = this.questionbox,
				Player = this.player,
				Paths = map.Paths,
				Areas = map.Areas
			};
			this.executor = new ScriptExecutor(context);
			if (map.Head.Script != null && this.enableLoadScripts)
			{
				Script? script = ScriptLoader.Load(map.Head.Script);
				if (script != null)
				{
					Console.WriteLine("Executing script on load: {0}", script.Value.Name);
					this.executor.PushScript(script.Value);
				}
				else
				{
					Console.WriteLine("Could not load script \"{0}\"", map.Head.Script);
				}
			}
			else
			{
				Console.WriteLine("This map has no onload scripts, or executing scripts on load is disabled");
			}
			string text = null;
			int num = -1;
			foreach (Map.BGM bgm in map.Music)
			{
				if (FlagManager.Instance[(int)bgm.Flag] && (int)bgm.Flag > num)
				{
					text = bgm.Name;
					num = (int)bgm.Flag;
				}
			}
			if (text != null)
			{
				this.musicName = text;
				Console.WriteLine("Play BGM {0}", this.musicName);
				AudioManager.Instance.SetBGM(this.musicName);
			}
			else
			{
				Console.WriteLine((map.Music.Count > 0) ? "No BGM flags were enabled for any BGM for this map." : "This map has no BGMs set.");
			}
			this.battleStartSound = AudioManager.Instance.Use(Paths.SFXBATTLE + "battleIntro.mp3", AudioType.Sound);
			this.initialized = true;
		}

		public override void Focus()
		{
			base.Focus();
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.pipeline.Each(delegate (Renderable x)
			{
				if (x is IndexedColorGraphic)
				{
					((IndexedColorGraphic)x).AnimationEnabled = true;
				}
				if (x is TileGroup)
				{
					((TileGroup)x).AnimationEnabled = true;
				}
			});
			ViewManager.Instance.FollowActor = this.player;
			ViewManager.Instance.Offset = new Vector2f(0f, (float)(-(float)((int)this.player.AABB.Size.Y) / 2));
			if (this.battleEnemies != null)
			{
				this.player.MovementLocked = false;
				battleEnemies.ForEach(x => this.collisionManager.Remove(x, false));
                battleEnemies.ForEach(x => this.actorManager.Remove(x));
				/*foreach (EnemyNPC enemy in this.battleEnemies)
				{
					this.collisionManager.Remove(enemy, false);
					this.actorManager.Remove(enemy);
					//this.collisionManager.Remove(enemy, false);
                    //this.collisionManager.Remove(null, false);
					Console.WriteLine($"Removing enemy {enemy}");
                    //this.collisionManager.Update(enemy, enemy.Position, enemy.Position);
				}*/

                battleEnemies = null;
            }
			if (FlagManager.Instance[3])
			{
				this.HandleCheckAction(true);
				FlagManager.Instance[3] = false;
			}
			Engine.ClearColor = this.backColor;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.footstepPlayer.Resume();
			if (!this.dontPauseMusic)
			{
				if (this.musicName != null)
				{
					AudioManager.Instance.SetBGM(this.musicName);
					AudioManager.Instance.BGM.Play();
					AudioManager.Instance.BGM.Position = this.musicPosition;
					return;
				}
				if (AudioManager.Instance.BGM != null)
				{
					AudioManager.Instance.BGM.Stop();
					return;
				}
			}
			else
			{
				this.dontPauseMusic = false;
			}
		}

		public void GoToMap(string map, float xto, float yto, int direction, bool running, bool extendParty, ITransition transition)
		{
			Console.WriteLine("DOOR TIME! Loading {0}", map);
			SceneManager.Instance.Transition = transition;
			SceneManager.Instance.Push(new OverworldScene(map, new Vector2f(xto, yto), direction, running, extendParty, this.enableLoadScripts), true);
		}

		public void GoToMap(Portal door, ITransition transition)
		{
			transition.Blocking = true;
			int direction = (door.DirectionTo < 0) ? this.player.Direction : door.DirectionTo;
			this.GoToMap(door.Map, door.PositionTo.X, door.PositionTo.Y, direction, this.player.Running, false, transition);
		}
		private void OnPlayerRunningChange(Player sender)
		{
			if (sender.Running)
			{
				this.footstepPlayer.Start();
				return;
			}
			this.footstepPlayer.Stop();
		}

		private void OnPlayerCollision(Player sender, ICollidable[] collisionObjects)
		{
			foreach (ICollidable collidable in collisionObjects)
			{
				if (collidable is Portal)
				{
					this.GoToMap((Portal)collidable, new ColorFadeTransition(0.5f, Color.Black));
					return;
				}

				if (collidable is TriggerArea)
				{
					string script = ((TriggerArea)collidable).Script;
					Console.WriteLine("Trigger Area - " + script);
					this.SetExecutorScript(script, false);
					this.executor.Execute();
					collidable.Solid = false;
					return;
				}
            }
		}

        public List<EnemyNPC> AllEnemyNpcs;
        public void StartBattle(EnemyNPC npc)
        {
            List<EnemyNPC> PullNearbyEnemies()
            {


                return null;
            }

            if (battleEnemies == null  || battleEnemies.Count<=0)
            {
                EnemyNPC enemy = npc;
                enemy.MovementLocked = true;
                enemy.FreezeSpriteForever();
                    
                /*List<EnemyNPC> nearby = new List<EnemyNPC>();
                // nearby.AddRange(PullNearbyEnemies());
				//allEnemyNpcs.Add(npc);
				foreach (var _npc in AllEnemyNpcs)
                {

                    if (VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - npc.Position) < 100 && _npc != npc)
                    {

                        nearby.Add(_npc);
						_npc.Snip();
                    }
                }*/
				List<EnemyData> allEnemyDatas = new List<EnemyData>();
              //  nearby.ForEach(x => allEnemyDatas.Add(x.Type));


				this.battleEnemies = new List<EnemyNPC>();

                battleEnemies.Add(enemy);
          //      battleEnemies.AddRange(nearby);

				List<EnemyData> aaa = new List<EnemyData>();
                aaa.Add(enemy.Type);
                aaa.AddRange(allEnemyDatas);

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Enemy specific data time!" 
                                  + 
                                  Environment.NewLine 
                                  +
                                  $"enemy: {enemy} "  + Environment.NewLine
                                  + 
                                //  $"nearby length: {nearby.Count} " + Environment.NewLine
                                  //+ 
                                  $"allEnemyDatas length: {allEnemyDatas.Count} " + Environment.NewLine
                                  + 
                                  $"battleEnemies length: {battleEnemies.Count} " + Environment.NewLine
                                  + 
                                  $"aaa length: {aaa.Count} " + Environment.NewLine
                         //        +
                         //         $"allEnemyDatas length: {allEnemyDatas.Count} " + Environment.NewLine


				);
				Console.ResetColor();
                //this.battleEnemies.AddRange(enemy);
                this.player.MovementLocked = true;
                this.musicPosition = AudioManager.Instance.BGM.Position;
                Carbine.Audio.AudioManager.Instance.BGM.Stop();
                this.battleStartSound.Play();
                SceneManager.Instance.Transition = new BattleSwirlTransition(BattleSwirlOverlay.Style.Blue);
                SceneManager.Instance.Push(new BattleScene(aaa.ToArray(), true));
                return;
            }
        }
		public override void Unfocus()
		{
			base.Unfocus();
			ViewManager.Instance.FollowActor = null;
			if (!this.openingMenu)
			{
				ViewManager.Instance.Offset = VectorMath.ZERO_VECTOR;
			}
			else
			{
				this.pipeline.Each(delegate (Renderable x)
				{
					if (x is IndexedColorGraphic)
					{
						((IndexedColorGraphic)x).AnimationEnabled = false;
					}
					if (x is TileGroup)
					{
						((TileGroup)x).AnimationEnabled = false;
					}
				});
				this.footstepPlayer.Pause();
				this.openingMenu = false;
			}
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			if (this.musicName != null && !this.dontPauseMusic)
			{
				this.musicPosition = AudioManager.Instance.BGM.Position;
			}
		}

		public override void Unload()
		{
			this.player.OnCollision -= this.OnPlayerCollision;
		}

		private void UpdateSpawners()
		{
			for (int i = 0; i < this.spawners.Count; i++)
			{
				EnemySpawner enemySpawner = this.spawners[i];
				if (!enemySpawner.Bounds.Intersects(ViewManager.Instance.Viewrect))
				{
					List<EnemyNPC> list = enemySpawner.GenerateEnemies(this.pipeline, this.collisionManager);
					if (list != null)
					{
						this.actorManager.AddAll<EnemyNPC>(list.ToArray());
						this.collisionManager.AddAll<EnemyNPC>(list);
                        list.ForEach(x=> AllEnemyNpcs.Add(x));
                    }
				}
				else
				{
					enemySpawner.SpawnFlag = true;
				}
			}
		}

		public override void Update()
		{
			base.Update();
			if (this.initialized)
			{
				if (this.rainOverlay != null)
				{
					this.rainOverlay.Update();
				}
				this.partyTrain.Update();
				this.UpdateSpawners();
				this.screenDimmer.Update();
				this.executor.Execute();
                collisionManager.Filter();

			}
		}

		public override void Draw()
		{
			if (this.testBack != null)
			{
				this.testBack.AddTranslation(this.player.Position.X - this.player.LastPosition.X, this.player.Position.Y - this.player.LastPosition.Y, 0.001f, 0.002f);
			}
			base.Draw();
			if (this.rainOverlay != null)
			{
				this.rainOverlay.Draw(this.pipeline.Target);
			}
			if (this.iris != null)
			{
				this.iris.Draw(this.pipeline.Target);
			}
			if (this.collisionManager != null && Engine.debugDisplay)
			{
				this.collisionManager.Draw(this.pipeline.Target);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (TileGroup tileGroup in this.mapGroups)
					{
						this.pipeline.Remove(tileGroup);
						tileGroup.Dispose();
					}
					this.actorManager.ClearActors();
					foreach (TileGroup tileGroup2 in this.mapGroups)
					{
						tileGroup2.Dispose();
					}
					foreach (ParallaxBackground parallaxBackground in this.parallaxes)
					{
						parallaxBackground.Dispose();
					}
					this.screenDimmer.Dispose();
					// this.letterboxing.Dispose();
					this.textbox.Dispose();
					this.footstepPlayer.Dispose();
					this.pipeline.Clear();
					if (this.iris != null)
					{
						this.iris.Dispose();
					}
				}
				this.actorManager = null;
				this.mapGroups = null;
				this.parallaxes = null;
				this.screenDimmer = null;
				//this.letterboxing = null;
				this.textbox = null;
				this.pipeline = null;
				this.collisionManager = null;
				this.footstepPlayer = null;
				this.player.OnCollision -= this.OnPlayerCollision;
				this.player = null;
			}
			base.Dispose(disposing);
		}

		private const int DIMMER_DEPTH = 2147450870;

		private Color backColor;

		private CollisionManager collisionManager;

		private Player player;

		private PartyTrain partyTrain;

		private ScreenDimmer screenDimmer;

		private TextBox textbox;

		private QuestionBox questionbox;

		private ScriptExecutor executor;

		private CarbineSound battleStartSound;

		private string musicName;

		private uint musicPosition;

		private bool dontPauseMusic;

		private IList<EnemySpawner> spawners;

		private List<EnemyNPC> battleEnemies;

		private IList<ParallaxBackground> parallaxes;

		private BattleBackgroundRenderable testBack;

		public IList<TileGroup> MapGroups
		{ get { 
				return mapGroups; 
			} 
		}

		private IList<TileGroup> mapGroups;

		private string mapName;

		private Vector2f initialPosition;

		private int initialDirection;

		private bool initialRunning;

		private IrisOverlay iris;

		private bool initialized;

		private bool enableLoadScripts;

		private bool extendParty;

		private bool openingMenu;

		private RainOverlay rainOverlay;

		private FootstepPlayer footstepPlayer;
	}
}