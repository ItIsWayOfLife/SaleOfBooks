using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IConverter<T, K>
       where T : class
       where K : class
    {
        T ConvertDTOByModel(K modelDTO);
        K ConvertModelByDTO(T model);
        IEnumerable<T> ConvertDTOsByModels(IEnumerable<K> modelDTOs);
        IEnumerable<K> ConvertModelsByDTOs(IEnumerable<T> models);
    }
}
