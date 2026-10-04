@echo off
REM Apply .gitignore cleanup for MyErpWeb repository
REM Run this from repository root: D:\DuAn\Quyen-ToiLoc\SourceCode\MyErpWeb

echo This script will remove tracked items that should be ignored and commit the change.
echo It will execute the following commands:
echo   git rm -r --cached MyErpWebSolution/.vs
echo   git rm -r --cached MyErpWebSolution/packages
echo   git rm -r --cached MyErpWebSolution/MyErpWeb/App_Data/logging
echo   git rm -r --cached MyErpWebSolution/MyErpWeb/Properties/PublishProfiles
echo   git add .gitignore
echo   git commit -m "Remove tracked IDE/build folders and add .gitignore"
echo
necho Press Y to continue and run commands, or any other key to abort.
set /p choice=Continue? (Y/N): 
if /i not "%choice%"=="Y" (
  echo Aborted by user.
  pause
  exit /b 1
)

n:: ensure running from script directory
ncd /d "%~dp0"

n:: verify git available
ngit --version >nul 2>&1
nif %errorlevel% neq 0 (
  echo Git not found in PATH. Install Git and re-run.
  pause
  exit /b 1
)

n:: run remove cached commands (ignore if not tracked)
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/.vs"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/packages"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/MyErpWeb/App_Data/logging"
ngit rm -r --cached --ignore-unmatch "MyErpWebSolution/MyErpWeb/Properties/PublishProfiles"

n:: add .gitignore and commit
ngit add .gitignore
ngit commit -m "Remove tracked IDE/build folders and add .gitignore" || (
  echo Nothing to commit or commit failed.
)
necho Done. Run 'git status' to verify. If you want to push, run 'git push'.
pause
