# Fishing Frenzy
### Team 7 — AE Chill

---

## Overview

### The Elevator Pitch

A casual 2D fishing game where players click to catch fish, dodge dangerous sea creatures, and use special items to maximize their score before time runs out.

---

### Project Description

**Fishing Frenzy** is a casual arcade fishing game built in Unity. The player uses a fishing rod cursor to click on fish swimming across the screen and score points. The challenge comes from an escalating threat system — sharks periodically sweep the screen and eat catchable fish, while bombs fall from above and destroy everything in their blast radius. Two powerful special fish can appear to help the player turn the tide. The game rewards quick reflexes, smart prioritization, and knowing when to use item effects.

---

## Theme / Setting / Genre

- **Theme:** Casual ocean fishing with arcade-style escalating hazards.
- **Setting:** A 2D side-scrolling underwater scene. Fish, crabs, and sea creatures swim continuously across the screen against a water background.
- **Genre:** Casual / Arcade Clicker.

---

## Core Gameplay Mechanics

### Mechanic #1 — Click to Catch
The player's cursor is replaced by a fishing rod sprite. Left-clicking on any fish triggers a catch attempt. Each fish has a `catchChance` value (0–1). On success, the fish is destroyed and points are added to the score. On failure, the fish shakes briefly and escapes. The system is handled by `FishCatchable.cs` and `ClickCatch.cs`.

### Mechanic #2 — Fish Movement
Regular fish spawn off-screen left and swim right (or vice versa), bouncing direction when they exit the opposite edge. Their speed is configurable per prefab. Crabs walk along the bottom of the screen and flip direction at the screen edges. The Special Fish spawns off the right edge and darts across the screen at high speed — the player must react quickly to click it. Handled by `FishMove.cs`, `CrabMove.cs`, and `SpecialFishMove.cs`.

### Mechanic #3 — Score System
Every successfully caught fish adds its `scoreValue` to a persistent score tracked by `ScoreManager`. The score is displayed on-screen via a TMP text element and persists across scene loads using `DontDestroyOnLoad`.

### Mechanic #4 — Shark Threat
A shark spawns off the right edge every 90 seconds and moves left across the screen. Any catchable fish it touches is instantly eaten and destroyed (handled by `FishEatable.cs`). A warning UI text appears as the shark approaches the screen edge, giving the player a heads-up to catch fish before they are lost. The Orca counters this — it spawns from the left, moves right, and destroys any shark it collides with.

### Mechanic #5 — Bomb Hazard
Bombs fall from above at a fixed speed and explode on hitting the screen bottom. The explosion triggers an `OverlapCircleAll` check at the bomb's position. Any `FishCatchable` within the radius is destroyed immediately. Any object with a `Health` component (like tougher fish) takes damage instead. A visual explosion effect and an audio SFX play on detonation.

### Mechanic #6 — Special Fish Items
Two rare special fish provide powerful item effects when caught:

- **Ace Fish (Mu Effect):** Catching it triggers a radial pulse effect that expands outward. All fish within the area are force-caught via `ForceCatch()`, guaranteeing 100% catch rate with no luck check. Handled by `MuEffect.cs`.
- **Stop Fish (HieuUng Effect):** Catching it triggers a flash effect (scales up and fades). It is designed to freeze fish movement — the freeze logic (`FishFreezeManager`) is stubbed in the codebase and planned for a future sprint. Handled by `HieuUngEffect.cs`.

### Mechanic #7 — Audio System
The game uses a centralized `AudioController` singleton with two channels: background music and sound effects. Audio clips are mapped to a `SoundType` enum and stored in ScriptableObject assets (`SoundFXData`, `MusicBGData`). SFX includes: `Click`, `CatchFish`, `MissFish`, `Freeze`, `Bomb`, `Explosion`, and `Death`. Music tracks support different moods: `MainMenu`, `LevelNormal`, `LevelEasy`, and `Boss`.

---

## Target Platform

- **Platform:** PC (Windows / WebGL)
- **Why:** The core interaction is mouse-click precision. A mouse provides the accuracy needed to click fast-moving fish, making PC the natural fit. WebGL is targeted for easy browser-based distribution without installation.

---

## Project Scope

- **Team Size:** 4 members
- **Engine:** Unity (Free Personal License)
- **Time Scale:** 2–3 months
- **Cost:** Time investment only

---

## Influences

- **Fishing Clash / Fish Idle** — Casual mobile fishing genre conventions: simple click interactions, score-driven progression, and approachable difficulty ramp.
- **Classic Arcade Clickers** — The bomb hazard and timed threat waves draw from classic arcade design where escalating dangers pressure the player to act faster.

---

## What Sets This Apart

- **Dual Threat System:** Both sharks (lateral threats) and bombs (vertical threats) attack the fish population at the same time, creating interesting moments where the player must choose which fish to prioritize before they are lost.
- **Orca as a Counter-Play Tool:** The Orca is not just flavour — it actively removes the shark threat, rewarding players who understand the ecosystem and wait for it.
- **Probabilistic Catching:** The `catchChance` system per fish means different fish feel different to catch. High-value fish can have lower catch rates, creating tension and a visible shake-feedback when the player narrowly misses.
- **Force-Catch via Item:** The Ace Fish's radial force-catch bypasses the probability system entirely, giving a satisfying burst moment when timed well during a dense fish cluster.

---

## Game Objects & Entities

| Entity | Movement | Behavior |
|---|---|---|
| Regular Fish | Left ↔ Right (bounces) | Catchable, eaten by Shark |
| Crab | Left ↔ Right at ground level | Catchable, walks along bottom |
| Special Fish | Fast left sweep (one-way) | Rare, triggers item effect on catch |
| Shark | Right → Left (one-way) | Eats catchable fish on contact |
| Orca | Left → Right (one-way) | Destroys Shark on contact |
| Bomb | Falls downward | Explodes at bottom, area damage |

---

## Audio Design

| Sound | Trigger |
|---|---|
| `CatchFish` | Successful fish catch |
| `MissFish` | Failed catch attempt (fish shakes) |
| `Bomb` | Bomb explodes |
| `Explosion` | Explosion VFX |
| `Freeze` | Stop Fish item activated |
| `Death` | Entity destroyed |
| `Click` | UI button press |
| `LevelNormal / LevelEasy` | In-game background music |
| `MainMenu` | Menu screen music |
| `Boss` | Shark / threat event music |

---

## Schedule and Milestones

**Objective 1 — Core Loop (Weeks 1–2)**
Fish spawning, movement, click-to-catch with probability, score display, and basic cursor replacement.

**Objective 2 — Threat System (Weeks 3–4)**
Shark spawning, shark warning UI, fish-eating on collision, Orca counter-threat, and bomb fall + explosion.

**Objective 3 — Special Items (Month 2, Weeks 1–2)**
Special Fish spawner, Ace Fish force-catch radial effect, Stop Fish freeze effect, and item audio integration.

**Objective 4 — Audio & Polish (Month 2, Weeks 3–4)**
Full AudioController integration across all events, background music per scene, and SFX tuning.

**Objective 5 — Release (Month 3)**
Final balancing of spawn rates, catch chances, and score values. UI polish, WebGL build, and QA pass.
