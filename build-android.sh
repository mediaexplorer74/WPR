#!/usr/bin/env bash
set -euo pipefail

# Use Microsoft's .NET SDK (Arch pacman dotnet-sdk lacks Android workload manifests).
export DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}"
export PATH="$DOTNET_ROOT:$PATH"
export JAVA_HOME="${JAVA_HOME:-/usr/lib/jvm/java-17-openjdk}"
export ANDROID_HOME="${ANDROID_HOME:-$HOME/Android/Sdk}"

ROOT="$(cd "$(dirname "$0")" && pwd)"
SOLUTION_DIR="$ROOT/Src"
PROJECT="$SOLUTION_DIR/UI/WPR.UI.Android/WPR.UI.Android.csproj"
FRAMEWORK="${1:-net8.0-android34.0}"

dotnet build "$PROJECT" \
  -p:SolutionDir="$SOLUTION_DIR/" \
  -f "$FRAMEWORK" \
  -p:EmbedAssembliesIntoApk=true \
  -p:AndroidEnableAssemblyCompression=false \
  "${@:2}"
