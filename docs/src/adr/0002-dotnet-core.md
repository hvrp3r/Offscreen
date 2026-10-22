# ADR-0002 C# / .NET core

**Context.** The core mostly does file and network I/O. What matters is library support for SMB, WebAssembly and mDNS, memory use, and single-binary deployment on Linux and Windows.

**Options.**
- Go: good network libraries, single static binary.
- Rust: best performance and memory use, young SMB libraries.
- TypeScript (Node): large ecosystem, weak SMB support, heavier runtime.
- C# / .NET: most complete SMB client, supported Extism host, ahead-of-time compilation to a single binary.

**Decision.** C# on the current .NET LTS.

**Consequences.** Mature libraries for network shares and for the extension host. Some libraries may prevent ahead-of-time compilation, in which case the core runs on the JIT with higher memory use.
