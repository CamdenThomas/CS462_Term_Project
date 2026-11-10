# Audio and Dialogue Setup

How the voices, speech bubbles and sound effects work, and what to plug in. Recorded sounds go in Drive (`DriveAssets/SFX`, `DriveAssets/Music`) as described in [ASSETS.md](ASSETS.md); everything on this page is in Git.

## Already wired

- **Mom talks.** The `Parent` prefab has a `Speaker` using `Barks_Mom` and `Voice_Mom`. As her suspicion rises she says the player's first name, then first and middle, then the full name, and when the player is caught she says a line in a speech bubble over her head. The bubble hides when she is off screen or behind a wall; her voice does not.
- **Mom has footsteps.** `Footsteps` on the `Parent` prefab plays `SFX_Footstep_Parent` every stride.
- **Doors, hiding spots and pickups** have optional sound slots (`Open Sound`, `Enter Sound`, `Pickup Sound`).

Everything is silent until clips are added: the bubbles already work without any audio.

## The gibberish voice (Sims / Animal Crossing style)

1. Record 20 to 30 nonsense syllables, about 0.1 to 0.2 s each: "ba", "meh", "doo", "hm", "sha", "ugh". One person can voice everyone.
2. Trim the silence off each one, name them `SFX_Voice_01.wav` and so on, and put them in `DriveAssets/SFX/Voice/`.
3. Drag them all into the **Syllables** list of `Data/Dialogue/Voice_Mom`, `Voice_Dad` and `Voice_Brother`.
4. Tune each voice: Mom is pitch 1.15 and fast, Dad is 0.8 and slower, the brother is 1.0 and chats less (`Letters Per Syllable` 4).

No recording? Free CC0 gibberish clips on freesound.org are safe to ship. Avoid ripping audio from YouTube.

## Lines (BarkSets)

`Data/Dialogue/Barks_*.asset` hold every line, grouped by trigger (`Suspicious`, `CaughtSeen`, `DistractionFound`, and so on). Add or edit lines in the Inspector. `{first}`, `{middle}` and `{last}` become the player's name (Sam James Carter for now, set in `Speaker`).

To make someone talk from code (a distraction, a chore, the brother menu):

```csharp
mom.GetComponent<Speaker>().Say( BarkTrigger.DistractionFound );
```

Ordinary barks wait out a cooldown and never cut someone off; pass `interrupt: true` to talk over them.

For the brother, swap between `Barks_Brother_Ally`, `_Neutral` and `_Enemy` as Brother Bond changes.

## Sound effects (SfxCues)

`Audio/Cues/SFX_*.asset` is one cue per sound. Put 3 or 4 recorded takes in **Clips**; each play picks one with a little pitch and volume variation. Play one from anywhere:

```csharp
[SerializeField] private SfxCue shatter;
shatter.PlayAt( transform.position );
```

**Noise Radius** is how far parents can hear it. Footsteps and creaky boards raise it on a `NoiseChannel` for when parent hearing is built.

## Still to hook up in the House scene

- [ ] **Player footsteps:** add `Footsteps` to the Player with `SFX_Footstep_Kid`, a stride of 0.5, and its `PlayerController` linked (crouching gets quiet, sprinting loud).
- [ ] **Creaky boards:** add a thin trigger BoxCollider and a `CreakyBoard` (with `SFX_FloorCreak`) outside the parents' door and on stairs 3 and 14.
- [ ] **The cookie cabinet:** set its `Door` to `SFX_Cabinet_Open` and `SFX_Cabinet_Close`.
- [ ] **A NoiseMade channel:** create one (Create > TroUble > Channels > Noise) and assign it to `Footsteps` and `CreakyBoard`, ready for parent hearing.
- [ ] **Fonts:** set the Speaker's `Font` to a Bangers TMP font asset to match the menus.
- [ ] **Mixer:** all cues and voices output to the `SFX` group, so the SFX volume slider already controls them. Add a `Voice` group later if you want a separate slider.
