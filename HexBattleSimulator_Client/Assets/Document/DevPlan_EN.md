# HexAutoBattle3D - 4 Week Development Plan

A 3D auto-battle simulator inspired by AFK: Journey, featuring hex-based unit placement, automatic PvE combat, and simple AI. Built with Unity and designed for Android mobile.

---

## 🗓️ Week 1: Project Setup & Hex Tile System

### ✅ Goals
- Unity project setup & GitHub linked
- 6-sided hex tile grid implementation
- Quater-view 3D camera using Cinemachine
- Basic placeholder units (from free 3D assets)

### 🔧 Tasks
- Setup Git repo and Unity folder structure
- Create reusable HexTile prefab
- Implement axial hex coordinates and grid generation script
- Basic camera movement (pan, zoom)
- Display debug coordinates on tiles (Gizmos or TextMesh)

---

## 🗓️ Week 2: Unit Data & Placement System

### ✅ Goals
- ScriptableObject-based unit data design
- Allow unit placement on hex grid
- Visual distinction between ally/enemy units
- Tile-unit connection and validation

### 🔧 Tasks
- Create UnitData & SkillData as ScriptableObjects
- Build Unit prefab (3D model + controller script)
- Click-to-place or UI-based unit placement
- Maintain tile → unit reference mapping
- Simple UI to preview unit info during placement

---

## 🗓️ Week 3: Battle Loop, AI & Skills

### ✅ Goals
- Frame/timer-based auto battle loop
- Implement simple AI (target search, range check)
- Add skill system (single target & AOE)
- Add animation/effect using DOTween or Particle FX

### 🔧 Tasks
- Build BattleManager (handles turns/loop/cooldowns)
- UnitAI: target selection, movement, skill logic
- Basic damage/healing calculation
- Skill effect prefabs with animation or particles
- Cinemachine camera shake or zoom during skill cast

---

## 🗓️ Week 4: UI, Polish & Build

### ✅ Goals
- In-battle UI: HP bar, cooldown indicator
- End-of-battle result screen (damage, healing report)
- Mobile optimization (touch input, aspect ratio)
- Final GitHub polish + demo video

### 🔧 Tasks
- Implement battle UI: HP bar above units, skill icons
- Build result report UI: summary of damage/healing per unit
- Adjust UI for Android landscape resolution
- Final pass: animation polish, sounds, effects
- Export demo APK / WebGL (optional)
- Create GitHub README + 30-60 sec gameplay video

---

## 📌 Summary Timeline

| Week | Focus Areas                            |
|------|----------------------------------------|
| 1    | Project setup, hex tile grid, camera   |
| 2    | Unit data structure, placement system  |
| 3    | Auto battle logic, AI, skills          |
| 4    | UI, polish, results screen, build      |

---

## 📱 Target Platform

- Unity 2022.x LTS
- Android (landscape)
- Resolution examples: 1920x1080, 2340x1080

