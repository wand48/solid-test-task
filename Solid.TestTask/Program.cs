using Solid.TestTask.Common;
using Solid.TestTask.Helpers;
using Solid.TestTask.Registration;
using Solid.TestTask.Features.Crb;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using Solid.TestTask.Models;
using Solid.TestTask.Features.Currency;
using Solid.TestTask.Features.Rate;

namespace Solid.TestTask
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ProviderRegistration.Register();
            ConfigurationRegistration.LoadConfiguration();

            Dictionary<string, Func<string>> commands = new Dictionary<string, Func<string>>
            {
                { KnownCommands.Help, () => Messages.GetHelp() }
            };

            Messages.WriteStartMessages();

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

                if (input == KnownCommands.Exit)
                {
                    isRunning = false;
                    Messages.WriteExitMessage();
                }
                else if (commands.ContainsKey(input))
                {
                    Console.WriteLine(commands[input]());
                }
                else
                {
                    ImportCbrQuotation(input);
                }
            }
        }

        public static void ImportCbrQuotation(string input)
        {
            ICbrService cbrService = new CbrService();
            ICurrencyService currencyService = new CurrencyService();
            IRateService rateService = new RateService();

            try
            {
                DateOnly date = Date.GetDate(input);

                ValCurs valCurs = cbrService.GetQuotations(date);

                foreach (var valute in valCurs.Valute)
                {
                    Currency currency = currencyService.ImportCurrency(valute);

                    RateBase rate = new(currency.CurrencyId, date, valute.Nominal, valute.Value);

                    rateService.ImportRate(rate);
                }

                rateService.ExportCrossRates(date);

                Console.WriteLine($"\nИмпорт котировки валют с сайта ЦБ РФ выполнен.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Ошибка при валидации данных: {ex.Message}");
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine($"Некорректный URI: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при выполнении приложения: {ex.Message}");
            }
        }
    }
}