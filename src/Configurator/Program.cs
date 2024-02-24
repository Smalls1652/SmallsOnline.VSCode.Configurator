using System.CommandLine;
using SmallsOnline.VSCode.Configurator;

CliConfiguration cliConfig = new(new RootCommand());

return await cliConfig.InvokeAsync(args);
