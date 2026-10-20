# NightCityNet Helper by PalcherBTW

A Windows helper for installing local AI components and personalizing the CET **NightCityNet** mod for Cyberpunk 2077.

**Preview 0.9.** The application compiled successfully. The new City Studio features have not been tested in the UI or in game. This is a preview for a manual pilot, not a verified stable release.

[YouTube / support the author](https://www.youtube.com/@Palcher1715) · [Releases](https://github.com/Pu1cher/NightCityNet-Helper/releases)

## Start here

1. Install the CET NightCityNet mod. This helper does not install Cyberpunk or NightCityNet; AI NPC LLM Companions is a different mod.
2. Download a GGUF model and llama.cpp runtime using the [model guide](docs/MODELS.md). Models and runtime binaries are not included.
3. Open the helper, choose your Cyberpunk root folder, select a model, press **Start model**, then **Connect to mod**. Keep the helper open while playing.
4. Optionally select a language and prompt style, preview the proposed changes, then install them with Cyberpunk closed.

Voice providers remain configured inside NightCityNet. **Whisper transcribes your microphone; it does not generate NPC voices.** Selecting Ukrainian text does not by itself install Ukrainian voice synthesis.

## Features

- Start a local llama.cpp server using a selected compatible GGUF model.
- Configure the mod's local LLM connection and multilingual Whisper settings.
- Apply language, contextual or extreme profanity, clean dialogue and dialogue-ending preferences.
- Create city atmosphere presets and removable custom instructions; add a visible description of V.
- Export and import small `.ncnsetup.json` setup files without models, account keys or character memory.
- Back up NPC memory and character prompts separately from shared setups.
- Keep native English facial animation; optional approximate jaw movement for unsupported non-English speech.

The helper changes selected fields and its own marked prompt sections. It preserves unrelated character profiles, World State and memory. Original/custom instructions may still affect model behavior. Removing a prompt does not erase events that NPCs have already remembered. Back up Cyberpunk savegames separately.

## Build from source

On 64-bit Windows with the .NET Framework 4.x compiler installed, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\build.ps1
```

The runnable folder is created at `dist/NightCityNet-Helper`. No models are downloaded and no tests are run by this build script. The source includes development test code, but build success alone does not validate runtime behavior.

## Project layout

| Folder | Contents |
| --- | --- |
| `src` | C# application source |
| `assets` | Prompt lexicons, compatibility files and empty model folders |
| `presets` | Example shared city setups |
| `docs` | Full user guide and model links |
| `scripts` | Windows build script |

See the [full user guide](docs/user-guide.txt) and [City Studio quick start](docs/city-studio.txt). The full user guide also describes the older bundled ZIP layout; this source repository uses the layout above.

## Sharing and privacy

Publish source and prepared public packages only. Do not upload your working helper folder: `settings.user.json`, transaction backups and Setup Library may contain private paths, settings, keys or authored text. Memory backups are personal data. Model weights and runtime binaries belong in their download locations, not in this repository.

No project-wide open-source license has been selected yet. Public visibility alone does not grant reuse rights. Third-party model/runtime licenses and lexicon source notes remain separate; see `assets/Prompts/Джерело`.
