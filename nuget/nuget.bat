nuget pack Transformalize.Transform.Fluid.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Transform.Fluid.Autofac.nuspec -OutputDirectory "c:\temp\modules"

REM nuget push "c:\temp\modules\Transformalize.Transform.Fluid.0.8.30-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Transform.Fluid.Autofac.0.8.30-beta.nupkg" -source https://api.nuget.org/v3/index.json