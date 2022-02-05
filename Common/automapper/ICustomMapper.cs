namespace plannerBackEnd.Common.automapper
{

    // In case complex mapping is required through this option you
    // can create custom mapping rules
    public interface ICustomMapping
    {
        void CreateMappings(AutomapperProfile profile);
    }

    public interface IMaps<T> { } // Class that implements it declares FROM which object will be mapped
}
