System.Web.StaticOptimization
=============================

A .NET MSBuild tasks package contains utilities for JS/CSS/HTML minification, HTML preprocessing

Quick start
===========
1. Install [System.Web.StaticOptimization package](https://www.nuget.org/packages/System.Web.StaticOptimization/) from Nuget (if you have probs, here is the [help](https://nuget.org/packages/NLapack/1.0.14/Download)).
2. Add following initialization code in Global.asax.cs (will be automated later):
```csharp
        protected void Application_Start()
        {
            ...
#if DEBUG
            BundleTable.Init(System.Web.HttpContext.Current.Server.MapPath("~/bundles.config"));
#endif
        }
```
3. Edit bundles.config fileby describing your JS and CSS bundles


Plans
=====
1. Prepare demo for SPA (using HTML preprocessing engine)
2. Automate Global.asax generation
