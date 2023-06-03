using SoftwareMethodology.Practice3.MetricsCalculator.Core;

var calculator = new Calculator();

Console.WriteLine("Got the PlaceHolder projects' code metrics:");
Console.WriteLine($"The project Depth of Inheritance is:{calculator.CalculateLibraryDepthOfInheritance()}");
Console.WriteLine($"The project Method Inheritance Factor is:{calculator.CalculateLibraryMethodInheritance()}");
Console.WriteLine($"The project Method Hiding Factor is:{calculator.CalculateLibraryMethodHidingFactor()}");
Console.WriteLine($"The project Attribute Inheritance Factor is:{calculator.CalculateLibraryAttributeInheritance()}");
Console.WriteLine($"The project Attribute Hiding Factor is:{calculator.CalculateLibraryPropertyHidingFactor()}");
Console.WriteLine($"The project Polymorphism Factor is:{calculator.CalculateLibraryPolymorphismFactor()}");
Console.WriteLine("You may also choose to view the following class-specific metrics: DOI, NOC, MIF, MHF, AHF, AIF and POF.");
while (true)
{
    Console.WriteLine("Please, enter the metric name you wish to view. Otherwise, press `Enter` to exit the app.");
    var methodInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(methodInput))
        break;
    Console.WriteLine("Please, enter the class name you wish to view hte metric for.");
    var className = Console.ReadLine();

    switch (methodInput.Trim().ToLower())
    {
        case "doi":
            Console.WriteLine($"The Depth of Inheritance for {className} is:{calculator.CalculateClassDepthOfInheritance(className)}");
            break;
        case "noc":
            Console.WriteLine($"The Number of Children for {className} is:{calculator.CalculateClassNumberOfChildren(className)}");
            break;
        case "mif":
            Console.WriteLine($"The Method Inheritance Factor for {className} is:{calculator.CalculateClassMethodInheritance(className)}");
            break;
        case "mhf":
            Console.WriteLine($"The Method Hiding Factor for {className} is:{calculator.CalculateClassMethodHidingFactor(className)}");
            break;
        case "ahf":
            Console.WriteLine($"The Attribute Inheritance Factor for {className} is:{calculator.CalculateClassAttributeInheritance(className)}");
            break;
        case "aif":
            Console.WriteLine($"The Attribute Hiding Factor for {className} is:{calculator.CalculateClassPropertyHidingFactor(className)}");
            break;
        case "pof":
            Console.WriteLine($"The Polymorphism Factor for {className} is:{calculator.CalculateClassPolymorphismFactor(className)}");
            break;

        default:
            Console.WriteLine($"Invalid metric name. Please try again.");
            break;
    }
}
