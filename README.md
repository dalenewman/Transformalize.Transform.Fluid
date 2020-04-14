### Fluid Transform
This is a [liquid template language](https://www.shopify.com/partners/shopify-cheat-sheet) transform for [Transformalize](https://github.com/dalenewman/Transformalize).

It's implemented using [fluid](https://github.com/sebastienros/fluid) by Sébastien Ros.

> Fluid is an open-source .NET template engine that is as close as possible to the Liquid template language.

### Usage

Here's a test arrangement that reads 5 rows from the `bogus` provider
and uses a short-hand variation of the fluid transform to create the `Score` field.

The liquid template is:

 `{% assign x = Stars * Reviewers %}<span>{{ x }}</span>`

The arrangement is:

```xml
<add name='TestProcess' read-only='true'>
   <connections>
      <add name='input' provider='bogus' seed='1' />
      <add name='output' provider='console' />
   </connections>
   <entities>
      <add name='Contact' size='5'>
         <fields>
            <add name='FirstName' />
            <add name='LastName' />
            <add name='Stars' type='byte' min='1' max='5' />
            <add name='Reviewers' type='int' min='0' max='500' />
         </fields>
         <calculated-fields>
            <add name='Score' raw='true' t='fluid({% assign x = Stars * Reviewers %}&lt;span>{{ x }}&lt;/span>)' />
         </calculated-fields>
      </add>
   </entities>
</add>
```

This output is:

```bash
FirstName,LastName,Stars,Reviewers,Score
Justin,Konopelski,5,438,<span>2190</span>
Carmen,Bradtke,1,37,<span>37</span>
Bryan,Boehm,3,172,<span>516</span>
Caleb,Hane,4,78,<span>312</span>
Willie,Tromp,1,65,<span>65</span>
```

### Benchmark
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.720 (1909/November2018Update/19H2)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.201
  [Host]    : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  RyuJitX64 : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

Job=RyuJitX64  Jit=RyuJit  Platform=X64  

```
|                    Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|-------------------------- |---------:|---------:|---------:|------:|--------:|
|         &#39;10000 test rows&#39; | 662.4 ms | 12.69 ms | 16.05 ms |  1.00 |    0.00 |
| &#39;10000 rows with 1 fluid&#39; | 706.5 ms |  7.32 ms |  6.49 ms |  1.06 |    0.03 |
