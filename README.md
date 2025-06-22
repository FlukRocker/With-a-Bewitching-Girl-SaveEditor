# 🛠 Unity Save Editor for MasyouSyoujo

A simple and interactive save file editor for the Unity game **"With a Bewitching Girl (MasyouSyoujo)"**.  
This tool allows you to **decrypt, view, and edit** key values such as affection level, cash, and work stats.

---

## ✨ Features

- ✅ Decrypt Unity save file (`saveDataX.bytes`)
- ✅ Edit values with friendly names (e.g., `Affection`, `Lewdness`)
- ✅ AES-128 CBC decryption with proper IV/key
- ✅ Saves modified file as `.xml` (for inspection) and re-encrypts back to `.bytes`
- ✅ Compatible with .NET Framework 4.7.2 and C# 7.3
- ✅ Console-based interactive UI

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
