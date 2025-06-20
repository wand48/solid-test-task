namespace Solid.TestTask.Common
{
    public static class Messages
    {
        public const string Welcome = "Добро пожаловать в консольное приложение Solid.TestTask!";
        public const string ForHelp = $"Введите '{KnownCommands.Help}' для получения для получения справки о приложении и списка доступных команд.";
        public const string ForExit = $"Для выхода введите '{KnownCommands.Exit}'.";
        public const string ForDataGetting = "Для получения котировки валют с сайта ЦБ РФ введите дату. Формат даты ДД.ММ.ГГГГ.";
        public const string ForDataGettingWithEmptyInput = "В случае отсутствия введенной даты будет использована текущая дата.";
        public const string Exit = "Завершение работы приложения...";

        public static void WriteStartMessages()
        {
            Console.WriteLine($"{Welcome}\n{ForHelp}\n{ForExit}\n\n{ForDataGetting}\n{ForDataGettingWithEmptyInput}");
        }

        public static void WriteExitMessage()
        {
            Console.WriteLine(Exit);
        }

        public static string GetHelp()
        {
            return
                $"{Messages.ForDataGetting}\n" +
                $"{Messages.ForDataGettingWithEmptyInput}\n\n" +
                $"Доступные команды:\n- {KnownCommands.Help}: показать список команд\n- {KnownCommands.Exit}: выход из приложения.";
        }
    }
}
