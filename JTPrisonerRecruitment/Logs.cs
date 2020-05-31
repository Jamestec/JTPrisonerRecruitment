using TaleWorlds.Core;
using TaleWorlds.Library;

namespace JTPrisonerRecruitment
{
    public static class Logs
    {
        public static void log(string message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message));
        }
        public static void log(string message, Color color)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, color));
        }
    }
}
