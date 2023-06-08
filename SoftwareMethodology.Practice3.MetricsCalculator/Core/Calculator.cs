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
        private const string BackingFieldNameSuffix = "_BackingField";
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
                .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName) //Ignoring methods inherited from object & getters/setters
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
                    .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName)//Ignoring methods inherited from object & getters/setters
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
                .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName) //Ignoring methods inherited from object & getters/setters
                .ToList();

            var visibleMethodsCount = allClassMethods
                .Count(method => method.IsPublic);

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
                    .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName)//Ignoring methods inherited from object & getters/setters
                    .ToList();

                allClassesMethodsCount += allClassMethods.Count;

                var visibleMethodsCount = allClassMethods
                    .Count(method => method.IsPublic);
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
                .Where(property => property.DeclaringType != typeof(object) && !property.IsSpecialName)//Ignoring methods inherited from object
                .ToList();

            if (allClassProperties.Count == 0)
                return 0;

            var inheritedPropertiesCount = allClassProperties
                .Count(property => property.DeclaringType != classType);

            var allClassFields = classType
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(field => !field.Name.EndsWith(BackingFieldNameSuffix))
                .ToList();

            if (allClassFields.Count == 0)
                return 0;

            var inheritedFieldsCount = allClassFields
                .Count(field => field.DeclaringType != classType);

            return (inheritedPropertiesCount + inheritedFieldsCount) / (double)(allClassProperties.Count + allClassFields.Count);

        }

        public double CalculateLibraryAttributeInheritance()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesAttributeCount = 0;
            double allInheritedAttributeCount = 0;
            
            foreach (var classType in allClassTypes)
            {
                var allClassProperties = classType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(property => property.DeclaringType != typeof(object) &&  //Ignoring methods inherited from object & getters/setters
                                       !property.IsSpecialName)
                    .ToList();

                var inheritedPropertyCount = allClassProperties
                    .Count(property => property.DeclaringType != classType);

                allClassesAttributeCount += allClassProperties.Count;
                allInheritedAttributeCount += inheritedPropertyCount; 
                
                var allClassFields = classType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(field => !field.Name.EndsWith(BackingFieldNameSuffix))
                    .ToList();

                var inheritedFieldsCount = allClassFields
                    .Count(field => field.DeclaringType != classType);

                allClassesAttributeCount += allClassFields.Count;
                allInheritedAttributeCount += inheritedFieldsCount;
            }

            return allInheritedAttributeCount / allClassesAttributeCount;
        }

        public double CalculateClassAttributeHidingFactor(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassDefinedProperties = classType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(property => !property.IsSpecialName && property.DeclaringType == classType)
                .ToList();

            var hiddenAttributesCount = 0;
            foreach (var property in allClassDefinedProperties)
            {
                var propertyGetterIsPublic = property.GetMethod?.IsPublic ?? false;
                var propertySetterIsPublic = property.SetMethod?.IsPublic ?? false;

                // Assuming that either the setter or getter being public is enough for public visibility
                if (!propertyGetterIsPublic && !propertySetterIsPublic)
                    hiddenAttributesCount++;
            }

            var allClassDefinedFields = classType
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(field => !field.Name.EndsWith(BackingFieldNameSuffix) && field.DeclaringType == classType)
                .ToList();

            var hiddenFieldsCount = allClassDefinedFields
                .Count(field => !field.IsPublic);
            hiddenAttributesCount += hiddenFieldsCount;

            var allAttributesCount = allClassDefinedProperties.Count + allClassDefinedFields.Count;

            return hiddenAttributesCount / (double)allAttributesCount;
        }


        public double CalculateLibraryAttributeHidingFactor()
        {
            var allClassTypes = _libraryLoader.GetAllTypes();

            double allClassesAttributesCount = 0;
            double allHiddenAttributesCount = 0;

            foreach (var classType in allClassTypes)
            {
                var allClassDefinedProperties = classType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(property => !property.IsSpecialName && property.DeclaringType == classType)
                    .ToList();

                allClassesAttributesCount += allClassDefinedProperties.Count;
                foreach (var property in allClassDefinedProperties)
                {
                    var propertyGetterIsPublic = property.GetMethod?.IsPublic ?? false;
                    var propertySetterIsPublic = property.SetMethod?.IsPublic ?? false;

                    // Assuming that either the setter or getter being public is enough for public visibility
                    if (!propertyGetterIsPublic && !propertySetterIsPublic)
                        allHiddenAttributesCount++;
                }

                var allClassDefinedFields = classType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(field => !field.Name.EndsWith(BackingFieldNameSuffix) && field.DeclaringType == classType)
                    .ToList();

                allClassesAttributesCount += allClassDefinedFields.Count;

                var hiddenFieldsCount = allClassDefinedFields
                    .Count(field => !field.IsPublic);
                allHiddenAttributesCount += hiddenFieldsCount;
            }

            return allHiddenAttributesCount / allClassesAttributesCount;
        }

        public double CalculateClassPolymorphismFactor(string className)
        {
            var classType = _libraryLoader.GetTypeByName(className);
            if (classType == null)
                throw new Exception("No such class in the library!");

            var allClassMethods = classType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName && //Ignoring methods inherited from object & getters/setters
                                 (method.IsPublic || (!method.IsPublic && !method.IsPrivate))) //Forcing to only retrieve the overridable methods - public and protected
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
                    .Where(method => method.DeclaringType != typeof(object) && !method.IsSpecialName && //Ignoring methods inherited from object & getters/setters
                                     (method.IsPublic || (!method.IsPublic && !method.IsPrivate))) //Forcing to only retrieve the overridable methods - public and protected
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

            while (currentClass.BaseType!.Name != nameof(Object))
            {
                depthOfInheritance++;
                currentClass = currentClass.BaseType;
            }

            return depthOfInheritance;
        }

        private int CalculateDescendantClassesCount(Type classType, IReadOnlyCollection<Type> allClasses) =>
            allClasses.Count(type =>
                classType.IsAssignableFrom(type) &&
                type.Name != classType.Name);
    }
}
