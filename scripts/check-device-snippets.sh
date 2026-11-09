#!/usr/bin/env bash
# Compiles or type-checks the Device SDK pages' app snippets (scripts/check-device-snippets.mjs). By default against the
# published SDKs; before a release, against local ones:
#   OHR_MAVEN_LOCAL=1                 net.ohrtech:device-sdk from ~/.m2 (OHR_DEVICE_SDK_VERSION, default the documented one)
#   OHR_FLUTTER_PATH=<checkout>       ohrtech_device
#   OHR_RN_PATH=<checkout>            @ohrtech/device-sdk-react-native (built: yarn prepare), with its node_modules
#   OHR_DEVICE_NUGET_SOURCE=<dir> OHR_DEVICE_VERSION=<v>  OhrTech.Device from a local feed (dotnet pack), or
#   OHR_DEVICE_DOTNET_PROJECT=<csproj> OhrTech.Device from source, with OHR_DEVICE_SDK_AAR=<the Kotlin library's aar>
# Pass languages to check only some: android flutter rn dotnet.
set -euo pipefail
cd "$(dirname "$0")/.."
D=examples/device
node scripts/check-device-snippets.mjs
langs=("$@")
has() { [ ${#langs[@]} -eq 0 ] || [[ " ${langs[*]} " == *" $1 "* ]]; }

if has android; then
  echo "== Kotlin and Java"
  (cd $D/android && ./gradlew -q --console=plain compileDebugKotlin compileDebugJavaWithJavac)
fi
if has flutter; then
  echo "== Dart"
  if [ -n "${OHR_FLUTTER_PATH:-}" ]; then
    printf 'dependency_overrides:\n  ohrtech_device:\n    path: %s\n' "$OHR_FLUTTER_PATH" > $D/flutter/pubspec_overrides.yaml
  else rm -f $D/flutter/pubspec_overrides.yaml; fi
  (cd $D/flutter && flutter pub get >/dev/null && flutter analyze --no-fatal-infos lib)
fi
if has rn; then
  echo "== TypeScript"
  if [ -n "${OHR_RN_PATH:-}" ]; then
    cat > $D/rn/tsconfig.local.json <<JSON
{ "extends": "./tsconfig.json", "compilerOptions": { "paths": {
  "@ohrtech/device-sdk-react-native": ["$OHR_RN_PATH/lib/typescript/src/index.d.ts"],
  "react": ["$OHR_RN_PATH/node_modules/@types/react"], "react/jsx-runtime": ["$OHR_RN_PATH/node_modules/@types/react/jsx-runtime"],
  "react-native": ["$OHR_RN_PATH/node_modules/react-native/types_generated/index.d.ts"] } } }
JSON
    (cd $D/rn && "$OHR_RN_PATH/node_modules/.bin/tsc" -p tsconfig.local.json)
  else
    (cd $D/rn && npx tsc -p tsconfig.json)
  fi
fi
if has dotnet; then
  echo "== C#"
  args=()
  [ -n "${OHR_DEVICE_DOTNET_PROJECT:-}" ] && args+=("-p:OhrDeviceProject=$OHR_DEVICE_DOTNET_PROJECT")
  [ -n "${OHR_DEVICE_NUGET_SOURCE:-}" ] && args+=("-p:RestoreAdditionalProjectSources=$OHR_DEVICE_NUGET_SOURCE")
  [ -n "${OHR_DEVICE_VERSION:-}" ] && args+=("-p:OhrDeviceVersion=$OHR_DEVICE_VERSION")
  [ -n "${OHR_DEVICE_SDK_AAR:-}" ] && args+=("-p:OhrDeviceSdkAar=$OHR_DEVICE_SDK_AAR")
  [ -n "${JAVA_HOME:-}" ] && args+=("-p:JavaSdkDirectory=$JAVA_HOME")
  dotnet build $D/dotnet/Snippets.csproj -c Release -v q -nologo "${args[@]}" | grep -E 'error|Build succeeded' || true
  dotnet build $D/dotnet/Snippets.csproj -c Release -v q -nologo --no-restore "${args[@]}" >/dev/null
fi
echo "device snippets: OK"
