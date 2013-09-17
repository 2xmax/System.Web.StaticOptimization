System.Web.StaticOptimization
=============================

A .NET MSBuild tasks package contains utilities for JS/CSS/HTML minification, HTML preprocessing

Quick start for ASP.NET MVC developers
======================================

Manual for developers who use cshtml, i.e. not static HTML. 
If you have no plans to use static HTML you can look for [alternative](http://yuicompressor.codeplex.com/), but IMHO the definition of the package is less convienment that [WebGrease] (http://webgrease.codeplex.com/wikipage?title=WebGrease%20configuration%20file)

1. Install [System.Web.StaticOptimization package](https://www.nuget.org/packages/System.Web.StaticOptimization/) from Nuget (if you have probs, here is the [help](https://nuget.org/packages/NLapack/1.0.14/Download)).
2. Add reference to System.Web.StaticOptimization.dll, System.Web.StaticOptimization.Mvc.dll (will be automated later in special NuGet package for ASP.NET MVC developers)
2. Add following initialization code in Global.asax.cs:


```csharp
        protected void Application_Start()
        {
            ...
#if DEBUG
            BundleTable.Init(System.Web.HttpContext.Current.Server.MapPath("~/bundles.config"));
#endif
        }
```
3. Edit bundles.config file by describing your JS and CSS bundles

How it works
============



Plans
=====
1. Prepare demo for SPA (using HTML preprocessing engine)
2. Automate Global.asax generation
