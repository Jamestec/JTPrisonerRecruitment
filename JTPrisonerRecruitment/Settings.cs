using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;
using System.ComponentModel;

namespace JTPrisonerRecruitment
{
    class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "JTPrisonerRecruitment";
        public override string DisplayName => "JTPrisonerRecruitment";
        public override string FolderName => "JTPrisonerRecruitment";
        public override string Format => "json";

        // Note, index 0, aka 1.0f is for Tier 0, which I don't think exists
        public float[] _chances = { 1.0f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.05f, 0.025f };

        public float _charmBase = 1.0f;
        public int _charmDiv = 300;
        
        public float _leadershipBase = 1.0f;
        public int _leadershipCap = 275;
        public int _leadershipDiv = 300;

        public int _clanTierChanceIncrease = 1;

        public float _clanTierChanceLossDecreaseBase = 1.0f;
        public float _clanTierChanceLossDecreaseFloat = 0.5f;
        public float _clanTierChanceLossDecreasePowerBase = 2;
        public int _clanTierChanceLossDecreasePowerAdd = 0;

        #region presents
        /*
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            var basePresets = new Dictionary<string, Func<BaseSettings>>();
            basePresets.Add("Default", () => new Settings()
            {
                ChanceTier0 = 1.0f,
                ChanceTier1 = 0.5f,
                ChanceTier2 = 0.4f,
                ChanceTier3 = 0.3f,
                ChanceTier4 = 0.2f,
                ChanceTier5 = 0.1f,
                ChanceTier6 = 0.05f,
                ChanceTier7 = 0.025f,
                CharmBase = 1.0f,
                CharmDiv = 300,
                CharmClanTierMod = 1,
                LeadershipBase = 1.0f,
                LeadershipCap = 275,
                LeadershipDiv = 300
            });
            return basePresets;
        }*/
        #endregion

        #region chance settings
        [SettingPropertyFloatingInteger("Tier 0 base chance", 0f, 10f, "0.###", Order = 0, RequireRestart = false, HintText = "Native is 1.0")]
        [SettingPropertyGroup("Chances", GroupOrder = 0)]
        public float ChanceTier0
        {
            get => _chances[0];
            set
            {
                _chances[0] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 1 base chance", 0f, 10f, "0.###", Order = 1, RequireRestart = false, HintText = "Native is 0.5")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier1
        {
            get => _chances[1];
            set
            {
                _chances[1] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 2 base chance", 0f, 10f, "0.###", Order = 2, RequireRestart = false, HintText = "Native is 0.3")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier2
        {
            get => _chances[2];
            set
            {
                _chances[2] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 3 base chance", 0f, 10f, "0.###", Order = 3, RequireRestart = false, HintText = "Native is 0.2")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier3
        {
            get => _chances[3];
            set
            {
                _chances[3] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 4 base chance", 0f, 10f, "0.###", Order = 4, RequireRestart = false, HintText = "Native is 0.1")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier4
        {
            get => _chances[4];
            set
            {
                _chances[4] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 5 base chance", 0f, 10f, "0.###", Order = 5, RequireRestart = false, HintText = "Native is 0")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier5
        {
            get => _chances[5];
            set
            {
                _chances[5] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 6 base chance", 0f, 10f, "0.###", Order = 6, RequireRestart = false, HintText = "Native is 0")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier6
        {
            get => _chances[6];
            set
            {
                _chances[6] = value;
            }
        }
        [SettingPropertyFloatingInteger("Tier 7+ base chance", 0f, 10f, "0.###", Order = 7, RequireRestart = false, HintText = "Native is 0")]
        [SettingPropertyGroup("Chances")]
        public float ChanceTier7
        {
            get => _chances[7];
            set
            {
                _chances[7] = value;
            }
        }
        #endregion

        #region charm settings
        [SettingPropertyFloatingInteger("CharmBase chance increase", -10f, 10f, "0.###", Order = 0, RequireRestart = false, HintText = "Recommend to leave at 1")]
        [SettingPropertyGroup("Charm",GroupOrder = 1)]
        public float CharmBase
        {
            get => _charmBase;
            set
            {
                _charmBase = value;
            }
        }
        [SettingPropertyInteger("Charm Level Divider", 1, 1000, "0", Order = 1, RequireRestart = false, HintText = "Increase in chance = chance * (CharmBase + charmLevel / CharmLevelMultiplier)")]
        [SettingPropertyGroup("Charm")]
        public int CharmDiv
        {
            get => _charmDiv;
            set
            {
                _charmDiv = value;
            }
        }
        #endregion

        #region leadership
        [SettingPropertyFloatingInteger("LeadershipBase chance decrease increase", -10f, 10f, "0.###", Order = 3, RequireRestart = false, HintText = "Recommend to leave at 1: increasing this will decrease your chance")]
        [SettingPropertyGroup("Leadership", GroupOrder = 2)]
        public float LeadershipBase
        {
            get => _leadershipBase;
            set
            {
                _leadershipBase = value;
            }
        }
        [SettingPropertyInteger("When leadership stops affecting chance", 0, 1000, "0", Order = 4, RequireRestart = false, HintText = "Recommend Cap < Mult: Cap = Mult means if Leadership level >= mult, you recruit all regardless of chance")]
        [SettingPropertyGroup("Leadership")]
        public int LeadershipCap
        {
            get => _leadershipCap;
            set
            {
                _leadershipCap = value;
            }
        }
        [SettingPropertyInteger("Leadership Level Divider", 1, 1000, "0", Order = 5, RequireRestart = false, HintText = "Recommend you set this to the highest you think Leadership should get before overpowered")]
        [SettingPropertyGroup("Leadership")]
        public int LeadershipDiv
        {
            get => _leadershipDiv;
            set
            {
                _leadershipDiv = value;
            }
        }
        #endregion

        #region clan tier
        [SettingPropertyInteger("Chance Clan Tier Modifier", -10, 10, "0", Order = 0, RequireRestart = false, HintText = "Recommend to leave at 1, chance = chance * charmStuff * (ClanTier + ClanTierModifier)")]
        [SettingPropertyGroup("Clan Tier", GroupOrder = 3)]
        public int CharmClanTierMod
        {
            get => _clanTierChanceIncrease;
            set
            {
                _clanTierChanceIncrease = value;
            }
        }

        [SettingPropertyFloatingInteger("Chance Loss Clan Tier Base Impact", -10f, 10f, "0.###", Order = 1, RequireRestart = false, HintText = "Recommend to leave at 1")]
        [SettingPropertyGroup("Clan Tier")]
        public float ClanTierChanceLossDecreaseBase
        {
            get => _clanTierChanceLossDecreaseBase;
            set
            {
                _clanTierChanceLossDecreaseBase = value;
            }
        }
        [SettingPropertyFloatingInteger("Chance Loss Clan Tier Multiplier Factor", -2f, 2f, "0.###", Order = 2, RequireRestart = false, HintText = "Changes how much Clan Tier impacts chance loss decrease")]
        [SettingPropertyGroup("Clan Tier")]
        public float ClanTierChanceLossDecreaseFloat
        {
            get => _clanTierChanceLossDecreaseFloat;
            set
            {
                _clanTierChanceLossDecreaseFloat = value;
            }
        }
        [SettingPropertyFloatingInteger("Chance Loss Clan Tier Exponent Base", -10f, 10f, "0.###", Order = 2, RequireRestart = false, HintText = "ChanceLoss = Loss / (Base + This^Clan Tier): Increases gradient of clan tier impact")]
        [SettingPropertyGroup("Clan Tier", GroupOrder = 3)]
        public float ClanTierChanceLossDecreasePowerBase
        {
            get => _clanTierChanceLossDecreasePowerBase;
            set
            {
                _clanTierChanceLossDecreasePowerBase = value;
            }
        }
        [SettingPropertyInteger("Chance Loss Clan Tier Modifier", -10, 10, "0", Order = 0, RequireRestart = false, HintText = "Adds to your Clan Tier level")]
        [SettingPropertyGroup("Clan Tier", GroupOrder = 3)]
        public int ClanTierChanceLossDecreasePowerAdd
        {
            get => _clanTierChanceLossDecreasePowerAdd;
            set
            {
                _clanTierChanceLossDecreasePowerAdd = value;
            }
        }
        #endregion

        // TODO display average excepted recruitment for each tier for the settings chosen
        // Will have to use boolean group and update when changes
    }
}
