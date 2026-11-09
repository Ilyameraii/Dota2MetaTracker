using System.Collections.Concurrent;
using System.Reflection;
using ImageResources;

namespace Images;

/// <summary>
///     Провайдер иконок рангов и ролей, встроенных в сборку ImageResources как EmbeddedResource.
/// </summary>
public static class ImageResourceProvider
{
    private const string ResourcesProjectName = nameof(ImageResources);
    
    // Явно указываем сборку FormattingResources через маркерный тип из того проекта.
    private static readonly Assembly ResourceAssembly = typeof(ResourceAssemblyMarker).Assembly;

    private static readonly ConcurrentDictionary<string, byte[]> Cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Возвращает байты PNG-иконки ранга или null, если ресурс не найден.
    ///     Имя файла передаётся без расширения, в нижнем регистре, например "herald".
    /// </summary>
    public static byte[]? GetRankIcon(string fileName)
        => GetResource($"{ResourcesProjectName}.Ranks.{fileName}.png");

    /// <summary>
    ///     Возвращает байты PNG-иконки роли или null, если ресурс не найден.
    ///     Имя файла передаётся без расширения, в нижнем регистре, например "hard_support".
    /// </summary>
    public static byte[]? GetRoleIcon(string fileName)
        => GetResource($"{ResourcesProjectName}.Roles.{fileName}.png");

    /// <summary>
    ///     Возвращает байты PNG-иконки героя или null, если ресурс не найден.
    ///     Имя файла — urlName героя, например "antimage", "shadow_fiend".
    /// </summary>
    public static byte[]? GetHeroIcon(string fileName)
        => GetResource($"{ResourcesProjectName}.Heroes.{fileName}.png");
    
    /// <summary>
    ///     Возвращает список всех встроенных имён ресурсов из FormattingResources.dll.
    /// </summary>
    public static IEnumerable<string> ListAll()
        => ResourceAssembly.GetManifestResourceNames();
    
    private static byte[]? GetResource(string resourceName)                                                                                                                          
     {                                                                                                                                                                                
         if (Cache.TryGetValue(resourceName, out var cached))                                                                                                                         
             return cached;                                                                                                                                                           
                                                                                                                                                                                      
         using var stream = ResourceAssembly.GetManifestResourceStream(resourceName);                                                                                                 
         if (stream == null)                                                                                                                                                          
             return null;                                                                                                                                                             
                                                                                                                                                                                      
         var bytes = new byte[stream.Length];                                                                                                                                         
         stream.ReadExactly(bytes);                                                                                                                                                   
                                                                                                                                                                                      
         Cache[resourceName] = bytes;                                                                                                                                                 
         return bytes;                                                                                                                                                                
     }    
}