#pragma checksum "E:\Faculta\IP\Proiect Smart View Access\Proiect-Smart-View-Access\dotnetnewmvc\Views\Home\Administrator_Menu_1.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "14cae72473bcc2a56c07e28d46956687b10a88fc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Administrator_Menu_1), @"mvc.1.0.view", @"/Views/Home/Administrator_Menu_1.cshtml")]
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
#line 1 "E:\Faculta\IP\Proiect Smart View Access\Proiect-Smart-View-Access\dotnetnewmvc\Views\_ViewImports.cshtml"
using dotnetnewmvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Faculta\IP\Proiect Smart View Access\Proiect-Smart-View-Access\dotnetnewmvc\Views\_ViewImports.cshtml"
using dotnetnewmvc.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"14cae72473bcc2a56c07e28d46956687b10a88fc", @"/Views/Home/Administrator_Menu_1.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed3f9eba2eba42d9cad24342d56cbb9b6a2015f9", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Administrator_Menu_1 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "14cae72473bcc2a56c07e28d46956687b10a88fc3369", async() => {
                WriteLiteral(@"


    <style>
        th {
            text-align: center;
        }

        body {
            background-color: #eeeeee;
            padding: 10px;
            margin: 10px;
        }

        .scrollClass {
            width: 100%;
            height: 300px;
            overflow-y: auto;
        }

        .form-select {
            width: 50%;
            height: 30px;
            background-color: rgba(255,255,255,0.1)
        }

        .form-control {
        }
    </style>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "14cae72473bcc2a56c07e28d46956687b10a88fc4849", async() => {
                WriteLiteral(@"
    <div class=""container"" style=""margin:0px;padding:0px;width:100%; top:10%;"">
        <div class=""col-sm-3"">
            <img src=""avatar1.png"" width=""200px"" height=""200px"" style=""display:block;"">
            <div style=""padding-top:15%"">
                <label>De la :</label>
                <input type=""date"" name=""1"" style=""float:right"">
                <br>
                <br>
                <br>
                <label>
                    Pana la:
                </label>
                <input type=""date"" name=""2"" style=""float:right"">
            </div>
        </div>

        <div class=""col-sm-6"" style="" margin-top:5%"">

            <h1 style=""text-align: center;"">Statistica prezentelor in firme </h1>
            <hr>
            <br>
            <div class=""col-sm-4"" style=""float:left"">

                <h4>Numar total de ore: 70</h4>
            </div>
            <div class=""col-sm-4"">

            </div>
            <div class=""col-sm-4"" style=""text-align:right"">");
                WriteLiteral(@"
                <input type=""text"" class=""form-control"" placeholder=""Nume angajat"">
            </div>
            <div class=""scrollClass"">
                <table class=""table"" style=""text-align:center;"">
                    <thead>
                        <tr>

                            <th scope=""col"">Intrare</th>
                            <th scope=""col"">Iesire</th>
                            <th scope=""col"">Numar de ore</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                        <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
        ");
                WriteLiteral(@"                <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                        <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                        <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                        <tr>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                        <tr>
                            <td>12/3/2021 8:20:00 ");
                WriteLiteral(@"AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                            <td>12/3/2021 8:20:00 AM</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class=""col-sm-3"">
            <img src=""logo.png"" width=""200px"" height=""200px"" style=""float:right"">
        </div>
    </div>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n</html>");
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
