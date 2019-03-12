/// <reference path="./DslQueryBlocks.ts"/>

module Query.Models {

    export class DslQuery {

        $type: string;

        // ReSharper disable once InconsistentNaming
        Blocks: IDslQueryBlocks;

        // ReSharper disable once InconsistentNaming
        IsTableRenderRequired: boolean;

        // ReSharper disable once InconsistentNaming
        Parameters: any;

        // ReSharper disable once InconsistentNaming
        TableKey: string;

        // ReSharper disable once InconsistentNaming
        TakeFirst: boolean;

        // ReSharper disable once InconsistentNaming
        TakeFirstOrDefault: string;

        // ReSharper disable once InconsistentNaming
        QueryEntityName: string;

        constructor()
        {
            this.$type = "Infrastructure.Engines.Dsl.Query.DslDataQuery, Infrastructure.Engines.Dsl";

            this.QueryEntityName = null;

            this.Blocks = new DslQueryBlocks();

            console.log("Query model created.");
        }
    }

}