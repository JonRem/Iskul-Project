#pragma checksum "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9f76c0969fa73777958c87cd6863460541733640"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Notifications), @"mvc.1.0.view", @"/Views/Shared/_Notifications.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\_ViewImports.cshtml"
using Iskul;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\_ViewImports.cshtml"
using Iskul_Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
using Iskul_Utility;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9f76c0969fa73777958c87cd6863460541733640", @"/Views/Shared/_Notifications.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d4e9559d4fb0476a1486f8b2e320edb560f09eab", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Notifications : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/dist/jquery.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
 if (TempData[WC.Success] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9f76c0969fa73777958c87cd68634605417336403990", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js\"></script>\r\n    <style>\r\n        #toast-container > .toast-success {\r\n            background-color: #008000;\r\n        }\r\n    </style>\r\n");
            WriteLiteral("    <script type=\"text/javascript\">\r\n        toastr.success(\'");
#nullable restore
#line 15 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
                   Write(TempData[WC.Success]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\');\r\n    </script>\r\n");
#nullable restore
#line 17 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 19 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
 if (TempData[WC.Error] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9f76c0969fa73777958c87cd68634605417336406123", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js\"></script>\r\n    <style>\r\n        #toast-container > .toast-error {\r\n            background-color: #BD362F;\r\n        }\r\n    </style>\r\n");
            WriteLiteral("    <script type=\"text/javascript\">\r\n        toastr.error(\'");
#nullable restore
#line 31 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
                 Write(TempData[WC.Error]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\');\r\n    </script>\r\n");
#nullable restore
#line 33 "C:\Users\jrem7_000\Documents\Dotnet-Core-MVC\Iskul Project\Iskul\Views\Shared\_Notifications.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
