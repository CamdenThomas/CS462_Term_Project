# Tro🧲ble

**Design Document** — by Camden Thomas, Davis Agnigbakou, Rylan Clark, Minh Le, Parker Kuykendall

<!-- TODO: insert logo here (rotate the magnet so it reads as a 'U') -->

## Contents

- [Introduction](#introduction)
  - [Game Summary Pitch](#game-summary-pitch)
  - [Inspiration](#inspiration)
  - [Player Experience](#player-experience)
  - [Platform](#platform)
  - [Development Software](#development-software)
  - [Genre](#genre)
  - [Target Audience](#target-audience)
- [Concept](#concept)
  - [Gameplay Overview](#gameplay-overview)
  - [Theme Interpretation](#theme-interpretation)
  - [Primary Mechanics](#primary-mechanics)
  - [Secondary Mechanics](#secondary-mechanics)
- [Level Design](#level-design)
- [Art](#art)
- [Audio](#audio)
- [Game Experience](#game-experience)
- [Team and Roles](#team-and-roles)
- [Development Timeline](#development-timeline)
  - [Progress So Far](#progress-so-far)

## Introduction

### Game Summary Pitch

TroUble: survive growing up under strict parents, where every choice from age 8 to 18 shapes the adult you become, if you make it out at all.

- You grow from age 8 to 18 in a house run by strict parents, across three life stages: Childhood, Early Teen, and Late Teen.
- Each stage is made of week-long play phases where you must finish chores and homework before your parents notice you are slacking.
- You also want to play video games, sneak snacks, watch TV, and get into your brother's room, but your parents are always watching. Being seen is fine (it's your house too); being seen *doing something wrong* is what gets you in trouble.
- You can hide in vents as a kid, but you grow every week, and one day you get stuck. Break things on purpose to pull a parent away, and raid your brother's secret stashes if you dare.
- Your brother starts out neutral and follows his own schedule. Treat him well and he becomes an ally who helps you sneak or recover confiscated items like your phone or game console; hurt him and he becomes an enemy who snitches.
- Daily meters (Hunger, Energy, Boredom, Suspicion) reset each week, while hidden life traits (Grades, Creativity, Communication, Discipline, Family Trust, Brother Bond) carry forward and decide your ending at 18.

### Inspiration

#### Schoolboy Runaway

- Stealth/Escape Game where you are grounded for terrible grades by parents but you're a kid trying to accomplish something without getting caught by your parents. We borrow its premise of a kid sneaking past strict parents inside one house.

#### Hello Neighbor

- Stealth Game where the player explores this house without being caught by the owner. We borrow its tense, line-of-sight stealth against an adult patrolling a home.

#### Kindergarten

- Puzzle adventure where each level is a single school day that the player replays to discover different outcomes and endings. We borrow its short, repeatable levels and the goal of finding every outcome.

### Player Experience

The player should feel like a kid trying to balance responsibilities and freedom. The game should create tension when the player is sneaking around because getting caught by the parents can cause problems. Completing chores and homework should make the player feel like they are making progress toward getting their freedom back but more bored.

The house itself should make the player anxious. It is always near dawn or dusk: a low sun throws long, hard shadows through the windows, only a few lamps are on, and the corners are dark. Familiar things look slightly wrong: a coat rack by the front door that reads as a person, light under the parents' door that dims when feet pass on the other side. The player should never be quite sure where a parent is until they learn the house and its routines.

### Platform

Developed for PC using keyboard and mouse.

### Development Software

- **Unity 6 (URP):** game engine and gameplay programming. ProBuilder for the greybox house, AI Navigation for parent pathfinding.
- **Blender:** 3D modeling and animation.
- **Figma:** menu and UI design (see [UI_HANDOFF.md](UI_HANDOFF.md)).
- **Git + Google Drive:** code, scenes, and data in Git; models, textures, and sounds in a shared Drive folder (see [ASSETS.md](ASSETS.md)).

### Genre

Simulation/Adventure/Stealth

### Target Audience

Teenagers or Young Adults who enjoy exploration or stealth games. TroUble will appeal to people who enjoy games like Hello Neighbor or Schoolboy Runaway.

## Concept

### Gameplay Overview

In TroUble, the player grows from age 8 to 18 across three life stages, each made of week-long play phases inside the family home. Each week the player balances chores, homework, and daily needs while sneaking the things they actually want past watchful, strict parents. Choices quietly build life traits that are revealed on a report card between stages and decide which of many adult endings the player reaches at 18. Getting kicked out or dying early ends the run, and every ending, good or bad, is an achievement to collect.

Parents move around the house and watch the player, forcing them to decide when it is safe to sneak. Being seen is not a crime on its own. The player only gets in trouble when a parent sees them doing something wrong, like reaching into the off-limits cookie cabinet, holding contraband, or ducking into a hiding spot while she is watching. The player's brother follows his own schedule around the house and starts out neutral. Depending on how the player treats him, he becomes an ally who distracts the parents and retrieves confiscated items, or an enemy who snitches.

### Theme Interpretation

Motto: **Sacrifice Is Strength**

Every week asks the player to trade something: fun now for discipline later, honesty for freedom, or their own record for their brother's. The adult the player becomes at 18 is shaped by what they chose to give up.

### Primary Mechanics

| Mechanic | Animated Mockup (Art not necessarily final) |
| --- | --- |
| **Sneaking and Line of Sight** — Parents have vision cones and can hear noise. Being seen is fine; a parent only builds suspicion while she watches the player misbehave: touching something off-limits, carrying contraband, or hiding while she looks on (she remembers where you went). There is no on-screen "she can see you" warning, so reading where a parent is looking is the skill. Crouching is quieter, and the player can hide behind furniture, in closets, or in vents. | Mom turns from the sink just as the player's hand reaches the cookie cabinet. |
| **Tasks** — Chores and homework are short timed interactions. Skipping them raises Suspicion and lowers Grades. | Player finishes the dishes as a progress bar fills. |
| **Daily Meters** — Hunger, Energy, and Boredom drain over the week. Letting one bottom out forces a bad choice, such as raiding the kitchen at night or falling asleep in class. A full Suspicion meter means the player is caught. | HUD meters draining, with Suspicion rising when a parent spots something. |
| **Choices Shape Traits** — Every action quietly feeds a hidden life trait. Lying your way out of trouble builds Communication but costs Family Trust, while finishing homework before gaming builds Discipline. | Report card between stages revealing trait changes. |
| **Getting Caught** — Being caught means a confiscated item, extra chores, and lost Family Trust. Getting caught repeatedly pushes the player toward the kicked-out ending. | Parent confiscates a game console and the Family Trust bar drops. |

How the mechanics interact: the daily meters push the player toward risky sneaking, sneaking risks getting caught, getting caught costs Family Trust, and Family Trust together with the other life traits decides the ending at 18.

### Secondary Mechanics

| Mechanic | Animated Mockup (Art not necessarily final) |
| --- | --- |
| **The Brother** — Your brother starts out neutral and follows his own daily schedule, so the player has to find him and talk to him to ask for a favor, and sometimes he is somewhere the player cannot reach. High Brother Bond makes him an ally who distracts parents, retrieves confiscated items, and once per stage can take the fall for you. Low Brother Bond makes him an enemy who may snitch on you. | Brother pulls a parent into the kitchen while the player slips upstairs. |
| **Contraband and Stashes** — Phones, consoles, and snacks can be hidden in stash spots around the house: a loose floorboard, item-only vents, a hollowed-out book, a shoe in the entry, the cereal box. Parents search the obvious spots first (under your bed, your closet), so odd spots are safer but farther from your room. | Player hides a phone under a loose floorboard, then a parent searches the room. |
| **Vents: You Outgrow Them** — As a kid you can crawl into some vents and vanish. You grow every week, so squeezing in takes longer, and the prompt warns you ("E: Squeeze in (tight)"). Eventually you get stuck halfway, legs sticking out, and a parent finds you; that vent only holds items from then on. Each crawl vent fails at a different stage (under-stairs closet in Childhood, your bedroom in Early Teen, the laundry room last), so the player keeps losing hiding spots as they grow up. | Player squeezes into the laundry vent, stops halfway, and Mom walks in. |
| **Distractions** — Break or trigger things to pull a parent somewhere else. Dad can't ignore tech (unplug the wifi router, hide the TV remote), Mom can't ignore mess (knock a cup off the island, shoes in the dryer, leave the tub running), and both come running for danger (the smoke alarm, flipping the breaker in Late Teen). The parent walks over, fixes it for a while, then returns to their schedule. If a parent sees you near it you get blamed, though you can let your brother take the fall. Using the same trick twice in a day stops working; they come check on the kids instead. | Player unplugs the router and Dad walks upstairs past the landing, clearing the TV room. |
| **The Brother's Secret Stashes** — Your brother hides his own stuff around the house (candy, cash, a magazine, a game), in spots picked at random each week. Some he has forgotten and some he checks on. Take a watched one and he notices, guesses it was you, and Brother Bond drops. Soft clues (dust and old wrappers versus a fresh note) hint at which is which, and putting it back before his next check undoes the damage. | Player finds cash taped under the dining table and hesitates. |
| **The Week Clock** — Each week is compressed into one session, and each day has three phases: After School, Evening, and Night. Parents follow daily schedules (work, dinner, TV time), so learning their routines is part of the skill. The light follows the clock: a low golden sun after school, deep red gloaming in the evening, moonlight at night, and a cold grey dawn for early mornings. | Clock speeds through a day as the shadows stretch across the floor and parents move between rooms on schedule. |
| **Endings and Achievements** — Every ending, good or bad, is recorded in an ending gallery as an achievement. Getting kicked out is an early ending. Dark-comedy deaths are on hold: the house has no outside anymore, so they would either move indoors (an attic hatch, eating mystery food) or be cut. | Ending gallery screen with unlocked and locked ending cards. |

## Level Design

### Structure and Progression

The game is split into three life stages, each made of two week-long play phases, for six levels in total. Every week takes place in the same family home, but the rules, tasks, and parent behavior change as the player grows up.

- **Childhood (ages 8 to 11):** simple rules and lenient parents teach the controls. Choices mainly build Creativity.
- **Early Teen (ages 12 to 14):** phones and friends appear, the parents grow stricter, and how the player treats the brother starts to matter. Choices mainly build Communication.
- **Late Teen (ages 15 to 17):** a job, a car, and curfews raise the stakes. Choices mainly build Discipline.

Between stages, a report card reveals the player's life traits. At 18, the mix of traits decides which adult ending the player reaches. Getting kicked out or dying before 18 ends the run early, and every ending, good or bad, unlocks an achievement.

### The House

The game takes place inside a two-story, 12 × 10 m suburban home. There is no outside: the front door is the edge of the world, and walking out of it *is* sneaking out (a trigger, then a fade). The garage, backyard, and roof were cut so the team can focus on the inside. The house opens up as the player ages: Childhood covers the ground floor and the player's bedroom, Early Teen opens the whole house, and in Late Teen the night sneak-out becomes the main event.

**The house is built around one route:** your bed → the upstairs hall → past the landing outside your parents' door → down the L-shaped stairs → past the TV room → out the front door. Every room either sits on that route and makes it harder, or sits off it and gives the player a reason to leave it (snacks, the TV, the console, the lockbox).

**Upstairs**

- **Your bedroom:** where you start, at the dead end of the hall, so there is only one way out and it goes past everyone. The safe zone and main stash: a closet, under the bed, a loose floorboard, and a wall vent you can crawl into while you're small.
- **Hall and landing:** passes your brother's door. The board outside the parents' door creaks, and from the landing railing you can peek down at the front door before committing.
- **Parents' bedroom:** the highest-risk room. The lockbox of confiscated items is in the walk-in closet, and they sleep here at night.
- **Brother's room:** off-limits at first. His TV and console, his own wall-vent stash, and one of the places he can be found on his schedule.
- **Bathroom:** locks, but you can only hide in it for a limited time before a parent knocks.
- **Study (optional):** the family computer and the wifi router.

**The stairs**

- **L-shaped stairs:** the one chokepoint between floors. The lower run turns you toward the living room, so the last few steps walk you straight at the TV spot. Steps 3 and 14 creak.
- **Under-stairs closet:** a low door that kids fit through and adults have to crouch for. The best hiding spot on the ground floor, with a small crawl vent at the back.

**Ground floor**

- **Entry:** the double-height space with the front door, only reachable past the living-room opening.
- **Living room:** the parents' evening TV spot. The couch faces away from the stairs, so at the bottom of the route sound gets you caught, not sight.
- **Kitchen:** snacks, the off-limits cookie cabinet, dishes, Mom at the sink at dinner, a pantry to duck into, and an island you can circle around.
- **Dining room:** homework table and dinner scenes.
- **Laundry room and half bath:** a quiet dead end with the biggest crawl vent and a half bath that locks for a short time.

**Why it's shaped this way:** sound gets you caught at the bottom of the route and sight at the top. A loop on the ground floor (living room → stairs → foyer → kitchen → living room) lets the player circle away from a parent instead of being trapped, and the quiet dead ends (laundry, bathroom) are safe places to wait until a parent's schedule sends them there.

Hiding spots include closets, under beds, behind the couch, the pantry, crawl vents (until you outgrow them), and the bathrooms, which lock but only for a limited time.

### Difficulty and Pacing

Difficulty rises both within each week and across the three life stages, with calm moments placed between the tense ones.

- **Weekly rhythm:** after school is calm task time while the parents are busy, evenings are tense because the parents patrol the house, and nights are high-risk, high-reward sneaking.
- **Stage ramp:** with each stage the parents see farther and patrol more often, more house rules apply, meters drain faster, and there is more contraband to manage. The player also loses crawl vents as they grow, and the rules themselves age: the cookie cabinet is a crime at 8 and nothing at 15, when the car keys are the crime instead.
- **Distractions run out:** the same trick twice in a day stops working, and new distractions unlock by stage (the cup and the remote in Childhood, the router and the dryer in Early Teen, the breaker and the smoke alarm in Late Teen).
- **Two-week structure:** the first week of a stage introduces a new mechanic, such as phones in Early Teen or the car in Late Teen, and the second week tests it under pressure.
- **Breathers:** the report card screen between stages gives the player a calm moment to reflect before the next stage begins.
- **Feedback loop:** low Family Trust makes the parents more suspicious, so trouble builds on itself. An allied brother can break the spiral, while an enemy brother makes it worse by snitching.

## Art

### Theme Interpretation

The house should feel familiar but wrong. It is a normal family home, but it is never seen in plain daylight: it is always near dawn or dusk. A low sun pushes through the windows and throws long, hard, window-shaped shadows across the floors, branches from trees you never see sway on the walls, and only a few lamps are on, so most corners are dark. The art supports the anxiety of hiding from your parents rather than a happy-go-lucky family sitcom.

- **Palette:** dingy greige walls, dark wood floors, a sickly olive kitchen, and faded crayon colors left over from childhood. Warm, slightly sick lamplight fights cold blue-teal shadows, and the clash itself is unsettling.
- **Readability:** Mom's clothes are the one saturated color in the house, so the player can always read where she is and which way she faces, even in the dark.
- **No danger overlay:** parents do not get a bright colored vision cone. The player reads danger from the world: a parent's shadow arriving in a room before she does, light under a door going dark as feet pass, a TV switching off.
- **Unsettling details:** a dying bulb in the upstairs hall, buzzing fluorescent tubes in the bathroom and laundry room, the brother's TV left on in an empty room, ceiling-fan blade shadows sweeping the dining-room ceiling, venetian blinds that cut the sun into stripes, a coat rack by the front door and a dress form in the study that read as people, a teddy bear on your bed facing the door, one family photo hung crooked and another turned to face the wall.

The time of day and the palette grow up with the player:

- **Childhood:** golden-hour dusk. Long amber light and faded crayon colors.
- **Early Teen:** colder and bluer, with posters and clutter filling the house.
- **Late Teen:** gloaming and night. Deep red last light, then moonlight, desaturated, with small neon accents such as phone glow.

Early mornings in any stage get a cold, lavender-grey dawn instead of the warm dusk.

> **TODO:** add color palette image.

### Design

TroUble uses stylized low-poly models, a look that is realistic to build in Blender, reads clearly during stealth, and holds up well over time. The house is currently a detailed greybox that final models will replace piece by piece.

The camera sits at a kid's eye level (about 1.1 m at age 8), and parents are drawn slightly oversized and looming, so the house feels huge at age 8 and shrinks as the player grows.

Visual references: Kindergarten for its creepy-cute simplicity, Hello Neighbor for its exaggerated proportions, and Little Nightmares for a small person in a big, dim, threatening house.

> **TODO:** add reference images.

## Audio

### Music

The music sets a tone of playful mischief that turns tense. Each life stage has its own theme that grows up with the player: toy piano and xylophone in Childhood, lo-fi or pop-punk guitar in Early Teen, and synth or indie in Late Teen.

The music is adaptive. Calm tracks play during normal tasks, a tension layer fades in when a parent is nearby and spikes when the player is spotted, the report card screen between stages is calm and reflective, and every ending gets its own short musical sting.

### Sound Effects

Sound is also a gameplay tool. Parents give away their position with jingling keys, the TV switching off, or footsteps on the stairs, while creaky floors and stairs make the player noisy: the board outside the parents' door, steps 3 and 14, and the front door all creak. Distractions need their own sounds (a cup shattering, shoes banging in the dryer, the smoke alarm, the router beeping as it reboots), and loud appliances like the dryer or the TV can mask the player's footsteps. Being caught plays a sting, finishing a task plays a satisfying chime, and everyday sounds like doors opening and objects being picked up keep the house feeling alive.

All music and sound effects will be original and made by the team, then layered and triggered in Unity by an adaptive audio system.

## Game Experience

### UI

The menus follow the Figma designs in [UI_HANDOFF.md](UI_HANDOFF.md): a night-time house behind a left-aligned text menu, the Bangers font in cream with black outlines, and a red magnet icon with yellow text on hover. In-game screens that are not designed yet (the HUD to-do list, the report card) can still borrow from school supplies: sticky notes and a real report card.

- **HUD:** Hunger, Energy, and Boredom meters sit in the bottom-left corner, Suspicion is an eye icon at the top center that fills and turns red, the day and time sit at the top right, and a sticky-note to-do list tracks the week's chores and homework.
- **Detection feedback:** a question mark at the top of the screen fills while a parent watches the player do something wrong, and turns into an exclamation mark when the player is caught. There is deliberately no warning that a parent can merely *see* the player.
- **Interaction prompts:** context prompts such as "E: Open", "E: Hide", or "E: Wash dishes" appear near objects, with a progress bar for timed tasks and hold-to-use actions.
- **Brother menu:** a radial menu of favors that opens only when the player finds and talks to the brother. Each favor shows its cost, and the options change with Brother Bond, so a hostile brother may refuse or threaten to snitch.
- **Report card:** between stages, life traits are shown as letter grades from A to F, with a short comment written by the parents.
- **Endings:** an ending screen closes each run, and the main menu's Ending Gallery shows locked endings as silhouettes with hints.
- **Menus:** the main menu offers New Game, Load Game, Options, Gallery, and Quit. New Game and Load Game open three save slots, each showing the stage, week, playtime, and save date; starting over a filled slot asks to overwrite it. Options has Audio, Controls, and Display tabs. The pause menu (Esc) offers Resume, Save Game, Options, and Quit to Menu, which asks for confirmation first.

### Controls

#### Keyboard and Mouse

- **Movement:** WASD to move and the mouse to look. Hold Left Shift to sprint (fast but noisy) or C to crouch (slow and quiet).
- **Interact:** E for doors, picking up items, hiding, and tasks. Hold E for timed actions, such as chores or squeezing into a vent.
- **Talk:** E when standing next to the brother opens the brother menu.
- **Inventory:** Tab opens the inventory and stash.
- **Pause:** Esc.
- **Restart (demo only):** R restarts the demo after you are caught.

#### Gamepad

Not planned for the minimum viable product. Gamepad support is a stretch goal if time allows.

## Team and Roles

| Name | Role |
| --- | --- |
| Davis Agnigbakou | Team Lead |
| Camden Thomas | Backend |
| Rylan Clark | Backend |
| Minh Le |Design|
| Parker Kuykendall | Backend/Music |

> **TODO (remaining team members):** add your name and role.

## Development Timeline

### Weekly Plan (Minimum Viable Product)

> **TODO (each team member):** add your own responsibilities for each week in the Responsibilities column.

| Week | Dates | Goals | Milestone | Responsibilities |
| :-: | :-: | --- | --- | --- |
| 1 | Oct 5–11 | Submit the design document, set up the Unity project and repository, and greybox the house layout. | Design document submitted | |
| 2 | Oct 12–18 | Player controller (move, crouch, sprint), camera, and interaction system. | Player moves, crouches, and interacts in the greybox house | |
| 3 | Oct 19–25 | Parent AI: schedules, patrols, vision cones, noise detection, Suspicion, and distractions (go to the broken thing, fix it, return). | One parent patrols and can spot the player | |
| 4 | Oct 26–Nov 1 | Tasks, daily meters, week clock, and HUD. | Playable prototype (one Childhood week) | |
| 5 | Nov 2–8 | Brother schedule, talk menu, and Brother Bond (including snitching). Contraband, stashes, confiscation, crawl vents, and the brother's secret stashes. | Brother, contraband, and stash systems working | |
| 6 | Nov 9–15 | Life traits, report cards, and stage transitions. | Alpha (Childhood stage complete) | |
| 7 | Nov 16–22 | Early Teen stage (phones, full house) and an art pass on the house. | Early Teen stage playable | |
| 8 | Nov 23–29 | Late Teen stage (the night sneak-out) and ending selection logic. Lighter load for Thanksgiving week. | Late Teen stage playable; ending logic working | |
| 9 | Nov 30–Dec 6 | Ending screens and gallery, early endings, and adaptive audio integration. | Beta (content complete) | |
| 10 | Dec 7–11 | Playtesting, bug fixes, menus and settings, and the itch.io page. | Final submission | |

### Progress So Far

As of November 8:

- **Done:** project setup (one assembly per code layer, event channels, Drive workflow for big files); the full two-story greybox house with furniture, working doors, real window openings, and an upstairs; the player (walk, sprint, crouch, mouse look at kid height); press-E interaction with hold-to-use, item pickups, an inventory, and hiding spots (including the under-stairs vent); Mom on a NavMesh patrol loop with a vision cone that only catches the player misbehaving; the main menu, three save slots, the Options tabs, the pause menu, and a first HUD; and the eerie lighting pass with a time-of-day system that blends dawn, dusk, gloaming, and night.
- **Playable demo:** sneak a cookie out of the off-limits kitchen cabinet and hide in the under-stairs vent before Mom sees you. Getting caught restarts the demo (R).
- **Not started yet:** the week clock, tasks, daily meters and life traits, the brother, distractions, stashes and searches, stages and the report card, endings, and audio. Against the plan above, weeks 1 and 2 are done, week 3 is partly done (patrol and sight work, but schedules and hearing are not built and the HUD's Suspicion bar is not hooked up yet), and weeks 4 and 5 have not started.

### Stretch Goals (If Ahead of Schedule)

| Feature | Notes |
| --- | --- |
| Gamepad support | Controller layout that mirrors the keyboard and mouse actions. |
| More weeks per stage | Additional week-long play phases in each life stage. |
| Extra endings | More adult and early endings to discover. |
| Expanded settings | Volume sliders, fullscreen toggle, and key rebinding. |
| Chores as a disguise | Carrying the laundry basket makes the parents' bedroom legal; taking out the trash is the one good reason to touch the front door. |
| The full-name meter | Mom's voice replaces a suspicion warning: first name, then first and middle, then the full name. |
| Evidence left behind | A cabinet door left open or a light left on gets noticed by a parent who walks past later. |
| Mom's secret cigarette | When she's stressed, Mom sneaks off for a cigarette; catch her and you get one get-out-of-jail-free card with her. |
| Growing-up details | Dragging a chair to reach the cookie shelf at 8, grounded weeks where being seen outside your room *is* the crime, pencil height marks on the doorframe each stage. |
| Brother moves out | In Late Teen his room becomes yours, but your ally is gone. |
