```script
dotnet new sln --name CartesionExplosionSln

dotnet new webapi -n cartesionExplosion-api

dotnet sln add .\cartesionExplosion-api\

dotnet new console -n cartesopmExplosion-benchmark 

dotnet sln add .\cartesopmExplosion-benchmark\ 

dotnet add package BenchmarkDotNet --version 0.14.0
```