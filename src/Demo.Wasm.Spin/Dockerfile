FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /opt/build
RUN apt-get update && apt-get install xz-utils
RUN curl -LO https://github.com/bytecodealliance/wizer/releases/download/v1.6.0/wizer-v1.6.0-x86_64-linux.tar.xz \
    && tar -xvf wizer-v1.6.0-x86_64-linux.tar.xz \
    && rm wizer-v1.6.0-x86_64-linux.tar.xz \
    && install wizer-v1.6.0-x86_64-linux/wizer /usr/local/bin

WORKDIR /src
COPY . .
RUN dotnet build -c Release

FROM scratch
COPY --from=build /src/bin/Release/net7.0/Demo.Wasm.Spin.wasm ./bin/Release/net7.0/Demo.Wasm.Spin.wasm
COPY --from=build /src/spin.toml .