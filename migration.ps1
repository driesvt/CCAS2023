param (
    [string]$name = $( Read-Host "Migration Point Name, please" )
)

dotnet ef migrations add "$name" --project src\Application --startup-project src\BlazorServer --output-dir Common\Persistence\Migrations
