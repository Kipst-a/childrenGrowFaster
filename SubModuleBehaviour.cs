﻿using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace childrenGrowFaster
{
    public class SubModuleBehaviour : CampaignBehaviorBase
    {
        private Hero spouse = Hero.MainHero.Spouse;
        private bool isKidnapped = false;
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick2);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnDailyTick2()
        {
            if (GlobalSettings<SubModuleSettings>.Instance.spouseEventsEnabled)
            {
                if (Hero.MainHero.Spouse != null && !Hero.MainHero.Spouse.IsPregnant)
                {
                    FireRandomSpouseEvent();
                }
            }
        }

        private void FireRandomSpouseEvent()
        {
            var events = new List<Action> { SpouseEvent1, SpouseEvent2, SpouseEvent3, SpouseEvent4 };
            if (MBRandom.RandomFloat < GlobalSettings<SubModuleSettings>.Instance.eventChance)
            {
                events[MBRandom.RandomInt(events.Count)]();
            }
        }

        private void SpouseEvent1()
        {
            if (spouse == null) return;
            
            var randomRelationGain = MBRandom.RandomInt(-1, 5);
            List<Hero> lordsInSpouseSettlement = spouse.CurrentSettlement.Parties.Select(p => p.LeaderHero).ToList();
            foreach (Hero lord in lordsInSpouseSettlement)
            {
                if (spouse.CurrentSettlement != Hero.MainHero.CurrentSettlement && !isKidnapped)
                {
                    if (MBRandom.RandomInt(1, 11) < GlobalSettings<SubModuleSettings>.Instance.eventChance)
                    {
                        Hero.MainHero.SetPersonalRelation(lord, randomRelationGain);
                        InformationManager.DisplayMessage(new InformationMessage($"{spouse.Name} has {(randomRelationGain < 0 ? "decreased" : "increased")} by {Math.Abs(randomRelationGain)}", Colors.Green));
                    }
                }
                else return;
            }
        }

        private void SpouseEvent2()
        {
            
            if (spouse == null) return;

            if (spouse.CurrentSettlement != null &&
                spouse.CurrentSettlement != Hero.MainHero.CurrentSettlement &&
                !isKidnapped)
            {
                int gainedAmount = MBRandom.RandomInt(500, 1000);

                Hero.MainHero.ChangeHeroGold(gainedAmount);
                InformationManager.DisplayMessage(
                    new InformationMessage($"{spouse.Name} has earned you {gainedAmount} gold!", Colors.Green));
            }
        }

        private void SpouseEvent3()
        {
            List<Workshop> workshops = new List<Workshop>();
            workshops.AddRange(Hero.MainHero.OwnedWorkshops);
            foreach (Workshop workshop in workshops)
            {
                if (workshop.Settlement != Hero.MainHero.CurrentSettlement && workshop.Settlement == spouse.CurrentSettlement && workshop != null)
                {
                    int randomProfit = MBRandom.RandomInt(100, 900);
                    workshop.ChangeGold(randomProfit);
                    InformationManager.DisplayMessage(new InformationMessage($"Your spouse has boosted the profits of {workshop.Name} by {randomProfit} gold!", Colors.Green));
                }
            }
        }

        private void SpouseEvent4()
        {
            foreach (Settlement s in Settlement.All)
            {
                if (!IsValidSettlement(s, spouse)) return;

                TroopRoster garrisonRoster = s.Town.GarrisonParty.MemberRoster;
                if (garrisonRoster == null) return;
                GiveXP(garrisonRoster, s, spouse);
            }
        }

        private void SpouseEvent5()
        {
        }

        private bool IsValidSettlement(Settlement s, Hero spouse)
        {
            return s.OwnerClan == Hero.MainHero.Clan
                && spouse.CurrentSettlement == s
                && Hero.MainHero.CurrentSettlement != spouse.CurrentSettlement
                && s != null;
        }

        private void GiveXP(TroopRoster garrisonRoster, Settlement s, Hero spouse)
        {
            foreach (TroopRosterElement troop in garrisonRoster.GetTroopRoster())
            {
                if (troop.Character == null) continue;

                var randomXP = MBRandom.RandomInt(100, 999);
                var skills = typeof(DefaultSkills).GetFields();
                foreach (var skill in skills)
                {
                    if (skill.GetValue(null) is SkillObject skillObject)
                    {
                        troop.Character.HeroObject?.AddSkillXp(skillObject, randomXP);
                        InformationMessage message = new InformationMessage(
                            $"{spouse.Name}'s leadership & steward skills have increased the xp of garrisoned troops in {s.Name} by {randomXP}",
                            Colors.Green);
                        InformationManager.DisplayMessage(message);
                    }
                }
            }
        }
    }
}