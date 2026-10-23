# Music

Music files are in `Assets/Project/DriveAssets/Music` (from the team Drive, see ASSETS.md). `MusicPlayer` (in `Scripts/Audio`) spawns itself on Play from `Audio/Resources/MusicPlayer.prefab`, so there's nothing to place in a scene.

- **Menu:** sneaky, playful theme
- **Gameplay:** one theme per stage: toy piano (Childhood), lo-fi guitar (Early Teen), night synth (Late Teen)
- **Report card:** calm piano. Call `MusicPlayer.PlayReportCard()`

Each stage has three loops that play in sync: Calm always plays, Tension fades in when a parent is near or suspicious, and Chase kicks in on the "!". The music is muffled while hiding. Stings play for "?", "!", caught and the demo win. `TaskComplete`, `StageComplete` and the two ending stings are in the folder, ready to hook up.

To replace a track, swap the clip on the prefab. A stage's Tension and Chase loops must be exactly half the length of its Calm loop.
