/*using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;

namespace Mother4.Data
{
    internal static class EnemyBattleActions
    {
        public static List<ActionParams> GetBattleActionParams(EnemyType enemy)
        {
            List<ActionParams> result = null;
            if (EnemyBattleActions.battleActionTypes.ContainsKey(enemy))
            {
                result = EnemyBattleActions.battleActionTypes[enemy];
            }

            return result;
        }

        private static Dictionary<EnemyType, List<ActionParams>> battleActionTypes =
            new Dictionary<EnemyType, List<ActionParams>>
            {
                {
                    EnemyType.Dummy,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyBashAction),
                            data = new object[]
                            {
                                2f
                            }
                        }
                    }
                },
                {
                    EnemyType.MagicSnail,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyBashAction),
                            data = new object[]
                            {
                                2f
                            }
                        }
                    }
                },
                {
                    EnemyType.Stickat,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Stickat started sweating generously!",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.Mouse,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Mouse is just laying there like a slug.",
                                false
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyBashAction),
                            data = new object[]
                            {
                                2f
                            }
                        }
                    }
                },
                {
                    EnemyType.HermitCan,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Hermit Can is just sort of hanging out, I guess.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.Flamingo,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "Mr. Flamingo is smirking.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.AtomicPowerRobo,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Atomic Power Robo is emitting a slight hum.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.CarbonPup,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Carbon Pup is wagging its tail nervously.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.MeltyRobot,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Melty Robot is slowly melting.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.ModernMind,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(DisablePSIAction),
                            data = new object[]
                            {
                                "a comet",
                                5
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyProjectileAction),
                            data = new object[]
                            {
                                "a comet",
                                5
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Modern Mind is having trouble thinking!",
                                true
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyBashAction),
                            data = new object[]
                            {
                                3f
                            }
                        }
                    }
                },
                {
                    EnemyType.NotSoDeer,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Not-So-Deer is staring blankly.",
                                false
                            }
                        }
                    }
                },
                {
                    EnemyType.MysteriousTank,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyProjectileAction),
                            data = new object[]
                            {
                                "a comet",
                                40,
                                true,
                                "fired!"
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "Something inside the tank is moving!",
                                true
                            }
                        },
                        new ActionParams
                        {
                            actionType = typeof(EnemyBashAction),
                            data = new object[]
                            {
                                25f,
                                true,
                                "rolled over"
                            }
                        }
                    }
                },
                {
                    EnemyType.RamblingMushroom,
                    new List<ActionParams>
                    {
                        new ActionParams
                        {
                            actionType = typeof(EnemyTurnWasteAction),
                            data = new object[]
                            {
                                "The Rambling Mushroom is just... standing there?",
                                false
                            }
                        }
                    }
                }
            };
    }
}
*/