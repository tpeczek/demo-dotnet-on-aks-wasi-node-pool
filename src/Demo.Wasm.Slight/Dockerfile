FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN curl https://github.com/WebAssembly/wasi-sdk/releases/download/wasi-sdk-20/wasi-sdk-20.0-linux.tar.gz -L --output wasi-sdk-20.0-linux.tar.gz
RUN tar -C /usr/local/lib -xvf wasi-sdk-20.0-linux.tar.gz
ENV WASI_SDK_PATH=/usr/local/lib/wasi-sdk-20.0

RUN dotnet workload install wasi-experimental

WORKDIR /src
COPY . .
RUN dotnet publish --configuration Release

FROM scratch

COPY --from=build /src/bin/Release/net8.0/wasi-wasm/AppBundle/Demo.Wasm.Slight.wasm ./app.wasm
COPY --from=build /src/slightfile.toml .