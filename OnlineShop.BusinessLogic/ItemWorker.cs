using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess;
using OnlineShop.DataAccess.Entities;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic
{
    public class ItemWorker : IModelWorker<ItemModel>
    {
        private readonly DataBaseUoW _dbUoW;

        public ItemWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public IEnumerable<ItemModel> GetModels(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbUoW.Items.GetCount();
            var itemModels = new List<ItemModel>();
            var items = _dbUoW.Items.GetRange((pageNumber - 1) * totalSize, totalSize);
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var item in items)
            {
                itemModels.Add(EntityToModel(item, displayedId));
                displayedId++;
            }

            return itemModels;
        }

        public ItemModel GetModel(int displayedId, int modelId)
        {
            var item = _dbUoW.Items.GetEntityByCondition(o => o.Id == modelId);
            if (item != null)
            {
                return EntityToModel(item, displayedId);
            }

            return null;
        }

        public void EditModel(ItemModel model)
        {
            var item = ModelToEntity(model);
            _dbUoW.Items.Update(item);
            _dbUoW.Save();
        }

        public void DeleteModel(int id)
        {
            _dbUoW.Items.Remove(id);
            _dbUoW.Save();
        }

        public IEnumerable<ItemModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel)
        {
            comonEntityCount = _dbUoW.Managers.GetCount();
            var itemModels = new List<ItemModel>();
            var items = _dbUoW.Items.GetRangeByCondition((pageNumber - 1) * totalSize, totalSize, s => s.Name.StartsWith(filterModel.Data));
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var item in items)
            {
                itemModels.Add(EntityToModel(item, displayedId));
                displayedId++;
            }

            return itemModels;
        }

        private ItemModel EntityToModel(Item item, int displayedId)
        {
            return new ItemModel()
            {
                Id = displayedId,
                ItemId = item.Id,
                Name = item.Name
            };
        }

        private Item ModelToEntity(ItemModel model)
        {
            return new Item()
            {
                Name = model.Name
            };
        }
    }
}
