using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess;
using OnlineShop.DataAccess.Entities;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic
{
    public class ClientWorker : IModelWorker<ClientModel>
    {
        private readonly DataBaseUoW _dbUoW;

        public ClientWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public IEnumerable<ClientModel> GetModels(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbUoW.Clients.GetCount();
            var clientModels = new List<ClientModel>();
            var clients = _dbUoW.Clients.GetRange((pageNumber - 1) * totalSize, totalSize);
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var client in clients)
            {
                clientModels.Add(EntityToModel(client, displayedId));
                displayedId++;
            }

            return clientModels;
        }

        public ClientModel GetModel(int displayedId, int modelId)
        {
            var client = _dbUoW.Clients.GetEntityByCondition(o => o.Id == modelId);
            return EntityToModel(client, displayedId);
        }

        public void EditModel(ClientModel model)
        {
            var client = ModelToEntity(model);
            _dbUoW.Clients.Update(client);
            _dbUoW.Save();
        }

        public void DeleteModel(int id)
        {
            _dbUoW.Clients.Remove(id);
            _dbUoW.Save();
        }

        public IEnumerable<ClientModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel)
        {
            comonEntityCount = _dbUoW.Clients.GetCountByCondition(s => s.Name.StartsWith(filterModel.Data));
            var clientModels = new List<ClientModel>();
            var clients = _dbUoW.Clients.GetRangeByCondition((pageNumber - 1) * totalSize, totalSize, s => s.Name.StartsWith(filterModel.Data));
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var client in clients)
            {
                clientModels.Add(EntityToModel(client, displayedId));
                displayedId++;
            }

            return clientModels;
        }

        private ClientModel EntityToModel(Client client, int displayedId)
        {
            return new ClientModel()
            {
                Id = displayedId,
                ClientId = client.Id,
                Name = client.Name
            };
        }

        private Client ModelToEntity(ClientModel model)
        {
            return new Client()
            {
                Name = model.Name
            };
        }
    }
}
