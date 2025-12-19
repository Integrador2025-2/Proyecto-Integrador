param(
    [int]$MaxRetries = 15,
    [int]$SleepSeconds = 4
)

$root = Split-Path -Parent $PSScriptRoot
Write-Host "[ensure-db] Starting docker-compose..."
docker-compose up -d

Set-Location $root

$i = 0
Write-Host "[ensure-db] Running 'dotnet ef database update' with up to $MaxRetries retries..."
while ($true) {
    try {
        dotnet ef database update
        Write-Host "[ensure-db] Migrations applied successfully."
        break
    } catch {
        $i++
        if ($i -ge $MaxRetries) {
            Write-Error "[ensure-db] Failed to apply migrations after $MaxRetries attempts."
            exit 1
        }
        Write-Host "[ensure-db] Attempt $i failed — waiting $SleepSeconds seconds..."
        Start-Sleep -Seconds $SleepSeconds
    }
}
