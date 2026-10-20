# Source and validation notes

- Input: user-provided `Pasted markdown(6).md`.
- Parsed English insult entries: **1,508**, numbered continuously from 1 through 1,508.
- The English CSV retains the source term and source URL for each row.
- The supplied source page states that contributions are released under CC0 1.0 Universal; this archive retains that source context. The new localized counterparts and prompt are editorial work product.
- Localized files: each contains 1,508 data rows with stable IDs, one row per English source entry. `IDIOMATIC_COUNTERPART` contains a curated contextual equivalent; `GENERAL_INSULT_FALLBACK` is a natural generic insult that does not retain the source-specific meaning or wordplay. Protected-trait/ableist slurs are blank and marked `SKIP_PROTECTED_TRAIT_SLUR`.
- `IDIOMATIC_COUNTERPART` is a practical dialogue equivalent, not a claim of semantic identity. Some terms are gendered, vulgar, regional, or context-dependent and need in-context review.
- The output is a resource pack only; it does not edit the game or any project files.
