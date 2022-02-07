using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess;
using OnlineShop.DataAccess.Entities;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic
{
    public class ManagerWorker : IModelWorker<ManagerModel>
    {
        private readonly DataBaseUoW _dbUoW;

        public ManagerWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public IEnumerable<ManagerModel> GetModels(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbUoW.Managers.GetCount();
            var managerModels = new List<ManagerModel>();
            var managers = _dbUoW.Managers.GetRange((pageNumber - 1) * totalSize, totalSize);
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var manager in managers)
            {
                managerModels.Add(EntityToModel(manager, displayedId));
                displayedId++;
            }

            return managerModels;
        }

        public ManagerModel GetModel(int displayedId, int modelId)
        {
            var manager = _dbUoW.Managers.GetEntityByCondition(m => m.Id == modelId);
            return EntityToModel(manager, displayedId);
        }

        public void EditModel(ManagerModel model)
        {
            var manager = ModelToEntity(model);
            _dbUoW.Managers.Update(manager);
            _dbUoW.Save();
        }

        public void DeleteModel(int id)
        {
            _dbUoW.Managers.Remove(id);
            _dbUoW.Save();
        }

        public IEnumerable<ManagerModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel)
        {
            comonEntityCount = _dbUoW.Managers.GetCountByCondition(s => s.Surname.StartsWith(filterModel.Data));
            var managers = _dbUoW.Managers.GetRangeByCondition((pageNumber - 1) * totalSize, totalSize, s => s.Surname.StartsWith(filterModel.Data));
            var managerModels = new List<ManagerModel>();
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var manager in managers)
            {
                managerModels.Add(EntityToModel(manager, displayedId));
                displayedId++;
            }

            return managerModels;
        }

        private ManagerModel EntityToModel(Manager manager, int displayedId)
        {
            return new ManagerModel()
            {
                Id = displayedId,
                ManagerId = manager.Id,
                Surname = manager.Surname
            };
        }

        private Manager ModelToEntity(ManagerModel model)
        {
            return new Manager()
            {
                Surname = model.Surname
            };
        }
    }
}
