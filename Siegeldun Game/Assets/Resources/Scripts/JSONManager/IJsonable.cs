public interface IJsonable
{
    public string componentName { get; }
    public JsonData BasePropToBasePropJD();
    public void SetBaseProp(string baseProp);
}
