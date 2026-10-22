(() => {
  const LANGUAGES = { en: "English", fr: "Français" };
  const DEFAULT_LANGUAGE = "en";

  const current = document.documentElement.lang || DEFAULT_LANGUAGE;
  const bookRoot = new URL(typeof path_to_root === "string" ? path_to_root : "./", location.href);
  const page = location.href.slice(bookRoot.href.length);
  const siteRoot = current === DEFAULT_LANGUAGE ? bookRoot : new URL("../", bookRoot);

  const select = document.createElement("select");
  select.className = "language-picker";
  select.setAttribute("aria-label", "Language");
  for (const [code, label] of Object.entries(LANGUAGES)) {
    const option = new Option(label, code, false, code === current);
    option.lang = code;
    select.add(option);
  }
  select.addEventListener("change", async () => {
    const requested = select.value;
    const prefix = requested === DEFAULT_LANGUAGE ? "" : `${requested}/`;
    const target = new URL(prefix + page, siteRoot).href;
    try {
      const response = await fetch(target, { method: "HEAD" });
      if (response.ok) {
        location.href = target;
        return;
      }
    } catch {}
    select.value = current;
    alert(`${LANGUAGES[requested]} is not available on this server. Use "scripts/docs.sh serve" to browse every language.`);
  });

  const target = document.querySelector(".right-buttons");
  if (target) target.prepend(select);
})();
