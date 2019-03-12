var Query;
(function (Query) {
    var Models;
    (function (Models) {
        var DslQueryBlocks = (function () {
            function DslQueryBlocks() {
                this.$sayHello = function () { return void {}; };
                this.$type = "Infrastructure.Engines.Dsl.Query.IDslQueryBlock[], Infrastructure.Engines.Dsl";
                this.$values = new Array();
                this.$sayHello = function () {
                    console.log("Hello");
                    return;
                };
                this.$sayHello();
                console.log("Query blocks model created");
            }
            return DslQueryBlocks;
        }());
        Models.DslQueryBlocks = DslQueryBlocks;
    })(Models = Query.Models || (Query.Models = {}));
})(Query || (Query = {}));
//# sourceMappingURL=DslQueryBlocks.js.map