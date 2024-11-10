using Autofac;
using System;

public class Program {
    static void Main(string[] args) {
        // Настройка DI контейнера
        var builder = new ContainerBuilder();

        // Регистрируем сервисы
        builder.RegisterType<ConsoleInputService>().As<IInputService>();
        builder.RegisterType<ConsoleDisplayService>().As<IDisplayService>();

        // Строим контейнер
        var container = builder.Build();

        // Разрешаем зависимости
        var inputService = container.Resolve<IInputService>();
        var displayService = container.Resolve<IDisplayService>();

        // Собираем данные о паллетах
        var pallets = inputService.CollectPalletData();

        // Отображаем сгруппированные паллеты по сроку годности
        displayService.DisplayPalletsGroupedByExpiryDate(pallets);

        // Отображаем топ-3 паллеты с наибольшим сроком годности
        displayService.DisplayTop3PalletsWithMaxExpiryDate(pallets);

        // Ожидание ввода для предотвращения мгновенного закрытия консоли
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
