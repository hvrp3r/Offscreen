FROM rust:1-slim-bookworm AS build
ARG I18N_HELPERS_VERSION=0.4.0
RUN cargo install --locked mdbook-i18n-helpers --version ${I18N_HELPERS_VERSION}

FROM debian:bookworm-slim
ARG MDBOOK_VERSION=0.5.4
RUN apt-get update \
    && apt-get install -y --no-install-recommends busybox ca-certificates curl gettext \
    && curl -sSL "https://github.com/rust-lang/mdBook/releases/download/v${MDBOOK_VERSION}/mdbook-v${MDBOOK_VERSION}-x86_64-unknown-linux-gnu.tar.gz" \
       | tar -xz -C /usr/local/bin \
    && apt-get purge -y curl && apt-get autoremove -y && rm -rf /var/lib/apt/lists/*
COPY --from=build /usr/local/cargo/bin/mdbook-* /usr/local/bin/
WORKDIR /docs
EXPOSE 3000
