# Download models separately

The helper does not include or automatically download model weights or runtime binaries. Download only the file listed below, then put it in the indicated folder beside the Helper application. You may also browse to files you already have.

| Component | Download | Destination |
| --- | --- | --- |
| Fast English LLM | [Download Qwen3.5 2B Q4_K_M](https://huggingface.co/unsloth/Qwen3.5-2B-GGUF/resolve/main/Qwen3.5-2B-Q4_K_M.gguf), about 1.28 GB | `Models/LLM` |
| Multilingual / better instruction following | [Download Qwen3.5 4B Q4_K_M](https://huggingface.co/unsloth/Qwen3.5-4B-GGUF/resolve/main/Qwen3.5-4B-Q4_K_M.gguf), about 2.74 GB | `Models/LLM` |
| Optional larger model | [Qwen3.5 9B GGUF files](https://huggingface.co/unsloth/Qwen3.5-9B-GGUF/tree/main), choose its Q4_K_M file | Select as a custom model |
| Local LLM server | [llama.cpp Windows releases](https://github.com/ggml-org/llama.cpp/releases/latest): NVIDIA users can start with Windows x64 CUDA 12; Windows x64 CPU is the slower fallback; AMD users can try Vulkan | Extract the downloaded ZIP into `Models/LLM Runtime`; keep `llama-server.exe` and its matching DLLs together |
| Multilingual microphone recognition | [Download Whisper Small multilingual](https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-small.bin) | `Models/Whisper` |

## Install the llama.cpp runtime

1. Open the official llama.cpp releases page and download the Windows x64 ZIP that matches your hardware. Do not download “Source code.”
2. Extract the ZIP archive. In Windows, right-click the ZIP and choose **Extract All**.
3. Copy the extracted runtime files into the Helper’s `Models/LLM Runtime` folder. Keep `llama-server.exe` and the DLL files from the same archive together; if the ZIP has subfolders, preserve them.
4. In the Helper, browse to the runtime folder (or directly to `llama-server.exe` if prompted). The Helper starts the runtime when you press **Start model**; do not launch `llama-server.exe` by double-clicking it.

The GGUF model file is separate: put it in `Models/LLM`. You need one model and one runtime package. Do not download every quantization or the entire Hugging Face repository.

`.en` Whisper models recognize English only. Whisper does not produce voices. Model file size is not total RAM/VRAM usage. Context, runtime buffers and Cyberpunk also need memory. Auto selects 4B for non-English, contextual profanity and clean dialogue; other English styles select 2B. Explicit selections override Auto. Actual speed and compatibility depend on hardware, runtime version and settings.

Review each model and runtime’s own license before redistribution. The public helper excludes all weights and runtime binaries.
