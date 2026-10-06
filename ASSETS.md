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

1. Open the shared `TroUble` folder in Google Drive and choose **Organize → Add shortcut → My Drive**, so it shows up at `My Drive/TroUble`.
2. Install [rclone](https://rclone.org/install/) (Windows, Mac, and Linux).
3. Run `rclone config`: create a new remote named **`gdrive`**, choose type **Google Drive**, and accept the defaults. A browser window opens to log in.
4. Do your first pull (below) **before** opening the project in Unity.

No rclone? You can download and upload `DriveAssets` in the Drive website instead. Just keep the folder structure identical and always include the `.meta` files.

## Every time you sit down to work

```bash
git pull
rclone copy "gdrive:TroUble/DriveAssets" Assets/Project/DriveAssets --update -P
```

Then open Unity. **Always pull Drive before opening Unity.** If Unity opens while a file is missing, things show up pink or empty until you pull and reopen.

## Adding or changing a big file

1. Put it in the right `Assets/Project/DriveAssets/` subfolder **with Unity open**, so Unity creates its `.meta`.
2. Set its import settings in Unity (scale, compression, and so on). Those settings live in the `.meta` file.
3. Upload it **and its `.meta`**:
   ```bash
   rclone copy Assets/Project/DriveAssets "gdrive:TroUble/DriveAssets" --update -P
   ```
4. **Then** commit and push the prefab, material, or scene that uses it.
5. Tell everyone in chat: "New in Drive: `Models/Couch.fbx`. Pull Drive."

Uploading to Drive **before** pushing to Git means nobody pulls a prefab whose model doesn't exist yet.

## Rules

- **Always upload the file and its `.meta` together.** A file without its `.meta` breaks everyone else's references.
- **Don't edit someone else's file in place.** Ask first, or save a new version (`Couch_v2.fbx`). Drive makes confusing "conflict" copies otherwise.
- **Rename or move files only inside Unity,** then upload and delete the old copy in Drive. `rclone copy` never deletes, so old files come back on the next pull unless you delete them in Drive.
- **To delete a file,** delete it (and its `.meta`) in Unity **and** in Drive.
- **Source files stay out of Unity:** `.blend` and `.psd` go to `TroUble/Source/`, and you export `.fbx` and `.png` into `DriveAssets`.
- **If Git says a `.png` or `.wav` is ignored** somewhere outside `DriveAssets`, that's the safety net. Move it into `DriveAssets/`.

## Naming

`SM_` models · `T_` textures · `A_` animations · `UI_` UI images · `MUS_` music · `SFX_` sounds. Use PascalCase with no spaces, for example `SM_Couch.fbx`, `T_Couch_Albedo.png`, `SFX_DoorOpen_01.wav`.
