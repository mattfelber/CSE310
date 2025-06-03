using System;

namespace TaskerPro.Models
{
    // Abstract base class - think of it as a template that other classes must follow
    public abstract class Animal
    {
        // Properties that all animals will have
        public string Name { get; set; } = "";
        public int Age { get; set; }

        // Abstract method - all animals must make a sound, but each does it differently
        public abstract string MakeSound();

        // Regular method - all animals can sleep the same way
        public string Sleep()
        {
            return $"{Name} is sleeping... Zzz";
        }
    }

    // Dog inherits from Animal (extends Animal)
    public class Dog : Animal
    {
        // Dog must implement MakeSound() because it's abstract in Animal
        public override string MakeSound()
        {
            return $"{Name} says: Woof!";
        }

        // Dog has its own special method
        public string WagTail()
        {
            return $"{Name} is wagging its tail happily!";
        }
    }

    // Cat also inherits from Animal
    public class Cat : Animal
    {
        // Cat must implement MakeSound() too, but does it differently
        public override string MakeSound()
        {
            return $"{Name} says: Meow!";
        }

        // Cat has its own special method
        public string Purr()
        {
            return $"{Name} is purring...";
        }
    }

    // Extension methods - add new functionality to existing classes
    public static class AnimalExtensions
    {
        // This adds a new method to ALL Animal objects
        public static string Play(this Animal animal)
        {
            return $"{animal.Name} is playing!";
        }

        // This adds a new method to ALL Dog objects
        public static string Fetch(this Dog dog)
        {
            return $"{dog.Name} is fetching the ball!";
        }
    }
} 