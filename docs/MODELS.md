# Download models separately

The helper opens download pages; it does not automatically download model weights. Download the exact file, then copy it to the indicated folder next to the built application, or select an existing compatible file in the helper.

| Component | Download | Destination |
| --- | --- | --- |
| Fast English LLM | [Qwen3.5 2B GGUF](https://huggingface.co/unsloth/Qwen3.5-2B-GGUF/tree/main), `Qwen3.5-2B-Q4_K_M.gguf`, about 1.28 GB | `Models/LLM` |
| Multilingual / better instruction following | [Qwen3.5 4B GGUF](https://huggingface.co/unsloth/Qwen3.5-4B-GGUF/tree/main), `Qwen3.5-4B-Q4_K_M.gguf`, about 2.74 GB | `Models/LLM` |
| Optional larger model | [Qwen3.5 9B GGUF](https://huggingface.co/unsloth/Qwen3.5-9B-GGUF/tree/main), Q4_K_M file | Select as a custom model |
| Local LLM server | [llama.cpp releases](https://github.com/ggml-org/llama.cpp/releases), Windows x64 package appropriate for your hardware | `Models/LLM Runtime`: keep `llama-server.exe` and matching DLLs together |
| Multilingual microphone recognition | [whisper.cpp models](https://huggingface.co/ggerganov/whisper.cpp/tree/main), `ggml-small.bin` | `Models/Whisper` |

Do not download every quantization or the entire model repository. `.en` Whisper models recognize English only. Whisper does not produce voices.

Model file size is not total RAM/VRAM usage. Context, runtime buffers and Cyberpunk also need memory. Auto selects 4B for non-English, contextual profanity and clean dialogue; other English styles select 2B. Explicit selections override Auto. Actual speed and compatibility depend on hardware, runtime version and settings.

Review each model and runtime's own license before redistribution. The public helper excludes all weights and runtime binaries.
