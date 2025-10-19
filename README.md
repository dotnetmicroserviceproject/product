# Product Microservice
Common libraries used by Play Economy services.

## Create and publish package

```powershell
$version="1.0.3"
$owner="dotnetmicroserviceproject"
$gh_pat="[PAT HERE]"

dotnet pack src\Product.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/product -o ..\packages

dotnet nuget push ..\packages\Product.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```