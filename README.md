# 🛠 Unity Save Editor for MasyouSyoujo

A simple and interactive save file editor for the Unity game **"With a Bewitching Girl (MasyouSyoujo)"**.  
This tool allows you to **decrypt, view, and edit** key values such as affection level, cash, and work stats.

---

## ✨ Features

- ✅ Decrypt Unity save file (`saveDataX.bytes`)
- ✅ Edit values with friendly labels (e.g., Affection, Lewdness)
- ✅ AES-128 CBC decryption and encryption
- ✅ View and modify fields interactively in the console
- ✅ Save results as both XML and encrypted `.bytes` format
- ✅ Compatible with C# 7.3+ and .NET Framework 4.7.2

---

## 🧰 Development Details

- **.NET Framework:** 4.7.2  
- **C# Language Version:** 11.0  
- **IDE:** [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/)

To ensure compatibility with modern C# features, your `.csproj` should include:

```xml
<PropertyGroup>
  <TargetFramework>net472</TargetFramework>
  <LangVersion>11.0</LangVersion>
</PropertyGroup>
```

---

## 🎮 Supported Fields

- `cafeCount` → **Café Count**
- `cash` → **Cash**
- `workPower` → **Work Power**
- `koukando` → **Affection**
- `inran` → **Lewdness**

---

## 🏁 Getting Started

### 1. Requirements

- Windows
- .NET Framework 4.7.2
- Your game's save files (e.g. `saveData1.bytes`, `saveData2.bytes`, etc.)

### 2. Usage

1. Place this tool in the same folder as your save files.
2. Run the tool: SaveEditor.exe
3. Enter the slot number (`1`, `2`, `3`, etc.).
4. Follow the menu to view and edit the values.
5. Save to apply changes.

Output files:
- `saveDataX-edited.xml`: editable XML dump
- `saveDataX-edited.bytes`: encrypted save file you can copy back into your game

---

## 🙏 Credits

- 🧠 **[dnSpy](https://github.com/dnSpy/dnSpy)** — for decompiling the Unity game DLLs
- 🤖 **ChatGPT-4o (OpenAI)** — for reverse engineering support and code generation assistance

---

## 📜 Disclaimer

This tool is for **educational and personal use only**.  
Do not redistribute modified game files or use this tool to violate the game’s terms of service.

---
