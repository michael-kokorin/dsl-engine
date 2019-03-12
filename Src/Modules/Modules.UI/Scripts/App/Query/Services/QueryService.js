/// <reference path="../Interfaces/IQueryService.ts"/>
var Query;
(function (Query) {
    var Services;
    (function (Services) {
        var QueryService = (function () {
            function QueryService($http) {
                var _this = this;
                this.getList = function (callback) {
                    var result = _this.httpService.get("/query/getqueries");
                    result.success(function (data) {
                        var arrayResult = data;
                        callback(arrayResult);
                    });
                };
                this.getQuery = function (queryId, callback, errCallback) {
                    var request = _this.httpService.get("/query/get", {
                        params: {
                            queryId: queryId
                        }
                    });
                    request
                        .success(function (data) {
                        callback(data);
                    })
                        .error(function (response) {
                        errCallback(response);
                    });
                };
                this.save = function (query, successClbk, errClbk) {
                    var request = _this.httpService.post("/query/Update", { query: query });
                    request
                        .success(function (data) {
                        successClbk(data);
                    })
                        .error(function (response) {
                        errClbk(response);
                    });
                };
                this.execute = function (queryId, userId, parameters, callback, errCallback) {
                    var request = _this.httpService.get("/query/Execute", {
                        params: {
                            queryId: queryId,
                            userId: userId,
                            parameters: parameters
                        }
                    });
                    request
                        .success(function (data) {
                        callback(data);
                    })
                        .error(function (response) {
                        errCallback(response);
                    });
                };
                this.httpService = $http;
            }
            QueryService.prototype.getDataSources = function (projectId, callback, errCallback) {
                var request = this.httpService.get("/query/GetDataSources", {
                    params: {
                        projectId: projectId
                    }
                });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            QueryService.prototype.getDataSourceFields = function (dataSourceKey, projectId, callback, errCallback) {
                var request = this.httpService.get("/query/GetDataSourceFields", {
                    params: {
                        dataSourceKey: dataSourceKey,
                        projectId: projectId
                    }
                });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            QueryService.prototype.getTextQuery = function (model, callback, errCallback) {
                var request = this.httpService.post("/query/GetText", { queryModel: model });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            QueryService.prototype.getModelQuery = function (query, callback, errCallback) {
                var request = this.httpService.post("/query/GetModel", { query: query });
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            QueryService.prototype.getFilterConditions = function (callback, errCallback) {
                var request = this.httpService.get("/query/GetFilterConditions", {});
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            QueryService.prototype.getFilterOperations = function (callback, errCallback) {
                var request = this.httpService.get("/query/GetFilterOperations", {});
                request
                    .success(function (data) {
                    callback(data);
                })
                    .error(function (response) {
                    errCallback(response);
                });
            };
            return QueryService;
        }());
        Services.QueryService = QueryService;
        angular.module("Scripts.App.Query").service("Query.Service.QueryService", QueryService);
    })(Services = Query.Services || (Query.Services = {}));
})(Query || (Query = {}));
//# sourceMappingURL=QueryService.js.map