/// <reference path="../../../typings/angularjs/angular.d.ts"/>
/// <reference path="../Model/DslQuery.ts"/>
/// <reference path="../Interfaces/IQueryService.ts"/>
/// <reference path="../Services/QueryService.ts"/>
var Query;
(function (Query) {
    var Controller;
    (function (Controller) {
        var EditQueryController = (function () {
            function EditQueryController(queryService, $rootScope) {
                var _this = this;
                this.removeFilter = function (block, parent, parentParent, specification) {
                    console.log("Removing filter specification");
                    if (block.Specification == specification) {
                        console.log("Can't remove root specification");
                        return;
                    }
                    if (parent.Specification == specification) {
                        console.log("Central specification can't be removed");
                    }
                    if (parent.LeftSpecification == specification) {
                        if (parentParent == null)
                            block.Specification = parent.RightSpecification;
                        else {
                            if (parentParent.Specification == parent) {
                                parentParent.Specification = parent.RightSpecification;
                            }
                            else if (parentParent.LeftSpecification == parent)
                                parentParent.LeftSpecification = parent.RightSpecification;
                            else
                                parentParent.RightSpecification = parent.RightSpecification;
                        }
                        console.log("Removed left side specification");
                    }
                    else if (parent.RightSpecification == specification) {
                        if (parentParent == null)
                            block.Specification = parent.LeftSpecification;
                        else {
                            if (parentParent.Specification == parent) {
                                parentParent.Specification = parent.LeftSpecification;
                            }
                            else if (parentParent.LeftSpecification == parent)
                                parentParent.LeftSpecification = parent.LeftSpecification;
                            else
                                parentParent.RightSpecification = parent.LeftSpecification;
                        }
                        console.log("Removed right side specification");
                    }
                    _this.syncWithText();
                    console.log("Filter specification removind finished");
                };
                this.inGroup = function (block, parent, specification) {
                    var groupSpec = {
                        $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterGroupSpecification, Infrastructure.Engines.Dsl",
                        Specification: specification
                    };
                    if (block.Specification == specification) {
                        block.Specification = groupSpec;
                        console.log("Added group in root specification");
                    }
                    else if (parent.Specification == specification) {
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
                    _this.syncWithText();
                };
                this.removeGroup = function (block, parent, parentParent, specification) {
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
                    _this.syncWithText();
                };
                this.addParameter = function () {
                    if (_this.model.Parameters == null) {
                        console.log("Creating properties object...");
                        _this.model.Parameters = {
                            $type: "Infrastructure.Engines.Dsl.Query.DslQueryParameter[], Infrastructure.Engines.Dsl",
                            $values: new Array()
                        };
                        console.log("Properties object created");
                    }
                    var parametersLenght = _this.model.Parameters.$values.length;
                    var parameter = {
                        $type: "Infrastructure.Engines.Dsl.Query.DslQueryParameter, Infrastructure.Engines.Dsl",
                        Key: null,
                        Value: null
                    };
                    _this.model.Parameters.$values.splice(parametersLenght, 0, parameter);
                    _this.setStatus("Parameter added");
                    _this.setChanged();
                };
                this.removeParameter = function (parameter) {
                    _this.setStatus("Removing parameter item");
                    var index = _this.model.Parameters.$values.indexOf(parameter);
                    _this.model.Parameters.$values.splice(index, 1);
                    _this.setStatus("Parameter removed");
                    _this.setChanged();
                };
                this.queryService = queryService;
                this.scope = $rootScope;
                this.status = "Ready";
                this.isChanged = false;
                this.blockTypeToAdd = null;
                this.model = new Query.Models.DslQuery();
            }
            EditQueryController.prototype.init = function (queryId) {
                this.queryId = queryId;
                this.getQuery(queryId);
            };
            EditQueryController.prototype.getQuery = function (queryId) {
                this.queryService.getQuery(this.queryId, this.onQueryReceived.bind(this), null);
            };
            EditQueryController.prototype.onQueryReceived = function (query) {
                this.accessReference = query.AccessReference;
                this.setStatus("Access reference initialized. Lenght=" + this.accessReference.length);
                this.sortOrderDirections = query.SortOrderDirections;
                this.setStatus("Access reference initialized. Lenght=" + this.sortOrderDirections.length);
                this.query = query.Query;
                this.setStatus("Query initialized. Id='" + this.query.Name + "', ProjectId=" + this.query.ProjectId);
                // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                if (this.query.Model != null) {
                    this.model = JSON.parse(this.query.Model);
                }
                else {
                    this.model = new Query.Models.DslQuery();
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
            };
            EditQueryController.prototype.getFilterConditions = function () {
                this.setStatus("Retrieving filter conditions");
                this.queryService.getFilterConditions(this.onFilterConditionsReceived.bind(this), null);
            };
            EditQueryController.prototype.onFilterConditionsReceived = function (data) {
                this.filterConditions = data;
                this.setStatus("Filter conditions received. Lenght='" + this.filterConditions.length + "'");
            };
            EditQueryController.prototype.getFilterOperations = function () {
                this.setStatus("Retrieving filter operations");
                this.queryService.getFilterOperations(this.onFilterOperationsReceived.bind(this), null);
            };
            EditQueryController.prototype.onFilterOperationsReceived = function (data) {
                this.filterOperations = data;
                this.setStatus("Filter operations received. Lenght='" + this.filterOperations.length + "'");
            };
            EditQueryController.prototype.saveQuery = function () {
                this.setStatus(" Trying to save query");
                this.query.Model = null;
                this.queryService.save(this.query, this.onQuerySaved.bind(this), this.onQueryNotSaved.bind(this));
            };
            EditQueryController.prototype.saveQueryModel = function () {
                this.setStatus(" Trying to save query model");
                this.query.Model = JSON.stringify(this.model);
                this.queryService.save(this.query, this.onQuerySaved.bind(this), this.onQueryNotSaved.bind(this));
            };
            EditQueryController.prototype.onQuerySaved = function (data) {
                this.isChanged = false;
                this.setStatus("Query saved succesfully");
            };
            EditQueryController.prototype.onQueryNotSaved = function (data) {
                this.setStatus("Failed to save query");
            };
            EditQueryController.prototype.run = function () {
                this.runFor(this.currentUserId);
            };
            EditQueryController.prototype.runFor = function (userId) {
                this.setStatus("Running query...");
                var string = null;
                if (this.model.Parameters != null) {
                    var parameters = this.model
                        .Parameters
                        .$values
                        .map(function (p) {
                        return {
                            Key: p.Key,
                            Value: p.Value
                        };
                    });
                    string = JSON.stringify(parameters);
                }
                this.queryResult = null;
                this.queryService.execute(this.query.Id, userId, string, this.onRunnedSuccessfully.bind(this), this.onRunFailed.bind(this));
            };
            EditQueryController.prototype.onRunnedSuccessfully = function (data) {
                this.queryResult = data;
                this.setStatus("Query execution finished.");
            };
            EditQueryController.prototype.onRunFailed = function () {
                this.queryResult = null;
                this.setStatus("Query execution failed");
            };
            EditQueryController.prototype.setStatus = function (newStatus) {
                var date = new Date();
                this.status = "[" + date.toLocaleString() + "] - " + newStatus;
                console.log("Status changed. Status='" + newStatus + "'");
            };
            EditQueryController.prototype.setChanged = function () {
                this.isChanged = true;
                this.setStatus("Query changed. Save query to get ability to run");
            };
            EditQueryController.prototype.addBlock = function () {
                var lastBlockPosition = this.model.Blocks.$values.length;
                var newBlock = {
                    $type: this.blockTypeToAdd
                };
                this.model.Blocks.$values.splice(lastBlockPosition, 0, newBlock);
                this.blockTypeToAdd = null;
                this.setStatus("Block was added to the Query.");
            };
            EditQueryController.prototype.moveUp = function (block) {
                this.setStatus("Moving query block up. Type='" + block.$type + "'");
                var index = this.model.Blocks.$values.indexOf(block);
                this.model.Blocks.$values.splice(index, 1);
                this.model.Blocks.$values.splice(index - 1, 0, block);
                this.setStatus("Query block moved up. Type='" + block.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveDown = function (block) {
                this.setStatus("Moving query block down. Type='" + block.$type + "'");
                var index = this.model.Blocks.$values.indexOf(block);
                this.model.Blocks.$values.splice(index, 1);
                this.model.Blocks.$values.splice(index + 1, 0, block);
                this.setStatus("Query block moved down. Type='" + block.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.removeQueryBlock = function (block) {
                this.setStatus("Removing query block. Type='" + block.$type + "'");
                var index = this.model.Blocks.$values.indexOf(block);
                this.model.Blocks.$values.splice(index, 1);
                this.setStatus("Query block removed. Type='" + block.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.getDataSources = function () {
                this.queryService.getDataSources(this.query.ProjectId, this.onDataSourcesReceived.bind(this), this.onDataSourcesReceivingFailed.bind(this));
            };
            EditQueryController.prototype.onDataSourcesReceived = function (data) {
                this.dataSources = data;
                this.setStatus("Data sources received. Lenght='" + this.dataSources.length + "'");
            };
            EditQueryController.prototype.onDataSourcesReceivingFailed = function (data) {
                this.dataSources = null;
                this.setStatus("Data sources receiving failed.");
            };
            EditQueryController.prototype.getDataSourceFields = function () {
                this.setStatus("Retrieving data source fields. Data source key='" + this.model.QueryEntityName + "'");
                this.queryService.getDataSourceFields(this.model.QueryEntityName, this.query.ProjectId, this.onDataSourceFieldsReceived.bind(this), null);
            };
            EditQueryController.prototype.onDataSourceFieldsReceived = function (data) {
                this.dataSourceFields = data;
                this.setStatus("Data source fields received. Lenght='" + this.dataSourceFields.length + "'");
            };
            EditQueryController.prototype.removeSelectItem = function (selectBlock, selectItem) {
                this.setStatus("Removing query block. Type='" + selectBlock.$type + "'");
                var index = selectBlock.Selects.$values.indexOf(selectItem);
                selectBlock.Selects.$values.splice(index, 1);
                this.setStatus("Query block removed. Type='" + selectBlock.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveSelectItemUp = function (selectBlock, selectItem) {
                this.setStatus("Moving select item up. Value='" + selectItem.Value + "'");
                var index = selectBlock.Selects.$values.indexOf(selectItem);
                selectBlock.Selects.$values.splice(index, 1);
                selectBlock.Selects.$values.splice(index - 1, 0, selectItem);
                this.setStatus("Select item moved up. Value='" + selectItem.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveSelectItemDown = function (selectBlock, selectItem) {
                this.setStatus("Moving select block item down. Value='" + selectItem.Value + "'");
                var index = selectBlock.Selects.$values.indexOf(selectItem);
                selectBlock.Selects.$values.splice(index, 1);
                selectBlock.Selects.$values.splice(index + 1, 0, selectItem);
                this.setStatus("Select item moved down. Value='" + selectItem.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.removeOrderItem = function (block, item) {
                this.setStatus("Removing order item. Type='" + block.$type + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                this.setStatus("Order item removed. Type='" + block.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveOrderItemUp = function (block, item) {
                this.setStatus("Moving order item up. Value='" + item.Value + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                block.Items.$values.splice(index - 1, 0, item);
                this.setStatus("Order item moved up. Value='" + item.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveOrderItemDown = function (block, item) {
                this.setStatus("Moving order item down. Value='" + item.Value + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                block.Items.$values.splice(index + 1, 0, item);
                this.setStatus("Order item moved down. Value='" + item.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.removeGroupItem = function (block, item) {
                this.setStatus("Removing group item. Type='" + block.$type + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                this.setStatus("Group item removed. Type='" + block.$type + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveGroupItemUp = function (block, item) {
                this.setStatus("Moving group item up. Value='" + item.Value + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                block.Items.$values.splice(index - 1, 0, item);
                this.setStatus("Group item moved up. Value='" + item.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.moveGroupItemDown = function (block, item) {
                this.setStatus("Moving group item down. Value='" + item.Value + "'");
                var index = block.Items.$values.indexOf(item);
                block.Items.$values.splice(index, 1);
                block.Items.$values.splice(index + 1, 0, item);
                this.setStatus("Group item moved down. Value='" + item.Value + "'");
                this.syncWithText();
            };
            EditQueryController.prototype.addSelectItem = function (selectBlock) {
                // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                if (selectBlock.Selects == null) {
                    console.log("Creates select property");
                    selectBlock.Selects = {
                        $type: "System.Collections.Generic.List`1[[Infrastructure.Engines.Dsl.Query.DslFormatItem, Infrastructure.Engines.Dsl]], mscorlib",
                        $values: new Array()
                    };
                }
                var selectBlockLenght = selectBlock.Selects.$values.length;
                var selectItem = {
                    $type: "Infrastructure.Engines.Dsl.Query.DslFormatItem, Infrastructure.Engines.Dsl",
                    Name: null,
                    Value: null,
                    DisplayName: null,
                    Description: null
                };
                selectBlock.Selects.$values.splice(selectBlockLenght, 0, selectItem);
                this.setStatus("Select item added. Position='" + selectBlockLenght + "'");
                this.setChanged();
            };
            EditQueryController.prototype.addOrderItem = function (block) {
                // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                if (block.Items == null) {
                    console.log("Creating order block item");
                    block.Items = {
                        $type: "Infrastructure.Engines.Dsl.OrderBlockItem[], Infrastructure.Engines.Dsl",
                        $values: new Array()
                    };
                }
                var selectBlockLenght = block.Items.$values.length;
                var selectItem = {
                    $type: "Infrastructure.Engines.Dsl.OrderBlockItem, Infrastructure.Engines.Dsl",
                    OrderFieldName: null,
                    SortOrder: 0
                };
                block.Items.$values.splice(selectBlockLenght, 0, selectItem);
                this.setStatus("Order block item added. Position='" + selectBlockLenght + "'");
                this.setChanged();
            };
            EditQueryController.prototype.addGroupItem = function (block) {
                // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                if (block.Items == null) {
                    console.log("Creating group block item");
                    block.Items = {
                        $type: "Infrastructure.Engines.Dsl.Query.DslGroupItem[], Infrastructure.Engines.Dsl",
                        $values: new Array()
                    };
                }
                var selectBlockLenght = block.Items.$values.length;
                var selectItem = {
                    $type: "Infrastructure.Engines.Dsl.Query.DslGroupItem, Infrastructure.Engines.Dsl",
                    OrderFieldName: null,
                    SortOrder: 0
                };
                block.Items.$values.splice(selectBlockLenght, 0, selectItem);
                this.setStatus("Group block item added. Position='" + selectBlockLenght + "'");
                this.setChanged();
            };
            EditQueryController.prototype.syncWithText = function () {
                this.setChanged();
                var modelJson = JSON.stringify(this.model);
                this.queryService.getTextQuery(modelJson, this.setTextQuery.bind(this), this.failedToSetTextQuery.bind(this));
            };
            EditQueryController.prototype.setTextQuery = function (data) {
                this.query.Query = data;
                console.log(data);
            };
            EditQueryController.prototype.failedToSetTextQuery = function (data) {
                this.query.Query = data;
                this.setStatus("Failed to convert query Model to DSL");
            };
            EditQueryController.prototype.syncWithModel = function () {
                this.setStatus("Updating query model...");
                this.queryService.getModelQuery(this.query.Query, this.setModelQuery.bind(this), null);
            };
            EditQueryController.prototype.setModelQuery = function (data) {
                this.model = data;
                this.setStatus("Query model updated from text");
            };
            EditQueryController.prototype.onQueryEntitynameChanged = function () {
                this.syncWithText();
                this.getDataSourceFields();
            };
            EditQueryController.prototype.addFilter = function (block, parent, specification) {
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
                if (parent == null) {
                    block.Specification = newSpec;
                    console.log("Added new filter to root block element");
                }
                else if (parent.Specification != null) {
                    parent.Specification = newSpec;
                    console.log("Added new filter to central specification element");
                }
                else if (parent.LeftSpecification != null && parent.LeftSpecification == specification) {
                    parent.LeftSpecification = newSpec;
                    console.log("Added new filter to left specification element");
                }
                else if (parent.RightSpecification != null && parent.RightSpecification == specification) {
                    parent.RightSpecification = newSpec;
                    console.log("Added new filter to right specification element");
                }
                this.syncWithText();
            };
            EditQueryController.$inject = ["Query.Service.QueryService", "$scope"];
            return EditQueryController;
        }());
        Controller.EditQueryController = EditQueryController;
        angular.module("Scripts.App.Query").controller("Query.Controllers.EditQueryController", EditQueryController);
    })(Controller = Query.Controller || (Query.Controller = {}));
})(Query || (Query = {}));
//# sourceMappingURL=EditQueryController.js.map