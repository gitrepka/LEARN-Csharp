var person = new { Name = "Алиса", Age = 25, IsAlive = true };

Console.WriteLine(person.Name);  // "Алиса"
Console.WriteLine(person.Age);   // 25

// person.Name = "Боб"; // ❌ Свойства readonly!