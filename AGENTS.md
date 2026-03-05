# Repository Guidelines

## Project Structure & Module Organization
This repository is a single-project WPF desktop app targeting **.NET Framework 4.8**.
- Solution: `bime.sln`
- Main project: `bime.csproj`
- UI entry points: `App.xaml`, `MainWindow.xaml`, `WinCandidate.xaml`, `WinConfig.xaml`, `WinAddCi.xaml`
- Core logic: root-level `*.cs` files such as `AutoSpace.cs`, `Config.cs`, `SendHistory.cs`, `Win32.cs`
- Assets/resources: `icon/`, `*.ico`, and `Properties/` generated resource/settings files
- NuGet package list: `packages.config`

Keep feature-related changes grouped (UI XAML + corresponding `*.xaml.cs` + supporting service/class files).

## Build, Test, and Development Commands
- `nuget restore bime.sln`  
  Restores missing packages (required for `Interop.UIAutomationClient`).
- `msbuild bime.sln /p:Configuration=Debug`  
  Builds a local debug binary.
- `msbuild bime.sln /p:Configuration=Release`  
  Produces optimized release output.

Typical output folder: `bin\Debug\` or `bin\Release\`.

## Coding Style & Naming Conventions
- Use 4-space indentation and UTF-8 encoding where possible.
- Follow existing C# conventions:
  - `PascalCase` for classes, methods, properties.
  - `camelCase` for local variables/parameters.
  - Keep window code-behind names aligned with XAML (`WinConfig.xaml` <-> `WinConfig.xaml.cs`).
- Prefer small, focused methods in large files (notably `MainWindow.xaml.cs`).

No formatter/linter config is committed; match surrounding style in edited files.

## Testing Guidelines
There is currently no dedicated automated test project in this repo. Use manual regression checks before opening a PR:
- Launch app and verify candidate window behavior (`WinCandidate`).
- Validate settings changes in `WinConfig`.
- Smoke-test input workflow and tray/menu interactions.

If you add non-trivial logic, include reproducible manual test steps in the PR.

## Commit & Pull Request Guidelines
Recent commits use short, imperative summaries (often Chinese), sometimes with prefixes like `fix:`. Keep commits:
- Small and logically scoped.
- Message format: one-line summary, e.g. `fix: 修正候选窗口滚动异常`.

PRs should include:
- What changed and why.
- Risk/impact notes.
- Manual verification steps.
- Screenshots/GIFs for UI-visible changes.
