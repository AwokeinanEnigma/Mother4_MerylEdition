using System;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Mother4.Battle;
using Mother4.Battle.Actions;
using Mother4.Battle.Background;
using Mother4.Battle.Combatants;
using Mother4.Battle.Combos;
using Mother4.Data;
using Mother4.Data.Enemies;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	internal class BattleScene : StandardScene
	{
		public BattleScene(EnemyData[] enemies, bool letterboxing)
		{
			this.enemies = enemies;
			this.letterboxing = letterboxing;
		}

		public BattleScene(EnemyData[] enemies, bool letterboxing, int bgmOverride, int bbgOverride) : this(enemies, letterboxing)
		{
			this.bgmOverride = bgmOverride;
			this.bbgOverride = bbgOverride;
		}

		private void Initialize()
		{
			CharacterType[] party = PartyManager.Instance.ToArray();
			this.combatantController = new CombatantController(party, this.enemies);
			EnemyCombatant enemyCombatant = null;
			Combatant[] factionCombatants = this.combatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			foreach (Combatant combatant in factionCombatants)
			{
				if ((combatant as EnemyCombatant).Enemy == enemies[0])
				{
					enemyCombatant = (combatant as EnemyCombatant);
                    Console.WriteLine($"{enemyCombatant.Enemy.PlayerFriendlyName} && {enemies[0].PlayerFriendlyName}");
				}
			}
			Combatant[] factionCombatants2 = this.combatantController.GetFactionCombatants(BattleFaction.PlayerTeam, true);
			PlayerCombatant playerCombatant = factionCombatants2[0] as PlayerCombatant;
			PlayerCombatant playerCombatant2 = factionCombatants2[(factionCombatants2.Length > 1) ? 1 : 0] as PlayerCombatant;
			string music = enemyCombatant.Enemy.MusicName;
		//	Console.WriteLine($"MUSIC - { music } // ENEMY - {enemyCombatant.Enemy}");
			ComboSet combos = ComboLoader.Load(music);

			AudioManager.Instance.SetBGM(Paths.BGMBATTLE +  $"{music}.wav");
			this.comboControl = new ComboController(combos, party);
			this.uiController = new BattleInterfaceController(this.pipeline, this.actorManager, this.combatantController, this.letterboxing);
           // Console.WriteLine("init btc");
            this.controller = new BattleController(this.uiController, this.combatantController, this.comboControl);
           // Console.WriteLine("init bc");
			this.background = new BattleBackground(Paths.GRAPHICS + $"BBG/xml/{enemyCombatant.Enemy.BackgroundName}.xml");
           // Console.WriteLine("init bbg");
			this.GenerateIntroMessage(factionCombatants2.Length, factionCombatants.Length, playerCombatant.Character, playerCombatant2.Character, enemyCombatant.Enemy);
            uiController.controller = controller;
            uiController.background = background;
            this.GenerateDebugVerts();
			this.debugBgmPos = (long)((ulong)AudioManager.Instance.BGM.Position);
			this.debugLastBgmPos = this.debugBgmPos;
		}

		private void GenerateIntroMessage(int partyCount, int enemyCount, CharacterType partyLead, CharacterType partySecondary, EnemyData enemyLead)
		{
			string arg;
			if (partyCount == 1)
			{
				arg = CharacterNames.GetName(partyLead);
			}
			else if (partyCount == 2)
			{
				arg = string.Format("{0} and {1}", CharacterNames.GetName(partyLead), CharacterNames.GetName(partySecondary));
			}
			else
			{
				arg = string.Format("{0} and friends", CharacterNames.GetName(partyLead));
			}
			string arg2;
			if (enemyCount == 1)
			{
				arg2 = string.Format("{0}{1}", enemyLead.Article, enemyLead.QualifiedName);
			}
			else if (enemyCount == 2)
			{
				arg2 = string.Format("{0}{1} and {2} cohort", enemyLead.Article, enemyLead.QualifiedName, enemyLead.GetStringQualifiedName("possessive"));
			}
			else
			{
				arg2 = string.Format("{0}{1} and {2} cohorts", enemyLead.Article, enemyLead.QualifiedName, enemyLead.GetStringQualifiedName("possessive"));
			}
			string text = string.Format("{0} engaged {1}.", arg, arg2);
			ActionParams aparams = default(ActionParams);
			aparams.actionType = typeof(MessageAction);
			aparams.controller = this.controller;
			aparams.sender = null;
			aparams.data = new object[]
			{
				text,
				false
			};
			this.controller.AddAction(BattleAction.GetInstance(aparams));
		}

		private void GenerateDebugVerts()
		{
			this.debugRenderStates = new RenderStates(BlendMode.None, Transform.Identity, null, null);
			this.debugRenderStates.Transform.Translate((float)(-(float)this.debugBgmPos) * 0.1f, 0f);
			Color color = new Color(0, 0, 0, 128);
			this.debugRect = new VertexArray(PrimitiveType.Quads, 4U);
			this.debugRect[0U] = new Vertex(new Vector2f(0f, 84f), color);
			this.debugRect[1U] = new Vertex(new Vector2f(320f, 84f), color);
			this.debugRect[2U] = new Vertex(new Vector2f(320f, 96f), color);
			this.debugRect[3U] = new Vertex(new Vector2f(0f, 96f), color);
			this.debugCrosshairVerts = new VertexArray(PrimitiveType.Lines, 8U);
			this.debugCrosshairVerts[0U] = new Vertex(new Vector2f(0f, 90f), Color.White);
			this.debugCrosshairVerts[1U] = new Vertex(new Vector2f(320f, 90f), Color.White);
			this.debugCrosshairVerts[2U] = new Vertex(new Vector2f(160f, 85f), Color.White);
			this.debugCrosshairVerts[3U] = new Vertex(new Vector2f(160f, 95f), Color.White);
			this.debugCrosshairVerts[4U] = new Vertex(new Vector2f(138f, 85f), Color.Red);
			this.debugCrosshairVerts[5U] = new Vertex(new Vector2f(138f, 95f), Color.Red);
			this.debugCrosshairVerts[6U] = new Vertex(new Vector2f(182f, 85f), Color.Green);
			this.debugCrosshairVerts[7U] = new Vertex(new Vector2f(182f, 95f), Color.Green);
			this.debugBeatVerts = new VertexArray(PrimitiveType.Lines);
			foreach (ComboNode comboNode in this.comboControl.ComboSet.ComboNodes)
			{
				int num = (int)(160L + (long)((int)(comboNode.Timestamp * 0.1f)));
				int num2 = 90;
				if (comboNode.Type == ComboType.Point)
				{
					this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num, (float)(num2 - 4)), Color.Cyan));
					this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num, (float)(num2 + 4)), Color.Cyan));
				}
				else if (comboNode.Type == ComboType.BPMRange)
				{
					for (long num3 = 0L; num3 < (long)((ulong)comboNode.Duration); num3 += (long)(60000f / comboNode.BPM.Value))
					{
						int num4 = num + (int)((float)num3 * 0.1f);
						this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num4, (float)(num2 - 4)), Color.Cyan));
						this.debugBeatVerts.Append(new Vertex(new Vector2f((float)num4, (float)(num2 + 4)), Color.Cyan));
					}
				}
			}
		}

		public override void Focus()
		{
			ViewManager.Instance.Reset();
			this.Initialize();
			Engine.ClearColor = Color.Black;
			ViewManager.Instance.Reset();
			AudioManager.Instance.BGM.Play();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.initialized = true;
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			
			Combatant combatant = null;
			switch (b)
			{
			case Button.One:
				combatant = this.combatantController[0];
				break;
			case Button.Two:
				combatant = this.combatantController[1];
				break;
			case Button.Three:
				combatant = this.combatantController[2];
				break;
			case Button.Four:
				combatant = this.combatantController[3];
				break;
			}
			if (combatant != null)
			{
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(TestingSmiteAction),
					controller = this.controller,
					targets = new Combatant[]
					{
						combatant
					},
					priority = int.MaxValue
				};
				this.controller.AddAction(BattleAction.GetInstance(aparams));
			}
		}

		public override void Unfocus()
		{
			AudioManager.Instance.BGM.Stop();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		public override void Unload()
		{
			base.Dispose();
		}

		public override void Update()
		{
			if (this.initialized)
			{
				this.uiController.Update();
				this.controller.Update();
			}
			base.Update();
		}

		public override void Draw()
		{
			if (this.initialized)
			{
				this.background.Draw(this.pipeline.Target);
				this.controller.InterfaceController.Draw(this.pipeline.Target);
			}
			base.Draw();
			if (Engine.debugDisplay && this.initialized)
			{
				this.debugLastBgmPos = this.debugBgmPos;
				this.debugBgmPos = (long)((ulong)AudioManager.Instance.BGM.Position);
				long num = this.debugBgmPos - this.debugLastBgmPos;
				this.debugRenderStates.Transform.Translate((float)(-(float)num) * 0.1f, 0f);
				this.pipeline.Target.Draw(this.debugRect);
				this.pipeline.Target.Draw(this.debugCrosshairVerts);
				this.pipeline.Target.Draw(this.debugBeatVerts, this.debugRenderStates);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.controller?.Dispose();
				this.uiController?.Dispose();
				this.comboControl?.Dispose();
			}
			base.Dispose(disposing);
		}

		private const float DEBUG_SCALE = 0.1f;

		private CombatantController combatantController;

        private BattleInterfaceController uiController;

		private BattleController controller;

		private ComboController comboControl;

		private BattleBackground background;

		private VertexArray debugRect;

		private VertexArray debugCrosshairVerts;

		private VertexArray debugBeatVerts;

		private RenderStates debugRenderStates;

		private long debugBgmPos;

		private long debugLastBgmPos;

		private bool initialized;

		private EnemyData[] enemies;

		private bool letterboxing;

		private int bgmOverride;

		private int bbgOverride;
	}
}
