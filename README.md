# VR_Robot Project

A Unity + SteamVR based VR Robot Interaction System.

## Overview
This project implements a VR-based human-robot interaction system using Unity and SteamVR.  
It allows users to interact with a robot in a virtual environment, providing immersive controls and visualization.

## Features
- VR Robot control via SteamVR
- Interactive UI panels and instruction system
- VR pointer and interaction system
- Haptic feedback (vibration support)
- Modular scripts for robot management and game flow

## Project Structure
```
VR_Robot/
│── Assets/               # Core project scripts, scenes, prefabs, materials
│   ├── Scrpts/           # Robot scripts (RobotCtrl.cs, RobotManager.cs, etc.)
│   ├── SteamVR/          # SteamVR integration and input system
│── ProjectSettings/      # Unity project settings
│── Packages/             # Package dependencies
│── .gitignore            # Git ignore rules
│── README.md             # Project documentation
```

## Requirements
- Unity 2021.x / 2022.x (recommended)
- [SteamVR Plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647)
- Windows + VR headset (HTC Vive / Valve Index / Oculus Rift with SteamVR)

## How to Run
1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/VR_Robot.git
   ```
2. Open the project in Unity Hub
3. Install required packages (SteamVR)
4. Press **Play** in Unity Editor with VR headset connected

## Key Scripts
- `RobotCtrl.cs` → Controls robot movement and behaviors
- `RobotManager.cs` → Manages robot state and interactions
- `GameManager.cs` → Handles overall game flow
- `InstructPanelCtrl.cs` → UI panel interactions
- `SteamVR_UIPointer.cs` → VR pointer control
- `PolygonAreaCreator.cs` → Defines robot working area

## License
This project is released under the MIT License.  
Feel free to use and modify it for your own projects.

---
Author: Ciyao Yu  
University of Bristol - Robotics MSc  
