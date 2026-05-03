namespace Arenar.AudioSystem {
    /// <summary>
    /// Ключи фоновых треков в <see cref="AmbientLibrary"/> (asset <c>Audio Library</c>).
    /// Добавляйте сюда новые значения для магнитофона/уровней и прописывайте к ним <see cref="UnityEngine.AudioClip"/> в инспекторе библиотеки.
    /// </summary>
    public enum AmbientType : byte {
        None = 0,
        MainMenu = 1,
        Gameplay_1_Default_1 = 2,
        Gameplay_1_Default_2 = 3,
        Gameplay_1_Cook_1 = 4,
        Gameplay_1_Cook_2 = 5,
    }
}
