using EmployeeManagement.Data.Entities;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Data.Operations
{
    public class Operations
    {
        public async Task TriggerOperations()
        {
            string tableName = "Employee";

            CloudTable cloudTable = await Common.Common.CreateTableAsync(tableName);

            try
            {
                //await AddDataOperation(cloudTable);
                //await UpdateDataOperation(cloudTable);
                //RetrieveAllDataOperation(cloudTable);
                EmployeeEntity entity = RetrieveSingleDataOperation(cloudTable, "Consulting", "oliver-bennett-rk-new");
                DeleteDataOperation(cloudTable, entity);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task AddDataOperation(CloudTable cloudTable)
        {
            //throw new NotImplementedException();

            //Create Entity
            EmployeeEntity newEmployeeEntity = new EmployeeEntity("Consulting")
            {
                FullName = "Oliver Bennett",
                Email = "oliver@bennett.com",
                RowKey = "oliver-bennett-rk"
            };

            TableOperation addOperation = TableOperation.InsertOrMerge(newEmployeeEntity);
            TableResult addResult = await cloudTable.ExecuteAsync(addOperation);
            EmployeeEntity addResponse = addResult.Result as EmployeeEntity;

            if (addResult.RequestCharge.HasValue)
            {
                Console.WriteLine($"addResponse ==> {addResponse.PartitionKey} - {addResponse.RowKey} {addResponse.FullName}");
            }
        }
        public static async Task UpdateDataOperation(CloudTable cloudTable)
        {
            //throw new NotImplementedException();

            //Create Entity
            EmployeeEntity newEmployeeEntity = new EmployeeEntity("Consulting")
            {
                FullName = "Oliver Bennett New",
                Email = "oliver@bennett.com",
                RowKey = "oliver-bennett-rk-new"
            };

            TableOperation addOperation = TableOperation.InsertOrMerge(newEmployeeEntity);
            TableResult addResult = await cloudTable.ExecuteAsync(addOperation);
            EmployeeEntity addResponse = addResult.Result as EmployeeEntity;

            if (addResult.RequestCharge.HasValue)
            {
                Console.WriteLine($"addResponse ==> {addResponse.PartitionKey} - {addResponse.RowKey} - {addResponse.FullName}");
            }
            newEmployeeEntity.FullName = "Updated Full Name";
            TableOperation updateOperation = TableOperation.InsertOrMerge(newEmployeeEntity);
            TableResult updateResult = await cloudTable.ExecuteAsync(updateOperation);
            EmployeeEntity updateResponse = updateResult.Result as EmployeeEntity;

            if (updateResult.RequestCharge.HasValue)
            {
                Console.WriteLine($"updateResponse ==> {updateResponse.PartitionKey} - {updateResponse.RowKey} - {updateResponse.FullName}");
            }
        }
        public void RetrieveAllDataOperation(CloudTable cloudTable)
        {
            var allDataQuery = cloudTable.ExecuteQuery(new TableQuery<EmployeeEntity>());
            foreach (var employee in allDataQuery)
            {
                Console.WriteLine($"RowKey: {employee.RowKey}, PartitionKey: {employee.PartitionKey}, Full Name {employee.FullName}");
            }
        }
        public EmployeeEntity RetrieveSingleDataOperation(CloudTable cloudTable, string partitionkey, string rowkey)
        {
            TableOperation retrieveSingleOperation = TableOperation.Retrieve<EmployeeEntity>(partitionkey, rowkey);

            TableResult result = cloudTable.Execute(retrieveSingleOperation);

            EmployeeEntity employee = result.Result as EmployeeEntity;

            if(employee != null)
            {
                Console.WriteLine($"Employee Data => {employee.RowKey} {employee.PartitionKey} {employee.FullName}");

                return employee;
            }
            return null;

            

        }
        public void DeleteDataOperation(CloudTable cloudTable, EmployeeEntity entity)
        {
            //TableOperation retrieveSingleOperation = TableOperation.Retrieve<EmployeeEntity>(partitionkey, rowkey);
            TableOperation deleteOperation = TableOperation.Delete(entity);
            TableResult result = cloudTable.Execute(deleteOperation);

            if (result.RequestCharge.HasValue)
            {
                Console.WriteLine($"Request Charge of Delete Operation : {result.RequestCharge}");
            }

        }

    }
}
