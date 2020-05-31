using MCM.Abstractions.Settings.Base.Global;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace JTPrisonerRecruitment
{
    public class JTGetDailyRecruitedPrisoners : DefaultPrisonerRecruitmentCalculationModel 
    {
        public override float[] GetDailyRecruitedPrisoners(MobileParty mainParty)
        {
            Settings settings = GlobalSettings<Settings>.Instance;
            // This gets called twice in a daily tick and I don't know why, vanilla calling it?
            float[] chances = (float[])settings._chances.Clone();
            // Chance increases based on charm skill level
            // Charm level of 100 will increase chance by 33%
            float charmIncrease = settings.CharmBase + Clan.PlayerClan.Leader.GetSkillValue(DefaultSkills.Charm) / (float)settings.CharmDiv;
            // Chance increases based on clan tier
            for (int i = 0; i < settings._chances.Length; i++)
            {
                chances[i] = chances[i] * charmIncrease * (Clan.PlayerClan.Tier + settings.CharmClanTierMod);
            }
            return chances;
        }
	}

}
