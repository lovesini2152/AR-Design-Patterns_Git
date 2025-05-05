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
- Git-structured project with `.gitignore` tailored for Unity

## 🧩 Structure

``` Assets/ Packages/ ProjectSettings/ ```

> Note: Unity-generated folders (e.g., `Library/`, `Logs/`) and MRTK source files are ignored via `.gitignore`.

## 📦 MRTK Installation

This project requires **Mixed Reality Toolkit (MRTK) version 2.8.3**.

To set up MRTK in this project:

### Option 1: Install via Unity `.unitypackage`

1. Download **MRTK 2.8.3** `.unitypackage` files from the official [MRTK GitHub Releases](https://github.com/microsoft/MixedRealityToolkit-Unity/releases).
2. In Unity, go to `Assets` → `Import Package` → `Custom Package...`.
3. Import the required packages (e.g., `Foundation`, `Examples`, `Tools`, depending on your needs).

### Option 2: Clone from GitHub (Advanced)

1. Clone the MRTK repository into your local machine.
2. Copy the necessary folders (e.g., `MixedRealityToolkit`, `MixedRealityToolkit.Examples`) into your `Assets/` folder.

> **Note:** MRTK files are ignored by version control via `.gitignore`. You must manually install MRTK before opening the project.

## 🛠 Requirements

- Unity **2022.X** or later
- **AR Foundation**
- **MRTK 2.8.3**
- HoloLens 2 or compatible AR HMD
- Git (for version control)

## 📘 Research Background

This project is part of the doctoral research by **Yu Liu** (Hochschule RheinMain), aiming to:

- Define reusable interaction design patterns for AR  
- Support developers through a prefab-based approach  
- Enhance user experience in AR museum exhibitions

## 📄 License

This project is licensed under the [MIT License](./LICENSE).  
You are free to use, modify, and distribute the code with proper attribution.

## 📬 Contact

**Yu Liu**  
PhD Candidate, Hochschule RheinMain  
GitHub: [lovesini2152](https://github.com/lovesini2152)  
Email: [yu.liu@hs-rm.de](mailto:yu.liu@hs-rm.de)
