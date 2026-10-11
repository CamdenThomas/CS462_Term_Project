# UI Handoff

Menu designs for TroUble (Trouble Magnet), made in Figma. This note lives in Git. The images live in Google Drive, as described in `ASSETS.md`.

* **Figma file:** https://www.figma.com/design/gEH4dHmiVDznxy3CykfHXt/Trouble-Magnet?node-id=0-1\&t=Jv6CZYGK7GrQ7nhF-1
* **Images:** Google Drive `TroUble/DriveAssets/UI/` and `TroUble/DriveAssets/Reference/`
* **Figma backup (.fig):** Google Drive `TroUble/Source/`

\---

## 1\. Basics

|Item|Value|
|-|-|
|Screen size|1920 x 1080 (16:9)|
|Font|**Bangers** (free, Google Fonts)|
|Style|Night-time house scene, left-aligned text menu, cream text with black outlines|

### Colors

|Name|Hex|Used for|
|-|-|-|
|Yellow|`#F9BE3A`|Hover and selected states, highlights|
|Red|`#DA251D`|Magnet icon, delete and danger actions|
|Cream|`#FFF4D6`|Main text, outlines|
|Black|`#111111`|Text outlines, shadows|
|Panel navy|`#0B0D24`|Card and panel fills (about 72-88% opacity)|
|Slot blue|`#2B2D5B`|Thumbnails, slider tracks, dropdowns|

\---

## 2\. Files in Drive

### `DriveAssets/UI/` (images for the game)

|File|What it is|
|-|-|
|`UI\_Logo.png`|Trouble Magnet logo, transparent background|
|`UI\_Icon\_Magnet.png`|Red magnet icon (menu hover, selected slot)|
|`UI\_Button\_Normal.png`|Menu button, normal state|
|`UI\_Button\_Hover.png`|Menu button, hover state|
|`UI\_Slot\_Normal.png`|Save slot card, normal|
|`UI\_Slot\_Hover.png`|Save slot card, hover|
|`UI\_Slot\_Selected.png`|Save slot card, selected|
|`UI\_Slot\_Empty.png`|Empty slot (dashed)|
|`UI\_Bg\_MainMenu.png`|Main menu background scene|
|`UI\_Bg\_SaveSlots.png`|Blurred, darkened scene for the slot and options screens|

### `DriveAssets/Reference/` (full-screen pictures to match)

|File|Screen|
|-|-|
|`UI\_Ref\_MainMenu.png`|Main menu|
|`UI\_Ref\_SaveSlot.png`|Save slot screen|
|`UI\_Ref\_Options\_Audio.png`|Options, Audio tab|
|`UI\_Ref\_Options\_Controls.png`|Options, Controls tab|
|`UI\_Ref\_Options\_Display.png`|Options, Display tab|

> The `UI/` images were uploaded without `.meta` files. Whoever imports them into Unity should upload the images \*\*and\*\* their `.meta` files back to Drive, as `ASSETS.md` describes.

\---

## 3\. Screens and flow

```
Main menu
  |- New Game  -> Save Slot (New Game)  -> \[Overwrite popup if the slot is filled] -> Childhood, week 1
  |- Load Game -> Save Slot (Load Game) -> resume the saved stage and week
  |- Options   -> Options (Audio / Controls / Display tabs)
  |- Gallery   -> (not designed yet)
  |- Quit      -> Quit confirmation popup
BACK on every sub-screen returns to the Main menu.
```

\---

## 4\. Components

### Menu Button

Used for NEW GAME, LOAD GAME, OPTIONS, GALLERY, QUIT and BACK.

|State|Look|
|-|-|
|Normal|Cream text, black outline|
|Hover|Yellow text, red magnet icon on the left|

Poppins Bold Italic, about 66 px on the main menu. Fade between states in about 150 ms.

### Slot Card

Size 960 x 190, corner radius 20.

|State|Look|
|-|-|
|Normal|Dark navy fill, thin cream outline, dim X delete button|
|Hover|Brighter cream outline|
|Selected|Yellow outline (7 px), red X, "LOAD" label in yellow, red magnet icon at the left|

Each card shows: slot number, stage name, week and a key stat, playtime, save date, and a screenshot thumbnail (224 x 136).

### Empty Slot

Dashed outline, text "EMPTY SLOT". On the Load Game screen it is dimmed and shows "NO SAVE".

### Built from the reference screens (no separate image)

Sliders, switches, dropdowns, key caps, tabs, the delete button and the popup buttons. Read their sizes and colors from the reference pictures or from Figma.

\---

## 5\. Behavior rules

* Menu items turn yellow and show the magnet icon on hover.
* Only **one** slot card can be selected at a time.
* **New Game:** empty slots can be selected to start a game. Filled slots ask "Overwrite save?" first.
* **Load Game:** filled slots can be loaded. Empty slots are disabled.
* Deleting a save asks for confirmation.
* Options tabs (Audio, Controls, Display) switch the panel content. The selected tab is yellow with an underline.
* BACK always returns to the Main menu.

\---

## 6\. Not designed yet

* Pause menu
* In-game HUD and the brother menu
* Report card screen
* Ending screens

## 7\. To confirm with the team

* **Key bindings** in the Controls tab (WASD, Shift, Ctrl, E, Esc) are placeholders.
* **Overwrite and Quit popups** are drafts, and their text can change.
* **Gamepad support** is a stretch goal, so no gamepad layout is shown.

