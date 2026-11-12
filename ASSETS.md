# Large Files (Models, Textures, Audio)

We don't use Git LFS. **Code, scenes, prefabs, materials, and data assets go in Git. Big files (models, images, sounds) go in the shared Google Drive folder.**

## Where things live

| What | Where | Synced by |
| --- | --- | --- |
| Scripts, scenes, prefabs, materials, shaders, `.asset` data | the repo | Git |
| `.fbx`, `.png`, `.jpg`, `.wav`, `.ogg`, `.mp3` and their `.meta` files | `Assets/Project/DriveAssets/` | Google Drive `TroUble/DriveAssets/` |
| `.blend`, `.psd`, music projects, raw recordings | **not in Unity** | Google Drive `TroUble/Source/` |
| Playtest builds | not in Unity | Google Drive `TroUble/Builds/` |

```
Google Drive: TroUble/                    Your computer: Assets/Project/
├── DriveAssets/   ◄── exact mirror ──►    └── DriveAssets/   (Git ignores this folder)
│   ├── Models/                                ├── Models/
│   ├── Textures/                              ├── Textures/
│   ├── Animations/                            ├── Animations/
│   ├── UI/                                    ├── UI/
│   ├── Reference/                             ├── Reference/
│   ├── Music/                                 ├── Music/
│   └── SFX/                                   └── SFX/
├── Source/        (.blend, .psd, DAW projects; never put in Unity)
└── Builds/        (zipped game builds)
```

## Why the `.meta` files go to Drive too

Unity gives every file a permanent ID, stored in its `.meta` file. Prefabs and materials in Git point to models and textures by that ID. If everyone imported `Couch.fbx` themselves, each computer would invent a different ID and every reference would break. So **the file and its `.meta` always travel together, through Drive.**

## One-time setup

1. Make sure you can open the shared `TroUble` folder in Google Drive (ask Camden to share it with you).
2. In the repo folder, run:
   ```bash
   ./pull --setup
   ```
   It installs [rclone](https://rclone.org/install/) if you don't have it (Homebrew on Mac, winget on Windows; on Windows run it from Git Bash), opens a browser tab to sign in to Google (read-only access), installs a git hook, and does your first Drive pull.
3. Do this **before** opening the project in Unity.

No rclone? You can download and upload `DriveAssets` in the Drive website instead. Just keep the folder structure identical and always include the `.meta` files.

## Every time you sit down to work

Close Unity, then:

```bash
./pull
```

That runs `git pull` and then copies anything new from Drive `TroUble/DriveAssets` into `Assets/Project/DriveAssets`. After `--setup`, a plain `git pull` also pulls Drive (through the git hook) whenever it brings in new commits; `./pull --drive` pulls only the Drive files.

Then open Unity. **Always pull Drive with Unity closed.** If Unity is open it can import a picture before its `.meta` arrives, give it a new ID, and every prefab using it loses the link. `./pull` refuses to pull Drive while Unity has the project open.

## Adding or changing a big file

1. Put it in the right `Assets/Project/DriveAssets/` subfolder **with Unity open**, so Unity creates its `.meta`.
2. Set its import settings in Unity (scale, compression, and so on). Those settings live in the `.meta` file.
3. Commit the prefab, material, or scene that uses it.
4. Run `./push`. It uploads every new or changed file in `DriveAssets` **with its `.meta`** to Drive, then runs `git push`. It never deletes anything on Drive, and it skips any file where the Drive copy is newer than yours. `./push --check` lists what would be uploaded without uploading.
5. Tell everyone in chat: "New in Drive: `Models/Couch.fbx`. Run ./pull."

`./push` uploads to Drive **before** pushing to Git, so nobody pulls a prefab whose model doesn't exist yet. It refuses to upload if a file has no `.meta` (open Unity once so it makes one).

**First time:** `./push --setup` opens a browser tab to sign in to Google again, this time with upload access (you need edit access to the `TroUble` folder). It makes a separate connection called `gdrive-rw`; `./pull` keeps its read-only one. No rclone? Drag the files **and their `.meta` files** into the matching Drive folder in the website instead.

## Rules

- **Always upload the file and its `.meta` together.** A file without its `.meta` breaks everyone else's references.
- **Don't edit someone else's file in place.** Ask first, or save a new version (`Couch_v2.fbx`). Drive makes confusing "conflict" copies otherwise.
- **Rename or move files only inside Unity,** then upload and delete the old copy in Drive. `./pull` and `./push` never delete, so old files come back on the next pull unless you delete them in Drive.
- **To delete a file,** delete it (and its `.meta`) in Unity **and** in Drive.
- **Source files stay out of Unity:** `.blend` and `.psd` go to `TroUble/Source/`, and you export `.fbx` and `.png` into `DriveAssets`.
- **If Git says a `.png` or `.wav` is ignored** somewhere outside `DriveAssets`, that's the safety net. Move it into `DriveAssets/`.

## Naming

`SM_` models · `T_` textures · `A_` animations · `UI_` UI images · `MUS_` music · `SFX_` sounds. Use PascalCase with no spaces, for example `SM_Couch.fbx`, `T_Couch_Albedo.png`, `SFX_DoorOpen_01.wav`.
