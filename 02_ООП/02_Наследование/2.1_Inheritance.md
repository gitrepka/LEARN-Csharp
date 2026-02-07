# Тема 2.2: Наследование

## Что ты узнаешь
- Базовый и производный класс
- virtual, override, new, sealed, abstract
- Ковариантные возвращаемые типы
- Класс object и переопределение ToString/Equals/GetHashCode

---

## Объяснение

### ЗАЧЕМ?
В игре есть Zombie, Skeleton, Boss — все они враги. У всех есть Health, TakeDamage(), Die(). Копировать этот код в каждый класс? Нет! Создаём базовый класс `BaseEnemy` и НАСЛЕДУЕМ от него.

**Аналогия:** Наследование — это "яблоко от яблони". Dog наследует от Animal. Все свойства Animal (Name, Age) автоматически есть у Dog + свои (Breed).

### Базовый синтаксис

```csharp
// Базовый класс
class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public virtual string Speak() => "..."; // virtual = можно переопределить
}

// Производный класс
class Dog : Animal // Dog наследует от Animal
{
    public string Breed { get; set; }

    // Вызываем конструктор базового класса через base
    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
    }

    // override = переопределяем поведение
    public override string Speak() => "Гав!";
}

class Cat : Animal
{
    public Cat(string name, int age) : base(name, age) { }
    public override string Speak() => "Мяу!";
}

// Использование:
Dog dog = new Dog("Рекс", 3, "Овчарка");
Console.WriteLine(dog.Speak()); // "Гав!"
Console.WriteLine(dog.Name);   // "Рекс" — унаследовано от Animal
```

### virtual / override / new / sealed

```csharp
class Base
{
    public virtual void Method() => Console.WriteLine("Base");
}

class Derived : Base
{
    public override void Method() => Console.WriteLine("Derived"); // ПЕРЕОПРЕДЕЛЯЕТ
}

class Hidden : Base
{
    public new void Method() => Console.WriteLine("Hidden"); // СКРЫВАЕТ (НЕ переопределяет!)
}

Base obj1 = new Derived();
obj1.Method(); // "Derived" — полиморфизм работает!

Base obj2 = new Hidden();
obj2.Method(); // "Base" — new НЕ работает через базовый тип!

// sealed — запрет дальнейшего переопределения
class Final : Derived
{
    public sealed override void Method() => Console.WriteLine("Final");
}
```

### Абстрактные классы

```csharp
// abstract — нельзя создать экземпляр, ОБЯЗЫВАЕТ наследников реализовать методы
abstract class Shape
{
    public abstract double CalculateArea();     // нет реализации!
    public abstract double CalculatePerimeter();

    // Обычные методы тоже можно:
    public void PrintInfo()
        => Console.WriteLine($"Площадь: {CalculateArea():F2}");
}

class Circle : Shape
{
    public double Radius { get; init; }

    public override double CalculateArea() => Math.PI * Radius * Radius;
    public override double CalculatePerimeter() => 2 * Math.PI * Radius;
}

// Shape shape = new Shape(); // ❌ Нельзя создать абстрактный класс!
Shape circle = new Circle { Radius = 5 };
circle.PrintInfo(); // "Площадь: 78.54"
```

### Ковариантные возвращаемые типы (C# 9)

```csharp
class Animal
{
    public virtual Animal Clone() => new Animal();
}

class Dog : Animal
{
    // override возвращает более конкретный тип!
    public override Dog Clone() => new Dog();
}

Dog original = new Dog();
Dog copy = original.Clone(); // Возвращает Dog, не Animal!
```

### Переопределение object

Все классы наследуют от `object`. Полезно переопределить:

```csharp
class Player
{
    public string Name { get; set; }
    public int Level { get; set; }

    public override string ToString()
        => $"Player({Name}, Level {Level})";

    public override bool Equals(object? obj)
        => obj is Player other && Name == other.Name;

    public override int GetHashCode()
        => Name.GetHashCode();
}

var p = new Player { Name = "Алиса", Level = 5 };
Console.WriteLine(p);  // "Player(Алиса, Level 5)" — вместо "Namespace.Player"
```

---

## Мини-упражнения
1. **⭐** Создай иерархию `Animal` → `Dog`, `Cat` с `Speak()`.
2. **⭐** Покажи разницу между `override` и `new` на примере.
3. **⭐** Используй covariant return: `Clone()` базовый → `Animal`, в Dog → `Dog`.
4. **⭐** Переопредели `ToString()` для красивого вывода.

---

## Практика Uno ⭐⭐⭐
**"Графический редактор"** — `abstract Shape` → `Circle`, `Rectangle`, `Triangle`.

## Практика Godot ⭐⭐⭐
**Иерархия врагов** — `abstract BaseEnemy` → `Zombie`, `Skeleton`, `Boss`. Виртуальные Move/Attack/TakeDamage/Die.

---

## Контрольные вопросы
1. Чем `override` отличается от `new`?
2. Можно ли создать экземпляр абстрактного класса?
3. Зачем нужен `sealed` в override?
4. Что делает `base()`?
5. Зачем переопределять `ToString()`?

## Что дальше
Дальше — **полиморфизм** (2.3): как работать с разными типами через один интерфейс.
