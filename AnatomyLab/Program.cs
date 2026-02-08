using System;

Console.Write("Введите ваше имя: ");
string? name = Console.ReadLine();

Console.Write("Введите ваш возраст: ");
string? ageInput = Console.ReadLine();

if (int.TryParse(ageInput, out int age))
{
    Console.WriteLine($"Привет, {name}! Через 10 лет тебе будет {age + 10}.");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Ошибка: введено не числовое значение для возраста.");
    Console.ResetColor();
}