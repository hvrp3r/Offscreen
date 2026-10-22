# ADR-0004 WebAssembly extensions

**Context.** Extensions are untrusted. They must stay within their permissions, and a faulty extension must not crash the media center. The system must be built by the project.

**Options.**
- One process per extension: strong isolation, heavy, permissions rely on an existing runtime.
- JavaScript isolates: light, JavaScript only, permissions to build from scratch.
- WebAssembly with Extism: strong isolation, light, any language that compiles to WebAssembly.

**Decision.** WebAssembly with Extism, hosted by the core.

**Consequences.** A permission that is not granted simply has no host function to call. Calls are bounded in time and memory. Extensions describe their widgets and settings as data, and the clients render them. Authors write TypeScript against the SDK and debug in a development mode that runs the extension in Node, connected to a real core.
