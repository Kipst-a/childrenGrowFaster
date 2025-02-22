using MCM.Abstractions.Base.Global;
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
        }

        public override void SyncData(IDataStore dataStore)
        {
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
                    Hero.MainHero.SetPersonalRelation(lord, randomRelationGain);
                    InformationManager.DisplayMessage(
                        new InformationMessage($"{spouse.Name} has {(randomRelationGain < 0 ? "decreased" : "increased")} by {Math.Abs(randomRelationGain)}", Colors.Green));
                }
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

        private void SpouseEvent3() // only works if the settlement is owned by the player
        {
            List<Workshop> workshops = new List<Workshop>();
            workshops.AddRange(Hero.MainHero.OwnedWorkshops);
            foreach (Workshop workshop in workshops)
            {
                if (workshop.Settlement != Hero.MainHero.CurrentSettlement && workshop.Settlement == spouse.CurrentSettlement && workshop != null)
                {
                    int randomProfit = MBRandom.RandomInt(100, 900);
                    workshop.ChangeGold(randomProfit);
                    InformationManager.DisplayMessage(
                        new InformationMessage($"Your spouse has boosted the profits of {workshop.Name} by {randomProfit} gold!", Colors.Green));
                }
            }
        }

        private void SpouseEvent4() // only works if the settlement is owned by the player 
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
            if (spouse == null) return;

            var notablesInSpouseSettlement = spouse.CurrentSettlement.Notables;
            foreach (var notable in notablesInSpouseSettlement)
            {
                if (spouse.CurrentSettlement != Hero.MainHero.CurrentSettlement)
                {
                    Hero.MainHero.SetPersonalRelation(notable, MBRandom.RandomInt(1, 5));
                    InformationManager.DisplayMessage(
                        new InformationMessage($"{spouse.Name} has increased relation with {notable.Name} by {MBRandom.RandomInt(1, 5)}", Colors.Green));
                }
            }
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

/*
 * TODO: 
 * Make it so that the events that only work if the settlement is owned by the player, to work for any settlement in the same kingdom as the player.
 Assuming the player is a vassal of a kingdom and didnt start their own kingdom. 

Fall back to the original spouse events when the player is in its own kingdom. 
 */
