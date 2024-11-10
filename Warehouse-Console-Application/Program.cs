using Autofac;
using System;

public class Program {
    static void Main(string[] args) {
        var builder = new ContainerBuilder();

        builder.RegisterType<ConsoleInputService>().As<IInputService>();
        builder.RegisterType<ConsoleDisplayService>().As<IDisplayService>();

        var container = builder.Build();

        var inputService = container.Resolve<IInputService>();
        var displayService = container.Resolve<IDisplayService>();

        var pallets = inputService.CollectPalletData();

        displayService.DisplayPalletsGroupedByExpiryDate(pallets);

        displayService.DisplayTop3PalletsWithMaxExpiryDate(pallets);

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
