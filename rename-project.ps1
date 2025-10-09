param(
    [Parameter(Mandatory=$true)]
    [string]$NewProjectName,
    
    [Parameter(Mandatory=$false)]
    [string]$ProjectRoot = $PSScriptRoot
)

# Validate input
if ([string]::IsNullOrWhiteSpace($NewProjectName)) {
    Write-Error "New project name cannot be empty"
    exit 1
}

# Remove invalid characters for project names
$NewProjectName = $NewProjectName -replace '[^a-zA-Z0-9_]', ''

if ([string]::IsNullOrWhiteSpace($NewProjectName)) {
    Write-Error "New project name contains only invalid characters"
    exit 1
}

Write-Host "Renaming project from 'ASPNETStarter' to '$NewProjectName'" -ForegroundColor Green
Write-Host "Project root: $ProjectRoot" -ForegroundColor Yellow

# Create lowercase version for file names and paths
$newProjectNameLower = $NewProjectName.ToLower()

# Step 1: Update file contents
Write-Host "`nStep 1: Updating file contents..." -ForegroundColor Cyan

# Define file extensions to process (exclude binary files, node_modules, obj, bin)
$fileExtensions = @("*.sln", "*.csproj", "*.esproj", "*.cs", "*.json", "*.md", "*.ts", "*.js", "*.vue", "*.html", "*.xml", "*.config", "*.txt", "*.dockerfile")

# Get all files to process, excluding certain directories
$filesToProcess = Get-ChildItem -Path $ProjectRoot -Recurse -Include $fileExtensions | 
    Where-Object { 
        $_.FullName -notmatch "\\(node_modules|bin|obj|\.git|\.vs|dist)\\?" -and
        $_.FullName -notmatch "\.lock$" -and
        -not $_.PSIsContainer
    }

$updatedFiles = 0
foreach ($file in $filesToProcess) {
    try {
        $content = Get-Content -Path $file.FullName -Raw -ErrorAction Stop
        $originalContent = $content
        
        # Replace case-sensitive occurrences
        $content = $content -replace "ASPNETStarter", $NewProjectName
        $content = $content -replace "aspnetstarter", $newProjectNameLower
        
        if ($content -ne $originalContent) {
            Set-Content -Path $file.FullName -Value $content -NoNewline -ErrorAction Stop
            Write-Host "  Updated: $($file.FullName)" -ForegroundColor Gray
            $updatedFiles++
        }
    }
    catch {
        Write-Warning "  Failed to update file: $($file.FullName) - $($_.Exception.Message)"
    }
}

Write-Host "  Updated $updatedFiles files" -ForegroundColor Green

# Step 2: Rename files
Write-Host "`nStep 2: Renaming files..." -ForegroundColor Cyan

$renamedFiles = 0
$filesToRename = Get-ChildItem -Path $ProjectRoot -Recurse -File | 
    Where-Object { 
        ($_.Name -like "*ASPNETStarter*" -or $_.Name -like "*aspnetstarter*") -and
        $_.FullName -notmatch "\\(node_modules|bin|obj|\.git|\.vs|dist)\\?"
    }

foreach ($file in $filesToRename) {
    try {
        $newFileName = $file.Name
        $newFileName = $newFileName -replace "ASPNETStarter", $NewProjectName
        $newFileName = $newFileName -replace "aspnetstarter", $newProjectNameLower
        
        $newPath = Join-Path -Path $file.Directory.FullName -ChildPath $newFileName
        
        if ($newPath -ne $file.FullName) {
            Rename-Item -Path $file.FullName -NewName $newFileName -ErrorAction Stop
            Write-Host "  Renamed file: $($file.Name) -> $newFileName" -ForegroundColor Gray
            $renamedFiles++
        }
    }
    catch {
        Write-Warning "  Failed to rename file: $($file.FullName) - $($_.Exception.Message)"
    }
}

Write-Host "  Renamed $renamedFiles files" -ForegroundColor Green

# Step 3: Rename directories
Write-Host "`nStep 3: Renaming directories..." -ForegroundColor Cyan

$renamedDirs = 0
# Get directories that need renaming, process from deepest to shallowest to avoid path issues
$dirsToRename = Get-ChildItem -Path $ProjectRoot -Recurse -Directory | 
    Where-Object { 
        ($_.Name -like "*ASPNETStarter*" -or $_.Name -like "*aspnetstarter*") -and
        $_.FullName -notmatch "\\(node_modules|bin|obj|\.git|\.vs|dist)\\?"
    } | Sort-Object { $_.FullName.Split('\').Count } -Descending

foreach ($dir in $dirsToRename) {
    try {
        $newDirName = $dir.Name
        $newDirName = $newDirName -replace "ASPNETStarter", $NewProjectName
        $newDirName = $newDirName -replace "aspnetstarter", $newProjectNameLower
        
        $newPath = Join-Path -Path $dir.Parent.FullName -ChildPath $newDirName
        
        if ($newPath -ne $dir.FullName) {
            Rename-Item -Path $dir.FullName -NewName $newDirName -ErrorAction Stop
            Write-Host "  Renamed directory: $($dir.Name) -> $newDirName" -ForegroundColor Gray
            $renamedDirs++
        }
    }
    catch {
        Write-Warning "  Failed to rename directory: $($dir.FullName) - $($_.Exception.Message)"
    }
}

Write-Host "  Renamed $renamedDirs directories" -ForegroundColor Green

# Step 4: Clean up build artifacts
Write-Host "`nStep 4: Cleaning up build artifacts..." -ForegroundColor Cyan

$cleanupDirs = @("bin", "obj", "dist", "node_modules")
$cleanedDirs = 0

foreach ($cleanupDir in $cleanupDirs) {
    $dirsToClean = Get-ChildItem -Path $ProjectRoot -Recurse -Directory -Name $cleanupDir -ErrorAction SilentlyContinue
    foreach ($dir in $dirsToClean) {
        try {
            Remove-Item -Path $dir.FullName -Recurse -Force -ErrorAction Stop
            Write-Host "  Cleaned: $($dir.FullName)" -ForegroundColor Gray
            $cleanedDirs++
        }
        catch {
            Write-Warning "  Failed to clean: $($dir.FullName) - $($_.Exception.Message)"
        }
    }
}

Write-Host "  Cleaned $cleanedDirs directories" -ForegroundColor Green

# Step 5: Update solution file if it exists in parent directory
Write-Host "`nStep 5: Checking for solution file..." -ForegroundColor Cyan

$solutionFile = Get-ChildItem -Path (Split-Path $ProjectRoot -Parent) -Filter "*.sln" -ErrorAction SilentlyContinue | Select-Object -First 1
if ($solutionFile -and $solutionFile.Name -like "*ASPNETStarter*") {
    try {
        $newSolutionName = $solutionFile.Name -replace "ASPNETStarter", $NewProjectName
        $newSolutionPath = Join-Path -Path $solutionFile.Directory.FullName -ChildPath $newSolutionName
        Rename-Item -Path $solutionFile.FullName -NewName $newSolutionName -ErrorAction Stop
        Write-Host "  Renamed solution: $($solutionFile.Name) -> $newSolutionName" -ForegroundColor Gray
    }
    catch {
        Write-Warning "  Failed to rename solution file: $($solutionFile.FullName) - $($_.Exception.Message)"
    }
}

Write-Host "`nProject rename completed!" -ForegroundColor Green
Write-Host "Summary:" -ForegroundColor Yellow
Write-Host "  - Updated $updatedFiles files" -ForegroundColor White
Write-Host "  - Renamed $renamedFiles files" -ForegroundColor White
Write-Host "  - Renamed $renamedDirs directories" -ForegroundColor White
Write-Host "  - Cleaned $cleanedDirs build directories" -ForegroundColor White

Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "  1. Restore NuGet packages: dotnet restore" -ForegroundColor White
Write-Host "  2. Install client dependencies: cd $newProjectNameLower.client && bun install" -ForegroundColor White
Write-Host "  3. Build the solution: dotnet build" -ForegroundColor White
Write-Host "  4. Test the application: dotnet run" -ForegroundColor White

Write-Host "`nNote: You may need to update any IDE-specific files or configurations manually." -ForegroundColor Cyan
