# Changelog

## 1.0.1

- Same-kingdom caravan filter uses map faction / kingdom (not only `Hero.Clan`), so notable merchants of kingdom settlements are classified correctly.
- MCM visibility filter no longer swallows unrelated MCM discovery exceptions.

## 1.0.0

First release of **Caravan Party Limit Increased Fixed** (module id `CaravanPartyLimitIncreasedFixed`).

Standalone rebuild of the caravan party-size mod for Bannerlord 1.4.8, with its own module id, namespaces, and MCM folder.

- Fixed or clan-tier-based caravan party sizes, with who-it-affects toggles.
- Soft hard-cap from Clan Tier Expanded when that mod is enabled.
- Public size API for Castle Market Towns castle-origin caravan percent scaling.
