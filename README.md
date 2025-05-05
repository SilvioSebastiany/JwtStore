mkdir JwtStore 
cd .\JwtStore\      

dotnet new sln 
dotnet new classlib -o JwtStore.Core
dotnet new classlib -o JwtStore.Infra
dotnet new web -o JwtStore.Api 

dotnet sln add .\JwtStore.Api\  
dotnet sln add .\JwtStore.Core\      
dotnet sln add .\JwtStore.Infra\  
