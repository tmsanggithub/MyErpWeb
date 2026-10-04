@echo off
REM Run this from the repository root (double-click or run from cmd/powershell)
REM It will remove tracked files that are now ignored by .gitignore and commit the change.

cd /d "%~dp0"
echo Current directory: %cd%

n=git --version >nul 2>&1
nif %errorlevel% neq 0 (
  echo Git not found in PATH. Install Git and re-run.
  pause
  exit /b 1
)

n:: Check for uncommitted changes
ngit status --porcelain
nset /p proceed=There may be uncommitted changes. Continue? (y/N): 
nif /i not "%proceed%"=="y" (
  echo Aborted.
  pause
  exit /b 1
)

necho Removing from git index (if tracked)...
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/.vs"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/packages"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/MyErpWeb/App_Data/logging"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/MyErpWeb/Properties/PublishProfiles"
necho Adding .gitignore
ngit add .gitignore
necho Committing changes
ngit commit -m "Remove tracked IDE/build folders and add .gitignore"
necho Done. Run 'git status' to verify. If you want to push, run 'git push'.
pause
