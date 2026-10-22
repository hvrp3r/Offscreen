# ADR-0005 Network protocols

**Context.** Media is often stored on a NAS, another computer or an internet box. No single protocol covers all this hardware, and network sources must accept credentials stored securely.

**Options.**
- SMB: Windows shares, internet boxes and most NAS, with credentials and discovery. Defined by Microsoft, with Samba as a free implementation.
- NFS: open standard, native on Linux and NAS. No user credentials in NFSv3, Linux servers expect a privileged client port by default, no mature C# library.
- WebDAV: open standard over HTTP, offered by NAS and Nextcloud, rare on internet boxes.
- DLNA/UPnP: automatic discovery on boxes and TVs, but the server decides how files are organized.

**Decision.** Several protocols behind one source contract, built in this order: SMB, NFS, WebDAV. When the user adds a network source, the core probes the host and suggests NFS first when it is available, SMB otherwise. A source stays bound to the protocol chosen when it was added.

**Consequences.** Most homes work from the first release. NFS needs a read-only NFSv3 client written in the project and, on Linux, permission to use a privileged port, granted at install. When an NFS connection is refused, the user is offered SMB with a clear message.
