# Tro🧲ble

**Design Document** — by Camden Thomas, Davis Agnigbakou

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
  - [Theme Interpretation (Sacrifice Is Strength)](#theme-interpretation-sacrifice-is-strength)
  - [Primary Mechanics](#primary-mechanics)
  - [Secondary Mechanics](#secondary-mechanics)
- [Level Design](#level-design)
- [Art](#art)
- [Audio](#audio)
- [Game Experience](#game-experience)
- [Team and Roles](#team-and-roles)
- [Development Timeline](#development-timeline)

## Introduction

### Game Summary Pitch

TroUble: survive growing up under strict parents, where every choice from age 8 to 18 shapes the adult you become, if you make it out at all.

- You grow from age 8 to 18 in a house run by strict parents, across three life stages: Childhood, Early Teen, and Late Teen.
- Each stage is made of week-long play phases where you must finish chores and homework before your parents notice you are slacking.
- You also want to play video games, sneak snacks, watch TV, and get into your brother's room, but your parents are always watching.
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

### Platform

Developed for PC using keyboard and mouse.

### Development Software

- **Unity:** game engine and gameplay programming.
- **Blender:** 3D modeling and animation.

### Genre

Simulation/Adventure/Stealth

### Target Audience

Teenagers or Young Adults who enjoy exploration or stealth games. TroUble will appeal to people who enjoy games like Hello Neighbor or Schoolboy Runaway.

## Concept

### Gameplay Overview

In TroUble, the player grows from age 8 to 18 across three life stages, each made of week-long play phases inside the family home. Each week the player balances chores, homework, and daily needs while sneaking the things they actually want past watchful, strict parents. Choices quietly build life traits that are revealed on a report card between stages and decide which of many adult endings the player reaches at 18. Getting kicked out or dying early ends the run, and every ending, good or bad, is an achievement to collect.

Parents move around the house and watch the player, forcing them to decide when it is safe to sneak. The player's brother follows his own schedule around the house and starts out neutral. Depending on how the player treats him, he becomes an ally who distracts the parents and retrieves confiscated items, or an enemy who snitches.

### Theme Interpretation (Sacrifice Is Strength)

Motto: **Sacrifice Is Strength**

Every week asks the player to trade something: fun now for discipline later, honesty for freedom, or their own record for their brother's. The adult the player becomes at 18 is shaped by what they chose to give up.

### Primary Mechanics

| Mechanic | Animated Mockup (Art not necessarily final) |
| --- | --- |
| **Sneaking and Line of Sight** — Parents have vision cones and can hear noise. Crouching is quieter, and the player can hide behind furniture or in closets to avoid being seen. | Parent's vision cone sweeps a hallway while the player crouches behind a couch. |
| **Tasks** — Chores and homework are short timed interactions. Skipping them raises Suspicion and lowers Grades. | Player finishes the dishes as a progress bar fills. |
| **Daily Meters** — Hunger, Energy, and Boredom drain over the week. Letting one bottom out forces a bad choice, such as raiding the kitchen at night or falling asleep in class. A full Suspicion meter means the player is caught. | HUD meters draining, with Suspicion rising when a parent spots something. |
| **Choices Shape Traits** — Every action quietly feeds a hidden life trait. Lying your way out of trouble builds Communication but costs Family Trust, while finishing homework before gaming builds Discipline. | Report card between stages revealing trait changes. |
| **Getting Caught** — Being caught means a confiscated item, extra chores, and lost Family Trust. Getting caught repeatedly pushes the player toward the kicked-out ending. | Parent confiscates a game console and the Family Trust bar drops. |

How the mechanics interact: the daily meters push the player toward risky sneaking, sneaking risks getting caught, getting caught costs Family Trust, and Family Trust together with the other life traits decides the ending at 18.

### Secondary Mechanics

| Mechanic | Animated Mockup (Art not necessarily final) |
| --- | --- |
| **The Brother** — Your brother starts out neutral and follows his own daily schedule, so the player has to find him and talk to him to ask for a favor, and sometimes he is somewhere the player cannot reach. High Brother Bond makes him an ally who distracts parents, retrieves confiscated items, and once per stage can take the fall for you. Low Brother Bond makes him an enemy who may snitch on you. | Brother pulls a parent into the kitchen while the player slips upstairs. |
| **Contraband and Stashes** — Phones, consoles, and snacks can be hidden in stash spots around the house. Parents occasionally search them, so stashes have to be moved and chosen with care. | Player hides a phone under a loose floorboard, then a parent searches the room. |
| **The Week Clock** — Each week is compressed into one session. Parents follow daily schedules (work, dinner, TV time), so learning their routines is part of the skill. | Clock speeds through a day as parents move between rooms on schedule. |
| **Endings and Achievements** — Every ending, good or bad, is recorded in an ending gallery as an achievement. Early endings include getting kicked out and dark-comedy deaths from risky actions like climbing onto the roof or eating mystery food. | Ending gallery screen with unlocked and locked ending cards. |

## Level Design

### Structure and Progression

The game is split into three life stages, each made of two week-long play phases, for six levels in total. Every week takes place in the same family home, but the rules, tasks, and parent behavior change as the player grows up.

- **Childhood (ages 8 to 11):** simple rules and lenient parents teach the controls. Choices mainly build Creativity.
- **Early Teen (ages 12 to 14):** phones and friends appear, the parents grow stricter, and how the player treats the brother starts to matter. Choices mainly build Communication.
- **Late Teen (ages 15 to 17):** a job, a car, and curfews raise the stakes. Choices mainly build Discipline.

Between stages, a report card reveals the player's life traits. At 18, the mix of traits decides which adult ending the player reaches. Getting kicked out or dying before 18 ends the run early, and every ending, good or bad, unlocks an achievement.

### The House

The game takes place in a two-story suburban home that opens up as the player ages. Childhood covers the ground floor and the player's bedroom, Early Teen opens the whole house, and Late Teen adds the garage and the outdoors at night.

- **Player's bedroom:** the safe zone and main stash.
- **Kitchen:** snacks and chores.
- **Living room:** the TV and the parents' evening spot.
- **Brother's room:** off-limits at first, and one of the places the brother can be found on his schedule.
- **Parents' bedroom:** holds the lockbox of confiscated items and is the highest-risk room in the house.
- **Garage (Late Teen):** the car and job gear.
- **Backyard and roof:** risky shortcuts where dark-comedy deaths can happen.

Hiding spots include closets, under beds, behind the couch, and the bathroom, which locks but can only be used for a limited time before a parent knocks.

### Difficulty and Pacing

Difficulty rises both within each week and across the three life stages, with calm moments placed between the tense ones.

- **Weekly rhythm:** after school is calm task time while the parents are busy, evenings are tense because the parents patrol the house, and nights are high-risk, high-reward sneaking.
- **Stage ramp:** with each stage the parents see farther and patrol more often, more house rules apply, meters drain faster, and there is more contraband to manage.
- **Two-week structure:** the first week of a stage introduces a new mechanic, such as phones in Early Teen or the car in Late Teen, and the second week tests it under pressure.
- **Breathers:** the report card screen between stages gives the player a calm moment to reflect before the next stage begins.
- **Feedback loop:** low Family Trust makes the parents more suspicious, so trouble builds on itself. An allied brother can break the spiral, while an enemy brother makes it worse by snitching.

## Art

### Theme Interpretation

The art should feel cozy until it isn't: a warm, familiar family home that turns threatening the moment a parent might be watching. The color palette grows up with the player:

- **Childhood:** warm, saturated crayon colors.
- **Early Teen:** muted blues and purples, with posters and clutter filling the house.
- **Late Teen:** dusk and night scenes, desaturated, with neon accents such as phone glow and car headlights.

Danger and safety are color coded throughout. Parent vision cones and suspicion use warm reds and oranges, while safe zones and stashes use cool blues.

> **TODO:** add color palette image.

### Design

TroUble uses stylized low-poly models with toon shading, a look that is realistic to build in Blender, reads clearly during stealth, and holds up well over time.

The camera sits at a kid's eye level, and parents are drawn slightly oversized and looming, so the house feels huge at age 8 and shrinks as the player grows.

Visual references: Kindergarten for its creepy-cute simplicity, Hello Neighbor for its exaggerated proportions, and Untitled Goose Game for its flat, readable colors.

> **TODO:** add reference images.

## Audio

### Music

The music sets a tone of playful mischief that turns tense. Each life stage has its own theme that grows up with the player: toy piano and xylophone in Childhood, lo-fi or pop-punk guitar in Early Teen, and synth or indie in Late Teen.

The music is adaptive. Calm tracks play during normal tasks, a tension layer fades in when a parent is nearby and spikes when the player is spotted, the report card screen between stages is calm and reflective, and every ending gets its own short musical sting.

### Sound Effects

Sound is also a gameplay tool. Parents give away their position with jingling keys, the TV switching off, or footsteps on the stairs, while creaky floors and stairs make the player noisy. Being caught plays a sting, finishing a task plays a satisfying chime, and everyday sounds like doors opening and objects being picked up keep the house feeling alive.

All music and sound effects will be original and made by the team, then layered and triggered in Unity by an adaptive audio system.

## Game Experience

### UI

The interface is themed like school supplies, using notebook paper, sticky notes, and a real report card.

- **HUD:** Hunger, Energy, and Boredom meters sit in the bottom-left corner, Suspicion is an eye icon at the top center that fills and turns red, the day and time sit at the top right, and a sticky-note to-do list tracks the week's chores and homework.
- **Detection feedback:** a question mark appears over a parent who is getting suspicious and an exclamation mark when the player is spotted, and the screen edges darken while the player is being watched.
- **Interaction prompts:** context prompts such as "E: Hide" or "E: Wash dishes" appear near objects, with a progress bar for timed tasks.
- **Brother menu:** a radial menu of favors that opens only when the player finds and talks to the brother. Each favor shows its cost, and the options change with Brother Bond, so a hostile brother may refuse or threaten to snitch.
- **Report card:** between stages, life traits are shown as letter grades from A to F, with a short comment written by the parents.
- **Endings:** an ending screen closes each run, and the main menu's Ending Gallery shows locked endings as silhouettes with hints.
- **Menus:** the main menu offers Play, Continue, Ending Gallery, Settings, and Quit, and the pause menu offers Resume, Settings, and Quit to Menu.

### Controls

#### Keyboard and Mouse

| Action | Input |
| --- | --- |
| Move / Look | WASD / Mouse |
| Sprint (fast but noisy) | Hold Shift |
| Crouch (slow and quiet) | C / Ctrl |
| Interact (tasks, pick up items, hide, doors) | E or Left Click |
| Talk (opens brother menu when next to him) | E |
| Inventory and stash | Tab |
| Pause | Esc |

#### Gamepad

Not planned for the minimum viable product. Gamepad support is a stretch goal if time allows.

## Team and Roles

| Name | Role |
| --- | --- |
| Davis Agnigbakou | Team Lead |
| Camden Thomas | Backend |
| Rylan Clark | Backend |

> **TODO (remaining team members):** add your name and role.

## Development Timeline

### Weekly Plan (Minimum Viable Product)

> **TODO (each team member):** add your own responsibilities for each week in the Responsibilities column.

| Week | Dates | Goals | Milestone | Responsibilities |
| :-: | :-: | --- | --- | --- |
| 1 | Oct 5–11 | Submit the design document, set up the Unity project and repository, and greybox the house layout. | Design document submitted | |
| 2 | Oct 12–18 | Player controller (move, crouch, sprint), camera, and interaction system. | | |
| 3 | Oct 19–25 | Parent AI: schedules, patrols, vision cones, noise detection, and Suspicion. | | |
| 4 | Oct 26–Nov 1 | Tasks, daily meters, week clock, and HUD. | Playable prototype (one Childhood week) | |
| 5 | Nov 2–8 | Brother schedule, talk menu, and Brother Bond (including snitching). Contraband, stashes, and confiscation. | | |
| 6 | Nov 9–15 | Life traits, report card, and stage transitions. | Alpha (Childhood stage complete) | |
| 7 | Nov 16–22 | Early Teen stage (phones, full house) and an art pass on the house. | | |
| 8 | Nov 23–29 | Late Teen stage (garage, night) and ending selection logic. Lighter load for Thanksgiving week. | | |
| 9 | Nov 30–Dec 6 | Ending screens and gallery, early endings, and adaptive audio integration. | Beta (content complete) | |
| 10 | Dec 7–11 | Playtesting, bug fixes, menus and settings, and the itch.io page. | Final submission | |

### Stretch Goals (If Ahead of Schedule)

| Feature | Notes |
| --- | --- |
| Gamepad support | Controller layout that mirrors the keyboard and mouse actions. |
| More weeks per stage | Additional week-long play phases in each life stage. |
| Extra endings | More adult and early endings to discover. |
| Expanded settings | Volume sliders, fullscreen toggle, and key rebinding. |
