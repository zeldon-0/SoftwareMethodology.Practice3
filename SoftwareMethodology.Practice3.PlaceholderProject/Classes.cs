namespace SoftwareMethodology.Practice3.PlaceholderProject
{
    public class ClassA
    {
        public virtual int PropertyA1 { get; set; }
        public string PropertyA2 { get; set; }
        private string _propertyA3 { get; set; }

        public virtual string DoStuffA1()
        {
            throw new NotImplementedException();
        }

        public string DoStuffA2()
        {
            throw new NotImplementedException();
        }

        private string DoStuffA3()
        {
            throw new NotImplementedException();
        }
    }

    public class ClassB : ClassA
    {
        public override int PropertyA1 { get; set; }
        protected string PropertyB1 { get; set; }

        public override string DoStuffA1()
        {
            throw new NotImplementedException();
        }

        protected string DoStuffB()
        {
            throw new NotImplementedException();
        }
    }

    public class ClassC : ClassB
    {
        public new int PropertyA2 { get; set; }

        public new string DoStuffA2()
        {
            throw new NotImplementedException();
        }
    }

    public class ClassD : ClassB
    {
        public static int PropertyD1 { get; set; }
        private int _propertyD2 { get; set; }

        public static string DoStuffD1()
        {
            throw new NotImplementedException();
        }

        private string DoStuffD2()
        {
            throw new NotImplementedException();
        }
    }
}