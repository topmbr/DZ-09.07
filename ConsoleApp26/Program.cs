using System.Text.Json;
class Program
{
    static List<Operation> history = new List<Operation>();

    static void Main(string[] args)
    {
        Console.WriteLine("Калькулятор");
        Console.WriteLine("Для сохранения истории операций введите 'history', для выхода введите 'exit'");

        while (true)
        {
            Console.Write("Введите выражение: ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            if (input.ToLower() == "history")
            {
                PrintHistory();
                continue;
            }

            try
            {
                double result = Calculate(input);
                Console.WriteLine($"Результат: {result}");

                // Сохранение операции в историю
                Operation operation = new Operation(input, result);
                history.Add(operation);
                SaveHistory();

            }
            catch
            {
                Console.WriteLine("Ошибка при выполнении операции");
            }
        }
    }

    static double Calculate(string input)
    {
        string[] parts = input.Split(' ');
        if (parts.Length != 3)
            throw new ArgumentException("Неверный формат ввода.");

        double num1 = double.Parse(parts[0]);
        double num2 = double.Parse(parts[2]);
        string op = parts[1];

        double result = 0;

        switch (op)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "*":
                result = num1 * num2;
                break;
            case "/":
                result = num1 / num2;
                break;
            default:
                throw new ArgumentException("Неверная операция.");
        }

        return result;
    }

    static void PrintHistory()
    {
        if (history.Count == 0)
        {
            Console.WriteLine("История операций пуста.");
            return;
        }

        Console.WriteLine("История операций:");

        foreach (var operation in history)
        {
            Console.WriteLine($"{operation.Expression} = {operation.Result}");
        }
    }

    static void SaveHistory()
    {
        using (StreamWriter file = new StreamWriter("history.json"))
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(history, options);
            file.Write(json);
        }
    }
    class Operation
    {
        public string Expression { get; set; }
        public double Result { get; set; }

        public Operation(string expression, double result)
        {
            Expression = expression;
            Result = result;
        }
    }
}
