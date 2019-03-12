/// <reference path="../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../Model/DslQuery.ts"/>
/// <reference path="../Interfaces/IQueryService.ts"/>
/// <reference path="../Services/QueryService.ts"/>

module Query.Controller {

    export class EditQueryController {

        accessReference: Array<any>;

        blockTypeToAdd: string;

        currentUserId: number;

        dataSources: Array<any>;

        dataSourceFields: Array<any>;

        filterConditions: Array<any>;

        filterOperations: Array<any>;

        queryService: Interfaces.IQueryService;

        queryId: number;

        query: any;

        sortOrderDirections: Array<any>;

        model: Models.DslQuery;

        queryResult: any;

        isCanEdit: boolean;

        isChanged: boolean;

        scope: ng.IScope;

        status: string;

        users: Array<any>;

        static $inject = ["Query.Service.QueryService", "$scope"];

        constructor(queryService: Interfaces.IQueryService, $rootScope: ng.IScope)
        {
            this.queryService = queryService;

            this.scope = $rootScope;
            this.status = "Ready";
            this.isChanged = false;
            this.blockTypeToAdd = null;

            this.model = new Models.DslQuery();
        }

        init(queryId: number)
        {
            this.queryId = queryId;

            this.getQuery(queryId);
        }

        getQuery(queryId: number)
        {
            this.queryService.getQuery(
                this.queryId,
                this.onQueryReceived.bind(this),
                null);
        }

        onQueryReceived(query: any)
        {
            this.accessReference = query.AccessReference;

            this.setStatus("Access reference initialized. Lenght=" + this.accessReference.length);

            this.sortOrderDirections = query.SortOrderDirections;

            this.setStatus("Access reference initialized. Lenght=" + this.sortOrderDirections.length);

            this.query = query.Query;

            this.setStatus("Query initialized. Id='" + this.query.Name + "', ProjectId=" + this.query.ProjectId);

            // ReSharper disable once CoercedEqualsUsingWithNullUndefined
            if (this.query.Model != null)
            {
                this.model = JSON.parse(this.query.Model);
            }
            else
            {
                this.model = new Models.DslQuery();
            }

            this.setStatus("Model initialized. type='" + this.model.$type + "'");

            this.currentUserId = query.CurrentUserId;

            this.setStatus("Current user Id='" + this.currentUserId + "'");

            this.isCanEdit = query.IsCanEdit;

            this.setStatus("Query initialized.Name=" + this.query.Name);

            this.users = query.Users;

            this.setStatus("Users array initialized. Lenght=" + this.users.length);

            this.getDataSources();

            this.getDataSourceFields();

            this.getFilterConditions();

            this.getFilterOperations();

            this.setStatus("Initialized succesfully");
        }

        getFilterConditions()
        {
            this.setStatus("Retrieving filter conditions");

            this.queryService.getFilterConditions(
                this.onFilterConditionsReceived.bind(this),
                null);
        }

        onFilterConditionsReceived(data)
        {
            this.filterConditions = data;

            this.setStatus("Filter conditions received. Lenght='" + this.filterConditions.length + "'");
        }

        getFilterOperations()
        {
            this.setStatus("Retrieving filter operations");

            this.queryService.getFilterOperations(
                this.onFilterOperationsReceived.bind(this),
                null);
        }

        onFilterOperationsReceived(data)
        {
            this.filterOperations = data;

            this.setStatus("Filter operations received. Lenght='" + this.filterOperations.length + "'");
        }

        saveQuery()
        {
            this.setStatus(" Trying to save query");

            this.query.Model = null;

            this.queryService.save(
                this.query,
                this.onQuerySaved.bind(this),
                this.onQueryNotSaved.bind(this));
        }

        saveQueryModel()
        {
            this.setStatus(" Trying to save query model");

            this.query.Model = JSON.stringify(this.model);

            this.queryService.save(
                this.query,
                this.onQuerySaved.bind(this),
                this.onQueryNotSaved.bind(this));
        }

        onQuerySaved(data)
        {
            this.isChanged = false;

            this.setStatus("Query saved succesfully");
        }

        onQueryNotSaved(data)
        {
            this.setStatus("Failed to save query");
        }

        run()
        {
            this.runFor(this.currentUserId);
        }

        runFor(userId: number)
        {
            this.setStatus("Running query...");

            var string = null;

            if (this.model.Parameters != null)
            {
                var parameters = this.model
                    .Parameters
                    .$values
                    .map(p =>
                    {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        }
                    });

                string = JSON.stringify(parameters);
            }

            this.queryResult = null;

            this.queryService.execute(
                this.query.Id,
                userId,
                string,
                this.onRunnedSuccessfully.bind(this),
                this.onRunFailed.bind(this));
        }

        onRunnedSuccessfully(data)
        {
            this.queryResult = data;

            this.setStatus("Query execution finished.");
        }

        onRunFailed()
        {
            this.queryResult = null;

            this.setStatus("Query execution failed");
        }

        setStatus(newStatus: string)
        {
            var date = new Date();

            this.status = "[" + date.toLocaleString() + "] - " + newStatus;

            console.log("Status changed. Status='" + newStatus + "'");
        }

        setChanged()
        {
            this.isChanged = true;

            this.setStatus("Query changed. Save query to get ability to run");
        }

        addBlock()
        {
            var lastBlockPosition = this.model.Blocks.$values.length;

            var newBlock = {
                $type: this.blockTypeToAdd
            };

            this.model.Blocks.$values.splice(lastBlockPosition, 0, newBlock);

            this.blockTypeToAdd = null;

            this.setStatus("Block was added to the Query.");
        }

        moveUp(block: any)
        {
            this.setStatus("Moving query block up. Type='" + block.$type + "'");

            var index = this.model.Blocks.$values.indexOf(block);

            this.model.Blocks.$values.splice(index, 1);

            this.model.Blocks.$values.splice(index - 1, 0, block);

            this.setStatus("Query block moved up. Type='" + block.$type + "'");

            this.syncWithText();
        }

        moveDown(block: any)
        {
            this.setStatus("Moving query block down. Type='" + block.$type + "'");

            var index = this.model.Blocks.$values.indexOf(block);

            this.model.Blocks.$values.splice(index, 1);

            this.model.Blocks.$values.splice(index + 1, 0, block);

            this.setStatus("Query block moved down. Type='" + block.$type + "'");

            this.syncWithText();
        }

        removeQueryBlock(block: any)
        {
            this.setStatus("Removing query block. Type='" + block.$type + "'");

            var index = this.model.Blocks.$values.indexOf(block);

            this.model.Blocks.$values.splice(index, 1);

            this.setStatus("Query block removed. Type='" + block.$type + "'");

            this.syncWithText();
        }

        getDataSources()
        {
            this.queryService.getDataSources(
                this.query.ProjectId,
                this.onDataSourcesReceived.bind(this),
                this.onDataSourcesReceivingFailed.bind(this));
        }

        onDataSourcesReceived(data)
        {
            this.dataSources = data;

            this.setStatus("Data sources received. Lenght='" + this.dataSources.length + "'");
        }

        onDataSourcesReceivingFailed(data)
        {
            this.dataSources = null;

            this.setStatus("Data sources receiving failed.");
        }

        getDataSourceFields()
        {
            this.setStatus("Retrieving data source fields. Data source key='" + this.model.QueryEntityName + "'");

            this.queryService.getDataSourceFields(
                this.model.QueryEntityName,
                this.query.ProjectId,
                this.onDataSourceFieldsReceived.bind(this),
                null);
        }

        onDataSourceFieldsReceived(data)
        {
            this.dataSourceFields = data;

            this.setStatus("Data source fields received. Lenght='" + this.dataSourceFields.length + "'");
        }

        removeSelectItem(selectBlock: any, selectItem: any)
        {
            this.setStatus("Removing query block. Type='" + selectBlock.$type + "'");

            var index = selectBlock.Selects.$values.indexOf(selectItem);

            selectBlock.Selects.$values.splice(index, 1);

            this.setStatus("Query block removed. Type='" + selectBlock.$type + "'");

            this.syncWithText();
        }

        moveSelectItemUp(selectBlock: any, selectItem: any)
        {
            this.setStatus("Moving select item up. Value='" + selectItem.Value + "'");

            var index = selectBlock.Selects.$values.indexOf(selectItem);

            selectBlock.Selects.$values.splice(index, 1);

            selectBlock.Selects.$values.splice(index - 1, 0, selectItem);

            this.setStatus("Select item moved up. Value='" + selectItem.Value + "'");

            this.syncWithText();
        }

        moveSelectItemDown(selectBlock: any, selectItem: any)
        {
            this.setStatus("Moving select block item down. Value='" + selectItem.Value + "'");

            var index = selectBlock.Selects.$values.indexOf(selectItem);

            selectBlock.Selects.$values.splice(index, 1);

            selectBlock.Selects.$values.splice(index + 1, 0, selectItem);

            this.setStatus("Select item moved down. Value='" + selectItem.Value + "'");

            this.syncWithText();
        }

        removeOrderItem(block: any, item: any)
        {
            this.setStatus("Removing order item. Type='" + block.$type + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            this.setStatus("Order item removed. Type='" + block.$type + "'");

            this.syncWithText();
        }

        moveOrderItemUp(block: any, item: any)
        {
            this.setStatus("Moving order item up. Value='" + item.Value + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            block.Items.$values.splice(index - 1, 0, item);

            this.setStatus("Order item moved up. Value='" + item.Value + "'");

            this.syncWithText();
        }

        moveOrderItemDown(block: any, item: any)
        {
            this.setStatus("Moving order item down. Value='" + item.Value + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            block.Items.$values.splice(index + 1, 0, item);

            this.setStatus("Order item moved down. Value='" + item.Value + "'");

            this.syncWithText();
        }

        removeGroupItem(block: any, item: any)
        {
            this.setStatus("Removing group item. Type='" + block.$type + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            this.setStatus("Group item removed. Type='" + block.$type + "'");

            this.syncWithText();
        }

        moveGroupItemUp(block: any, item: any)
        {
            this.setStatus("Moving group item up. Value='" + item.Value + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            block.Items.$values.splice(index - 1, 0, item);

            this.setStatus("Group item moved up. Value='" + item.Value + "'");

            this.syncWithText();
        }

        moveGroupItemDown(block: any, item: any)
        {
            this.setStatus("Moving group item down. Value='" + item.Value + "'");

            var index = block.Items.$values.indexOf(item);

            block.Items.$values.splice(index, 1);

            block.Items.$values.splice(index + 1, 0, item);

            this.setStatus("Group item moved down. Value='" + item.Value + "'");

            this.syncWithText();
        }

        addSelectItem(selectBlock: any)
        {
            // ReSharper disable once CoercedEqualsUsingWithNullUndefined
            if (selectBlock.Selects == null)
            {
                console.log("Creates select property");

                selectBlock.Selects = {
                    $type: "System.Collections.Generic.List`1[[Infrastructure.Engines.Dsl.Query.DslFormatItem, Infrastructure.Engines.Dsl]], mscorlib",
                    $values: new Array<any>()
                };
            }

            var selectBlockLenght = selectBlock.Selects.$values.length;

            var selectItem = {
                $type: "Infrastructure.Engines.Dsl.Query.DslFormatItem, Infrastructure.Engines.Dsl",
                Name: null,
                Value: null,
                DisplayName: null,
                Description: null
            }

            selectBlock.Selects.$values.splice(selectBlockLenght, 0, selectItem);

            this.setStatus("Select item added. Position='" + selectBlockLenght + "'");

            this.setChanged();
        }

        addOrderItem(block: any)
        {
            // ReSharper disable once CoercedEqualsUsingWithNullUndefined
            if (block.Items == null)
            {
                console.log("Creating order block item");

                block.Items = {
                    $type: "Infrastructure.Engines.Dsl.OrderBlockItem[], Infrastructure.Engines.Dsl",
                    $values: new Array<any>()
                };
            }

            var selectBlockLenght = block.Items.$values.length;

            var selectItem = {
                $type: "Infrastructure.Engines.Dsl.OrderBlockItem, Infrastructure.Engines.Dsl",
                OrderFieldName: null,
                SortOrder: 0
            }

            block.Items.$values.splice(selectBlockLenght, 0, selectItem);

            this.setStatus("Order block item added. Position='" + selectBlockLenght + "'");

            this.setChanged();
        }

        addGroupItem(block: any)
        {
            // ReSharper disable once CoercedEqualsUsingWithNullUndefined
            if (block.Items == null)
            {
                console.log("Creating group block item");

                block.Items = {
                    $type: "Infrastructure.Engines.Dsl.Query.DslGroupItem[], Infrastructure.Engines.Dsl",
                    $values: new Array<any>()
                };
            }

            var selectBlockLenght = block.Items.$values.length;

            var selectItem = {
                $type: "Infrastructure.Engines.Dsl.Query.DslGroupItem, Infrastructure.Engines.Dsl",
                OrderFieldName: null,
                SortOrder: 0
            }

            block.Items.$values.splice(selectBlockLenght, 0, selectItem);

            this.setStatus("Group block item added. Position='" + selectBlockLenght + "'");

            this.setChanged();
        }

        syncWithText()
        {
            this.setChanged();

            var modelJson = JSON.stringify(this.model);

            this.queryService.getTextQuery(modelJson,
                this.setTextQuery.bind(this),
                this.failedToSetTextQuery.bind(this));
        }

        setTextQuery(data: string)
        {
            this.query.Query = data;

            console.log(data);
        }

        failedToSetTextQuery(data: string)
        {
            this.query.Query = data;

            this.setStatus("Failed to convert query Model to DSL");
        }

        syncWithModel()
        {
            this.setStatus("Updating query model...");

            this.queryService.getModelQuery(
                this.query.Query,
                this.setModelQuery.bind(this),
                null);
        }

        setModelQuery(data)
        {
            this.model = data;

            this.setStatus("Query model updated from text");
        }

        onQueryEntitynameChanged()
        {
            this.syncWithText();

            this.getDataSourceFields();
        }

        addFilter(block: any, parent: any, specification: any)
        {
            var newSpec = {
                $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConditionSpecification, Infrastructure.Engines.Dsl",
                Condition: 0,
                LeftSpecification: specification,
                RightSpecification: {
                    $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterSpecification, Infrastructure.Engines.Dsl",
                    LeftSpecification: {
                        $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConstantSpecification, Infrastructure.Engines.Dsl",
                        Value: ""
                    },
                    Operator: 2,
                    RightSpecification: {
                        $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConstantSpecification, Infrastructure.Engines.Dsl",
                        Value: ""
                    }
                }
            };

            if (parent == null)
            {
                block.Specification = newSpec;

                console.log("Added new filter to root block element");
            }
            else if (parent.Specification != null)
            {
                parent.Specification = newSpec;

                console.log("Added new filter to central specification element");
            }
            else if (parent.LeftSpecification != null && parent.LeftSpecification == specification)
            {
                parent.LeftSpecification = newSpec;

                console.log("Added new filter to left specification element");
            }
            else if (parent.RightSpecification != null && parent.RightSpecification == specification)
            {
                parent.RightSpecification = newSpec;

                console.log("Added new filter to right specification element");
            }

            this.syncWithText();
        }

        removeFilter = (block: any, parent: any, parentParent: any, specification: any) =>
        {
            console.log("Removing filter specification");

            if (block.Specification == specification)
            {
                console.log("Can't remove root specification");

                return;
            }

            if (parent.Specification == specification)
            {
                console.log("Central specification can't be removed");
            }
            if (parent.LeftSpecification == specification)
            {
                if (parentParent == null)
                    block.Specification = parent.RightSpecification;
                else
                {
                    if (parentParent.Specification == parent)
                    {
                        parentParent.Specification = parent.RightSpecification;
                    }else  if (parentParent.LeftSpecification == parent)
                        parentParent.LeftSpecification = parent.RightSpecification;
                    else
                        parentParent.RightSpecification = parent.RightSpecification;
                }

                console.log("Removed left side specification");
            }
            else if (parent.RightSpecification == specification)
            {
                if (parentParent == null)
                    block.Specification = parent.LeftSpecification;
                else
                {
                    if (parentParent.Specification == parent) {
                        parentParent.Specification = parent.LeftSpecification;
                    } else if (parentParent.LeftSpecification == parent)
                        parentParent.LeftSpecification = parent.LeftSpecification;
                    else
                        parentParent.RightSpecification = parent.LeftSpecification;
                }

                console.log("Removed right side specification");
            }

            this.syncWithText();

            console.log("Filter specification removind finished");
        }

        inGroup = (block: any, parent: any, specification: any) =>
        {
            var groupSpec = {
                $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterGroupSpecification, Infrastructure.Engines.Dsl",
                Specification: specification
            };

            if (block.Specification == specification)
            {
                block.Specification = groupSpec;

                console.log("Added group in root specification");
            }
            else if (parent.Specification == specification)
            {
                parent.Specification = groupSpec;

                console.log("Added group in specification");
            }
            else if (parent.LeftSpecification == specification) {
                parent.LeftSpecification = groupSpec;

                console.log("Added group in left specification");
            }
            else if (parent.RightSpecification == specification) {
                parent.RightSpecification = groupSpec;

                console.log("Added group in right specification");
            }


            this.syncWithText();
        }

        removeGroup = (block: any, parent: any, parentParent: any, specification: any) => {

            if (block.Specification == specification) {

                block.Specification = specification.Specification;
            }
            else if (parent.Specification == specification) {

                parent.Specification = specification.Specification;
            }
            else if (parent.LeftSpecification == specification) {
                
                parent.LeftSpecification = specification.Specification;
            }
            else if (parent.RightSpecification == specification) {

                parent.RightSpecification = specification.Specification;
            }

            this.syncWithText();
        }

        addParameter = () =>
        {
            if (this.model.Parameters == null)
            {
                console.log("Creating properties object...");

                this.model.Parameters = {
                    $type: "Infrastructure.Engines.Dsl.Query.DslQueryParameter[], Infrastructure.Engines.Dsl",
                    $values: new Array<any>()
                };

                console.log("Properties object created");
            }

            var parametersLenght = this.model.Parameters.$values.length;

            var parameter = {
                $type: "Infrastructure.Engines.Dsl.Query.DslQueryParameter, Infrastructure.Engines.Dsl",
                Key: null,
                Value: null
            }

            this.model.Parameters.$values.splice(parametersLenght, 0, parameter);

            this.setStatus("Parameter added");

            this.setChanged();
        }

        removeParameter = (parameter: any) => {

            this.setStatus("Removing parameter item");

            var index = this.model.Parameters.$values.indexOf(parameter);

            this.model.Parameters.$values.splice(index, 1);

            this.setStatus("Parameter removed");

            this.setChanged();
        }
    }

    angular.module("Scripts.App.Query").controller("Query.Controllers.EditQueryController", EditQueryController);
}