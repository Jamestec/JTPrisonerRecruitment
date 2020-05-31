# JTPrisonerRecruitment
 Bannerlord Mod, Increases Prisoner Recruitment

Bonjour,

This mod makes prisoner recruitment be affected by clan tier, charm and leadership. Recruitment chances apply to every unit in a stack, not just once per stack.

There are settings, but it's pretty unpolished and a lot of the names/explanation is not exactly helpful. I'm uploading it in this state since Uni is starting and I've very unlikely to update the mod now.

Release v1.0.0 is for beta 1.4.1, I have ADSL at the moment, so testing compatibility with 1.4.0 would be painful, so I have not.

Look at the pictures for the algorithm, but basically with default settings (all values are adjustable - note that I'm using MCM, which means you can enter values directly when you press on the number):

Chances to recruit each daily tick are changed to, for Unit Tier 0->Tier 7+: 1 (meaning 100%), 0.5, 0.4, 0.3, 0.2, 0.1, 0.05, 0.025 Meaning Tier 4 has 20% chance each day (vanilla is 10%). For Tiers 7+ (inclusive, which doesn't exist in vanilla), they are all pooled in the same chance with a base chance of 2.5%.

Charm affects chance by Charm / 300 * base chance for Tier.

Clan Tier affects chance by (Clan Tier + 1) * chance modified by charm.

Leadership affects chance loss by changing the flat 1.0 (100%) reduction from each successful recruitment to 1 - Math.min(Leadership / 300, 275); if leadership = 100, then each success reduces chance by 0.66 (66.66%) instead of 100%, if leadership = 280, it would be the same as leadership = 275.

Clan Tier also affects chance loss by dividing the above leadership chance loss by (2^Clan Tier * 0.5) - 0.5; if leadership = 100 and Clan Tier = 4, the final chance loss is 0.0888 (8.88%)

TLDR: Charm and Leadership affect chance linearly, charm increases chance, leadership decreases chance loss per successful recruitment. Clan Tier affects chance linearly and chance loss exponentially. Tier 0 will have vanilla behavior except chance applies to each prisoner that hasn't been recruited yet, not just once per stack.

Bug: if you recruit prisoners with CTRL button pressed, the amount of prisoners recruitable from that stack only decreases by 1, this is a vanilla bug that I can't be bothered fixing.

If you get two Mod Options buttons and Exit to Main Menu is hard to press, I recommend going to the Mod Options with MCM UI Impl. 3.1.4 and ticking Use Standard Option Screen which will place those mod optins in ESC>Options>Mod Options. This is because some mods are using MCM 2.0 and I'm using 3.0. MCM's documentation bad in my opinion, so I have no idea what I'm suppose to do to fix that.
