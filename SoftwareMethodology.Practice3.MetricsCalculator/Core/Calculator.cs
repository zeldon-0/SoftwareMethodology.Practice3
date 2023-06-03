using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareMethodology.Practice3.MetricsCalculator.Core
{
    public class Calculator
    {
        private readonly LibraryLoader _libraryLoader;

        public Calculator()
        {
            _libraryLoader = new LibraryLoader();
        }

        public int CalculateClassDepthOfInheritance(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            return CalculateClassDepthOfInheritanceInternal(classType);
        }

        public int CalculateLibraryDepthOfInheritance()
        {
            var allClasses = _libraryLoader
                .GetAllTypes()
                .ToList();

            return allClasses
                    .Select(CalculateClassDepthOfInheritanceInternal)
                    .Max();
        }

        public int CalculateClassNumberOfChildren(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClasses = _libraryLoader.GetAllTypes();

            return allClasses.Count(c =>
                classType.IsAssignableFrom(c) &&
                c.BaseType?.Name == classType.Name);
        }

        public double CalculateClassMethodInheritance(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassMethods = classType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                            !m.IsSpecialName)
                .ToList();
            if (allClassMethods.Count == 0)
                return 0;

            var inheritedMethodsCount = allClassMethods
                .Count(m => m.DeclaringType != classType);

            return inheritedMethodsCount / (double)allClassMethods.Count;
        }

        public double CalculateLibraryMethodInheritance()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesMethodsCount = 0;
            double allInheritedMethodsCount = 0;

            foreach (var classType in allClassTypes)
            {
                var allClassMethods = classType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                !m.IsSpecialName)
                    .ToList();

                var inheritedMethodsCount = allClassMethods
                    .Count(m => m.DeclaringType != classType);

                allClassesMethodsCount += allClassMethods.Count;
                allInheritedMethodsCount += inheritedMethodsCount;
            }

            return allInheritedMethodsCount / allClassesMethodsCount;
        }

        public double CalculateClassMethodHidingFactor(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassMethods = classType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                            !m.IsSpecialName)
                .ToList();

            var visibleMethodsCount = allClassMethods
                .Count(m => m.IsPublic);

            return 1 - visibleMethodsCount / (double)allClassMethods.Count;
        }

        public double CalculateLibraryMethodHidingFactor()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesMethodsCount = 0;
            double allVisibleMethodsCount = 0;

            foreach (var classType in allClassTypes)
            {
                var allClassMethods = classType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                !m.IsSpecialName)
                    .ToList();

                allClassesMethodsCount += allClassMethods.Count;

                var visibleMethodsCount = allClassMethods
                    .Count(m => m.IsPublic);
                allVisibleMethodsCount += visibleMethodsCount;
            }

            return 1 - allVisibleMethodsCount / allClassesMethodsCount;
        }

        public double CalculateClassAttributeInheritance(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassProperties = classType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object
                            !m.IsSpecialName)
                .ToList();
            if (allClassProperties.Count == 0)
                return 0;

            var inheritedPropertiesCount = allClassProperties
                .Count(m => m.DeclaringType != classType);

            return inheritedPropertiesCount / (double)allClassProperties.Count;
        }

        public double CalculateLibraryAttributeInheritance()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesPropertyCount = 0;
            double allInheritedPropertyCount = 0;

            foreach (var classType in allClassTypes)
            {
                var allClassProperties = classType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                !m.IsSpecialName)
                    .ToList();

                var inheritedPropertyCount = allClassProperties
                    .Count(m => m.DeclaringType != classType);

                allClassesPropertyCount += allClassProperties.Count;
                allInheritedPropertyCount += inheritedPropertyCount;
            }

            return allInheritedPropertyCount / allClassesPropertyCount;
        }

        public double CalculateClassPropertyHidingFactor(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassProperties = classType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                            !m.IsSpecialName)
                .ToList();

            var visiblePropertiesCount = 0;
            foreach (var property in allClassProperties)
            {
                var propertyGetterIsPublic = property.GetMethod?.IsPublic ?? false;
                var propertySetterIsPublic = property.SetMethod?.IsPublic ?? false;

                // Assuming that eother the setter or getter being public is enough for public visibility
                if (propertyGetterIsPublic || propertySetterIsPublic)
                    visiblePropertiesCount++;
            }

            return 1 - visiblePropertiesCount / (double)allClassProperties.Count;
        }


        public double CalculateLibraryPropertyHidingFactor()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesPropertiesCount = 0;
            double allVisiblePropertiesCount = 0;

            foreach (var classType in allClassTypes)
            {
                var allClassProperties = classType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                !m.IsSpecialName)
                    .ToList();

                allClassesPropertiesCount += allClassProperties.Count;
                foreach (var property in allClassProperties)
                {
                    var propertyGetterIsPublic = property.GetMethod?.IsPublic ?? false;
                    var propertySetterIsPublic = property.SetMethod?.IsPublic ?? false;

                    // Assuming that eother the setter or getter being public is enough for public visibility
                    if (propertyGetterIsPublic || propertySetterIsPublic)
                        allVisiblePropertiesCount++;
                }

            }

            return 1 - allVisiblePropertiesCount / allClassesPropertiesCount;
        }

        public double CalculateClassPolymorphismFactor(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassMethods = classType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                            !m.IsSpecialName &&
                            (m.IsPublic || (!m.IsPublic && !m.IsPrivate))) //Forcing to only retrieve the overridable methods - public and protected
                .ToList();

            double newMethodCount = 0;
            double overridenMethodCount = 0;
            foreach (var method in allClassMethods)
            {
                var methodBaseDefinition = method.GetBaseDefinition();

                if (methodBaseDefinition != method && method.DeclaringType == classType)
                    overridenMethodCount++;
                else if (method.DeclaringType == classType)
                    newMethodCount++;
            }

            var allClassTypes = _libraryLoader.GetAllTypes();
            var classChildrenCount = CalculateDescendantClassesCount(classType, allClassTypes);
            var maxNumberOfOverrides = newMethodCount * classChildrenCount;

            return maxNumberOfOverrides == 0 ? 0 : overridenMethodCount / (newMethodCount * classChildrenCount);
        }

        public double CalculateLibraryPolymorphismFactor()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double overridenMethodCount = 0;
            double maxNumberOfOverrides = 0;
            foreach (var classType in allClassTypes)
            {
                var allClassMethods = classType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                !m.IsSpecialName &&
                                (m.IsPublic || (!m.IsPublic && !m.IsPrivate))) //Forcing to only retrieve the overridable methods - public and protected
                        .ToList();
                double newMethodCount = 0;
                foreach (var method in allClassMethods)
                {
                    var methodBaseDefinition = method.GetBaseDefinition();

                    if (methodBaseDefinition != method && method.DeclaringType == classType)
                        overridenMethodCount++;
                    else if (method.DeclaringType == classType)
                        newMethodCount++;
                }
                var classChildrenCount = CalculateDescendantClassesCount(classType, allClassTypes);
                maxNumberOfOverrides += newMethodCount * classChildrenCount;
            }

            return maxNumberOfOverrides == 0 ? 0 : overridenMethodCount / maxNumberOfOverrides;
        }

        private int CalculateClassDepthOfInheritanceInternal(Type classType)
        {
            // Assuming that a base class has depth of 1, and not 0
            var depthOfInheritance = 1;
            var currentClass = classType;

            while (currentClass.BaseType.Name != "Object")
            {
                depthOfInheritance++;
                currentClass = currentClass.BaseType;
            }

            return depthOfInheritance;
        }

        private int CalculateDescendantClassesCount(Type classType, IReadOnlyCollection<Type> allClasses) =>
            allClasses.Count(c =>
                classType.IsAssignableFrom(c) &&
                c.Name != classType.Name);
    }
}
