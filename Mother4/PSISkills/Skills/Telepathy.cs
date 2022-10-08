using SFML.Graphics;
using SFML.System;
using Mother4.Battle;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.GUI.Modifiers;
using Mother4.Psi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carbine.Graphics;

namespace Mother4.SOMETHING
{
    public class Telepathy : PSIBase
    {
        public override int AUCost => 0; 
        public override TargetingMode TargetMode => TargetingMode.Enemy;
        public override int[] Symbols => new int[2];
        public override string QualifiedName => "Telapathy";
        public override string Key => "psi.telepathy"; 
        internal override IPsi identifier => new AssistivePsi(); 

        public Telepathy()
        {
         //   Console.WriteLine("THE PURPOSE OF MAN IS TO PERFORM TELEPATHY");
        }

        internal override void Initialize(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets)
        {
            string message = string.Format("{0} tried {1} {2}!", CharacterNames.GetName(combantant.Character), QualifiedName, PsiLetters.Get(action.psiLevel));

            action.state = PlayerPsiAction.State.WaitForUI;
            interfaceController.OnTextboxComplete += OnTextboxComplete;
            interfaceController.ShowMessage(message, false);
            interfaceController.PopCard(combantant.ID, 23);

            void OnTextboxComplete()
            {
                interfaceController.OnTextboxComplete -= OnTextboxComplete;
                action.state = PlayerPsiAction.State.Animate;
            }
            Console.WriteLine("initialize");
        }

        internal override void Animate(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets)
        {
            action.state = PlayerPsiAction.State.DamageNumbers;
        }
        internal override void Act(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action)
        {
            Console.WriteLine("act");
            foreach (Combatant combatant in combatants)
            {
                EnemyCombatant enemy = combatant as EnemyCombatant;
                if (enemy == null)
                {               
                    throw new Exception("Combatant is not an enemy or somehow has null data!");
                    return;
                }
                interfaceController.ShowStyledMessage($"{enemy.Enemy.GetStringQualifiedName("telepathy")}", true, Carbine.GUI.WindowBox.Style.Telepathy);
                interfaceController.OnTextboxComplete += InterfaceController_OnTextboxComplete;
                void InterfaceController_OnTextboxComplete()
                {
                    interfaceController.OnTextboxComplete -= InterfaceController_OnTextboxComplete;
                    interfaceController.ResetTextboxStyle();
                    action.state = PlayerPsiAction.State.Finish;
                }
            }
            StatSet statChange2 = new StatSet
            {
                PP = -this.AUCost,
                Meter = 0.026666667f
            };
            combantant.AlterStats(statChange2);
            action.state = PlayerPsiAction.State.WaitForUI;

        }

        

        internal override void ScaleToLevel(PlayerCombatant combatant)
        {
        }

        internal override void ShowUnavaliableMessage(PlayerCombatant combatant, BattleInterfaceController interfaceController)
        {
            interfaceController.ShowMessage("Not enough AU!", false);
        }

        internal override void Finish(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action)
        {
            interfaceController.PopCard(combantant.ID, 0);
            action.Finish();
        }
    }
}
