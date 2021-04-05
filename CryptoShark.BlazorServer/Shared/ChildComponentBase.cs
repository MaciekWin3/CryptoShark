using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoShark.BlazorServer.Shared
{
    public class ChildComponentBase : ComponentBase
    {

        protected bool DarkThemeOn;
        protected string AlertTheme => DarkThemeOn ? "dark" : "light";
        [Parameter]
        public string AlertText { get; set; }

        protected override void OnInitialized()
        {
            DarkThemeOn = true;
        }
    }
}
