# Caravan Party Limit Increased Fixed

A Mount & Blade II: Bannerlord mod that sets caravan party sizes (fixed or based on clan tier) in [Mod Configuration Menu (MCM)][mcm].

**Module id:** `CaravanPartyLimitIncreasedFixed`

## About this mod

Inspired by **[Clan Tier based Caravan Party Size](https://www.nexusmods.com/mountandblade2bannerlord/mods/661)** (**CaravanPartyLimitIncreased**) by **[Grant2600](https://www.nexusmods.com/profile/Grant2600?gameId=3174)**. Credit for the idea only—this is a standalone mod for Bannerlord 1.4.8 with its own module id, namespaces, and MCM, not a continuation or rename of that original.

### What this mod does differently

- Own module id and settings folder (safe alongside other caravan mods after a clean switch).
- Soft optional link to Clan Tier Expanded for a shared hard cap.
- Same-kingdom filter follows map faction, so notable merchants of your kingdom count correctly.
- Public size API for optional castle caravan scaling in Castle Market Towns.

## Features

| Feature | Default |
|---|---|
| Who it affects | Player, same-kingdom, and other caravans (each toggleable) |
| Clan tier based | On — size is `30 + (extra slots × clan tier)` |
| Fixed limit | 50 when clan tier based is off |
| Hard cap from Clan Tier Expanded | Used when that mod is enabled |
| Castle Market Towns | Optional percent scaling for castle-origin caravans |

## Mod options ([MCM][mcm])

**Options > Mod Options > Caravan Party Limit Increased Fixed**

Choose who is affected, then fixed size or clan-tier formula. Same-kingdom includes notable caravans tied to your kingdom.

## Optional support

- **[Clan Tier Expanded][cte]** — optional hard cap from that mod's caravan support setting.
- **[Castle Market Towns][cmt]** — can shrink castle-origin caravans relative to this mod's limit when both are enabled.

## Requirements

| Dependency | Tested |
|---|---|
| Mount & Blade II: Bannerlord | 1.4.8 |
| War Sails (optional) | 1.2.x |
| [Harmony][harmony] | 2.4.2 |
| [Mod Configuration Menu v5][mcm] | 5.13.1 |
| Clan Tier Expanded (optional) | 1.0.x |
| Improved Garrisons (optional) | load after IG if you use it |

MCM also needs [ButterLib][butterlib] and [UIExtenderEx][uiextenderex].

## Installation

1. Install Harmony and MCM (with ButterLib and UIExtenderEx).
2. Copy `CaravanPartyLimitIncreasedFixed` into `Modules`.
3. Enable it after Harmony, MCM, and Clan Tier Expanded (if used).

## Building from source

```bat
copy CaravanPartyLimitIncreasedFixed.user.props.example CaravanPartyLimitIncreasedFixed.user.props
build.cmd /p:DeployToGame=true
```

## License

GNU GPL v3. See [LICENSE](LICENSE) and [NOTICE](NOTICE).

[harmony]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2006
[butterlib]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2018
[uiextenderex]: https://www.nexusmods.com/mountandblade2bannerlord/mods/2102
[mcm]: https://www.nexusmods.com/mountandblade2bannerlord/mods/612
[cmt]: https://www.nexusmods.com/mountandblade2bannerlord/mods/14182
[cte]: https://github.com/AtanuDebbarma/ClanTierExpanded
