# AR Design Patterns (Unity Project)

This Unity project is part of an academic research initiative on **Head-Mounted Display (HMD)-based Augmented Reality interaction design patterns**, developed as part of a PhD research project.

## 🔍 Overview

The goal of this project is to create a reusable set of **interaction design patterns** and **Unity-based prefab modules** that support museum and cultural heritage experiences using AR glasses like HoloLens 2.

## ✨ Features

- Unity project configured for **AR Foundation** and **HoloLens 2**
- Modular prefabs implementing key interaction patterns:
  - Guiding users to points of interest
  - AR jigsaw puzzle and reassembly
  - AR exhibit drawing and feature highlighting
  - Environmental protection game (e.g., coral reef defense)
- Optimized for cultural heritage applications
- Git-structured project with `.gitignore` tailored for Unity and MRTK

## 🧩 Project Structure

```
Assets/ 
Packages/
ProjectSettings/
```

> Note: Unity-generated folders (e.g., `Library/`, `Logs/`) and MRTK source files are ignored via `.gitignore`.

## 📦 MRTK Installation

This project requires **Mixed Reality Toolkit (MRTK) version 2.8.3**.

To set up MRTK in this project:

### Option 1: Install via Unity `.unitypackage`

1. Download **MRTK 2.8.3** `.unitypackage` files from the official [MRTK GitHub Releases](https://github.com/microsoft/MixedRealityToolkit-Unity/releases).
2. In Unity, go to `Assets` → `Import Package` → `Custom Package...`.
3. Import the required packages (e.g., `Foundation`, `Examples`, `Tools`, depending on your needs).

### Option 2: Use the Mixed Reality Feature Tool

We recommend using the [Mixed Reality Feature Tool](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mrtk) for installing MRTK.

#### ✅ Recommended MRTK 2.8.3 Packages

When using the Feature Tool, select only the following components:

- **Mixed Reality Toolkit Foundation** ✅
- **Mixed Reality Toolkit Standard Assets** ✅
- **Mixed Reality Toolkit Tools** ✅
- **Mixed Reality Toolkit Examples** *(optional)*
- **Mixed Reality Toolkit Extensions** *(optional)*

> This setup provides all essential MRTK functionality while keeping the project lightweight.

## 🛠 Requirements

- Unity **2020.3.25f1 (LTS)** or later
- **AR Foundation**
- **MRTK 2.8.3**
- HoloLens 2 or compatible AR HMD
- Git (for version control)

## 📘 Research Background

This project is part of the doctoral research by **Yu Liu** (Hochschule RheinMain), aiming to:

- Define reusable interaction design patterns for AR  
- Support developers through a prefab-based approach  
- Enhance user experience in AR museum exhibitions

## 📦 UnityPackage

A ready-to-import `.unitypackage` file is included:

**Path**: `Assets/UnityPackages/ForwardCueRooting_Unitypackage.unitypackage`

> This file allows quick import of the Forward Cue Rooting pattern prefab into other Unity projects.  
> If you've already cloned this repository, there's no need to re-import it.

## 📘 Pattern Prefab Documentation

Each interaction pattern prefab will have its own `README.md` under its respective folder:

```
Assets/AR_Design_Patterns/ARPatternPrefabs/Prefab/[PatternName]/README.md
```

These include:
- Pattern description and purpose
- Setup and configuration instructions
- Optional screenshots or demo info



## 📄 License

This project is licensed under the [MIT License](./LICENSE).  
You are free to use, modify, and distribute the code with proper attribution.

## 📬 Contact

**Yu Liu**  
PhD Candidate, Hochschule RheinMain  
GitHub: [lovesini2152](https://github.com/lovesini2152)  
Email: [yu.liu@hs-rm.de](mailto:yu.liu@hs-rm.de)
