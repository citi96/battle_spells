using Battle_Spells.Models.Enums.Card;
using Godot.Collections;

namespace BattleSpells.Scripts.Resources.Effects.Atherex
{
    public static class AtherexEffectFactory
    {
        public static CardEffect CreateMalformedExplosion(int primaryDamage = 2, int splashDamage = 1)
        {
            return new CardEffect
            {
                EffectName = "Malformed Explosion",
                Description = "Explodes on death dealing direct and splash damage.",
                Activation = ECardEffectActivation.OnDeath,
                Steps = new Array<EffectStep>
                {
                    new DamageEffectStep
                    {
                        Damage = new DamageProfile { Amount = primaryDamage },
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.Enemy,
                            Strategy = TargetingStrategy.Nearest,
                            AllowedType = ECardType.Minion
                        }
                    },
                    new DamageEffectStep
                    {
                        Damage = new DamageProfile { Amount = splashDamage },
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.Enemy,
                            Strategy = TargetingStrategy.Adjacent,
                            AllowedType = ECardType.Minion
                        }
                    }
                }
            };
        }

        public static SummonProfile CreateMalformedProfile(int attack, int health, int copies = 1, BoardRow row = BoardRow.Front)
        {
            return new SummonProfile
            {
                TokenName = "Malformed",
                Attack = attack,
                Health = health,
                Copies = copies,
                Placement = new EffectTargetQuery
                {
                    Team = TargetTeam.Ally,
                    Strategy = TargetingStrategy.FreeSlots,
                    Row = row,
                    RequireFreeTile = true
                },
                DeathEffects = new Array<CardEffect> { CreateMalformedExplosion() }
            };
        }

        public static CardEffect CreateVaultPressurePassive()
        {
            return new CardEffect
            {
                EffectName = "Cathedral Body",
                Description = "Stacks Vault Pressure on allied deaths and spawns a Malformed when full.",
                Activation = ECardEffectActivation.OnAllyDeath,
                Steps = new Array<EffectStep>
                {
                    new StackingEffectStep
                    {
                        CounterKey = "vault_pressure",
                        MaxStacks = 5,
                        Threshold = 5,
                        ResetOnTrigger = true,
                        OnThresholdReached = new Array<EffectStep>
                        {
                            new SummonEffectStep
                            {
                                Profile = CreateMalformedProfile(1, 2)
                            }
                        }
                    }
                }
            };
        }

        public static CardEffect CreateCervicalBloom()
        {
            return new CardEffect
            {
                EffectName = "Cervical Bloom",
                Description = "Summons a Malformed 1/2 on the front row that explodes on death.",
                Activation = ECardEffectActivation.OnPlay,
                Steps = new Array<EffectStep>
                {
                    new SummonEffectStep
                    {
                        Profile = CreateMalformedProfile(1, 2)
                    }
                }
            };
        }

        public static CardEffect CreatePlacentalShroud()
        {
            return new CardEffect
            {
                EffectName = "Placental Shroud",
                Description = "Reduces damage taken by 3 and heals the remainder at end of turn.",
                Activation = ECardEffectActivation.OnPlay,
                Steps = new Array<EffectStep>
                {
                    new ShieldEffectStep
                    {
                        Shield = new ShieldProfile
                        {
                            DamageReduction = 3,
                            DurationTurns = 1,
                            ConvertUnusedReductionToHeal = true
                        },
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.SelfOnly,
                            Strategy = TargetingStrategy.Self,
                            AllowedType = ECardType.Hero
                        }
                    }
                }
            };
        }

        public static CardEffect CreateBreachCycle()
        {
            return new CardEffect
            {
                EffectName = "Breach Cycle",
                Description = "Damages allied minions; heals Atherex for each that dies.",
                Activation = ECardEffectActivation.OnPlay,
                Steps = new Array<EffectStep>
                {
                    new DamageEffectStep
                    {
                        Damage = new DamageProfile { Amount = 2 },
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.Ally,
                            Strategy = TargetingStrategy.All,
                            AllowedType = ECardType.Minion
                        }
                    },
                    new ScaledHealEffectStep
                    {
                        HealPerDefeatedUnit = 1,
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.SelfOnly,
                            Strategy = TargetingStrategy.Self,
                            AllowedType = ECardType.Hero
                        }
                    }
                }
            };
        }

        public static CardEffect CreateCathedralOfUnbirth()
        {
            var malformedSpawner = new CardEffect
            {
                EffectName = "Malformed Choir",
                Description = "Summons up to three Malformed 2/2 then damages all minions.",
                Activation = ECardEffectActivation.EndOfTurn,
                Steps = new Array<EffectStep>
                {
                    new SummonEffectStep
                    {
                        Profile = CreateMalformedProfile(2, 2, 3)
                    },
                    new DamageEffectStep
                    {
                        Damage = new DamageProfile { Amount = 1 },
                        TargetQuery = new EffectTargetQuery
                        {
                            Team = TargetTeam.Everyone,
                            Strategy = TargetingStrategy.All,
                            AllowedType = ECardType.Minion
                        }
                    }
                }
            };

            return new CardEffect
            {
                EffectName = "Cathedral of Unbirth",
                Description = "Places a structure that births Malformed each turn and harms all minions.",
                Activation = ECardEffectActivation.OnPlay,
                Steps = new Array<EffectStep>
                {
                    new SummonEffectStep
                    {
                        Profile = new SummonProfile
                        {
                            TokenName = "Cathedral of Unbirth",
                            Attack = 0,
                            Health = 10,
                            SpawnType = ECardType.Structure,
                            Placement = new EffectTargetQuery
                            {
                                Team = TargetTeam.Ally,
                                Strategy = TargetingStrategy.FreeSlots,
                                Row = BoardRow.Back,
                                RequireFreeTile = true
                            },
                            OngoingEffects = new Array<CardEffect> { malformedSpawner }
                        }
                    }
                }
            };
        }
    }
}
