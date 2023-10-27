#Define Local Deploy Action
#Define Remote Deploy Action
#Define standard build Action
#Define test Action
Set-Location .\src\application\gRpcFileService-Sample.Server
dotnet publish --configuration Release

Set-Location bin\Release\net7.0\publish
rm -r deploy.zip
Compress-Archive -Path * -DestinationPath deploy.zip

Publish-AzWebApp -ResourceGroupName Rg-gRpcFileService-Sample -Name grpcfileserviceserver -ArchivePath (Get-Item .\deploy.zip).FullName -Force -Timeout 400000
Restart-AzWebApp -ResourceGroupName Rg-gRpcFileService-Sample -Name grpcfileserviceserver

cd ../../../../../../..
