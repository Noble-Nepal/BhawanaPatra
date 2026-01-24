using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
    public class QuilService
    {
        private readonly IJSRuntime _js;

        public QuilService(IJSRuntime js)
        {
            _js = js;
        }
        public async Task InitializeAsync()
        {
            await Task.Delay(200);
            await _js.InvokeVoidAsync("initializeQuill");
        }
        public async Task<string> GetContentAsync()
        {
            return await _js.InvokeAsync<string>("getQuillContent");
        }
        public async Task SetContentAsync(string content)
        {
            await _js.InvokeVoidAsync("setQuillContent", content);
        }
        public async Task ClearContentAsync()
        {
            await _js.InvokeVoidAsync("clearQuillContent");
        }
    }
}
