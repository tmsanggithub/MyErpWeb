# Run this script from the repository root of MyErpWeb
# Example: cd D:\DuAn\Quyen-ToiLoc\SourceCode\MyErpWeb
# It will remove tracked files that are now ignored by .gitignore and commit the change.

$paths = @(
    'MyErpWebSolution/.vs',
    'MyErpWebSolution/packages',
    'MyErpWebSolution/MyErpWeb/App_Data/logging',
    'MyErpWebSolution/MyErpWeb/Properties/PublishProfiles'
)

function Run-Git([string] $args) {
    $git = Get-Command git -ErrorAction SilentlyContinue
    if (-not $git) {
        Write-Error "git not found in PATH. Install Git and re-run."
        exit 1
    }
    & git $args
}

# Show current git status
Write-Host "Current git branch and status:"
Run-Git 'status --short'

# Check for uncommitted changes
$hasChanges = (& git status --porcelain) -ne $null
if ($hasChanges) {
    Write-Host "There are uncommitted changes. It's recommended to stash or commit them before running this script. Continue? (y/N)"
    $c = Read-Host
    if ($c -ne 'y' -and $c -ne 'Y') { Write-Host 'Aborted.'; exit 1 }
}

# Remove each tracked path from index (but keep files on disk)
foreach ($p in $paths) {
    if (Test-Path $p) {
        Write-Host "Removing from git index (if tracked): $p"
        Run-Git "rm -r --cached --ignore-unmatch -- "$p""
    } else {
        Write-Host "Path does not exist in working tree: $p"
    }
}

# Ensure .gitignore is added
Run-Git 'add .gitignore'

# Commit the changes
Write-Host "Committing changes..."
Run-Git 'commit -m "Remove tracked IDE/build folders and add .gitignore"'

Write-Host "Done. Run 'git status' to verify. If you want to push, run 'git push'."