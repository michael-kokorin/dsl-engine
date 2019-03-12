module Query.Interfaces {

    export interface IQueryService {

        getList: (callback: Function) => void;

        getQuery(queryId: number, callback: Function, errorCallback: Function);

        save: (query: any, callback: Function, errClbk: Function) => void;

        execute: (queryId: number, userId: number, parameters: any, callback: Function, errCalback: Function) => void;

        getDataSources: (projectId: number, callback: Function, errCallback: Function) => void;

        getFilterConditions: (callback: Function, errCallback: Function) => void;

        getFilterOperations: (callback: Function, errCallback: Function) => void;

        getDataSourceFields(dataSourceKey: string, projectId: number, callback: Function, errCallback: Function);

        getTextQuery(model: string, callback: Function, errCallback: Function);

        getModelQuery(query: string, callback: Function, errCallback: Function);
    }
}