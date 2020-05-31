using MCM.Abstractions.Settings.Base.Global;
using MountAndBlade.CampaignBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace JTPrisonerRecruitment
{
	public class JTRecruitPrisonersCampaignBehavior : CampaignBehaviorBase, IRecruitPrisonersCampaignBehavior, ICampaignBehavior
	{
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
		}

		private void DailyTick()
		{
			Settings settings = GlobalSettings<Settings>.Instance;
			TroopRoster prisonRoster = MobileParty.MainParty.PrisonRoster;
			// I'm not sure how the following line redirects to my function. Todd Howard: "It just works"
			float[] dailyRecruitedPrisoners = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetDailyRecruitedPrisoners(MobileParty.MainParty);
			float excess = dailyRecruitedPrisoners[dailyRecruitedPrisoners.Length - 1]; // This is pooled chance to recruit higher tiers than we have settings for
			Dictionary<int, int> nPrisoners = GetNaughtyPrisoners(prisonRoster); // Get prisoner index -> not yet recruitable number
			int totalN = GetTotalNaughty(nPrisoners); // Determine how many loops we need to process the above dictionary
			for (int n = 0; n < totalN; n++)
            {
				int i = MBRandom.RandomInt(nPrisoners.Count); // Get random for better distribution
				int index = nPrisoners.ElementAt(i).Key;
				nPrisoners[index] -= 1;
				if (nPrisoners[index] < 1)
				{
					nPrisoners.Remove(index);
				}
				CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
				int tier = characterAtIndex.Tier;
				if (dailyRecruitedPrisoners[tier] > 0f) // TODO We should probably organise nPrisoners by Tier, index, then unrecruited so we can remove Tier chance < 0
                {
					if (tier < dailyRecruitedPrisoners.Length)
                    {
						if (MBRandom.RandomFloat < dailyRecruitedPrisoners[tier])
                        {
							// 100 Leadership decreases chance decrease per recruitment by 33%, capped at 275
							float chanceDecreaseLeadership = settings.LeadershipBase 
								- Math.Min(settings.LeadershipCap, Clan.PlayerClan.Leader.GetSkillValue(DefaultSkills.Charm)) / (float)settings.LeadershipDiv;

							SetRecruitableNumberInternal(characterAtIndex, GetRecruitableNumberInternal(characterAtIndex) + 1);

							// Clan Tier has exponential effect, increase power base to increase steepness, change float multipler to change ceiling
							float clanPowerThing = (float)Math.Pow(settings.ClanTierChanceLossDecreasePowerBase, Clan.PlayerClan.Tier + settings.ClanTierChanceLossDecreasePowerAdd);
							float clanImpact = settings.ClanTierChanceLossDecreaseBase + settings.ClanTierChanceLossDecreaseFloat * clanPowerThing - settings.ClanTierChanceLossDecreaseFloat;
							dailyRecruitedPrisoners[tier] -= chanceDecreaseLeadership / (clanImpact);
						}

                    }
					else if(MBRandom.RandomFloat < excess)
					{
						float chanceDecreaseLeadership = settings.LeadershipBase
							- Math.Min(settings.LeadershipCap, Clan.PlayerClan.Leader.GetSkillValue(DefaultSkills.Charm)) / (float)settings.LeadershipDiv;
						SetRecruitableNumberInternal(characterAtIndex, GetRecruitableNumberInternal(characterAtIndex) + 1);
						float clanPowerThing = (float)Math.Pow(settings.ClanTierChanceLossDecreasePowerBase, Clan.PlayerClan.Tier + settings.ClanTierChanceLossDecreasePowerAdd);
						float clanImpact = settings.ClanTierChanceLossDecreaseBase + settings.ClanTierChanceLossDecreaseFloat * clanPowerThing - settings.ClanTierChanceLossDecreaseFloat;
						excess -= chanceDecreaseLeadership / clanImpact;
					}
                }
			}
			RemoveUnused(prisonRoster);
		}

		private Dictionary<int, int> GetNaughtyPrisoners(TroopRoster prisonRoster)
        {
			Dictionary<int, int> nPrisoners = new Dictionary<int, int>();
			for (int i = 0; i < prisonRoster.Count; i++)
			{
				CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(i);
				if (IsPrisonerRecruitable(characterAtIndex))
				{
					int naughty = prisonRoster.GetElementNumber(i) - GetRecruitableNumberInternal(characterAtIndex);
					if (naughty > 0)
					{
						nPrisoners[i] = naughty;
					}
				}
			}
			return nPrisoners;
        }

		private int GetTotalNaughty(Dictionary<int, int> nPrisoners)
        {
			int total = 0;
			foreach (int key in nPrisoners.Keys)
            {
				total += nPrisoners[key];
            }
			return total;
        }

		private bool IsPrisonerRecruitable(CharacterObject character)
		{
			if (!character.IsHero)
			{
				// 1.4.1, this is always true
				// return Campaign.Current.Models.PrisonerRecruitmentCalculationModel.IsPrisonerRecruitable(character);
				return true;
			}
			return false;
		}

		Dictionary<CharacterObject, int> _recruitables = new Dictionary<CharacterObject, int>();

		private int GetRecruitableNumberInternal(CharacterObject character)
		{
			if (!_recruitables.TryGetValue(character, out int value))
			{
				return 0;
			}
			return value;
		}

		private int SetRecruitableNumberInternal(CharacterObject character, int numberRetrained)
		{
			return _recruitables[character] = numberRetrained;
		}

		private void RemoveUnused(TroopRoster roster)
		{
			if (_recruitables.Count == 0)
			{
				return;
			}
			Dictionary<CharacterObject, int> dictionary = new Dictionary<CharacterObject, int>();
			for (int i = 0; i < roster.Count; i++)
			{
				CharacterObject characterAtIndex = roster.GetCharacterAtIndex(i);
				_recruitables.TryGetValue(characterAtIndex, out int value);
				int num = Math.Min(value, roster.GetElementNumber(i));
				if (num > 0)
				{
					dictionary[characterAtIndex] = num;
				}
			}
			_recruitables = dictionary;
		}

		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData("_recruitables", ref _recruitables);
		}

		public int GetRecruitableNumber(CharacterObject character)
		{
			if (!character.IsHero)
			{
				int recruitableNumberInternal = GetRecruitableNumberInternal(character);
				int troopCount = MobileParty.MainParty.PrisonRoster.GetTroopCount(character);
				return Math.Min(recruitableNumberInternal, troopCount);
			}
			return 0;
		}

		public void SetRecruitableNumber(CharacterObject character, int number)
		{
			SetRecruitableNumberInternal(character, number);
		}
	}
}
