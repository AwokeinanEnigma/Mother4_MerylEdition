using Mother4.Battle;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mother4.SOMETHING
{
    public abstract class PSIBase
    {
        internal abstract Psi.IPsi identifier { get; }

        public abstract int AUCost { get; }
        public abstract TargetingMode TargetMode { get; }
        public abstract int[] Symbols { get; }
        public abstract string QualifiedName { get; }
        public abstract string Key { get; }

        internal abstract void Initialize(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets);
        internal abstract void Animate(PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action, Combatant[] targets);
        internal abstract void Act(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action);
        internal abstract void Finish(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action);
        internal abstract void ShowUnavaliableMessage(PlayerCombatant combatant, BattleInterfaceController interfaceController);
        internal abstract void ScaleToLevel(PlayerCombatant combatant);
        internal void Update(Combatant[] combatants, PlayerCombatant combantant, BattleInterfaceController interfaceController, PlayerPsiAction action) 
        { 
            // do nothing
        }

        internal virtual bool GetAvailiability(PlayerCombatant combantant, BattleInterfaceController interfaceController) {
            if (combantant.Stats.PP < AUCost) {
                return false;
            }
            return true;
        }
    }
}
