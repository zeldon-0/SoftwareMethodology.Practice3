using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefault(t => t.Name == name);
        }

        public IReadOnlyCollection<Type> GetAllTypes()
        {
            return _exampleProjectAssembly
                .GetTypes()
                .Where(c => c.BaseType?.Name != "Attribute") // Excluding the three default attribute types
                .ToList();
        }


        private static Assembly LoadAssembly()
        {
            return Assembly.LoadFile(@"C:\Personal\Uni\SoftwareMethodology\SoftwareMethodology.Practice3\SoftwareMethodology.Practice3.PlaceholderProject\bin\Debug\net6.0\SoftwareMethodology.Practice3.PlaceholderProject.dll");
        }
    }
}
