using System.Reflection;

namespace SoftwareMethodology.Practice3.MetricsCalculator.Core
{
    public class LibraryLoader
    {
        private readonly Assembly _exampleProjectAssembly;

        public LibraryLoader()
        {
            _exampleProjectAssembly = LoadAssembly();
        }

        public Type? GetTypeByName(string name)
        {
            return _exampleProjectAssembly
                .GetTypes()
                .FirstOrDefault(type => type.Name == name);
        }

        public IReadOnlyCollection<Type> GetAllTypes()
        {
            return _exampleProjectAssembly
                .GetTypes()
                .Where(type => type.BaseType?.Name != "Attribute") // Excluding the three default attribute types
                .ToList();
        }


        private static Assembly LoadAssembly()
        {
            return Assembly.LoadFile(@"C:\Personal\Uni\SoftwareMethodology\SoftwareMethodology.Practice3\SoftwareMethodology.Practice3.PlaceholderProject\bin\Debug\net6.0\SoftwareMethodology.Practice3.PlaceholderProject.dll");
        }
    }
}
