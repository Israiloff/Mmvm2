namespace Israiloff.Mmvm.Net.Container.Attributes
{
    public class Qualifier : ParameterRegistrationAttribute
    {
        public Qualifier(string name)
        {
            Named = name;
        }
    }
}