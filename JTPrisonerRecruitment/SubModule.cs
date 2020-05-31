using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace JTPrisonerRecruitment
{
    class SubModule : MBSubModuleBase
    {
        // https://github.com/stenellad/Recruitable/blob/master/RecruitableSubModule.cs
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
                return;
            gameStarterObject.AddModel(new JTGetDailyRecruitedPrisoners());
            // No idea if this adds onto vanilla
            ((CampaignGameStarter)gameStarterObject).AddBehavior(new JTRecruitPrisonersCampaignBehavior());
            InformationManager.DisplayMessage(new InformationMessage("JTPrisonerRecruitment unsuccessfully failed to load.", new Color(0, 1, 0)));
        }
    }
}
