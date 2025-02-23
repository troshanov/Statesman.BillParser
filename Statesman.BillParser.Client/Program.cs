using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Statesman.BillParser.Client;
using Statesman.BillParser.Client.Services;
using Statesman.BillParser.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(@"https://localhost:7059") });

builder.Services.AddScoped<IBillsService, BillsService>();
builder.Services.AddMudServices();


await builder.Build().RunAsync();
