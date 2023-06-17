namespace ProjectMemphis.StoryEditor
{
    public interface IServiceCollection
    {
        T GetService<T>() where T : class;
        void AddService<T,O>(O o) where T : class where O : class, T;
        void AddService<T>() where T : class, new();
    }
}
