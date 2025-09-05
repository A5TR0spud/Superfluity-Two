# Changelog

## TODO

### Additions

- Add Azimuth: magic zenith:
  - Fires last prism-esque laser
  - Fires homing swirls
- Tracer Round: bullet ammo. little damage. creates a reticle stuck to the npc. striking it deals 30 damage + turns the hit into a crit.

### Changes

- Meridian: laser is no longer constant, but is much stronger
- Meridian: vortices no longer aim at or home in on the cursor
  - instead move towards enemies while firing in a constant direction
- Meridian: buff single target, nerf group attack (but keep it clean-up worthy)
- Meridian: improved "arrow" homing the faster it is going
- Meridian: "arrows" will no longer home for the first 5 ticks
- Magpie: NPCs are locked in order of proximity to cursor, rather than arbitrarily (by time)
- Super Sniper: have the reticle grow to size at 50% opacity while the rifle is charging up (more fair for pvp, better feedback for user)

### Fixes

- Meridian: homing "arrows" no longer target enemies they won't be able to aim at in time

### Internal Fixes/Changes

- Move everything to folders/namespaces that make more sense
- Magpie: switch to having either holdout + reticle or JUST holdout
  - static int[] trackedNPCs;
  - static int[Main.maxNPCs] trackedNPCFireTimers;

## 0.32.2

### 0.32.2 Fixes

- Pal 9k no longer lingers after death

## 0.32.1

### 0.32.1 Changes

- Pal 9k is now visible as a vanity accessory
- Desperado shrapnel no longer ignores 18 defense

### 0.32.1 Fixes

- Pal 9k should no longer jitter when stepping up or down tiles
- Desperado explosion can no longer hit through walls or spawn behind the muzzle
- Desperado shrapnel now have tile collision FX
- Desperado FX should now work in multiplayer if they didn't already

## 0.32.0

### 0.32.0 Additions

- Added Desperado, a gun

### 0.32.0 Fixes

- Added proper shimmer decrafting to Red Alert

## 0.31.0

### 0.31.0 Additions

- Added Safety Lamp, a summon

### 0.31.0 Changes

- Significantly reduced knockback of Decommissioned Fumigator
- Changed sound of summoning an Asterism minion
- Increased knockback and damage of Growth Spurt

## 0.30.1

### 0.30.1 Changes

- Midnight Sun:
  - Damage buffed: 24 -> 28
  - Max Anger increased: 1.5 -> 2.5
    - Anger affects max speed and how long wisps track a target before searching for a new one
  - Insta-kill threshold decreased 250 -> 200
  - Retargetting penalty increased 30 -> 48
  - Killer Wisps no longer collide with tiles, fixing an issue where they could become stuck
  - Midnight Sun's sprite has changed
  - The Light That Hates buff's sprite has changed

## 0.30.0

### 0.30.0 Additions

- Added Asterism, a summon
- Added Decommissioned Fumigator, a summon
- Added Midnight Sun, a summon

## 0.29.0

### 0.29.0 Additions

- Added Fairy Knife
- Added PAL 9k

## 0.28.3

### 0.28.3 Changes

- Magpie locks now interact with chlorophyte bullets to prevent wonky behaviour

## 0.28.2

### 0.28.2 Changes

- Magpie: reduced damage 60 -> 45
- Relaxed Magpie targetting restrictions; can now target imp fireballs, goblin sorcery, etc
- Magpie now has a much more complicated line-of-sight check, so should fire more intelligently at enemies behind partial cover

### 0.28.2 Fixes

- Magpie: losing locks no longer puts pressure on magpie to fire early
- Fixed Magpie and Super Sniper counting as specialist weapons

## 0.28.1

### 0.28.1 Changes

- Magpie's aim now corrects for movement
- Magpie now has a spread of 1 degree
- Magpie attacks slower (relatively) the more targets there are
- Magpie now has a cooldown for obtaining new locks (10 locks per second)
- Magpie's damage has been increased
- Magpie now points in the direction it's firing

### 0.28.1 Fixes

- Actually fixed Floe Crystal dusts, +Shadow Rite and Vile Stone
- Fixed a rendering bug related to Magpie's reticle and locks

## 0.28.0

### 0.28.0 Additions

- Added Magpie, a post-twins ranged gun

### 0.28.0 Changes

- Updated reticle visual for Super Sniper

### 0.28.0 Fixes

- Fixed Floe Crystal dust applying to projectiles that don't do damage

## 0.27.0

### 0.27.0 Additions

- Added Hurricane Lantern, a hardmode magic weapon
- Added Disposable Rocket Launcher, a hardmode ranged weapon
- Added Super Sniper, a post-Plantera ranged weapon
- Added Foggy Mirror

### 0.27.0 Fixes

- Fixed Meridian shimmer decrafting in corruption vs crimson
- Meridian's ammo counter now correctly discounts ammo it can't actually shoot (e.g. fallen stars, coins)
- Bastet's VFX is now properly implemented as a projectile, rather than rendering as part of the player
- Joining players should now see Meridian and its laser if its being constantly fired at time of joining

## 0.26.0

### 0.26.0 Additions

- Added Meridian, the ranger's Zenith

## 0.25.1

### 0.25.1 Fixes

- *Actually* fixed thaumaturgy syncing with players in multiplayer

## 0.25

### 0.25 Additions

- Added **Phylactery,** dropped by Necromancers
  - Increases max life by 50
  - Increases life regen the closer you are to death
  - Reduces damage taken the closer you are to death
- Added shimmer-craft for Zygoma to and from Spectre Staff
- Added shimmer-craft for Cultivating Flame to and from Inferno Trident
- Added shimmer-craft for Phylactery to and from Shadowbeam Staff
- Added a unique sprite for the circle from Ancient Magi's Lost Lore

### 0.25 Changes

- Removed shimmer-craft for Zygoma to and from Cultivating Flame
- Increased duration of Thuamaturgy from 7 to 8 seconds
- Increased cooldown of Thaumaturgy from 7 to 9 seconds
- Nerfed Thaumaturgy damage from 32 to 20
  - Magi's Lost Lore does 30 damage
  - Combined, they deal 40 damage
- Ancient Magi's Lost Lore:
  - No longer increases magic damage by 6%
  - While circle is active, magic attacks inflict Starsplit:
    - +10% damage taken
    - 20 dps
  - While circle is active, increases magic attack speed by 12%

## 0.24.2

### 0.24.2 Changes

- Updated description on workshop and mod browser
- Updated sprites for:
  - Scroll of Thunder
  - Book of Thaumaturgy
  - Ancient Magi's Lost Lore
  - Cultivating Flame
- Reduced Thaumaturgy defense from 4 to 3
  - Reduced Ancient Magi's Lost Lore defense from 6 to 4
- Specified behaviour of Thaumaturgy VFX if it's both equipped and in vanity (Thaumaturgy + Lost Lore)

### 0.24.2 Fixes

- Fixed Cultivating Flame having a way too rare drop chance
- Fixed Cultivating Flame and Zygoma not increasing drop rates in expert+
- Fixed bug causing Book of Thaumaturgy to not create VFX when in vanity slot

## 0.24.1

### 0.24.1 Changes

- Adjusted clover growing chances to put more emphasis on living trees
- Book of Thaumaturgy now provides 4 defense while active
  - Ancient Magi's Lost Lore provides 6 defense while active
  - These effects stack
- Adjusted Red Alert overhead sprite

### 0.24.1 Fixes

- Fixed tile 4-Leaf Clovers being able to drop 2 items 1 in 7 times
- Fixed Red Alert overhead visual being too emissive and not transparent
- Book of Thaumaturgy *should* better sync with other players in multiplayer

## -0.24

- Untracked in any meaningful way
