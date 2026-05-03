namespace Arenar.Options
{
    public interface IOptionsController
    {
        T GetOption<T>() where T : IOption;

        void SetOption<T>(T settingsOption) where T : IOption;
    }
}
