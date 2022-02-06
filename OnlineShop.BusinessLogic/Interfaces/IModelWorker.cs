using OnlineShop.BusinessLogic.Models;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic.Interfaces
{
    public interface IModelWorker<TModel>
    {
        IEnumerable<TModel> GetModels(int pageNumber, int totalSize, out int comonEntityCount);

        TModel GetModel(int displayedId, int modelId);

        void EditModel(TModel model);

        void DeleteModel(int id);

        IEnumerable<TModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel);
    }
}
