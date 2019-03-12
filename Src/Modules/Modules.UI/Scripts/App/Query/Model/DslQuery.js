/// <reference path="./DslQueryBlocks.ts"/>
var Query;
(function (Query) {
    var Models;
    (function (Models) {
        var DslQuery = (function () {
            function DslQuery() {
                this.$type = "Infrastructure.Engines.Dsl.Query.DslDataQuery, Infrastructure.Engines.Dsl";
                this.QueryEntityName = null;
                this.Blocks = new Models.DslQueryBlocks();
                console.log("Query model created.");
            }
            return DslQuery;
        }());
        Models.DslQuery = DslQuery;
    })(Models = Query.Models || (Query.Models = {}));
})(Query || (Query = {}));
//# sourceMappingURL=DslQuery.js.map